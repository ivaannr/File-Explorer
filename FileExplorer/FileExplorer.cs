using System.Windows.Forms;
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
            PreparePathBox();
            SetUpDrives();

            List<String> dirs = Utils.GetFavoriteDirectories();

            var favDirectoryViewer = new FavoriteDirectoriesViewer(pathTextBox);

            foreach (var dir in dirs) {
                favoriteDirectoriesPanel.Controls.Add(favDirectoryViewer.Render(dir));
            }
        }

        private void SetUpDrives() {
            foreach (DriveInfo drive in drivesInfo)
            {
                var driveViewer = new DriveViewer(drive, pathTextBox);
                drivesWrapperPanel.Controls.Add(driveViewer.Render());
            }
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
                        sizeListBox.Items.Add(Utils.CastToCorrectSize(dir.Size));
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
