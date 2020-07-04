using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Meridian.Helper
{
    public class FileUtils
    {
        //Root folder of TreeView
        private static string RootDir
        {
            get
            {
                var rootDirName = ConfigManager.RootFolderName;
                if (string.IsNullOrEmpty(rootDirName))
                    throw new Exception("Failed to obtain root directory name from Config.xml file! Please provide document directory name!!");
                    
                return rootDirName;
            }
        }

        public static string RemoveAccents(string input)
        {
            string normalized = input.Normalize(NormalizationForm.FormKD);
            Encoding removal = Encoding.GetEncoding(Encoding.ASCII.CodePage,
                                                    new EncoderReplacementFallback(""),
                                                    new DecoderReplacementFallback(""));
            byte[] bytes = removal.GetBytes(normalized);
            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// Method that returns Meridian.exe path
        /// </summary>
        /// <returns></returns>
        public static string GetExecutingDirectoryName()
        {
            var path = GetExecutingDirectoryPath();
            var arrPath = path.Split("\\");

            return arrPath[^1];
        }

        public static string GetExeFolderPath()
        {
            var currentPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var infoPath = new DirectoryInfo(currentPath);
            return infoPath.FullName;
        }

        /// <summary>
        /// Method that returns path to "Dosare"
        /// </summary>
        /// <returns>Path to "Dosare"</returns>
        public static string GetExecutingDirectoryPath()
        {
            var currentPath = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            var arrPath = currentPath.Split("\\");

            var partialPath = new DirectoryInfo(currentPath);
            var rootFolder = "";

            for (int i = 0; i < arrPath.Length; i++)
            {
                var isRoot = Directory
                    .GetDirectories(partialPath.FullName)
                    .Any(dirName => dirName.EndsWith(RootDir, StringComparison.OrdinalIgnoreCase));
                
                if (isRoot)
                {
                    rootFolder = Directory
                        .GetDirectories(partialPath.FullName)
                        .Where(dirName => dirName.EndsWith(RootDir, StringComparison.OrdinalIgnoreCase))
                        .First();
                    break;
                }

                partialPath = Directory.GetParent(partialPath.FullName);
            }

            return rootFolder;
        }

        /// <summary>
        /// Method that returns list of files in folder
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        public static List<string> GetFileListInFolder(string folderPath)
        {
            var filePaths = Directory
                .GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .ToList()
                .Where(filePath => !File.GetAttributes(filePath).HasFlag(FileAttributes.Hidden));

            return filePaths.ToList();
        }

        /// <summary>
        /// Method that returns Temp folder for current user
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string GetTempPath(string filePath)
        {
            var arrPath = filePath.Split("\\");
            var tempFolder = arrPath[^3] + "\\" + arrPath[^2];
            string outputPath = Path.Combine(Path.GetTempPath(), "Thumbnails");

            return RemoveAccents(Path.Combine(outputPath, tempFolder));
        }

        /// <summary>
        /// Deletes all files in Tmp Folder
        /// </summary>
        public static void ClearTmpFolder()
        {
            var pdfTmpFolder = Path.Combine(Path.GetTempPath(), "PdfStorage");
            if (Directory.Exists(pdfTmpFolder))
            {
                var di = new DirectoryInfo(pdfTmpFolder);
                di.GetFiles().ToList().ForEach(file => file.Delete());
            }
            else
                Directory.CreateDirectory(pdfTmpFolder);
        }
    }
}
