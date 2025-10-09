using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    public struct Dir
    {

        public String Path { get; private set; }
        public String Name { get; private set; }
        public String Extension { get; private set; }
        public long Size { get; private set; }

        public Dir(String path, String name, String extension, long size) { 
            Path = path;
            Name = name;
            Extension = string.IsNullOrEmpty(extension) ? "Folder" : extension;
            Size = Extension == "Folder" ? size : size;
        }


    }
}
