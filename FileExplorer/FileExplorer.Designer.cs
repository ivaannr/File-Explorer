using FileExplorer.Model;
using FileExplorer.Properties;

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
            ButtonMetadata buttonMetadata1 = new ButtonMetadata();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileExplorer));
            sideBar = new Panel();
            favoriteDirectoriesPanel = new FlowLayoutPanel();
            folderSeparator2 = new Panel();
            drivesWrapperPanel = new FlowLayoutPanel();
            folderSeparator = new Panel();
            mainFoldersWrapper = new Panel();
            desktopButton = new Button();
            imagesButton = new Button();
            documentsButton = new Button();
            downloadsButton = new Button();
            pathBar = new Panel();
            pathTextBoxWrapper = new Panel();
            pathTextBox = new TextBox();
            toolsPanel = new Panel();
            utilsWrapperPanel = new Panel();
            invertSelectionButton = new Button();
            deselectAllButton = new Button();
            selectAllButton = new Button();
            panel1 = new Panel();
            renameButton = new Button();
            favoriteButton = new Button();
            separatorPanel3 = new Panel();
            cutButton = new Button();
            deleteButton = new Button();
            pasteButton = new Button();
            copyButton = new Button();
            separatorPanel2 = new Panel();
            historyButton = new Button();
            separatorPanel = new Panel();
            forwardButton = new Button();
            returnButton = new Button();
            parentButton = new Button();
            directoryPanel = new Panel();
            loadingLabel = new Label();
            directoriesViewPanel = new TableLayoutPanel();
            sideBar.SuspendLayout();
            mainFoldersWrapper.SuspendLayout();
            pathBar.SuspendLayout();
            pathTextBoxWrapper.SuspendLayout();
            toolsPanel.SuspendLayout();
            utilsWrapperPanel.SuspendLayout();
            directoryPanel.SuspendLayout();
            SuspendLayout();
            // 
            // sideBar
            // 
            sideBar.BackColor = Color.FromArgb(30, 30, 30);
            sideBar.Controls.Add(favoriteDirectoriesPanel);
            sideBar.Controls.Add(folderSeparator2);
            sideBar.Controls.Add(drivesWrapperPanel);
            sideBar.Controls.Add(folderSeparator);
            sideBar.Controls.Add(mainFoldersWrapper);
            sideBar.Location = new Point(-3, 0);
            sideBar.Name = "sideBar";
            sideBar.Size = new Size(236, 532);
            sideBar.TabIndex = 0;
            // 
            // favoriteDirectoriesPanel
            // 
            favoriteDirectoriesPanel.AutoScroll = true;
            favoriteDirectoriesPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            favoriteDirectoriesPanel.FlowDirection = FlowDirection.TopDown;
            favoriteDirectoriesPanel.Location = new Point(13, 314);
            favoriteDirectoriesPanel.Margin = new Padding(0);
            favoriteDirectoriesPanel.Name = "favoriteDirectoriesPanel";
            favoriteDirectoriesPanel.Size = new Size(213, 206);
            favoriteDirectoriesPanel.TabIndex = 0;
            favoriteDirectoriesPanel.WrapContents = false;
            // 
            // folderSeparator2
            // 
            folderSeparator2.BackColor = Color.FromArgb(227, 226, 227);
            folderSeparator2.Location = new Point(13, 302);
            folderSeparator2.Name = "folderSeparator2";
            folderSeparator2.Size = new Size(213, 2);
            folderSeparator2.TabIndex = 2;
            // 
            // drivesWrapperPanel
            // 
            drivesWrapperPanel.AutoScroll = true;
            drivesWrapperPanel.BackColor = Color.FromArgb(30, 30, 30);
            drivesWrapperPanel.FlowDirection = FlowDirection.TopDown;
            drivesWrapperPanel.Location = new Point(13, 158);
            drivesWrapperPanel.Margin = new Padding(0);
            drivesWrapperPanel.Name = "drivesWrapperPanel";
            drivesWrapperPanel.Size = new Size(213, 135);
            drivesWrapperPanel.TabIndex = 0;
            drivesWrapperPanel.WrapContents = false;
            // 
            // folderSeparator
            // 
            folderSeparator.BackColor = Color.FromArgb(227, 226, 227);
            folderSeparator.Location = new Point(13, 146);
            folderSeparator.Name = "folderSeparator";
            folderSeparator.Size = new Size(213, 2);
            folderSeparator.TabIndex = 1;
            // 
            // mainFoldersWrapper
            // 
            mainFoldersWrapper.BackColor = Color.FromArgb(30, 30, 30);
            mainFoldersWrapper.Controls.Add(desktopButton);
            mainFoldersWrapper.Controls.Add(imagesButton);
            mainFoldersWrapper.Controls.Add(documentsButton);
            mainFoldersWrapper.Controls.Add(downloadsButton);
            mainFoldersWrapper.Location = new Point(13, 12);
            mainFoldersWrapper.Name = "mainFoldersWrapper";
            mainFoldersWrapper.Size = new Size(213, 129);
            mainFoldersWrapper.TabIndex = 0;
            // 
            // desktopButton
            // 
            desktopButton.AutoSize = true;
            desktopButton.BackColor = Color.FromArgb(30, 30, 30);
            desktopButton.FlatAppearance.BorderSize = 0;
            desktopButton.FlatStyle = FlatStyle.Flat;
            desktopButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            desktopButton.ForeColor = SystemColors.ButtonFace;
            desktopButton.Image = Resources.DESKTOP_MAC;
            desktopButton.ImageAlign = ContentAlignment.MiddleLeft;
            desktopButton.Location = new Point(3, 4);
            desktopButton.Name = "desktopButton";
            desktopButton.Size = new Size(112, 30);
            desktopButton.TabIndex = 3;
            desktopButton.Text = "Desktop";
            desktopButton.TextAlign = ContentAlignment.MiddleLeft;
            desktopButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            desktopButton.UseVisualStyleBackColor = false;
            desktopButton.Click += desktopButton_Click;
            // 
            // imagesButton
            // 
            imagesButton.AutoSize = true;
            imagesButton.BackColor = Color.FromArgb(30, 30, 30);
            imagesButton.FlatAppearance.BorderSize = 0;
            imagesButton.FlatStyle = FlatStyle.Flat;
            imagesButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            imagesButton.ForeColor = SystemColors.ButtonHighlight;
            imagesButton.Image = Resources.PANORAMA;
            imagesButton.ImageAlign = ContentAlignment.MiddleLeft;
            imagesButton.Location = new Point(3, 91);
            imagesButton.Name = "imagesButton";
            imagesButton.Size = new Size(112, 30);
            imagesButton.TabIndex = 2;
            imagesButton.Text = "Images";
            imagesButton.TextAlign = ContentAlignment.MiddleLeft;
            imagesButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            imagesButton.UseVisualStyleBackColor = false;
            imagesButton.Click += imagesButton_Click;
            // 
            // documentsButton
            // 
            documentsButton.AutoSize = true;
            documentsButton.BackColor = Color.FromArgb(30, 30, 30);
            documentsButton.FlatAppearance.BorderSize = 0;
            documentsButton.FlatStyle = FlatStyle.Flat;
            documentsButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            documentsButton.ForeColor = SystemColors.ButtonFace;
            documentsButton.Image = Resources.DOCUMEN_SEARCH;
            documentsButton.ImageAlign = ContentAlignment.MiddleLeft;
            documentsButton.Location = new Point(3, 62);
            documentsButton.Name = "documentsButton";
            documentsButton.Size = new Size(112, 30);
            documentsButton.TabIndex = 1;
            documentsButton.Text = "Documents";
            documentsButton.TextAlign = ContentAlignment.MiddleLeft;
            documentsButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            documentsButton.UseVisualStyleBackColor = false;
            documentsButton.Click += documentsButton_Click;
            // 
            // downloadsButton
            // 
            downloadsButton.AutoSize = true;
            downloadsButton.BackColor = Color.FromArgb(30, 30, 30);
            downloadsButton.FlatAppearance.BorderSize = 0;
            downloadsButton.FlatStyle = FlatStyle.Flat;
            downloadsButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            downloadsButton.ForeColor = SystemColors.ButtonHighlight;
            downloadsButton.Image = Resources.DOWNLOADS;
            downloadsButton.ImageAlign = ContentAlignment.MiddleLeft;
            downloadsButton.Location = new Point(3, 33);
            downloadsButton.Name = "downloadsButton";
            downloadsButton.Size = new Size(112, 30);
            downloadsButton.TabIndex = 0;
            downloadsButton.Text = "Downloads";
            downloadsButton.TextAlign = ContentAlignment.MiddleLeft;
            downloadsButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            downloadsButton.UseVisualStyleBackColor = false;
            downloadsButton.Click += downloadsButton_Click;
            // 
            // pathBar
            // 
            pathBar.BackColor = Color.FromArgb(35, 35, 35);
            pathBar.Controls.Add(pathTextBoxWrapper);
            pathBar.Location = new Point(233, 0);
            pathBar.Name = "pathBar";
            pathBar.Size = new Size(796, 52);
            pathBar.TabIndex = 1;
            // 
            // pathTextBoxWrapper
            // 
            pathTextBoxWrapper.BackColor = Color.FromArgb(45, 45, 47);
            pathTextBoxWrapper.CausesValidation = false;
            pathTextBoxWrapper.Controls.Add(pathTextBox);
            pathTextBoxWrapper.Location = new Point(8, 12);
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
            pathTextBox.Text = "C:\\";
            pathTextBox.TextChanged += pathTextBox_TextChanged;
            // 
            // toolsPanel
            // 
            toolsPanel.BackColor = Color.FromArgb(30, 30, 30);
            toolsPanel.Controls.Add(utilsWrapperPanel);
            toolsPanel.Location = new Point(233, 52);
            toolsPanel.Name = "toolsPanel";
            toolsPanel.Size = new Size(796, 52);
            toolsPanel.TabIndex = 2;
            // 
            // utilsWrapperPanel
            // 
            utilsWrapperPanel.Controls.Add(invertSelectionButton);
            utilsWrapperPanel.Controls.Add(deselectAllButton);
            utilsWrapperPanel.Controls.Add(selectAllButton);
            utilsWrapperPanel.Controls.Add(panel1);
            utilsWrapperPanel.Controls.Add(renameButton);
            utilsWrapperPanel.Controls.Add(favoriteButton);
            utilsWrapperPanel.Controls.Add(separatorPanel3);
            utilsWrapperPanel.Controls.Add(cutButton);
            utilsWrapperPanel.Controls.Add(deleteButton);
            utilsWrapperPanel.Controls.Add(pasteButton);
            utilsWrapperPanel.Controls.Add(copyButton);
            utilsWrapperPanel.Controls.Add(separatorPanel2);
            utilsWrapperPanel.Controls.Add(historyButton);
            utilsWrapperPanel.Controls.Add(separatorPanel);
            utilsWrapperPanel.Controls.Add(forwardButton);
            utilsWrapperPanel.Controls.Add(returnButton);
            utilsWrapperPanel.Controls.Add(parentButton);
            utilsWrapperPanel.Location = new Point(8, 10);
            utilsWrapperPanel.Name = "utilsWrapperPanel";
            utilsWrapperPanel.Size = new Size(539, 32);
            utilsWrapperPanel.TabIndex = 0;
            // 
            // invertSelectionButton
            // 
            invertSelectionButton.FlatAppearance.BorderSize = 0;
            invertSelectionButton.FlatStyle = FlatStyle.Flat;
            invertSelectionButton.Image = Resources.INVERT_SELECTION;
            invertSelectionButton.Location = new Point(410, 6);
            invertSelectionButton.Name = "invertSelectionButton";
            invertSelectionButton.Size = new Size(22, 20);
            invertSelectionButton.TabIndex = 16;
            invertSelectionButton.UseVisualStyleBackColor = true;
            invertSelectionButton.Click += invertSelectionButton_Click;
            // 
            // deselectAllButton
            // 
            deselectAllButton.FlatAppearance.BorderSize = 0;
            deselectAllButton.FlatStyle = FlatStyle.Flat;
            deselectAllButton.Image = Resources.DESELECT_ALL;
            deselectAllButton.Location = new Point(381, 6);
            deselectAllButton.Name = "deselectAllButton";
            deselectAllButton.Size = new Size(22, 20);
            deselectAllButton.TabIndex = 15;
            deselectAllButton.UseVisualStyleBackColor = true;
            deselectAllButton.Click += deselectAllButton_Click;
            // 
            // selectAllButton
            // 
            selectAllButton.AutoEllipsis = true;
            selectAllButton.FlatAppearance.BorderSize = 0;
            selectAllButton.FlatStyle = FlatStyle.Flat;
            selectAllButton.Image = Resources.SELECT_ALL;
            selectAllButton.Location = new Point(352, 6);
            selectAllButton.Name = "selectAllButton";
            selectAllButton.Size = new Size(22, 20);
            selectAllButton.TabIndex = 14;
            selectAllButton.UseVisualStyleBackColor = true;
            selectAllButton.Click += selectAllButton_Click;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(227, 226, 227);
            panel1.Location = new Point(340, 5);
            panel1.Name = "panel1";
            panel1.Size = new Size(2, 24);
            panel1.TabIndex = 13;
            // 
            // renameButton
            // 
            renameButton.FlatAppearance.BorderSize = 0;
            renameButton.FlatStyle = FlatStyle.Flat;
            renameButton.Image = Resources.KEYBOARD;
            renameButton.Location = new Point(304, 6);
            renameButton.Name = "renameButton";
            renameButton.Size = new Size(22, 20);
            renameButton.TabIndex = 12;
            buttonMetadata1.CanDisable = true;
            buttonMetadata1.Path = null;
            buttonMetadata1.Size = null;
            buttonMetadata1.Type = null;
            renameButton.Tag = buttonMetadata1;
            renameButton.UseVisualStyleBackColor = true;
            renameButton.Click += renameButton_Click;
            // 
            // favoriteButton
            // 
            favoriteButton.FlatAppearance.BorderSize = 0;
            favoriteButton.FlatStyle = FlatStyle.Flat;
            favoriteButton.Image = Resources.STAR;
            favoriteButton.Location = new Point(275, 6);
            favoriteButton.Name = "favoriteButton";
            favoriteButton.Size = new Size(20, 20);
            favoriteButton.TabIndex = 11;
            favoriteButton.UseVisualStyleBackColor = true;
            favoriteButton.Click += favoriteButton_Click;
            // 
            // separatorPanel3
            // 
            separatorPanel3.BackColor = Color.FromArgb(227, 226, 227);
            separatorPanel3.Location = new Point(263, 5);
            separatorPanel3.Name = "separatorPanel3";
            separatorPanel3.Size = new Size(2, 24);
            separatorPanel3.TabIndex = 10;
            // 
            // cutButton
            // 
            cutButton.FlatAppearance.BorderSize = 0;
            cutButton.FlatStyle = FlatStyle.Flat;
            cutButton.Image = Resources.SCISSORS;
            cutButton.Location = new Point(230, 6);
            cutButton.Name = "cutButton";
            cutButton.Size = new Size(20, 20);
            cutButton.TabIndex = 9;
            cutButton.UseVisualStyleBackColor = true;
            cutButton.Click += cutButton_Click;
            // 
            // deleteButton
            // 
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.FlatStyle = FlatStyle.Flat;
            deleteButton.Image = Resources.PAPER_BIN;
            deleteButton.Location = new Point(201, 6);
            deleteButton.Name = "deleteButton";
            deleteButton.Size = new Size(20, 20);
            deleteButton.TabIndex = 8;
            deleteButton.UseVisualStyleBackColor = true;
            deleteButton.Click += deleteButton_Click;
            // 
            // pasteButton
            // 
            pasteButton.FlatAppearance.BorderSize = 0;
            pasteButton.FlatStyle = FlatStyle.Flat;
            pasteButton.Image = Resources.CONTENT_PASTE;
            pasteButton.Location = new Point(142, 6);
            pasteButton.Name = "pasteButton";
            pasteButton.Size = new Size(20, 20);
            pasteButton.TabIndex = 7;
            pasteButton.UseVisualStyleBackColor = true;
            // 
            // copyButton
            // 
            copyButton.FlatAppearance.BorderSize = 0;
            copyButton.FlatStyle = FlatStyle.Flat;
            copyButton.Image = Resources.CONTENT_COPY;
            copyButton.Location = new Point(172, 6);
            copyButton.Name = "copyButton";
            copyButton.Size = new Size(20, 20);
            copyButton.TabIndex = 6;
            copyButton.UseVisualStyleBackColor = true;
            copyButton.Click += copyButton_Click;
            // 
            // separatorPanel2
            // 
            separatorPanel2.BackColor = Color.FromArgb(227, 226, 227);
            separatorPanel2.Location = new Point(129, 5);
            separatorPanel2.Name = "separatorPanel2";
            separatorPanel2.Size = new Size(2, 24);
            separatorPanel2.TabIndex = 5;
            // 
            // historyButton
            // 
            historyButton.FlatAppearance.BorderSize = 0;
            historyButton.FlatStyle = FlatStyle.Flat;
            historyButton.Image = Resources.HISTORY;
            historyButton.Location = new Point(97, 6);
            historyButton.Name = "historyButton";
            historyButton.Size = new Size(20, 20);
            historyButton.TabIndex = 4;
            historyButton.UseVisualStyleBackColor = true;
            historyButton.Click += historyButton_Click;
            // 
            // separatorPanel
            // 
            separatorPanel.BackColor = Color.FromArgb(227, 226, 227);
            separatorPanel.Location = new Point(85, 5);
            separatorPanel.Name = "separatorPanel";
            separatorPanel.Size = new Size(2, 24);
            separatorPanel.TabIndex = 3;
            // 
            // forwardButton
            // 
            forwardButton.BackColor = Color.FromArgb(30, 30, 30);
            forwardButton.FlatAppearance.BorderSize = 0;
            forwardButton.FlatStyle = FlatStyle.Flat;
            forwardButton.Image = Resources.ARROW_FORWARD;
            forwardButton.Location = new Point(55, 6);
            forwardButton.Name = "forwardButton";
            forwardButton.Size = new Size(20, 20);
            forwardButton.TabIndex = 2;
            forwardButton.UseVisualStyleBackColor = false;
            forwardButton.Click += forwardButton_Click;
            // 
            // returnButton
            // 
            returnButton.BackColor = Color.FromArgb(30, 30, 30);
            returnButton.FlatAppearance.BorderSize = 0;
            returnButton.FlatStyle = FlatStyle.Flat;
            returnButton.Image = Resources.ARROW_BACK;
            returnButton.Location = new Point(29, 6);
            returnButton.Name = "returnButton";
            returnButton.Size = new Size(20, 20);
            returnButton.TabIndex = 1;
            returnButton.UseVisualStyleBackColor = false;
            returnButton.Click += returnButton_Click;
            // 
            // parentButton
            // 
            parentButton.BackColor = Color.FromArgb(30, 30, 30);
            parentButton.FlatAppearance.BorderSize = 0;
            parentButton.FlatStyle = FlatStyle.Flat;
            parentButton.Image = Resources.DOUBLE_ARROW_BACK;
            parentButton.Location = new Point(3, 6);
            parentButton.Name = "parentButton";
            parentButton.Size = new Size(20, 20);
            parentButton.TabIndex = 0;
            parentButton.UseVisualStyleBackColor = false;
            parentButton.Click += backButton_Click;
            // 
            // directoryPanel
            // 
            directoryPanel.AutoScroll = true;
            directoryPanel.Controls.Add(loadingLabel);
            directoryPanel.Controls.Add(directoriesViewPanel);
            directoryPanel.Location = new Point(241, 118);
            directoryPanel.Name = "directoryPanel";
            directoryPanel.Size = new Size(770, 402);
            directoryPanel.TabIndex = 3;
            // 
            // loadingLabel
            // 
            loadingLabel.AutoSize = true;
            loadingLabel.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            loadingLabel.ForeColor = SystemColors.ButtonFace;
            loadingLabel.Location = new Point(316, 171);
            loadingLabel.Name = "loadingLabel";
            loadingLabel.Size = new Size(106, 32);
            loadingLabel.TabIndex = 1;
            loadingLabel.Text = "Loading";
            loadingLabel.Visible = false;
            // 
            // directoriesViewPanel
            // 
            directoriesViewPanel.AutoSize = true;
            directoriesViewPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            directoriesViewPanel.ColumnCount = 3;
            directoriesViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            directoriesViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            directoriesViewPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            directoriesViewPanel.Dock = DockStyle.Top;
            directoriesViewPanel.Location = new Point(0, 0);
            directoriesViewPanel.Name = "directoriesViewPanel";
            directoriesViewPanel.Size = new Size(770, 0);
            directoriesViewPanel.TabIndex = 0;
            // 
            // FileExplorer
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(27, 27, 27);
            ClientSize = new Size(1029, 530);
            Controls.Add(directoryPanel);
            Controls.Add(toolsPanel);
            Controls.Add(pathBar);
            Controls.Add(sideBar);
            Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MaximizeBox = false;
            Name = "FileExplorer";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FileExplorer";
            FormClosed += FileExplorer_OnClose;
            KeyDown += FileExplorer_KeyDown;
            sideBar.ResumeLayout(false);
            mainFoldersWrapper.ResumeLayout(false);
            mainFoldersWrapper.PerformLayout();
            pathBar.ResumeLayout(false);
            pathTextBoxWrapper.ResumeLayout(false);
            pathTextBoxWrapper.PerformLayout();
            toolsPanel.ResumeLayout(false);
            utilsWrapperPanel.ResumeLayout(false);
            directoryPanel.ResumeLayout(false);
            directoryPanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel sideBar;
        private Panel pathBar;
        private Panel toolsPanel;
        private Panel pathTextBoxWrapper;
        private TextBox pathTextBox;
        private Panel directoryPanel;
        private Panel mainFoldersWrapper;
        private Button downloadsButton;
        private Button documentsButton;
        private Button desktopButton;
        private Button imagesButton;
        private Panel utilsWrapperPanel;
        private Button parentButton;
        private Button returnButton;
        private Button forwardButton;
        private Panel separatorPanel;
        private Button historyButton;
        private Panel separatorPanel2;
        private Button copyButton;
        private Button pasteButton;
        private Button cutButton;
        private Button deleteButton;
        private Panel separatorPanel3;
        private Button favoriteButton;
        private Button renameButton;
        private Panel folderSeparator;
        private FlowLayoutPanel drivesWrapperPanel;
        private Panel folderSeparator2;
        private FlowLayoutPanel favoriteDirectoriesPanel;
        private TableLayoutPanel directoriesViewPanel;
        private Panel panel1;
        private Button selectAllButton;
        private Button deselectAllButton;
        private Button invertSelectionButton;
        private Label loadingLabel;
    }
}
