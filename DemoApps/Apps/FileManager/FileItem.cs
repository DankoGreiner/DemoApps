using System;

namespace YourApp.FileManager
{
    public class FileItem
    {
        public string Type { get; set; }          // "File" or "Folder"
        public string Name { get; set; }          // filename or folder name
        public string RelativePath { get; set; }  // relative to /FileManager/Files, uses '/' separators
        public string FullUrl { get; set; }       // only for files
        public DateTime? LastWriteTimeUtc { get; set; }
        public long? SizeBytes { get; set; }      // only for files
    }
}
