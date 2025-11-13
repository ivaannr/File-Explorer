using FileExplorer.Model;
using FileExplorer.Properties;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using Font = System.Drawing.Font;

namespace FileExplorer
{
    partial class Utils
    {
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
            button.Enabled = string.IsNullOrEmpty(text) ? false : true;


            return button;
        }



        public static Label CreateSizeLabel(ISystemFile sf, string? customText = null)
        {
            Label label = CreateLabel(null);
            label.Text = customText ?? CastToCorrectSize(sf.Size, false);
            label.Name = $"{sf.Name}SizeLabel";
            return label;
        }

        public static Label CreateLabel(String? text)
        {
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

        public static void ChangeButtonsState(List<Button> buttons)
        {
            buttons.ForEach(b =>
            {
                b.Enabled = !b.Enabled;
            });
        }

        public static void DisableUtilsButtons(List<Button> buttons)
        {

            var buttonsToDisable =
                !_copiedButtons.Any()
                ? buttons
                : buttons.Where(b => b.Name != "pasteButton")
                         .ToList();


            buttonsToDisable.ForEach(b =>
            {
                b.Enabled = false;
            });
        }

        public static void EnableUtilsButtons(List<Button> buttons)
        {
            var buttonsToEnable =
                !_copiedButtons.Any()
                ? buttons
                : buttons.Where(b => b.Name != "pasteButton")
                         .ToList();


            buttonsToEnable.ForEach(b =>
            {
                b.Enabled = true;
            });
        }

        public static Button CreateDirectoryButton(ISystemFile sf, TextBox pathTextBox)
        {

            String type = sf.Type.ToLower();

            Button button = new DoubleClickButton();
            button.AutoSize = true;
            button.BackColor = Color.FromArgb(27, 27, 27);
            button.FlatAppearance.BorderSize = 0;
            button.FlatStyle = FlatStyle.Flat;
            button.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            button.ForeColor = SystemColors.ButtonFace;
            button.Image = GetImageExtension(sf.Type);
            button.ImageAlign = ContentAlignment.MiddleLeft;
            button.Name = $"{sf.Name}Button";
            button.Size = new Size(400, 30);
            button.TabIndex = 1;
            button.Text = $" {sf.Name}";
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.TextImageRelation = TextImageRelation.ImageBeforeText;
            button.UseVisualStyleBackColor = false;
            button.Tag = new ButtonMetadata
            {
                Path = sf.Path,
                Type = sf.Type
            };

            button.MouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Right) { return; }
                ;
                if (type != "folder") { return; }

                Console.WriteLine("Doble clicked on " + button.Name);
                pathTextBox.Text = $"{sf.Path}";
            };
            button.MouseClick += directoryButton_MouseClick!;
            return button;
        }

        private static async void directoryButton_MouseClick(object sender, MouseEventArgs me)
        {
            await Task.Run(() =>
            {
                try
                {

                    if (controlHeld && me.Button == MouseButtons.Right)
                    {

                        Button? selectedButton = sender as Button;

                        if (_selectedButtons.Contains(selectedButton!))
                        {
                            _selectedButtons.Remove(selectedButton!);
                            Console.WriteLine("Count after deselecting: " + _selectedButtons.Count);
                            selectedButton!.BackColor = Color.FromArgb(27, 27, 27);

                            if (_selectedButtons.Count == 0)
                            {
                                DisableUtilsButtons(FileExplorer.utilsButtons!);
                            }

                            return;
                        }

                        _selectedButtons.Add(selectedButton!);

                        EnableUtilsButtons(FileExplorer.utilsButtons!);

                        Console.WriteLine("Count after selecting: " + _selectedButtons!.Count);

                        selectedButton!.BackColor = Color.FromArgb(50, 50, 50);

                        Console.WriteLine("Clicked right button while holding ctrl");

                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Selecting a new directory threw an error: " + ex.Message);
                }
            });

        }

        public static void ClearSelectedButtons()
        {
            foreach (var button in _selectedButtons)
            {
                button.BackColor = Color.FromArgb(27, 27, 27);
            }
            _selectedButtons.Clear();
            DisableUtilsButtons(FileExplorer.utilsButtons!);
        }

        public static async Task ReloadFavoriteDirectories(Panel panel, TextBox pathTextBox)
        {
            panel.SuspendLayout();

            try
            {
                panel.Controls.Clear();

                List<FavoriteDirectory> favDirs = await ParseCSVData(await GetFavoriteDirectories());

                if (favDirs == null || favDirs.Count == 0) { return; }

                var favDirectoryViewer = new FavoriteDirectoriesViewer(pathTextBox);

                foreach (FavoriteDirectory dir in favDirs)
                {
                    panel.Controls.Add(favDirectoryViewer.Render(dir.Path));
                }
            }
            finally
            {
                panel.ResumeLayout();
            }
        }
        public static void ShowPopUp(string message, string title, Icon? icon)
        {
            if (isPopupOpen) return;

            isPopupOpen = true;

            Form customBox = new Form();
            customBox.Text = title;
            customBox.BackColor = Color.FromArgb(30, 30, 30);
            customBox.Size = new Size(300, 150);
            customBox.StartPosition = FormStartPosition.CenterScreen;
            customBox.ShowIcon = true;
            customBox.ShowInTaskbar = false;
            customBox.TopMost = true;
            customBox.Icon = icon;
            customBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            customBox.MaximizeBox = false;
            customBox.MinimizeBox = false;

            Button messageButton = new DisabledButton();
            messageButton.Text = message;
            messageButton.Dock = DockStyle.Fill;
            messageButton.Height = 80;
            messageButton.FlatAppearance.BorderSize = 0;
            messageButton.FlatStyle = FlatStyle.Flat;
            messageButton.BackColor = Color.FromArgb(30, 30, 30);
            messageButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            messageButton.TextAlign = ContentAlignment.MiddleCenter;

            Button okButton = new Button();
            okButton.FlatAppearance.BorderSize = 0;
            okButton.FlatStyle = FlatStyle.Flat;
            okButton.Text = "OK";
            okButton.ForeColor = Color.White;
            okButton.BackColor = Color.FromArgb(27, 27, 27);
            okButton.Dock = DockStyle.Bottom;
            okButton.Height = 35;
            okButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            okButton.Click += (sender, e) => { customBox.Close(); };

            customBox.Controls.Add(messageButton);
            customBox.Controls.Add(okButton);

            customBox.FormClosed += (sender, e) => { isPopupOpen = false; };

            customBox.ShowDialog();
        }

        public static string? ShowTextBoxPopUp(string title, Icon? icon, String text = "Write...", String closeButtonText = "OK")
        {
            if (isPopupOpen) return null;

            isPopupOpen = true;

            string? textBoxText = null;

            Form customBox = new Form();
            customBox.Text = title;
            customBox.BackColor = Color.FromArgb(30, 30, 30);
            customBox.Size = new Size(300, 125);
            customBox.StartPosition = FormStartPosition.CenterScreen;
            customBox.ShowIcon = true;
            customBox.ShowInTaskbar = false;
            customBox.TopMost = true;
            customBox.Icon = icon;
            customBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            customBox.MaximizeBox = false;
            customBox.MinimizeBox = false;

            TextBox textBox = new TextBox();
            textBox.Size = new Size(260, 30);
            textBox.Dock = DockStyle.None;
            textBox.Margin = new Padding(10);
            textBox.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            textBox.ForeColor = Color.Gray;
            textBox.BackColor = Color.FromArgb(30, 30, 30);
            textBox.Text = text;
            textBox.BorderStyle = BorderStyle.None;
            textBox.TextAlign = HorizontalAlignment.Center;

            textBox.Location = new Point(
                (customBox.ClientSize.Width - textBox.Width) / 2,
                20
            );

            textBox.TextChanged += (s, e) =>
            {
                textBoxText = textBox.Text;
            };

            textBox.GotFocus += (sender, e) =>
            {
                if (textBox.Text == text)
                {
                    textBox.Text = "";
                    textBox.ForeColor = Color.White;
                }
            };

            textBox.LostFocus += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    textBox.Text = text;
                    textBox.ForeColor = Color.Gray;
                }
            };

            Button okButton = new Button();
            okButton.FlatAppearance.BorderSize = 0;
            okButton.FlatStyle = FlatStyle.Flat;
            okButton.Text = closeButtonText;
            okButton.ForeColor = Color.White;
            okButton.BackColor = Color.FromArgb(27, 27, 27);
            okButton.Dock = DockStyle.Bottom;
            okButton.Height = 35;
            okButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            okButton.Click += (sender, e) =>
            {
                customBox.Close();
            };

            customBox.Controls.Add(okButton);
            customBox.Controls.Add(textBox);

            customBox.FormClosed += (sender, e) =>
            {
                isPopupOpen = false;
            };

            customBox.ShowDialog();

            return textBoxText;
        }

        private static Button CreatePopUpListButton(String path, Form box)
        {
            Button button = new Button();
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = Color.FromArgb(35, 35, 35);
            button.FlatStyle = FlatStyle.Flat;
            button.Text = TruncateFilename(path, len: 50);
            button.ForeColor = Color.White;
            button.BackColor = Color.FromArgb(27, 27, 27);
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Height = 35;
            button.Width = 377;
            button.Dock = DockStyle.Left;
            button.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            button.Click += (sender, e) =>
            {
                box.Tag = path;
                box.Close();
            };

            return button;
        }


        public static void CreateListPopUp(string title, Icon? icon, List<String> history, TextBox pathTextBox)
        {

            if (isPopupOpen) { return; }

            isPopupOpen = true;

            Form customBox = new Form();
            customBox.Text = title;
            customBox.BackColor = Color.FromArgb(30, 30, 30);
            customBox.Size = new Size(450, 250);
            customBox.StartPosition = FormStartPosition.CenterScreen;
            customBox.ShowIcon = true;
            customBox.ShowInTaskbar = false;
            customBox.TopMost = true;
            customBox.Icon = icon;
            customBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            customBox.MaximizeBox = false;
            customBox.MinimizeBox = false;
            customBox.AutoScroll = false;


            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Size = new Size(410, 192);
            panel.BackColor = Color.FromArgb(30, 30, 30);
            panel.FlowDirection = FlowDirection.TopDown;
            panel.WrapContents = false;
            panel.AutoScroll = true;
            panel.Location = new Point(10, 10);

            foreach (String path in history)
            {
                panel.Controls.Add(CreatePopUpListButton(path, customBox));
            }

            customBox.Controls.Add(panel);

            customBox.FormClosed += (s, e) =>
            {
                isPopupOpen = false;
                String selectedPath = customBox.Tag as String;

                if (selectedPath is null) { return; }

                pathTextBox.Text = selectedPath;
            };

            ThemePopUp(customBox);

            customBox.Show();
        }

        public static Form CreateDecisionPopUp(string message, string title, Icon? icon)
        {

            int fontSize = 12;

            if (message.Length > 80) { fontSize--; }

            if (message.Length > 120) { fontSize--; }

            Form customBox = new Form();
            customBox.Text = title;
            customBox.BackColor = Color.FromArgb(30, 30, 30);
            customBox.Size = new Size(300, 150);
            customBox.StartPosition = FormStartPosition.CenterScreen;
            customBox.ShowIcon = true;
            customBox.ShowInTaskbar = false;
            customBox.TopMost = true;
            customBox.Icon = icon;
            customBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            customBox.MaximizeBox = false;
            customBox.MinimizeBox = false;

            Button messageButton = new DisabledButton();
            messageButton.Text = message;
            messageButton.Dock = DockStyle.Top;
            messageButton.Height = 80;
            messageButton.FlatAppearance.BorderSize = 0;
            messageButton.FlatStyle = FlatStyle.Flat;
            messageButton.BackColor = Color.FromArgb(30, 30, 30);
            messageButton.Font = new Font("Segoe UI", fontSize, FontStyle.Bold);
            messageButton.TextAlign = ContentAlignment.MiddleCenter;

            Panel buttonPanel = new Panel();
            buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Height = 40;
            buttonPanel.Padding = new Padding(10);
            buttonPanel.BackColor = Color.FromArgb(30, 30, 30);

            Button yesButton = new Button();
            yesButton.FlatAppearance.BorderSize = 0;
            yesButton.FlatStyle = FlatStyle.Flat;
            yesButton.Text = "Yes";
            yesButton.ForeColor = Color.White;
            yesButton.BackColor = Color.FromArgb(30, 30, 30);
            yesButton.Size = new Size(100, 30);
            yesButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            yesButton.Click += (sender, e) => { customBox.DialogResult = DialogResult.Yes; customBox.Close(); };

            Button noButton = new Button();
            noButton.FlatAppearance.BorderSize = 0;
            noButton.FlatStyle = FlatStyle.Flat;
            noButton.Text = "No";
            noButton.ForeColor = Color.White;
            noButton.BackColor = Color.FromArgb(30, 30, 30);
            noButton.Size = new Size(100, 30);
            noButton.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            noButton.Click += (sender, e) => { customBox.DialogResult = DialogResult.No; customBox.Close(); };

            yesButton.Location = new Point(30, 5);
            noButton.Location = new Point(160, 5);
            buttonPanel.Controls.Add(yesButton);
            buttonPanel.Controls.Add(noButton);

            customBox.Controls.Add(messageButton);
            customBox.Controls.Add(buttonPanel);

            return customBox;
        }

        public static void AddToTableView(TableLayoutPanel panel, Control item, int column, int rowIndex)
        {
            if (panel.InvokeRequired)
            {
                panel.Invoke(() => { panel.Controls.Add(item, column, rowIndex); });
                return;
            }
            panel.Controls.Add(item, column, rowIndex);
        }

        public static async Task<bool> DirectoryAlreadyFavorite(string path)
        {
            try
            {
                var favDirs = await ParseCSVData(await GetFavoriteDirectories());

                List<string> dirs = favDirs.Select(dir => dir.Path).ToList();

                return dirs.Contains(path);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                return false;
            }
        }

        public static void SetUpTooltips(params Panel[] panels)
        {

            ToolTip toolTip = new ToolTip
            {
                ToolTipTitle = "Information",
                AutoPopDelay = 5000,
                InitialDelay = 500,
                ReshowDelay = 200,
                ShowAlways = true
            };

            toolTip.BackColor = Color.FromArgb(30, 30, 30);
            toolTip.ForeColor = Color.White;

            toolTip.OwnerDraw = true;
            toolTip.Draw += (s, e) =>
            {
                e.Graphics.FillRectangle(new SolidBrush(toolTip.BackColor), e.Bounds);

                using (Font boldFont = new Font(SystemFonts.DialogFont, FontStyle.Bold))
                {
                    TextRenderer.DrawText(e.Graphics, e.ToolTipText, boldFont, e.Bounds, toolTip.ForeColor);
                }
            };
            toolTip.Popup += (s, e) =>
            {
                using (Font boldFont = new Font(SystemFonts.DialogFont, FontStyle.Bold))
                {

                    Size textSize = TextRenderer.MeasureText(_buttonMessages[GetButtonName((Button)e.AssociatedControl!)], boldFont);

                    int paddingHorizontal = 25;
                    int paddingVertical = 17;

                    e.ToolTipSize = new Size(textSize.Width + paddingHorizontal, textSize.Height + paddingVertical);
                }
            };


            var buttons = panels[0].Controls.OfType<Button>().ToList();

            panels[1].Controls.OfType<Button>()
                .ToList()
                .ForEach(b => buttons.Add(b));

            foreach (Button button in buttons.ToList())
            {
                toolTip.SetToolTip(button, _buttonMessages[GetButtonName(button)]);
            }
        }

        public static void AddRowToTableLayoutPanel(TableLayoutPanel panel, List<RowItems> rowItems)
        {

            if (!rowItems.Any()) { return; }

            panel.SuspendLayout();

            int newRows = rowItems.Count;

            var controlsToMove = panel.Controls.Cast<Control>()
                .Where(c => panel.GetRow(c) >= 0)
                .OrderByDescending(c => panel.GetRow(c))
                .ToList();

            foreach (var control in controlsToMove)
            {
                panel.SetRow(control, panel.GetRow(control) + newRows);
            }


            panel.RowCount += newRows;

            for (int row = 0; row < newRows; row++)
            {
                RowItems items = rowItems[row];
                AddToTableView(panel, items.NameButton, 0, row);
                AddToTableView(panel, items.ExtensionButton, 1, row);
                AddToTableView(panel, items.SizeLabel, 2, row);
            }

            panel.ResumeLayout();
        }

        public static RowItems GetItemsFromRow(TableLayoutPanel panel, Control control)
        {
            int targetRow = panel.GetRow(control);

            var rowControls = panel.Controls
                .Cast<Control>()
                .Where(c => panel.GetRow(c) == targetRow)
                .OrderBy(c => panel.GetColumn(c))
                .ToList();

            if (rowControls.Count < 3)
            {
                throw new InvalidOperationException($"Expected at least 3 controls in row {targetRow}, found {rowControls.Count}.");
            }

            return new RowItems(
                nameButton: rowControls[0] as Button,
                extensionButton: rowControls[1] as Button,
                sizeLabel: rowControls[2] as Label
            );
        }

        private static void ThemePopUp(Control parent)
        {
            if (parent == null) return;

            int trueValue = 0x01;

            Action<Control> Theme = control =>
            {
                try
                {
                    NativeMethods.SetWindowTheme(control.Handle, "DarkMode_Explorer", null);
                    NativeMethods.DwmSetWindowAttribute(control.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref trueValue, Marshal.SizeOf(typeof(int)));
                    NativeMethods.DwmSetWindowAttribute(control.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, ref trueValue, Marshal.SizeOf(typeof(int)));
                }
                catch { }
            };

            Theme(parent);

            foreach (Control control in parent.Controls)
            {
                ThemePopUp(control);
            }
        }

        public static void DeleteRowFromTableLayoutPanel(TableLayoutPanel panel, int targetRow)
        {
            panel.SuspendLayout();

            int totalRows = panel.RowCount;

            var controlsInRow = panel.Controls.Cast<Control>()
                .Where(c => panel.GetRow(c) == targetRow)
                .ToList();

            foreach (var control in controlsInRow)
            {
                panel.Controls.Remove(control);
                control.Dispose();
            }

            for (int row = targetRow + 1; row < totalRows; row++)
            {
                var controlsBelow = panel.Controls.Cast<Control>()
                    .Where(c => panel.GetRow(c) == row)
                    .ToList();

                foreach (var control in controlsBelow)
                {
                    panel.SetRow(control, row - 1);
                }
            }

            panel.RowCount--;

            panel.ResumeLayout();
        }

        /// <summary>
        /// Establishes the button's background to rgb(50, 50, 50) (selected)
        /// </summary>
        /// <param name="button"></param>
        public static void HighlightButton(Button button)
        {
            button.BackColor = Color.FromArgb(50, 50, 50);
        }

        /// <summary>
        /// Establishes the button's background to rgb(27, 27, 27) (not selected)
        /// </summary>
        /// <param name="button"></param>
        public static void LowlightButton(Button button)
        {
            button.BackColor = Color.FromArgb(27, 27, 27);
        }

        public static void DisableAllUtilButtons(List<Button> buttons)
        {
            buttons.ForEach(b => { b.Enabled = false; });
        }

        public static void HighlightAndSelectButton(Button button)
        {
            if (button.InvokeRequired)
            {
                button.Invoke(() => HighlightButton(button));
            }
            else
            {
                HighlightButton(button);
            }

            _selectedButtons.Add(button);
            EnableUtilsButtons(FileExplorer.utilsButtons!);
        }

        public static void InvokeSafely(Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                control.Invoke(action);
                return;
            }

            action();
        }

        public static async Task AnimateLabel(
            CancellationToken token,
            Label targetLabel,
            Panel directoriesViewPanel,
            TextBox pathTextBox,
            string? text = null
        )
        {
            var labelParent = targetLabel.Parent!;
            targetLabel.Left = (labelParent.Width - targetLabel.Width) / 2;
            targetLabel.Top = (labelParent.Height - targetLabel.Height) / 2;

            long seconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            targetLabel.Show();

            try
            {
                int i = 0;
                string originalText = text ?? "Loading";
                while (!token.IsCancellationRequested)
                {
                    Console.WriteLine("Hola");

                    if (i == 4) { i = 0; }

                    InvokeSafely(targetLabel, () => { targetLabel.Text = $"{text}{new string('.', i)}"; });

                    long currentSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                    bool timePassed = currentSeconds == seconds + 2;

                    if (timePassed) { throw new TimeoutException("Directory missing or search limit exceeded."); }

                    i++;
                    await Task.Delay(250);

                    Console.WriteLine("Susn't");
                }
            }
            catch (OperationCanceledException oce)
            {
                Console.WriteLine("Animation was cancelled");
            }
            catch (TimeoutException te)
            {
                Console.WriteLine(te.Message);

                ShowPopUp(te.Message, "Time exceeded", Resources.EXCLAMATION);
                string latestPath = FileExplorer.history.DefaultIfEmpty(@"C:\").Max()!;

                if (!string.IsNullOrEmpty(latestPath) && latestPath.EndsWith("\\"))
                {
                    latestPath = latestPath.Remove(latestPath.Length - 1);
                }

                Console.WriteLine(latestPath);

                InvokeSafely(directoriesViewPanel, () => {
                    directoriesViewPanel.Show();
                    directoriesViewPanel.ResumeLayout();
                });

                pathTextBox.Text = latestPath;
            }
            finally
            {
                pathTextBox.SelectionStart = pathTextBox.Text.Length;
                pathTextBox.SelectionLength = 0;
                animationPlaying = false;
                targetLabel.Hide();
                Console.WriteLine(animationPlaying);
            }
        }

    }
}
