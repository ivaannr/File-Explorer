using FileExplorer.Properties;

namespace FileExplorer
{
    internal class DriveViewer
    {
        private DriveInfo drive;
        private long totalSpace;
        private long availableSpace;
        private string driveLetter;
        private TextBox pathTextBox;

        public DriveViewer(DriveInfo drive, TextBox pathTextBox)
        {
            this.drive = drive;
            this.pathTextBox = pathTextBox;
            totalSpace = drive.TotalSize;
            availableSpace = drive.AvailableFreeSpace;
            driveLetter = drive.Name[0].ToString();
        }

        public Panel Render()
        {
            Panel panel = new Panel
            {
                Location = new Point(0, 0),
                Name = $"{driveLetter.ToLower()}DriveWrapperPanel",
                Size = new Size(181, 64),
                TabIndex = 0,
                BackColor = Color.FromArgb(30, 30, 30),
                Margin = new Padding(0, 0, 0, 0)
            };

            var button = CreateButton();
            var bar = CreateProgressBar();

            panel.Controls.Add(button);
            panel.Controls.Add(bar);

            return panel;
        }

        private ProgressBar CreateProgressBar() {
            return new ProgressBar
            {
                ForeColor = Color.Lime,
                Location = new Point(8, 39),
                Name = "driveSpaceBar",
                Size = new Size(168, 15),
                TabIndex = 1,
                Maximum = 100,
                Value = totalSpace > 0 ? (int) ((totalSpace - availableSpace) * 100 / totalSpace) : 0,
            };
        }

        private Button CreateButton() {
            Button button = new Button
            {
                AutoSize = true,
                BackColor = Color.FromArgb(30, 30, 30),
                FlatAppearance = { BorderSize = 0 },
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = SystemColors.ButtonFace,
                Image = Resources.HARD_DRIVE_2,
                ImageAlign = ContentAlignment.MiddleLeft,
                Location = new Point(3, 6),
                Name = $"{driveLetter.ToLower()}DriveButton",
                Size = new Size(160, 30),
                TabIndex = 3,
                Text = $"{driveLetter}: Drive   {Utils.CastToCorrectSize(availableSpace, true)} / {Utils.CastToCorrectSize(totalSpace, true)}",
                TextAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                UseVisualStyleBackColor = false

            };
            button.Click += (s, e) =>
            {
                pathTextBox.Text = $"{driveLetter}:\\";
            };
            return button;
        }

        public void RenderInto(Panel targetPanel)
        {
            targetPanel.Controls.Clear();
            targetPanel.Controls.Add(Render());
        }
    }
}
