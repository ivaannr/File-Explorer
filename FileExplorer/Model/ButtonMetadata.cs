using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Model
{
    public class ButtonMetadata
    {
        public string? Path { get; set; }
        public string? Type { get; set; }
        public string? Size { get; set; }
        public bool? CanDisable { get; set; }
    }
}
