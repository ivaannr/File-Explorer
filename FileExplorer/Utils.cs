using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using FileExplorer.Model;
using FileExplorer.Properties;

namespace FileExplorer
{
    internal static class Utils
    {
        private static readonly string[] audioExtensions = { "mp3", "wav", "ogg", "flac", "aac", "m4a" };
        private static readonly string[] videoExtensions = { "mp4", "avi", "mkv", "mov", "wmv", "webm" };
        private static readonly string[] documentExtensions = { "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "rtf" };
        private static readonly string[] compressedExtensions = { "zip", "rar", "7z", "tar", "gz", "bz2" };
        private static readonly string[] executableExtensions = { "exe", "msi", "bat", "sh", "cmd", "ps1" };
        private static readonly string[] configExtensions = { "json", "xml", "yml", "yaml", "md", "csv" };
        private static readonly string[] sourceCodeExtensions = { "cs", "java", "py", "js", "html", "css", "cpp", "h", "php", "rb", "kt", "kts", "csx" };
        private static readonly string[] databaseExtensions = { "sql", "db", "sqlite", "mdb" };
        private const String favsPath = "favs.csv";
        private const long KB = 1024;
        private const long MB = KB * 1024;
        private const long GB = MB * 1024;

        public static string CastToCorrectSize(long size) {


            return size switch
            {
                < KB => $"{size} bytes",
                < MB => $"{size / KB} KB",
                < GB => $"{size / MB} MB",
                _ => $"{size / GB} GB"
            };
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


        public static long CalculateDirectorySize(string folderPath, CancellationToken token)
        {
            long size = 0;
            try
            {
                foreach (string file in Directory.EnumerateFiles(folderPath))
                {
                    token.ThrowIfCancellationRequested();

                    try
                    {
                        FileInfo info = new FileInfo(file);
                        size += info.Length;
                    }
                    catch(Exception ex) { Console.WriteLine(ex.Message); return 0; }
                }

                foreach (string dir in Directory.EnumerateDirectories(folderPath)) {

                    token.ThrowIfCancellationRequested();
                    size += CalculateDirectorySize(dir, token);
                }
            }
            catch (Exception ex) { Console.WriteLine("ERROR: " + ex.Message); return 0; }
            return size;
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
                        String ID = parts[0];
                        String path = parts[1];

                        if (!Directory.Exists(path))
                        {
                            throw new DirectoryNotFoundException($"Directory {path} not found.");
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
                Console.WriteLine($"ERROR: {ex.Message}");
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

        public static Button CreateDirectoryButton(ISystemFile sf, TextBox pathTextBox)
        {

            String type = sf.Type.ToLower();
            String name = $"{sf.Name}&{sf.Type}&Button";

            Button button = type == "folder" ? new DoubleClickButton() : new Button();
            button.AutoSize = true;
            button.BackColor = Color.FromArgb(27, 27, 27);
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.ForeColor = SystemColors.ButtonFace;
            button.Image = GetImageExtension(sf.Type);
            button.ImageAlign = ContentAlignment.MiddleLeft;
            button.Name = name;
            button.Size = new Size(400, 30);
            button.TabIndex = 1;
            button.Text = $" {sf.Name}";
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
            button.UseVisualStyleBackColor = false;
            button.MouseDoubleClick += (s, e) =>
            {
                Console.WriteLine("Doble clicked on " + button.Name);
                pathTextBox.Text = $"{sf.Path}";
            };
            button.Click += directoryButton_Click!;
            return button;
        }

        private static void directoryButton_Click(object sender, EventArgs e) {

            if (FileExplorer.CurrentSelectedButton is null) {
                FileExplorer.CurrentSelectedButton = sender as Button;

                FileExplorer.CurrentSelectedPath = FileExplorer.CurrentSelectedButton!.Name.Substring(0, FileExplorer.CurrentSelectedButton.Name.Length - 6);

                FileExplorer.CurrentSelectedButton!.BackColor = Color.FromArgb(40, 40, 40);
            }

        }

        public static Button CreateExtensionButton(String text)
        {
            Button button = new Button();
            button.AutoSize = true;
            button.BackColor = Color.FromArgb(27, 27, 27);
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.ForeColor = SystemColors.ButtonFace;
            button.Name = $"{text}Button";
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Size = new Size(112, 30);
            button.TabIndex = 1;
            button.Text = text;
            button.FlatStyle = FlatStyle.Flat;
            button.UseVisualStyleBackColor = false;


            return button;
        }

        public static Label CreateSizeLabel(ISystemFile sf)
        {
            Label label = CreateLabel(null);
            label.Text = CastToCorrectSize(sf.Size);
            label.Name = $"{sf.Name}SizeLabel";
            return label;
        }

        public static Label CreateLabel(String? text) {
            Label label = new Label();
            label.Size = new Size(112, 30);
            label.BackColor = Color.FromArgb(27, 27, 27);
            label.AutoSize = true;
            label.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Dock = DockStyle.Fill;
            label.Text = text;
            label.Name = $"{text}SizeLabel";
            label.ForeColor = SystemColors.ButtonFace;
            return label;
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
                _ => Resources.FILE
            };
        }

        public static void AddToTableView(TableLayoutPanel panel, Control item, int column, int rowIndex)
        {
            panel.Controls.Add(item, column, rowIndex);
        }

        public static async Task<string> GetLastID()
        {
            List<FavoriteDirectory> favs = await ParseCSVData(await GetFavoriteDirectories());

            var ids = favs.Select(fav => fav.ID).ToList();

            return ids.Count > 0 ? ids.Last() : "0";
        }

        public static async Task RegisterFavoriteDirectory(String path) {
            int newID = Convert.ToInt32(await GetLastID()) + 1;
            String[] line = { $"{newID};{path}" };
            File.AppendAllLines(favsPath, line);
        }

        public static async Task DeleteDirectoryRecord(string ID)
        {
            List<FavoriteDirectory> favs = await ParseCSVData(await GetFavoriteDirectories());

            FavoriteDirectory? dirToRemove = favs.FirstOrDefault(fav => fav.ID == ID);

            if (!dirToRemove.HasValue) { return; }

            favs.Remove(dirToRemove.Value);

            var newContent = favs.Select(fav => $"{fav.ID};{fav.Path}");

            File.WriteAllLines(favsPath, newContent);
        }

    }
}
