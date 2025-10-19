using System.Drawing.Drawing2D;
using System.Security.Principal;
using System.Windows.Forms;
using FileExplorer.Model;
using FileExplorer.Properties;

namespace FileExplorer
{
    public partial class FileExplorer : Form
    {


        private bool suppressTextChanged = false;
        private CancellationTokenSource cts;
        private String path = "C:\\";
        private List<ISystemFile> systemFiles = new List<ISystemFile>();
        private DriveInfo[] drivesInfo;
        private List<FavoriteDirectory> favDirs;
        public static Button? CurrentSelectedButton { get; set; } = null;
        public static List<Button>? utilsButtons;

        public FileExplorer()
        {
            InitializeComponent();

            utilsButtons = new List<Button> {
                                    favoriteButton,
                                    deleteButton,
                                    copyButton,
                                    pasteButton,
                                    cutButton};

            drivesInfo = DriveInfo.GetDrives()
                                  .Where(d => d.DriveType == DriveType.Fixed)
                                  .Where(d => d.IsReady)
                                  .ToArray();


            //Utils.ChangeButtonsState(utilsButtons);
            PreparePathBox();
            SetUpFavoriteDirectories();
            SetUpDrives();

            bool isAdmin = new WindowsPrincipal(WindowsIdentity.GetCurrent())
                .IsInRole(WindowsBuiltInRole.Administrator);

            Console.WriteLine("Admin: " + (isAdmin ? "Yes" : "No"));
        }

        private async void SetUpFavoriteDirectories()
        {
            favDirs = await Utils.ParseCSVData(await Utils.GetFavoriteDirectories());

            if (favDirs == null || favDirs.Count == 0) { return; }

            var favDirectoryViewer = new FavoriteDirectoriesViewer(pathTextBox);

            foreach (FavoriteDirectory dir in favDirs)
            {
                favoriteDirectoriesPanel.Controls.Add(favDirectoryViewer.Render(dir.Path));
            }
        }

        private void SetUpDrives()
        {
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

        private static List<ISystemFile> GetSystemFiles(string path, CancellationToken token)
        {
            try
            {
                List<ISystemFile> systemFiles = new List<ISystemFile>();

                foreach (var dir in Directory.GetDirectories(path))
                {
                    systemFiles.Add(new Dir(
                        dir,
                        new DirectoryInfo(dir).Name,
                        Utils.CalculateDirectorySize(dir, token)
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
            catch
            {
                return new List<ISystemFile>() { };
            }
        }

        private async void pathTextBox_TextChanged(object sender, EventArgs e)
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
                await ChangeDirectory(currentPath, cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private async Task ChangeDirectory(string path, CancellationToken token)
        {
            ClearAll();

            try
            {

                directoriesViewPanel.SuspendLayout();

                int rowIndex = 0;

                var systemFiles = await Task.Run(() => GetSystemFiles(path, token), token);

                foreach (ISystemFile dir in systemFiles)
                {
                    token.ThrowIfCancellationRequested();

                    if (dir is Dir && !Utils.CanAccessDirectory(dir.Path)) { continue; }

                    directoriesViewPanel.RowCount++;
                    directoriesViewPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                    Utils.AddToTableView(directoriesViewPanel, Utils.CreateDirectoryButton(dir, pathTextBox), 0, rowIndex);
                    Utils.AddToTableView(directoriesViewPanel, Utils.CreateExtensionButton(dir.Type), 1, rowIndex);
                    Utils.AddToTableView(directoriesViewPanel, Utils.CreateSizeLabel(dir), 2, rowIndex);

                    rowIndex++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                directoriesViewPanel.ResumeLayout();
            }
        }



        private void ClearAll()
        {
            directoriesViewPanel.SuspendLayout();

            directoriesViewPanel.Controls.Clear();
            directoriesViewPanel.RowStyles.Clear();
            directoriesViewPanel.ColumnStyles.Clear();
            directoriesViewPanel.RowCount = 0;

            directoriesViewPanel.ColumnCount = 3;
            directoriesViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            directoriesViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            directoriesViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));

            directoriesViewPanel.ResumeLayout();
        }

        private async void AddNotFound()
        {
            directoriesViewPanel.SuspendLayout();
            await Task.Delay(100);
            ClearAll();
            directoriesViewPanel.RowCount = 1;
            Utils.AddToTableView(directoriesViewPanel, Utils.CreateLabel("Directory not found"), 0, 0);
            Utils.AddToTableView(directoriesViewPanel, Utils.CreateLabel("-"), 1, 0);
            Utils.AddToTableView(directoriesViewPanel, Utils.CreateLabel("-"), 2, 0);
            directoriesViewPanel.ResumeLayout();
        }

        public async Task ReloadUI()
        {
            string currentPath = pathTextBox.Text;

            suppressTextChanged = true;

            pathTextBox.Text = currentPath + "-";

            await Task.Delay(50);

            pathTextBox.Text = currentPath;

            suppressTextChanged = false;
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

        private void backButton_Click(object sender, EventArgs e)
        {
            String parentFullPath = Directory.GetParent(pathTextBox.Text)?.FullName!;

            if (parentFullPath == null)
            {
                Utils.ShowPopUp("You can't go back any further", "Warning", Resources.NOTIFICATION_IMPORTANT);
                return;
            }

            pathTextBox.Text = parentFullPath;
        }



        private void returnButton_Click(object sender, EventArgs e)
        {

        }

        private void forwardButton_Click(object sender, EventArgs e)
        {

        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = CurrentSelectedButton!;
            ButtonMetadata? data = clickedButton.Tag as ButtonMetadata;

            try
            {
                await Utils.DeleteDirectory(data!);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting directory: " + ex.Message);
            }

            Utils.ClearCurrentSelectedButton();

            await ChangeDirectory(pathTextBox.Text, CancellationToken.None);
        }

        private void cutButton_Click(object sender, EventArgs e)
        {

        }

        private async void favoriteButton_Click(object sender, EventArgs e)
        {
            try
            {
                String path = (CurrentSelectedButton!.Tag as ButtonMetadata)!.Path!;

                bool containsDir = await Utils.DirectoryAlreadyFavorite(path);

                if (containsDir)
                {
                    Utils.ShowPopUp("You have already marked that directory as favorite.", "Warning", Resources.NOTIFICATION_IMPORTANT);
                    return;
                }

                await Utils.RegisterFavoriteDirectory(path);

                Utils.ReloadFavoriteDirectories(favoriteDirectoriesPanel, pathTextBox);
            }
            catch (NullReferenceException nullReference) {
                Console.WriteLine("Button was null: " + nullReference.Message);
                Utils.ShowPopUp("Please select a directory to set as a favorite.", "Warning", Resources.WARNING);
            }
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

        private void FileExplorer_Load(object sender, EventArgs e)
        {

        }

        private void directoryPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}

