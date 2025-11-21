using FileExplorer.Model;
using FileExplorer.Properties;

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
                if (!File.Exists(path)) { throw new FileNotFoundException($"File '{path}' not found."); }

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

        public async static Task<List<string>> GetFavoriteDirectories()
        {
            try
            {
                return await Task.Run(() =>
                {
                    List<string> directories = new List<string>();
                    var lines = ReadAllLinesFromFile(favsPath);

                    foreach (var line in lines)
                    {
                        var parts = line.Split(";");

                        if (parts.Length < 2)
                        {
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

        public static async Task<List<FavoriteDirectory>> ParseCSVData(List<String> directories)
        {
            try
            {
                return await Task.Run(() =>
                {
                    List<FavoriteDirectory> favoriteDirectories = new List<FavoriteDirectory>();

                    foreach (String dir in directories)
                    {

                        var parts = dir.Split(";");
                        String ID = parts[0];
                        String path = parts[1];

                        favoriteDirectories.Add(new FavoriteDirectory(ID, path));
                    }
                    return favoriteDirectories;
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

        public static async Task RegisterFavoriteDirectory(String path, Panel favDirsPanel)
        {
            try
            {
                int newID = Convert.ToInt32(await GetLastID()) + 1;
                String[] line = { $"{newID};{path}" };
                File.AppendAllLines(favsPath, line);
            }
            catch (Exception ex)
            {
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
            return Task.Run(() =>
            {
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

        public static void ClearCurrentSelectedButton()
        {
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

        private static Task RenameFile(string oldFilePath, string newFilePath, bool overwrite = false)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (!File.Exists(oldFilePath))
                    {
                        Console.WriteLine($"Source file does not exist: {oldFilePath}");
                        return;
                    }

                    if (File.Exists(newFilePath))
                    {
                        if (!overwrite)
                        {
                            Console.WriteLine($"Destination file already exists: {newFilePath}");
                            return;
                        }

                        File.Delete(newFilePath);
                    }

                    File.Move(oldFilePath, newFilePath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while trying to rename directory {oldFilePath} to {newFilePath}: {ex.Message}");
                }
            });
        }

        private static Task RenameDirectory(string oldDirPath, string newDirPath)
        {
            return Task.Run(() =>
            {
                try
                {
                    if (!Directory.Exists(oldDirPath))
                    {
                        Console.WriteLine($"Source directory does not exist: {oldDirPath}");
                        return;
                    }

                    if (Directory.Exists(newDirPath))
                    {
                        Console.WriteLine($"Destination directory already exists: {newDirPath}");
                        return;
                    }

                    Directory.Move(oldDirPath, newDirPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error while trying to rename directory {oldDirPath} to {newDirPath}: {ex.Message}");
                }
            });
        }

        public static async Task RenameSystemFile(string oldPath, string newPath, bool overwrite = false)
        {
            try
            {
                if (File.Exists(oldPath))
                {
                    await RenameFile(oldPath, newPath, overwrite);
                    return;
                }

                if (Directory.Exists(oldPath))
                {
                    await RenameDirectory(oldPath, newPath);
                    return;
                }

                Console.WriteLine($"No file or directory found at: {oldPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error renaming {oldPath} to {newPath}: {ex.Message}");
            }
        }

        /// <summary>
        /// Moves the directory specified in <paramref name="metadata.Path"/> to the <paramref name="destination"/> path.
        /// If the destination directory exists, it will be deleted first. 
        /// To avoid conflicts, the source directory is first moved into a temporary folder before being moved to the final destination.
        /// </summary>
        /// <param name="metadata">An ButtonMetadata object containing the path of the directory to move.</param>
        /// <param name="destination">The target path where the directory should be moved.</param>
        public static async Task PasteSystemFiles(ButtonMetadata metadata, String destination)
        {

            Console.WriteLine("File exists: " + File.Exists(metadata.Path));

            string baseFolder = Path.GetDirectoryName(metadata.Path)!;
            string tempFolder = Path.Combine(baseFolder, $"temp_{Guid.NewGuid()}");

            Console.WriteLine(tempFolder);

            try
            {
                if (File.Exists(metadata.Path))
                {
                    if (!metadata.DeleteOnPaste)
                    {
                        File.Copy(metadata.Path, destination, overwrite: true);
                        return;
                    }

                    Directory.CreateDirectory(tempFolder);

                    string tempFilePath = Path.Combine(tempFolder, Path.GetFileName(metadata.Path));
                    File.Move(metadata.Path, tempFilePath);

                    if (File.Exists(destination))
                    {
                        File.Delete(destination);
                    }

                    File.Move(tempFilePath, destination);
                    Directory.Delete(tempFolder);
                }
                else if (Directory.Exists(metadata.Path))
                {

                    if (!metadata.DeleteOnPaste)
                    {
                        await CopyDirectoryAsync(metadata.Path, destination, copySubDirs: true, overwrite: true);
                        return;
                    }

                    Directory.CreateDirectory(tempFolder);

                    string tempFilePath = Path.Combine(tempFolder, Path.GetFileName(metadata.Path));
                    Directory.Move(metadata.Path, tempFilePath);

                    if (Directory.Exists(destination))
                    {
                        Directory.Delete(destination, true);
                    }

                    Directory.Move(tempFilePath, destination);
                    Directory.Delete(tempFolder, true);
                }
                else
                {
                    Console.WriteLine($"Source path does not exist: {metadata.Path}");
                }

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);

            }
            finally
            {
                if (Directory.Exists(tempFolder))
                {
                    Directory.Delete(tempFolder, true);
                }
            }

        }

        public static void CopyDirectory(String sourceDir, String destDir, bool copySubDirs, bool overwrite = false)
        {

            DirectoryInfo dir = new DirectoryInfo(sourceDir);

            if (!dir.Exists) { throw new DirectoryNotFoundException($"{sourceDir} directory doesn't exist."); }

            if (Directory.Exists(destDir) && overwrite)
            {
                Directory.Delete(destDir, recursive: true);
            }

            DirectoryInfo[] dirs = dir.EnumerateDirectories().ToArray();
            Directory.CreateDirectory(destDir);

            foreach (FileInfo file in dir.EnumerateFiles())
            {
                string tempPath = Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, overwrite: true);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDir, subdir.Name);
                    CopyDirectory(subdir.FullName, tempPath, copySubDirs: true, overwrite: true);
                }
            }

        }

        public static async Task CopyDirectoryAsync(string sourceDir, string destDir, bool copySubDirs, bool overwrite = false)
        {
            await Task.Run(() => CopyDirectory(sourceDir, destDir, copySubDirs, overwrite));
        }

        public static async Task PasteExistingDirectories(List<Button> commonDirectories, string currentPath)
        {
            foreach (var directoryButton in commonDirectories)
            {
                if (directoryButton.Tag is not ButtonMetadata metadata) continue;
                string destination = Path.Combine(currentPath, Path.GetFileName(metadata.Path)!);

                try
                {
                    await PasteSystemFiles(metadata, destination);
                    HighlightAndSelectButton(directoryButton);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error moving {metadata.Path} to {destination}: {ex.Message}");
                }
            }
        }
        public static async Task PasteDirectories(string currentPath, List<Button> directoriesToPaste, TableLayoutPanel directoriesViewPanel)
        {
            if (directoriesToPaste == null || !directoriesToPaste.Any()) { return; }

            List<RowItems> rowItems = new List<RowItems>();

            foreach (var directoryButton in directoriesToPaste)
            {
                if (directoryButton.Tag is not ButtonMetadata metadata) { continue; }

                string destination = Path.Combine(currentPath, Path.GetFileName(metadata.Path)!);

                try
                {
                    await PasteSystemFiles(metadata, destination).ConfigureAwait(false);


                    var row = new RowItems(
                        directoryButton,
                        CreateExtensionButton(metadata.Type!),
                        CreateSizeLabel(
                            new Dir(
                                "",
                                $"placeholder{directoryButton.Name}", 0),
                            metadata.Size)
                    );

                    rowItems.Add(row);

                    InvokeSafely(directoryButton, () => HighlightButton(directoryButton));
                    _selectedButtons.Add(directoryButton);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error moving {metadata.Path} to {destination}: {ex.Message}");
                }
            }

            InvokeSafely(directoriesViewPanel, () =>
            {
                AddRowToTableLayoutPanel(directoriesViewPanel, rowItems);
            });
        }

        public static void AddToHistory(List<String> history, String pathToAdd)
        {

            string normalizedPath = pathToAdd.TrimEnd('\\');

            if (history.Contains(normalizedPath)) { return; }

            if (string.IsNullOrEmpty(pathToAdd)) { return; }

            if (history.Contains(pathToAdd)) { return; }

            history.Add(pathToAdd);
        }

        public static void HandleReparsePoint(string path)
        {

            bool isSensitive = sensitiveFolders.Contains(path);

            if (!isSensitive) { return; }

            Form form = CreateDecisionPopUp(
                "This folder may contain important files. Do you want to continue?",
                "Warning",
                Resources.INFO_CIRCLE_ICON
            );

            var result = form.ShowDialog();

            if (result == DialogResult.No) { throw new UserCanceledException(); }
        }

        public static async Task<string> GetValidLatestPath(string path)
        {
            return await Task.Run(() =>
            {
                if (!sensitiveFolders.Contains(path)) { return path; }

                foreach (string hPath in FileExplorer.history.ToList())
                {
                    if (!sensitiveFolders.Contains(hPath)) { return hPath; }
                }

                return "C:\\";
            });
        }

        public static string GetLatestPath() => FileExplorer.history.LastOrDefault() ?? @"C:\";
        private static string GetButtonName(Button button) => button.Name.Substring(0, button.Name.Length - 6);
        public static void EnableButton(Button button) => button.Enabled = true;
        public static void DisableButton(Button button) => button.Enabled = false;

    }
}
