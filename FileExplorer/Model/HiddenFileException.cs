namespace FileExplorer.Model;

public class HiddenFileException : Exception { 
    
    public HiddenFileException() { }
    public HiddenFileException(string message) : base(message) { }
}