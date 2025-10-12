using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileExplorer.Model
{
    internal class FsFile : ISystemFile
    {
        public string Path { get; private set; }
        public string Name { get; private set; }
        public long Size { get; private set; }
        public string Type { get; private set; }
        public bool IsDirectory => false;

        public FsFile(string path)
        {
            Path = path;
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Type = System.IO.Path.GetExtension(path);
            Size = new FileInfo(path).Length;
        }
    }
}
