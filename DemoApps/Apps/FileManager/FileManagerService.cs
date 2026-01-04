using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace YourApp.FileManager
{
    public class FileManagerService
    {
        private readonly HttpServerUtility _server;
        private readonly HttpRequest _request;

        public FileManagerService(HttpServerUtility server, HttpRequest request)
        {
            _server = server;
            _request = request;
        }

        // Storage root (physical): ~/FileManager/Files
        private string FilesRootPhysical => _server.MapPath("~/Apps/FileManager/Files");

        private static string NormalizeRel(string rel)
        {
            rel = (rel ?? "").Trim();
            rel = rel.Replace('\\', '/');
            while (rel.StartsWith("/")) rel = rel.Substring(1);
            return rel;
        }

        // Prevent path traversal: ensure resolved path stays inside ~/FileManager/Files
        private string GetSafePhysicalPath(string relativePath)
        {
            var rel = NormalizeRel(relativePath);

            // root folder upload
            if (string.IsNullOrEmpty(rel))
                return FilesRootPhysical;

            var combined = Path.Combine(
                FilesRootPhysical,
                rel.Replace('/', Path.DirectorySeparatorChar)
            );

            var full = Path.GetFullPath(combined);

            var rootFull = Path.GetFullPath(
                FilesRootPhysical.TrimEnd(Path.DirectorySeparatorChar)
                + Path.DirectorySeparatorChar
            );

            if (!full.StartsWith(rootFull, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Invalid path.");

            return full;
        }

        private string MakeRelativeFromRoot(string fullPath)
        {
            var root = Path.GetFullPath(FilesRootPhysical.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
            var full = Path.GetFullPath(fullPath);

            var rel = full.Substring(root.Length);
            rel = rel.Replace(Path.DirectorySeparatorChar, '/');
            return rel;
        }

        private string ToFullUrl(string relativePath)
        {
            // relativePath is relative to /FileManager/Files
            var rel = NormalizeRel(relativePath);

            var appPath = _request.ApplicationPath;
            if (string.IsNullOrEmpty(appPath) || appPath == "/") appPath = "";

            var baseUrl = string.Format("{0}://{1}{2}", _request.Url.Scheme, _request.Url.Authority, appPath);
            return string.Format("{0}/Apps/FileManager/Files/{1}", baseUrl, Uri.EscapeUriString(rel));
        }

        public List<FileItem> ListAll()
        {
            Directory.CreateDirectory(FilesRootPhysical);

            var items = new List<FileItem>();

            // Folders first (recursive)
            foreach (var dir in Directory.EnumerateDirectories(FilesRootPhysical, "*", SearchOption.AllDirectories))
            {
                var rel = MakeRelativeFromRoot(dir);

                items.Add(new FileItem
                {
                    Type = "Folder",
                    Name = new DirectoryInfo(dir).Name,
                    RelativePath = rel
                });
            }

            // Files (recursive)
            foreach (var file in Directory.EnumerateFiles(FilesRootPhysical, "*", SearchOption.AllDirectories))
            {
                var fi = new FileInfo(file);
                var rel = MakeRelativeFromRoot(file);

                items.Add(new FileItem
                {
                    Type = "File",
                    Name = fi.Name,
                    RelativePath = rel,
                    FullUrl = ToFullUrl(rel),
                    LastWriteTimeUtc = fi.LastWriteTimeUtc,
                    SizeBytes = fi.Length
                });
            }

            return items
                .OrderBy(i => i.Type == "Folder" ? 0 : 1)
                .ThenBy(i => i.RelativePath, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }

        public void CreateFolder(string folderRelativePath)
        {
            var rel = NormalizeRel(folderRelativePath);

            if (string.IsNullOrWhiteSpace(rel))
                throw new InvalidOperationException("Folder name/path is required.");

            if (rel.Split('/').Any(seg => string.IsNullOrWhiteSpace(seg) || seg.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0))
                throw new InvalidOperationException("Folder name contains invalid characters.");

            var physical = GetSafePhysicalPath(rel);
            Directory.CreateDirectory(physical);
        }

        public string SaveUpload(HttpPostedFile postedFile, string targetFolderRelativePath)
        {
            if (postedFile == null || postedFile.ContentLength <= 0)
                throw new InvalidOperationException("No file uploaded.");

            var folderRel = NormalizeRel(targetFolderRelativePath);
            var folderPhysical = GetSafePhysicalPath(folderRel);
            Directory.CreateDirectory(folderPhysical);

            var originalName = Path.GetFileName(postedFile.FileName);

            if (string.IsNullOrWhiteSpace(originalName) || originalName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new InvalidOperationException("Invalid file name.");

            // avoid overwrite: add (n)
            var savePath = Path.Combine(folderPhysical, originalName);
            var nameNoExt = Path.GetFileNameWithoutExtension(originalName);
            var ext = Path.GetExtension(originalName);

            int n = 1;
            while (File.Exists(savePath))
            {
                var candidate = string.Format("{0} ({1}){2}", nameNoExt, n, ext);
                savePath = Path.Combine(folderPhysical, candidate);
                n++;
            }

            postedFile.SaveAs(savePath);

            return MakeRelativeFromRoot(savePath);
        }

        public void DeleteFile(string fileRelativePath)
        {
            var physical = GetSafePhysicalPath(fileRelativePath);
            if (!File.Exists(physical))
                throw new FileNotFoundException("File not found.");

            File.Delete(physical);
        }

        public List<FileItem> ListFolder(string folderRelativePath)
        {
            Directory.CreateDirectory(FilesRootPhysical);

            var rel = NormalizeRel(folderRelativePath);
            var folderPhysical = GetSafePhysicalPath(rel);

            if (!Directory.Exists(folderPhysical))
                throw new DirectoryNotFoundException("Folder not found.");

            var items = new List<FileItem>();

            // child folders (non-recursive)
            foreach (var dir in Directory.EnumerateDirectories(folderPhysical, "*", SearchOption.TopDirectoryOnly))
            {
                var childRel = MakeRelativeFromRoot(dir);

                items.Add(new FileItem
                {
                    Type = "Folder",
                    Name = new DirectoryInfo(dir).Name,
                    RelativePath = childRel
                });
            }

            // files (non-recursive)
            foreach (var file in Directory.EnumerateFiles(folderPhysical, "*", SearchOption.TopDirectoryOnly))
            {
                var fi = new FileInfo(file);
                var fileRel = MakeRelativeFromRoot(file);

                items.Add(new FileItem
                {
                    Type = "File",
                    Name = fi.Name,
                    RelativePath = fileRel,
                    FullUrl = ToFullUrl(fileRel),
                    LastWriteTimeUtc = fi.LastWriteTimeUtc,
                    SizeBytes = fi.Length
                });
            }

            return items
                .OrderBy(i => i.Type == "Folder" ? 0 : 1)
                .ThenBy(i => i.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();
        }


        public void RenameFile(string fileRelativePath, string newFileName)
        {
            var physical = GetSafePhysicalPath(fileRelativePath);
            if (!File.Exists(physical))
                throw new FileNotFoundException("File not found.");

            newFileName = (newFileName ?? "").Trim();
            newFileName = Path.GetFileName(newFileName);

            if (string.IsNullOrWhiteSpace(newFileName) || newFileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                throw new InvalidOperationException("Invalid new file name.");

            var dir = Path.GetDirectoryName(physical);
            var newPhysical = Path.Combine(dir, newFileName);

            // ensure new path is still safe
            newPhysical = Path.GetFullPath(newPhysical);
            var rootFull = Path.GetFullPath(FilesRootPhysical.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar);
            if (!newPhysical.StartsWith(rootFull, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException("Invalid rename.");

            if (File.Exists(newPhysical))
                throw new InvalidOperationException("A file with that name already exists.");

            File.Move(physical, newPhysical);
        }

        public void DeleteFolder(string relativePath)
        {
            relativePath = (relativePath ?? "").Replace('\\', '/').Trim('/');

            // Map safely under root
            var root = _server.MapPath("~/Apps/FileManager/Files");
            var target = System.IO.Path.GetFullPath(System.IO.Path.Combine(root, relativePath));

            if (!target.StartsWith(System.IO.Path.GetFullPath(root), StringComparison.OrdinalIgnoreCase))
                throw new Exception("Invalid folder path.");

            if (!System.IO.Directory.Exists(target))
                throw new Exception("Folder not found.");

            // Safety: do NOT allow deleting root folder
            if (string.Equals(System.IO.Path.GetFullPath(root).TrimEnd('\\', '/'),
                              target.TrimEnd('\\', '/'),
                              StringComparison.OrdinalIgnoreCase))
                throw new Exception("Root folder cannot be deleted.");

            // Default: only empty folder
            if (System.IO.Directory.EnumerateFileSystemEntries(target).Any())
                throw new Exception("Folder is not empty.");

            System.IO.Directory.Delete(target, recursive: false);
        }

    }
}
