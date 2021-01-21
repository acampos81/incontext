using System;

public class IOProcessEventArgs : EventArgs
{
    public string filePath;
    public IOProcessType processType;

    public IOProcessEventArgs(string filePath, IOProcessType processType)
    {
        this.filePath = filePath;
        this.processType = processType;
    }
}
