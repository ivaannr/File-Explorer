using FileExplorer.Model;
using FileExplorer.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    internal static partial class Utils
    {
        public static string CastToCorrectSize(double bytes, bool rounded)
        {
            string[] sizes = { "B", "KB", "MB", "GB", "TB" };

            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            if (rounded)
            {
                return $"{len:0} {sizes[order]}";
            }

            else
            {
                return $"{len:0.#} {sizes[order]}";
            }
        }

        public static bool CanAccessDirectory(string path)
        {
            try
            {
                Directory.GetFiles(path);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error accediendo al directorio: {ex.Message}");
                return false;
            }
        }

        public static long CalculateDirectorySize(DirectoryInfo directory, CancellationToken token)
        {
            long totalSize = 0;

            try
            {
                foreach (var file in directory.EnumerateFiles())
                {
                    token.ThrowIfCancellationRequested();

                    try
                    {
                        totalSize += file.Length;
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (IOException) { }
                    catch (OperationCanceledException) { Console.WriteLine("Operation cancelled"); }
                }

                var subDirs = directory.EnumerateDirectories();

                Parallel.ForEach(subDirs, new ParallelOptions { CancellationToken = token }, subDir =>
                {
                    try
                    {
                        long subSize = CalculateDirectorySize(subDir, token);
                        Interlocked.Add(ref totalSize, subSize);
                    }
                    catch (UnauthorizedAccessException) { }
                    catch (DirectoryNotFoundException) { }
                    catch (OperationCanceledException) { Console.WriteLine("Operation cancelled"); }
                });
            }
            catch (UnauthorizedAccessException) { }
            catch (DirectoryNotFoundException) { }
            catch (OperationCanceledException) { Console.WriteLine("Operation cancelled"); }

            return totalSize;
        }



        public static string[] ReadAllLinesFromFile(string path)
        {
            try
            {
                if (!File.Exists(path)) { throw new FileNotFoundException($"File '{path}' not found.");  }

                string[] lines = File.ReadAllLines(path);

                if (lines.Length == 0) { throw new InvalidDataException($"File '{path}' is empty."); }

                return lines;
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            catch (InvalidDataException ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }

            return Array.Empty<string>();
        }

        public async static Task<List<string>> GetFavoriteDirectories() {
            try
            {
                return await Task.Run(() =>
                {
                    List<string> directories = new List<string>();
                    var lines = ReadAllLinesFromFile(favsPath);

                    foreach (var line in lines)
                    {
                        var parts = line.Split(";");

                        if (parts.Length < 2) {
                            Console.WriteLine($"Invalid line: {line}");
                            continue;
                        }

                        String ID = parts[0];
                        String path = parts[1];

                        if (!Directory.Exists(path))
                        {
                            Console.WriteLine($"Directory {path} not found.");
                            continue;
                        }

                        directories.Add(line);
                    }

                    return directories;
                });
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"File not found: {ex.Message}");
            }
            catch (DirectoryNotFoundException ex)
            {
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error ocurred while parsing favorite directory: {ex.Message}");
            }

            return new List<String> { };
        }

        public static async Task<List<FavoriteDirectory>> ParseCSVData(List<String> directories) {
            try {
                return await Task.Run(() => {
                    List<FavoriteDirectory> favoriteDirectories = new List<FavoriteDirectory>();

                    foreach (String dir in directories) {

                        var parts = dir.Split(";");
                        String ID = parts[0];
                        String path = parts[1];

                        favoriteDirectories.Add(new FavoriteDirectory(ID, path));
                    }
                    return favoriteDirectories;
                });
            } catch (FileNotFoundException ex) {
                Console.WriteLine($"File not found: {ex.Message}");
            }
            catch (DirectoryNotFoundException ex) {
                Console.WriteLine($"Directory not found: {ex.Message}");
            }
            catch (Exception ex) {
                Console.WriteLine($"ERROR error: {ex.Message}");
            }

            return new List<FavoriteDirectory> { };
        }
        public static Bitmap GetImageExtension(string fileType)
        {
            if (string.IsNullOrWhiteSpace(fileType))
                return Resources.FILE;

            string extension = fileType.StartsWith(".") ? fileType.Substring(1).ToLower() : fileType.ToLower();

            return extension switch
            {
                "folder" => Resources.FOLDER,
                var ext when audioExtensions.Contains(ext) => Resources.AUDIO_FILE,
                var ext when videoExtensions.Contains(ext) => Resources.VIDEO_FILE,
                var ext when documentExtensions.Contains(ext) => Resources.DOCUMENT_FILE,
                var ext when compressedExtensions.Contains(ext) => Resources.COMPRESSED,
                var ext when executableExtensions.Contains(ext) => Resources.COMMIT,
                var ext when configExtensions.Contains(ext) => Resources.DATA,
                var ext when sourceCodeExtensions.Contains(ext) => Resources.CODE,
                var ext when databaseExtensions.Contains(ext) => Resources.DATABASE,
                var ext when noVectorialImageExtensions.Contains(ext) => Resources.VRPANO_IMAGE,
                _ => Resources.FILE
            };
        }
        public static async Task<string> GetLastID()
        {
            List<FavoriteDirectory> favs = await ParseCSVData(await GetFavoriteDirectories());

            var ids = favs.Select(fav => fav.ID).ToList();

            return ids.Count > 0 ? ids.Last() : "0";
        }

        public static async Task RegisterFavoriteDirectory(String path, Panel favDirsPanel) {
            try
            {
                int newID = Convert.ToInt32(await GetLastID()) + 1;
                String[] line = { $"{newID};{path}" };
                File.AppendAllLines(favsPath, line);
            }
            catch (Exception ex) {
                Utils.ShowPopUp("You may not click the favorite button that fast", "Warning", Resources.WARNING);
                await RemoveAllFavoriteDirectoriesAsync(favDirsPanel);
            }
        }

        public static async Task DeleteDirectoryRecord(string ID, Panel favDirsPanel)
        {
            try
            {
                List<FavoriteDirectory> favs = await ParseCSVData(await GetFavoriteDirectories());

                FavoriteDirectory? dirToRemove = favs.FirstOrDefault(fav => fav.ID == ID);

                if (!dirToRemove.HasValue) { return; }

                favs.Remove(dirToRemove.Value);

                var newContent = favs.Select(fav => $"{fav.ID};{fav.Path}");

                File.WriteAllLines(favsPath, newContent);

            }
            catch (Exception ex)
            {
                Utils.ShowPopUp("You may not click the favorite button that fast", "Warning", Resources.WARNING);
                await RemoveAllFavoriteDirectoriesAsync(favDirsPanel);
            }

        }

        public static Task DeleteDirectory(ButtonMetadata data)
        {
            return Task.Run(() => {
                if (data == null || string.IsNullOrWhiteSpace(data.Path)) { return; }

                try
                {
                    if (File.Exists(data.Path))
                    {
                        File.Delete(data.Path);
                        Console.WriteLine($"File deleted: {data.Path}");
                        return;
                    }

                    if (Directory.Exists(data.Path))
                    {
                        Directory.Delete(data.Path, true);
                        Console.WriteLine($"Directory deleted: {data.Path}");
                        return;
                    }

                    Console.WriteLine($"No such path found: {data.Path}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Directoy deletion failed: {ex.Message}");
                }
            });

        }

        public static void ClearCurrentSelectedButton() {
            FileExplorer.CurrentSelectedButton = null;
            ChangeButtonsState(FileExplorer.utilsButtons!);
        }

        public static string TruncateFilename(string name, int len = 30)
        {
            int max = len;

            if (name.Length <= max)
            {
                return name;
            }

            return name.Substring(0, max - 3) + "...";
        }

        public static async Task<string?> GetDirectoryIDFromPath(string path)
        {
            List<FavoriteDirectory> favs = await ParseCSVData(await GetFavoriteDirectories());
            var match = favs.FirstOrDefault(fav => fav.Path == path);

            return match.IsValid ? match.ID : null;
        }


        public static async void HandleFavoriteDirectory(bool containsDir, String buttonPath, Panel favoriteDirectoriesPanel, TextBox pathTextBox)
        {
            if (containsDir)
            {
                String? id = await Utils.GetDirectoryIDFromPath(buttonPath);
                await Utils.DeleteDirectoryRecord(id!, favoriteDirectoriesPanel);
                await Utils.ReloadFavoriteDirectories(favoriteDirectoriesPanel, pathTextBox);
                return;
            }

            await Utils.RegisterFavoriteDirectory(buttonPath, favoriteDirectoriesPanel);

            await Utils.ReloadFavoriteDirectories(favoriteDirectoriesPanel, pathTextBox);
        }

        private static async Task RemoveAllFavoriteDirectoriesAsync(Panel favoriteDirsPanel)
        {
            try
            {
                favoriteDirsPanel.SuspendLayout();

                if (favoriteDirsPanel.InvokeRequired)
                {
                    favoriteDirsPanel.Invoke(new Action(() => favoriteDirsPanel.Controls.Clear()));
                    return;
                }

                favoriteDirsPanel.Controls.Clear();
                await File.WriteAllTextAsync(favsPath, string.Empty);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while removing favorite directories: {ex.Message}");
            }
            finally 
            { 
                favoriteDirsPanel.ResumeLayout();
            }
        }

        public static Task ChangeFileName(String oldFilePath, String newFilePath, bool overrideExisting = false) {
            return Task.Run(() => {
                try {
                    if (File.Exists(oldFilePath)) {
                        File.Move(oldFilePath, newFilePath, overrideExisting);
                    }
                } catch (Exception ex) {
                    Console.WriteLine($"An error ocurred when renaming file {oldFilePath}: {ex.Message}");
                }
            });
        }

        public static Task ChangeDirectoryName(String oldFilePath, String newFilePath)
        {
            return Task.Run(() => {
                try
                {
                    if (Directory.Exists(oldFilePath))
                    {
                        Directory.Move(oldFilePath, newFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error ocurred when renaming directory {oldFilePath}: {ex.Message}");
                }
            });
        }

        public static void EnableButton(Button button) => button.Enabled = true;
        public static void DisableButton(Button button) => button.Enabled = false;

    }
}
