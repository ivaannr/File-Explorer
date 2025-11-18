namespace FileExplorer.Model;

public class DirectoryAlreadyExistsException : Exception
{
    public DirectoryAlreadyExistsException() { }
    public DirectoryAlreadyExistsException(string message) : base(message) { }

}
