namespace FileExplorer
{
    partial class FileExplorer
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            sideBar = new Panel();
            panel1 = new Panel();
            desktopButton = new Button();
            imagesButton = new Button();
            documentsButton = new Button();
            downloadsButton = new Button();
            pathBar = new Panel();
            pathTextBoxWrapper = new Panel();
            pathTextBox = new TextBox();
            toolsPanel = new Panel();
            directoryPanel = new Panel();
            sizeListBox = new ListBox();
            extensionListBox = new ListBox();
            directoryListBox = new ListBox();
            sideBar.SuspendLayout();
            panel1.SuspendLayout();
            pathBar.SuspendLayout();
            pathTextBoxWrapper.SuspendLayout();
            directoryPanel.SuspendLayout();
            SuspendLayout();
            // 
            // sideBar
            // 
            sideBar.BackColor = Color.FromArgb(30, 30, 30);
            sideBar.Controls.Add(panel1);
            sideBar.Location = new Point(0, 0);
            sideBar.Name = "sideBar";
            sideBar.Size = new Size(203, 500);
            sideBar.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(255, 128, 128);
            panel1.Controls.Add(desktopButton);
            panel1.Controls.Add(imagesButton);
            panel1.Controls.Add(documentsButton);
            panel1.Controls.Add(downloadsButton);
            panel1.Location = new Point(11, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(181, 108);
            panel1.TabIndex = 0;
            // 
            // desktopButton
            // 
            desktopButton.BackColor = Color.FromArgb(30, 30, 30);
            desktopButton.FlatAppearance.BorderSize = 0;
            desktopButton.FlatStyle = FlatStyle.Flat;
            desktopButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            desktopButton.ForeColor = SystemColors.ButtonFace;
            desktopButton.Location = new Point(3, 5);
            desktopButton.Name = "desktopButton";
            desktopButton.Size = new Size(175, 23);
            desktopButton.TabIndex = 3;
            desktopButton.Text = "Desktop";
            desktopButton.TextAlign = ContentAlignment.MiddleLeft;
            desktopButton.UseVisualStyleBackColor = false;
            // 
            // imagesButton
            // 
            imagesButton.Location = new Point(2, 79);
            imagesButton.Name = "imagesButton";
            imagesButton.Size = new Size(176, 23);
            imagesButton.TabIndex = 2;
            imagesButton.Text = "button1";
            imagesButton.UseVisualStyleBackColor = true;
            // 
            // documentsButton
            // 
            documentsButton.BackColor = Color.FromArgb(30, 30, 30);
            documentsButton.FlatAppearance.BorderSize = 0;
            documentsButton.FlatStyle = FlatStyle.Flat;
            documentsButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            documentsButton.ForeColor = SystemColors.ButtonFace;
            documentsButton.Location = new Point(3, 55);
            documentsButton.Name = "documentsButton";
            documentsButton.Size = new Size(175, 23);
            documentsButton.TabIndex = 1;
            documentsButton.Text = "Documents";
            documentsButton.TextAlign = ContentAlignment.MiddleLeft;
            documentsButton.UseVisualStyleBackColor = false;
            // 
            // downloadsButton
            // 
            downloadsButton.BackColor = Color.FromArgb(30, 30, 30);
            downloadsButton.FlatAppearance.BorderSize = 0;
            downloadsButton.FlatStyle = FlatStyle.Flat;
            downloadsButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            downloadsButton.ForeColor = SystemColors.ButtonHighlight;
            downloadsButton.Location = new Point(3, 30);
            downloadsButton.Name = "downloadsButton";
            downloadsButton.Size = new Size(175, 23);
            downloadsButton.TabIndex = 0;
            downloadsButton.Text = "Downloads";
            downloadsButton.TextAlign = ContentAlignment.MiddleLeft;
            downloadsButton.UseVisualStyleBackColor = false;
            // 
            // pathBar
            // 
            pathBar.BackColor = Color.FromArgb(35, 35, 35);
            pathBar.Controls.Add(pathTextBoxWrapper);
            pathBar.Location = new Point(202, 0);
            pathBar.Name = "pathBar";
            pathBar.Size = new Size(796, 52);
            pathBar.TabIndex = 1;
            // 
            // pathTextBoxWrapper
            // 
            pathTextBoxWrapper.BackColor = Color.FromArgb(45, 45, 47);
            pathTextBoxWrapper.CausesValidation = false;
            pathTextBoxWrapper.Controls.Add(pathTextBox);
            pathTextBoxWrapper.Location = new Point(13, 12);
            pathTextBoxWrapper.Name = "pathTextBoxWrapper";
            pathTextBoxWrapper.Size = new Size(629, 28);
            pathTextBoxWrapper.TabIndex = 0;
            // 
            // pathTextBox
            // 
            pathTextBox.BackColor = Color.FromArgb(45, 45, 47);
            pathTextBox.BorderStyle = BorderStyle.None;
            pathTextBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            pathTextBox.ForeColor = Color.Ivory;
            pathTextBox.HideSelection = false;
            pathTextBox.Location = new Point(6, 6);
            pathTextBox.Name = "pathTextBox";
            pathTextBox.RightToLeft = RightToLeft.No;
            pathTextBox.Size = new Size(616, 16);
            pathTextBox.TabIndex = 0;
            pathTextBox.TextChanged += pathTextBox_TextChanged;
            // 
            // toolsPanel
            // 
            toolsPanel.BackColor = Color.FromArgb(30, 30, 30);
            toolsPanel.Location = new Point(202, 52);
            toolsPanel.Name = "toolsPanel";
            toolsPanel.Size = new Size(796, 52);
            toolsPanel.TabIndex = 2;
            // 
            // directoryPanel
            // 
            directoryPanel.Controls.Add(sizeListBox);
            directoryPanel.Controls.Add(extensionListBox);
            directoryPanel.Controls.Add(directoryListBox);
            directoryPanel.Location = new Point(215, 118);
            directoryPanel.Name = "directoryPanel";
            directoryPanel.Size = new Size(771, 366);
            directoryPanel.TabIndex = 3;
            directoryPanel.Paint += directoryPanel_Paint;
            // 
            // sizeListBox
            // 
            sizeListBox.BackColor = Color.FromArgb(27, 27, 27);
            sizeListBox.BorderStyle = BorderStyle.None;
            sizeListBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            sizeListBox.ForeColor = SystemColors.Menu;
            sizeListBox.FormattingEnabled = true;
            sizeListBox.Location = new Point(472, 10);
            sizeListBox.Name = "sizeListBox";
            sizeListBox.Size = new Size(120, 345);
            sizeListBox.TabIndex = 2;
            // 
            // extensionListBox
            // 
            extensionListBox.BackColor = Color.FromArgb(27, 27, 27);
            extensionListBox.BorderStyle = BorderStyle.None;
            extensionListBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            extensionListBox.ForeColor = SystemColors.Menu;
            extensionListBox.FormattingEnabled = true;
            extensionListBox.Location = new Point(330, 10);
            extensionListBox.Name = "extensionListBox";
            extensionListBox.Size = new Size(136, 345);
            extensionListBox.TabIndex = 1;
            // 
            // directoryListBox
            // 
            directoryListBox.BackColor = Color.FromArgb(27, 27, 27);
            directoryListBox.BorderStyle = BorderStyle.None;
            directoryListBox.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            directoryListBox.ForeColor = SystemColors.Menu;
            directoryListBox.FormattingEnabled = true;
            directoryListBox.Location = new Point(10, 8);
            directoryListBox.Name = "directoryListBox";
            directoryListBox.Size = new Size(314, 345);
            directoryListBox.TabIndex = 0;
            // 
            // FileExplorer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(27, 27, 27);
            ClientSize = new Size(998, 496);
            Controls.Add(directoryPanel);
            Controls.Add(toolsPanel);
            Controls.Add(pathBar);
            Controls.Add(sideBar);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "FileExplorer";
            Text = "FireExplorer";
            Load += FileExplorer_Load;
            sideBar.ResumeLayout(false);
            panel1.ResumeLayout(false);
            pathBar.ResumeLayout(false);
            pathTextBoxWrapper.ResumeLayout(false);
            pathTextBoxWrapper.PerformLayout();
            directoryPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel sideBar;
        private Panel pathBar;
        private Panel toolsPanel;
        private Panel pathTextBoxWrapper;
        private TextBox pathTextBox;
        private Panel directoryPanel;
        private ListBox directoryListBox;
        private ListBox extensionListBox;
        private ListBox sizeListBox;
        private Panel panel1;
        private Button downloadsButton;
        private Button documentsButton;
        private Button desktopButton;
        private Button imagesButton;
    }
}
