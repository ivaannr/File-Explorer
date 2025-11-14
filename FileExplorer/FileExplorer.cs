using FileExplorer.Model;
using FileExplorer.Properties;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;

namespace FileExplorer
{

    public partial class FileExplorer : Form
    {
        private bool suppressTextChanged = false;
        private CancellationTokenSource cts;
        private CancellationTokenSource animationCts;
        private readonly List<ISystemFile> systemFiles = new List<ISystemFile>();
        private DriveInfo[] drivesInfo;
        private List<FavoriteDirectory> favDirs;
        private volatile bool isRunning = true;

        private String path = "C:\\";

        private Stack<String> backHistory = new(),
                              forwardHistory = new();

        public static List<String> history = new List<string>();

        public static Button? CurrentSelectedButton { get; set; } = null;

        internal List<ISystemFile> SystemFiles => systemFiles;

        public static List<Button>? utilsButtons;

        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int key);

        Thread checkControlKeyThread = null;

        public FileExplorer()
        {
            InitializeComponent();

            pasteButton.Click += async (s, e) => await pasteButton_Click(s, e);

            checkControlKeyThread = new Thread(CheckKey);
            checkControlKeyThread.IsBackground = true;
            checkControlKeyThread.Start();

            utilsButtons = new List<Button> {
                                    favoriteButton,
                                    deleteButton,
                                    copyButton,
                                    pasteButton,
                                    cutButton,
                                    renameButton,
                                    deselectAllButton,
                                    invertSelectionButton
            };

            drivesInfo = DriveInfo.GetDrives()
                                  .Where(d => d.DriveType == DriveType.Fixed)
                                  .Where(d => d.IsReady)
                                  .ToArray();

            SetupOnEnabledChanged();
            Utils.SetUpTooltips(utilsWrapperPanel, mainFoldersWrapper);
            Utils.ChangeButtonsState(utilsButtons);
            ChangeDirectory(pathTextBox.Text, CancellationToken.None);
            ThemeAllControls();
            PreparePathBox();
            SetUpFavoriteDirectories();
            SetUpDrives();

            returnButton.Enabled = false;
            forwardButton.Enabled = false;
            historyButton.Enabled = false;

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
            var systemFiles = new List<ISystemFile>();

            try
            {
                foreach (var dir in Directory.EnumerateDirectories(path))
                {
                    token.ThrowIfCancellationRequested();
                    var dirInfo = new DirectoryInfo(dir);

                    systemFiles.Add(new Dir(dir, dirInfo.Name, size: 0));
                }

                foreach (var file in Directory.EnumerateFiles(path))
                {
                    try
                    {
                        token.ThrowIfCancellationRequested();
                        systemFiles.Add(new FsFile(file));
                    }
                    catch (HiddenFileException ex)
                    {
                        continue;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR : {ex.Message}");
            }

            return systemFiles;
        }

        private async Task ChangeDirectory(string path, CancellationToken token)
        {
            ClearAll();

            if (!directoriesViewPanel.Visible) {
                directoriesViewPanel.Show();
            }

            try
            {
                Utils.ClearSelectedButtons();

                directoriesViewPanel.SuspendLayout();

                int rowIndex = 0;

                var systemFiles = await Task.Run(() => GetSystemFiles(path, token), token);

                foreach (ISystemFile item in systemFiles)
                {
                    token.ThrowIfCancellationRequested();

                    if (item is Dir && !Utils.CanAccessDirectory(item.Path)) { continue; }

                    directoriesViewPanel.RowCount++;
                    directoriesViewPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                    var nameButton = Utils.CreateDirectoryButton(item, pathTextBox);
                    var typeButton = Utils.CreateExtensionButton(item.Type);
                    var sizeLabel = Utils.CreateSizeLabel(item);

                    Utils.AddToTableView(directoriesViewPanel, nameButton, 0, rowIndex);
                    Utils.AddToTableView(directoriesViewPanel, typeButton, 1, rowIndex);
                    Utils.AddToTableView(directoriesViewPanel, sizeLabel, 2, rowIndex);

                    if (item is Dir dir)
                    {
                        _ = Task.Run(() =>
                        {
                            try
                            {
                                long size = Utils.CalculateDirectorySize(new DirectoryInfo(dir.Path), token);
                                dir.Size = size;

                                if (sizeLabel.InvokeRequired)
                                {
                                    sizeLabel.Invoke(() => sizeLabel.Text = Utils.CastToCorrectSize(dir.Size, true));
                                }
                                else { sizeLabel.Text = Utils.CastToCorrectSize(dir.Size, true); }
                                   
                            }
                            catch (OperationCanceledException) { Console.WriteLine("The loading was cancelled"); }
                            catch (Exception ex) { Console.WriteLine("ERROR " + ex.Message); }
                        });
                    }

                    rowIndex++;
                }
            }
            catch (OperationCanceledException) { Console.WriteLine("The loading was cancelled"); }
            catch (Exception ex) { Console.WriteLine("ERROR " + ex.Message); }
            finally
            {
                directoriesViewPanel.ResumeLayout();
            }
        }

        private async void pathTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Console.WriteLine(Utils.animationPlaying);

                string currentPath = pathTextBox.Text;

                cts?.Cancel();
                cts = new CancellationTokenSource();

                bool dirExists = Directory.Exists(currentPath);

                if (!dirExists && !Utils.animationPlaying)
                {
                    Utils.animationPlaying = true;

                    _ = Task.Run(ExecuteLoadingAnimation);

                    return;
                }
                else
                {
                    if (Utils.animationPlaying) {
                        if (dirExists) {
                            CancelAnimation();
                        }
                    }
                }

                Utils.HandleReparsePoint(currentPath);

                if (pathTextBox.Text != this.path)
                {
                    backHistory.Push(this.path);
                    forwardHistory.Clear();

                    if (!returnButton.Enabled) { returnButton.Enabled = true; }
                }

                if (dirExists) {
                    Utils.AddToHistory(history, pathTextBox.Text);
                }

                if (!historyButton.Enabled) { historyButton.Enabled = true; }

                await ChangeDirectory(currentPath, token: cts.Token);
            }
            catch (UserCanceledException uce)
            {
                string latestPath = await Utils.GetValidLatestPath(Utils.GetLatestPath());
                Console.WriteLine(latestPath);
                pathTextBox.Text = latestPath;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                this.path = pathTextBox.Text;
            }
        }

        private void CancelAnimation() {
            Console.WriteLine("Cancelling");
            animationCts?.Cancel();
            directoriesViewPanel.Show();
            directoriesViewPanel.ResumeLayout();
            loadingLabel.Hide();

        }
        private void ThemeAllControls(Control parent = null)
        {
            parent = parent ?? this;
            Action<Control> Theme = control =>
            {
                int trueValue = 0x01;
                NativeMethods.SetWindowTheme(control.Handle, "DarkMode_Explorer", null);
                NativeMethods.DwmSetWindowAttribute(control.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref trueValue, Marshal.SizeOf(typeof(int)));
                NativeMethods.DwmSetWindowAttribute(control.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, ref trueValue, Marshal.SizeOf(typeof(int)));
            };
            if (parent == this) Theme(this);
            foreach (Control control in parent.Controls)
            {
                Theme(control);
                if (control.Controls.Count != 0)
                    ThemeAllControls(control);
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

        private async void ExecuteLoadingAnimation()
        {
            animationCts = new CancellationTokenSource();


            Utils.InvokeSafely(directoriesViewPanel, () => {
                directoriesViewPanel.SuspendLayout();
                ClearAll();
                directoriesViewPanel.Hide();
            });

            await Utils.AnimateLabel(
                animationCts.Token, 
                loadingLabel, 
                directoriesViewPanel, 
                pathTextBox, 
                "Checking path"
            );

        }

        public async Task ReloadUI()
        {
            directoriesViewPanel.SuspendLayout();

            string currentPath = pathTextBox.Text;

            suppressTextChanged = true;

            pathTextBox.Text = currentPath + "-";

            await Task.Delay(50);

            pathTextBox.Text = currentPath;

            suppressTextChanged = false;

            directoriesViewPanel.ResumeLayout();
        }

        private void desktopButton_Click(object sender, EventArgs e)
        {
            pathTextBox.Text = $@"C:\Users\{Utils.userName}\Desktop";
        }

        private void downloadsButton_Click(object sender, EventArgs e)
        {
            pathTextBox.Text = $@"C:\Users\{Utils.userName}\Downloads";
        }

        private void documentsButton_Click(object sender, EventArgs e)
        {
            pathTextBox.Text = $@"C:\Users\{Utils.userName}\Documents";
        }

        private void imagesButton_Click(object sender, EventArgs e)
        {
            pathTextBox.Text = $@"C:\Users\{Utils.userName}\Pictures";
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
            try
            {
                if (!backHistory.Any()) { throw new Exception("There is no back history."); }

                string? backPath = backHistory.Pop();

                if (!backHistory.Any()) { returnButton.Enabled = false; }

                forwardHistory.Push(path);

                if (!forwardButton.Enabled) { forwardButton.Enabled = true; }

                Console.WriteLine("Actual path " + path);
                this.path = backPath;
                pathTextBox.Text = backPath;
            }
            catch (Exception ex)
            {
                Utils.ShowPopUp(
                    $"An error ocurred:\n{ex.Message}",
                    "Warning",
                    Resources.NOTIFICATION_IMPORTANT
                );
            }
        }

        private void forwardButton_Click(object sender, EventArgs e)
        {
            try
            {

                if (!forwardHistory.Any()) { throw new Exception("There is no forward history."); }

                string? forwardPath = forwardHistory.Pop();

                if (!forwardHistory.Any()) { forwardButton.Enabled = false; }

                backHistory.Push(this.path);

                if (!returnButton.Enabled) { returnButton.Enabled = true; }

                this.path = forwardPath;
                pathTextBox.Text = forwardPath;
            }
            catch (Exception ex)
            {
                Utils.ShowPopUp(
                    $"An error ocurred:\n{ex.Message}",
                    "Warning",
                    Resources.NOTIFICATION_IMPORTANT
                );
            }
        }

        private async void deleteButton_Click(object sender, EventArgs e)
        {

            try
            {
                foreach (Button button in Utils._selectedButtons)
                {
                    await Utils.DeleteDirectory((button.Tag as ButtonMetadata)!);
                }
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
            try
            {
                Utils.DisableUtilsButtons(utilsButtons!);

                if (Utils._selectedButtons.Count > 0)
                {
                    directoriesViewPanel.SuspendLayout();

                    foreach (var but in Utils._selectedButtons.ToList())
                    {
                        ButtonMetadata metadata = (but.Tag as ButtonMetadata)!;

                        RowItems items = Utils.GetItemsFromRow(directoriesViewPanel, but);

                        metadata.Type = items.ExtensionButton.Text;
                        metadata.Size = items.SizeLabel.Text;

                        but.Tag = metadata;

                        Utils._copiedButtons.Add(but);
                    }
                }

                Utils.EnableButton(pasteButton);

            }
            catch (NullReferenceException nullReference)
            {

                Console.WriteLine($"ERROR {nullReference.Message}");

            }
            finally
            {
                Utils.ClearSelectedButtons();
                directoriesViewPanel.ResumeLayout();
            }
        }

        private void copyButton_Click(object sender, EventArgs e)
        {
            try
            {
                Utils.DisableUtilsButtons(utilsButtons!);

                if (Utils._selectedButtons.Count > 0)
                {
                    directoriesViewPanel.SuspendLayout();

                    foreach (var but in Utils._selectedButtons.ToList())
                    {
                        ButtonMetadata metadata = (but.Tag as ButtonMetadata)!;

                        RowItems items = Utils.GetItemsFromRow(directoriesViewPanel, but);

                        metadata.Type = items.ExtensionButton.Text;
                        metadata.Size = items.SizeLabel.Text;

                        but.Tag = metadata;

                        Utils._copiedButtons.Add(but);

                        Console.WriteLine($"{but.Name} coppied");
                    }

                    Utils.EnableButton(pasteButton);
                }


            }
            catch (NullReferenceException nullReference)
            {
                Console.WriteLine($"ERROR {nullReference.Message}");
            }
            finally
            {
                Utils.ClearSelectedButtons();
                directoriesViewPanel.ResumeLayout();
            }
        }

        private async void favoriteButton_Click(object sender, EventArgs e)
        {
            try
            {

                Console.WriteLine(Utils._selectedButtons.Count);

                if (Utils._selectedButtons.Count > 0)
                {

                    ButtonMetadata? data = null;

                    foreach (var but in Utils._selectedButtons)
                    {

                        data = but.Tag as ButtonMetadata;

                        String buttonPath = data!.Path!;
                        String buttonType = data!.Type!.ToLower();

                        if (buttonType != "folder")
                        {

                            Utils.ShowPopUp("You only can pin folders", "Warning", Resources.NOTIFICATION_IMPORTANT);
                            Utils.ClearSelectedButtons();
                            return;
                        }

                        bool containsDirectory = await Utils.DirectoryAlreadyFavorite(buttonPath);

                        Utils.HandleFavoriteDirectory(containsDirectory, buttonPath, favoriteDirectoriesPanel, pathTextBox);

                        await Task.Delay(50);

                    }
                }
            }
            catch (NullReferenceException nullReference)
            {
                Console.WriteLine("Button was null: " + nullReference.Message);
                Utils.ShowPopUp("Please select a directory to set as a favorite.", "Warning", Resources.WARNING);
            }
        }

        private async Task pasteButton_Click(object sender, EventArgs e)
        {
            directoriesViewPanel.SuspendLayout();

            try
            {
                String currentPath = pathTextBox.Text;

                List<Button> directories = directoriesViewPanel.Controls.OfType<Button>()
                    .Where(b => b.Tag is ButtonMetadata)
                    .ToList();

                List<Button> directoriesToPaste = Utils._copiedButtons.OfType<Button>()
                    .Where(b => b.Tag is ButtonMetadata)
                    .ToList();

                if (!directoriesToPaste.Any()) { return; }

                HashSet<Button> existingPaths = new HashSet<Button>(directories, new ButtonMetadataComparer());
                List<Button> commonDirectories = directoriesToPaste.Where(existingPaths.Contains).ToList();

                if (commonDirectories.Any())
                {
                    var overwrite = Utils.CreateDecisionPopUp(
                        "Some files with the same name are already in this folder. Do you want to replace them?",
                        "Overwrite files",
                        Resources.NOTIFICATION_IMPORTANT
                    );

                    var result = overwrite.ShowDialog();

                    if (result == DialogResult.No)
                    {
                        Utils.ClearSelectedButtons();
                        Utils._copiedButtons.Clear();
                        Utils.DisableButton(pasteButton);
                        directoriesViewPanel.ResumeLayout();
                        return;
                    }

                    await Utils.PasteExistingDirectories(commonDirectories, currentPath);

                    return;
                }

                await Utils.PasteDirectories(
                    currentPath,
                    directoriesToPaste.Except(commonDirectories).ToList(),
                    directoriesViewPanel
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
            finally
            {
                if (Utils._selectedButtons.Any())
                {
                    Utils.EnableUtilsButtons(utilsButtons!);
                }

                Utils._copiedButtons.Clear();
                Utils.DisableButton(pasteButton);
                directoriesViewPanel.ResumeLayout();
            }

        }

        private async void renameButton_Click(object sender, EventArgs e)
        {
            if (Utils._selectedButtons.Count > 1)
            {
                Utils.ShowPopUp("You can only rename button at a time", "Warning", Resources.NOTIFICATION_IMPORTANT);
                Utils.ClearSelectedButtons();
                return;
            }

            Button buttonToRename = Utils._selectedButtons.First();

            ButtonMetadata? data = buttonToRename.Tag as ButtonMetadata;

            String? newName = Utils.ShowTextBoxPopUp("Rename", Resources.LIGHTBULB_ICON, "New name...", "Rename");
            var extension = data?.Type?.ToLower() == "folder" ? "" : data?.Type;
            String newNameWithExtension = $"{newName}{extension}";

            if (string.IsNullOrWhiteSpace(newName))
                return;

            if (newName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                Utils.ShowPopUp("Invalid characters in file name.", "Error", Resources.NOTIFICATION_IMPORTANT);
                return;
            }

            string originalDir = Directory.GetParent(data!.Path!)!.FullName;
            string newPath = Path.Combine(originalDir, newNameWithExtension);

            try
            {
                await Utils.RenameSystemFile(data.Path!, newPath);
                data.Path = newPath;
                buttonToRename.Text = newName;
                Utils.ClearSelectedButtons();
            }
            catch (Exception ex)
            {
                Utils.ShowPopUp($"Rename failed: {ex.Message}", "Error", Resources.NOTIFICATION_IMPORTANT);
            }
        }

        private void CheckKey()
        {
            while (isRunning)
            {
                Utils.controlHeld = (GetAsyncKeyState(0x11) & 0x8000) > 0;

                Thread.Sleep(100);
            }
        }
        private void FileExplorer_OnClose(object sender, FormClosedEventArgs e)
        {
            isRunning = false;

            if (checkControlKeyThread != null && checkControlKeyThread.IsAlive)
            {
                checkControlKeyThread.Join();
                checkControlKeyThread = null;
            }

        }

        private void FileExplorer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Console.WriteLine("Escape pressed");
                Utils.ClearSelectedButtons();
            }
        }

        private void SetupOnEnabledChanged()
        {
            renameButton.EnabledChanged += (s, e) =>
            {
                Button b = s as Button;
                b.Image = b.Enabled ? Resources.KEYBOARD : Resources.RENAME_DISABLED;
            };
            favoriteButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.STAR : Resources.FAVORITE_DISABLED;
            };
            cutButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.SCISSORS : Resources.CUT_DISABLED;
            };
            deleteButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.PAPER_BIN : Resources.DELETE_DISABLED;
            };
            pasteButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.CONTENT_PASTE : Resources.PASTE_DISABLED;
            };
            copyButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.CONTENT_COPY : Resources.COPY_DISABLED;
            };
            forwardButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.ARROW_FORWARD : Resources.FORWARD_DISABLED;
            };
            returnButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.ARROW_BACK : Resources.RETURN_DISABLED;
            };
            parentButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.DOUBLE_ARROW_BACK : Resources.BACK_DISABLED;
            };
            selectAllButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.SELECT_ALL : Resources.SELECT_ALL_DISABLED;
            };
            deselectAllButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.DESELECT_ALL : Resources.DESELECT_ALL_DISABLED;
            };
            invertSelectionButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.INVERT_SELECTION : Resources.INVERT_SELECTION_DISABLED;
            };
            historyButton.EnabledChanged += (s, e) =>
            {
                var b = s as Button;
                b.Image = b.Enabled ? Resources.HISTORY : Resources.HISTORY_DISABLED;
            };
        }

        private void selectAllButton_Click(object sender, EventArgs e)
        {
            const int TARGET_COLUMN = 0;
            var buttonsToSelect = directoriesViewPanel.Controls
                                    .OfType<Button>()
                                    .Where(btn => directoriesViewPanel.GetColumn(btn) == TARGET_COLUMN)
                                    .ToList();

            Utils.ClearSelectedButtons();

            foreach (var button in buttonsToSelect)
            {
                Utils.HighlightAndSelectButton(button);
            }
        }

        private void deselectAllButton_Click(object sender, EventArgs e)
        {
            if (!Utils._selectedButtons.Any()) { throw new Exception("There are no selected buttons."); }
            Utils.ClearSelectedButtons();
        }

        private void invertSelectionButton_Click(object sender, EventArgs e)
        {
            if (!Utils._selectedButtons.Any()) { throw new Exception("There are no selected buttons."); }

            const int TARGET_COLUMN = 0;
            var buttons = directoriesViewPanel.Controls
                                    .OfType<Button>()
                                    .Where(btn => directoriesViewPanel.GetColumn(btn) == TARGET_COLUMN)
                                    .ToList();

            var selectedButtons = Utils._selectedButtons.ToList();
            Utils.ClearSelectedButtons();

            buttons
                .Where(b => !selectedButtons.Contains(b))
                .ToList()
                .ForEach(Utils.HighlightAndSelectButton);
        }

        private void historyButton_Click(object sender, EventArgs e)
        {
            Utils.CreateListPopUp("History", Resources.HISTORY_DISABLED_ICON, history, pathTextBox);
        }
    }

    public enum DwmWindowAttribute
    {
        DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
        DWMWA_MICA_EFFECT = 1029
    }
    public static class NativeMethods
    {
        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        public static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute attr, ref int attrValue, int attrSize);
    }
}

