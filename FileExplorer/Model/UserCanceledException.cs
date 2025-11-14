using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileExplorer.Model;

public class UserCanceledException : Exception
{
    public UserCanceledException() : base("The action was canceled by the user.") { }
    public UserCanceledException(string message) : base(message) { }
    public UserCanceledException(string message, Exception inner) : base(message, inner) { }
}