using System.IO;

namespace FileExplorer
{
    public partial class FileExplorer : Form
    {

        private String path = "C:\\";
        private List<Dir> directories = new List<Dir>();

        public FileExplorer()
        {
            InitializeComponent();
            directories = GetDirectories();
        }

        private void FileExplorer_Load(object sender, EventArgs e)
        {
            PreparePathBox();
            foreach (Dir dir in directories)
            {
                directoryListBox.Items.Add(dir.Name);
                extensionListBox.Items.Add(dir.Extension);
                sizeListBox.Items.Add(dir.Size);

            }
        }

        private async void PreparePathBox()
        {
            await Task.Delay(100);
            pathTextBox.Text = path;
            pathTextBox.SelectionStart = pathTextBox.Text.Length;
        }

        private List<Dir> GetDirectories()
        {
            var directories = Directory.GetDirectories(path);
            List<Dir> dirs = new List<Dir>();
            foreach (var dir in directories)
            {
                dirs.Add(
                    new Dir(
                        dir,
                        new DirectoryInfo(dir).Name,
                        new FileInfo(dir).Extension,
                        1000
                    )
                );
            }

            return dirs;
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
            catch { }

            return size;
        }

        private void directoryPanel_Paint(object sender, PaintEventArgs e)
        {
            // TODO
        }

        private void pathTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
