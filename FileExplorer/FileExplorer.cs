using System.Security.Principal;
using System.Windows.Forms;
using FileExplorer.Model;
using FileExplorer.Properties;

namespace FileExplorer
{
    public partial class FileExplorer : Form
    {

        private CancellationTokenSource cts;
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
            SetUpFavoriteDirectories();
            SetUpDrives();

            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);

            Console.WriteLine("Admin: " + (isAdmin ? "Yes" : "No"));
        }

        private async void SetUpFavoriteDirectories() { 
            List<FavoriteDirectory> dirs = await Utils.ParseCSVData(await Utils.GetFavoriteDirectories());

            if (dirs == null || dirs.Count == 0) { return; }

            var favDirectoryViewer = new FavoriteDirectoriesViewer(pathTextBox); 

            foreach (FavoriteDirectory dir in dirs) { 
                favoriteDirectoriesPanel.Controls.Add(favDirectoryViewer.Render(dir.Path)); 
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

        private static List<ISystemFile> GetSystemFiles(string path)
        {
            try
            {
                List<ISystemFile> systemFiles = new List<ISystemFile>();

                foreach (var dir in Directory.GetDirectories(path))
                {
                    systemFiles.Add(new Dir(
                        dir,
                        new DirectoryInfo(dir).Name,
                        Utils.CalculateDirectorySize(dir)
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
            catch {
                return new List<ISystemFile>();
            }
        }

        private void pathTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                cts?.Cancel();
                cts = new CancellationTokenSource();

                String currentPath = pathTextBox.Text;
                if (!Directory.Exists(currentPath))
                {
                    AddNotFound();
                    return;
                }
                ChangeDirectory(currentPath, cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private async void ChangeDirectory(string path, CancellationToken token)
        {
            ClearAll();

            try
            {
                var systemFiles = await Task.Run(() => GetSystemFiles(path), token);

                foreach (ISystemFile dir in systemFiles)
                {
                    token.ThrowIfCancellationRequested();

                    if (dir is Dir && !Utils.CanAccessDirectory(dir.Path)) { continue; }

                    directoryListBox.Items.Add(dir.Name);
                    extensionListBox.Items.Add(dir.Type);
                    sizeListBox.Items.Add(Utils.CastToCorrectSize(dir.Size));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }



        private void ClearAll()
        {
            systemFiles.Clear();
            directoryListBox.Items.Clear();
            extensionListBox.Items.Clear();
            sizeListBox.Items.Clear();
        }

        private async void AddNotFound()
        {
            ClearAll();
            await Task.Delay(1);
            directoryListBox.Items.Add("Directory not found");
            extensionListBox.Items.Add("-");
            sizeListBox.Items.Add("-");
        }

        private void pathTextBox_KeyDown(object sender, KeyEventArgs e)
        {


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


    }
}

