using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    internal static class Utils
    {
        public static string CastToCorrectSize(long size)
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

        public static String[] ReadAllLinesFromFile(String path) => File.Exists(path) ? File.ReadAllLines(path) : throw new FileNotFoundException($"File {path} not found.");

        public static List<String> GetFavoriteDirectories() {
            List<String> directories = new List<String>();
            var lines = ReadAllLinesFromFile("favs.txt");

            foreach (var line in lines) {
                if (!Directory.Exists(line)) { throw new DirectoryNotFoundException($"Directory {line} not found!"); }

                directories.Add(line);
            }

            return directories;
        } 




    }
}
