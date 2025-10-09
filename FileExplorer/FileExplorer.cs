using System.IO;
using System.Numerics;

namespace FileExplorer
{
    public partial class FileExplorer : Form
    {

        private String path = "C:\\";
        private List<Dir> directories = new List<Dir>();

        public FileExplorer()
        {
            InitializeComponent();
        }

        private void FileExplorer_Load(object sender, EventArgs e)
        {
            PreparePathBox();
        }

        private async void PreparePathBox()
        {
            await Task.Delay(100);
            pathTextBox.Text = path;
            pathTextBox.SelectionStart = pathTextBox.Text.Length;
        }

        private List<Dir> GetDirectories(String path)
        {
            try
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
                            GetDirectorySize(dir)
                        )
                    );
                }
                return dirs;

            } catch(Exception ex) {
                return null;
            }
        }

        private long GetDirectorySize(string folderPath)
        {
            long size = 0;

            try {

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
            } catch { 
            
            }

            return size;
        }

        private void directoryPanel_Paint(object sender, PaintEventArgs e)
        {
            // TODO
        }

        private async void pathTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String currentPath = pathTextBox.Text;
                if (!Directory.Exists(currentPath))
                {
                    AddNotFound();
                }
                ChangeDirectory(currentPath);
            } catch (Exception ex) { 
                Console.WriteLine(ex.Message); 
            }
        }

        async void ChangeDirectory(String path) {

            ClearAll();

            try
            {
                directories = GetDirectories(path);

                foreach (Dir dir in directories)
                {
                    directoryListBox.Items.Add(dir.Name);
                    extensionListBox.Items.Add(dir.Extension);
                    sizeListBox.Items.Add(CastToCorrectSize(dir.Size));
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.Message);
            }

        }

        private async void pathTextBox_KeyDown(object sender, KeyEventArgs e) {
        
        
        }

        void ClearAll() {
            directories.Clear();
            directoryListBox.Items.Clear();
            extensionListBox.Items.Clear();
            sizeListBox.Items.Clear();
        }

        private string CastToCorrectSize(long size)
        {
            const long KB = 1024;
            const long MB = KB * 1024;
            const long GB = MB * 1024;

            return size switch
            {
                < KB => $"{size} bytes",
                < MB => $"{size / KB} KB",
                < GB => $"{size / MB} MB",
                _ => $"{size / GB} GB"
            };
        }

        private void AddNotFound() {
            directoryListBox.Items.Add("Directory not found");
            extensionListBox.Items.Add("-");
            sizeListBox.Items.Add("-");
        }
    }
}
