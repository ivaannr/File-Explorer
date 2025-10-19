using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Model
{
    internal struct FavoriteDirectory
    {
        public string ID { get; private set; }    
        public string Path { get; private set; }
        public bool IsValid => !string.IsNullOrEmpty(ID) && !string.IsNullOrEmpty(Path);
        public FavoriteDirectory(string ID, string path) { 
            this.ID = ID;
            this.Path = path;
        }
    }
}
