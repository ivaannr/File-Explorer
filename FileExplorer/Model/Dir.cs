using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Model
{
    public class Dir : ISystemFile
    {

        public string Path { get; private set; }
        public string Name { get; private set; }
        public long Size { get; private set; }
        public bool IsDirectory => false;
        public string Type => "Folder";

        public Dir(string path, string name, long size) { 
            Path = path;
            Name = Utils.TruncateFilename(name);

        }


    }
}
