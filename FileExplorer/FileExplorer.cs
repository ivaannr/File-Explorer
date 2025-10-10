using FileExplorer.Properties;

namespace FileExplorer
{
    public partial class FileExplorer : Form
    {

        private String path = "C:\\";
        private List<ISystemFile> systemFiles = new List<ISystemFile>();
        private DriveInfo[] drivesInfo;

        public FileExplorer()
        {
            InitializeComponent();

            drivesInfo = DriveInfo.GetDrives()
                                  .Where(d => d.DriveType == DriveType.Fixed)
                                  .Where(d => d.IsReady)
                                  .ToArray();
            ApplyToDriveButton(drivesInfo[0], cDriveButton);

        }

        private void FileExplorer_Load(object sender, EventArgs e)
        {
            PreparePathBox();

        }

        private void ApplyToDriveButton(DriveInfo drive, Button button)
        {
            String letter = drive.Name[0].ToString();
            long totalSpace = drive.TotalSize;
            long availableSpace = drive.AvailableFreeSpace;
            String totalSpaceText = CastToCorrectSize(totalSpace);
            String availableSpaceText = CastToCorrectSize(availableSpace);
            button.AutoSize = true;
            button.BackColor = Color.FromArgb(30, 30, 30);
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.ForeColor = SystemColors.ButtonFace;
            button.Image = Resources.HARD_DRIVE_2;
            button.ImageAlign = ContentAlignment.MiddleLeft;
            button.Location = new Point(3, 6);
            button.Name = $"{letter.ToLower()}DriveButton";
            button.Size = new Size(112, 30);
            button.TabIndex = 3;
            button.Text = $"{letter}: Drive   {availableSpaceText} / {totalSpaceText}";
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
            button.UseVisualStyleBackColor = false;
            button.Click += (s, e) =>
            {
                pathTextBox.Text = drive.Name;
            };

            driveSpaceBar.Value = (int)((totalSpace - availableSpace) * 100 / totalSpace);
            driveSpaceBar.Maximum = 100;
        }

        private async void PreparePathBox()
        {
            await Task.Delay(100);
            pathTextBox.Text = path;
            pathTextBox.SelectionStart = pathTextBox.Text.Length;
        }

        private List<ISystemFile> GetSystemFiles(string path)
        {
            try
            {
                List<ISystemFile> systemFiles = new List<ISystemFile>();

                foreach (var dir in Directory.GetDirectories(path))
                {
                    systemFiles.Add(new Dir(
                        dir,
                        new DirectoryInfo(dir).Name,
                        GetDirectorySize(dir)
                    ));
                }
                foreach (var dirPath in Directory.GetFiles(path))
                {
                    var fileInfo = new FileInfo(dirPath);
                    systemFiles.Add(new FsFile(
                        dirPath
                    ));
                }

                return systemFiles;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private long GetDirectorySize(string folderPath)
        {
            long size = 0;

            try
            {

                string[] files = Directory.GetFiles(folderPath, "*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    try
                    {
                        FileInfo info = new FileInfo(file);
                        size += info.Length;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error accesing the file {file}: {ex.Message}");
                    }
                }
            }
            catch
            {

            }



            return size;
        }

        private long GetFileSize(string filePath)
        {
            try
            {
                var fileInfo = new FileInfo(filePath);
                return fileInfo.Length;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        private void directoryPanel_Paint(object sender, PaintEventArgs e)
        {
            // TODO
        }

        private async void pathTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String currentPath = pathTextBox.Text;
                if (!Directory.Exists(currentPath))
                {
                    AddNotFound();
                }
                ChangeDirectory(currentPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        void ChangeDirectory(String path)
        {
            new Thread(() => {
                ClearAll();

                try
                {
                    systemFiles = GetSystemFiles(path);

                    foreach (ISystemFile dir in systemFiles)
                    {
                        directoryListBox.Items.Add(dir.Name);
                        extensionListBox.Items.Add(dir.Type);
                        sizeListBox.Items.Add(CastToCorrectSize(dir.Size));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }).Start();
        }

        private async void pathTextBox_KeyDown(object sender, KeyEventArgs e)
        {


        }

        void ClearAll()
        {
            new Thread(() => {
                systemFiles.Clear();
                directoryListBox.Items.Clear();
                extensionListBox.Items.Clear();
                sizeListBox.Items.Clear();
            }).Start();
        }

        private string CastToCorrectSize(long size)
        {
            const long KB = 1024;
            const long MB = KB * 1024;
            const long GB = MB * 1024;

            return size switch
            {
                < KB => $"{size} bytes",
                < MB => $"{size / KB} KB",
                < GB => $"{size / MB} MB",
                _ => $"{size / GB} GB"
            };
        }

        private void AddNotFound()
        {
            directoryListBox.Items.Add("Directory not found");
            extensionListBox.Items.Add("-");
            sizeListBox.Items.Add("-");
        }

        private void desktopButton_Click(object sender, EventArgs e)
        {
            pathTextBox.Text = @"C:\Users\Usuario\Desktop";
        }

        private void downloadsButton_Click(object sender, EventArgs e)
        {
            pathTextBox.Text = @"C:\Users\Usuario\Downloads";
        }

        private void documentsButton_Click(object sender, EventArgs e)
        {
            pathTextBox.Text = @"C:\Users\Usuario\Documents";
        }

        private void imagesButton_Click(object sender, EventArgs e)
        {
            pathTextBox.Text = @"C:\Users\Usuario\Pictures";
        }

        private void utilsWrapperPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        private void returnButton_Click(object sender, EventArgs e)
        {

        }

        private void forwardButton_Click(object sender, EventArgs e)
        {

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {

        }

        private void cutButton_Click(object sender, EventArgs e)
        {

        }

        private void favoriteButton_Click(object sender, EventArgs e)
        {

        }

        private void renameButton_Click(object sender, EventArgs e)
        {

        }

        private void sideBar_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mainFoldersWrapper_Paint(object sender, PaintEventArgs e)
        {

        }

        private void driveSpaceBar_Click(object sender, EventArgs e)
        {

        }
    }
}
