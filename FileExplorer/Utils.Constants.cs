using FileExplorer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    partial class Utils
    {

        private static readonly string[] audioExtensions = { "mp3", "wav", "ogg", "flac", "aac", "m4a" };
        private static readonly string[] videoExtensions = { "mp4", "avi", "mkv", "mov", "wmv", "webm" };
        private static readonly string[] documentExtensions = { "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "txt", "rtf" };
        private static readonly string[] compressedExtensions = { "zip", "rar", "7z", "tar", "gz", "bz2" };
        private static readonly string[] executableExtensions = { "exe", "msi", "bat", "sh", "cmd", "ps1" };
        private static readonly string[] configExtensions = { "json", "xml", "yml", "yaml", "md", "csv" };
        private static readonly string[] sourceCodeExtensions = { "cs", "java", "py", "js", "html", "css", "cpp", "h", "php", "rb", "kt", "kts", "csx" };
        private static readonly string[] databaseExtensions = { "sql", "db", "sqlite", "mdb" };
        private static readonly string[] noVectorialImageExtensions = new string[] { "jpg", "jpeg", "png", "bmp", "gif", "tiff", "ico", "webp" };
        public static List<Button> _selectedButtons = new List<Button>();
        public static List<Button> _copiedButtons = new List<Button>();
        public static List<Button> _cutButtons = new List<Button>();

        private const String favsPath = "favs.csv";
        public static String userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Split('\\').Last();


        private const long KB = 1024;
        private const long MB = KB * 1024;
        private const long GB = MB * 1024;
        private const long TB = GB * 1024;

        public static bool controlHeld = false;
        private static bool isPopupOpen = false;

        private static readonly Dictionary<string, string> _buttonMessages = new Dictionary<string, string> {
            { "favorite", "Add or remove from favorites" },
            { "cut", "Cut the selected item" },
            { "copy", "Copy the selected item" },
            { "paste", "Paste the copied or cut item" },
            { "rename", "Rename the selected item" },
            { "reload", "Reload or refresh the current view" },
            { "return", "Go back to the previous location" },
            { "forward", "Go forward to the next location" },
            { "delete", "Delete the selected item" },
            { "back", "Go to the parent directory of the current one" },
            { "desktop", @$"C:\Users\{userName}\Desktop" },
            { "images", @$"C:\Users\{userName}\Pictures"  },
            { "documents", @$"C:\Users\{userName}\Documents"  },
            { "downloads", @$"C:\Users\{userName}\Downloads"  }
        };




    }
}
