using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Model
{
    internal interface ISystemFile
    {
        string Path { get; }
        string Name { get; }
        long Size { get; }
        string Type { get; }
        bool IsDirectory { get; }
    }
}
