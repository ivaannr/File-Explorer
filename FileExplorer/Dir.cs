using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer
{
    public struct Dir : ISystemFile
    {

        public String Path { get; private set; }
        public String Name { get; private set; }
        public long Size { get; private set; }
        public bool IsDirectory => false;
        public string Type => "Folder";

        public Dir(String path, String name, long size) { 
            Path = path;
            Name = name;

        }


    }
}
