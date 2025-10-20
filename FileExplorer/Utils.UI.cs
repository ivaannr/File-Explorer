using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileExplorer.Model;

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
            buttons.ForEach(b => {
                b.Enabled = !b.Enabled;

                //TODO => Make button icons turn slightly darker when disabled

            });
        }

        public static void DisableUtilsButtons(List<Button> buttons) {
            buttons.ForEach(b => {
                b.Enabled = false;

                //TODO => Make button icons turn slightly darker when disabled

            });
        }

        public static void EnableUtilsButtons(List<Button> buttons)
        {
            buttons.ForEach(b => {
                b.Enabled = true;

                //TODO => Make button icons turn slightly darker when disabled

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
                if (e.Button == MouseButtons.Right) { return; };
                if (type != "folder") { return; }

                Console.WriteLine("Doble clicked on " + button.Name);
                pathTextBox.Text = $"{sf.Path}";
            };
            button.MouseClick += directoryButton_MouseClick!;
            return button;
        }

        private static async void directoryButton_MouseClick(object sender, MouseEventArgs me)
        {

            /*
             * TODO List
             * make that when clicking on a directory the fav buttons state change
             * change util buttons state when a button is clicked
            */

            await Task.Run(() => {
                try
                {

                    if (controlHeld && me.Button == MouseButtons.Right) {

                        Button? selectedButton = sender as Button;

                        if (_selectedButtons.Contains(selectedButton!)) {
                            _selectedButtons.Remove(selectedButton!);
                            Console.WriteLine("Count after deselecting: " + _selectedButtons.Count);
                            selectedButton!.BackColor = Color.FromArgb(27, 27, 27);

                            if (_selectedButtons.Count == 0) {
                                DisableUtilsButtons(FileExplorer.utilsButtons!);
                            }

                            return;
                        }

                        _selectedButtons.Add(selectedButton!);

                        EnableUtilsButtons(FileExplorer.utilsButtons!);

                        Console.WriteLine("Count after selecting: " + _selectedButtons!.Count);

                        selectedButton!.BackColor = Color.FromArgb(50,50,50);

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
            Form customBox = new Form();
            customBox.Text = title;
            customBox.BackColor = Color.FromArgb(30, 30, 30);
            customBox.Size = new Size(300, 150);
            customBox.StartPosition = FormStartPosition.CenterScreen;
            customBox.ShowIcon = true;
            customBox.ShowInTaskbar = false;
            customBox.TopMost = true;
            customBox.Icon = icon;

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

            customBox.ShowDialog();
        }

        public static void AddToTableView(TableLayoutPanel panel, Control item, int column, int rowIndex)
        {
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


    }
}
