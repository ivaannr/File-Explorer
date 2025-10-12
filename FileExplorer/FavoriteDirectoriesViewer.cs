using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileExplorer.Properties;

namespace FileExplorer
{
    internal class FavoriteDirectoriesViewer
    {

        private TextBox pathTextBox;
        public FavoriteDirectoriesViewer(TextBox pathTextBox) { 
            this.pathTextBox = pathTextBox;
        }

        public Button Render(String path) => CreateButton(path);

        private Button CreateButton(String path) {
            String pathName = Path.GetFileName(path);

            Button button = new Button
            {
                AutoSize = true,
                BackColor = Color.FromArgb(30, 30, 30),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = SystemColors.ButtonFace,
                Image = Resources.FOLDER,
                ImageAlign = ContentAlignment.MiddleLeft,
                Location = new Point(3, 62),
                Name = $"{pathName}Button",
                Size = new Size(112, 30),
                TabIndex = 1,
                Margin = new Padding(0, 0, 0, 0),
                Text = pathName,
                TextAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                UseVisualStyleBackColor = false,
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += (s, e) => { 
                pathTextBox.Text = path;
            };

            return button;
        }
    }
}
