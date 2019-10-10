using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Security.AccessControl;




namespace SanityArchiver
{
    class FileManager
    {

        private static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }
        public static void CopyFile(String name, String sourcePath, String targetPath)
        {
            string sourceFile = Path.Combine(sourcePath, name);
            string targetFile = Path.Combine(targetPath, name);

            string sourceDir = sourceFile.Substring(0, sourceFile.Length - 4);
            if (Directory.Exists(sourceDir))
            {
                double DirSize = DirectoryManager.GetDirSize(new DirectoryInfo(sourceDir));
                List<string> files = DirectoryManager.GetFiles(sourceDir, "*");

                foreach (string s in files)
                {
                    name = s.Replace(sourcePath, targetPath+"\\");
                    FileInfo file = new FileInfo(s);
                    targetFile = Path.Combine(targetPath, name);
                    string targetSubDir = targetFile.Replace(file.Name, "");
                    if (!Directory.Exists(targetSubDir))
                    {
                        Directory.CreateDirectory(targetSubDir);
                    }
                    File.Copy(s, targetFile, true);
                }
            }
            else if (File.Exists(sourceFile))
            {
                Directory.CreateDirectory(targetPath);
                File.Copy(sourceFile, targetFile, true);

            }
        }
        public static void MoveFile(String fileName, String sourcePath, String targetPath)
        {
            string sourceFile = Path.Combine(sourcePath, fileName);
            string targetFile = Path.Combine(targetPath, fileName);

            string sourceDir = sourceFile.Substring(0, sourceFile.Length - 4);
            if (Directory.Exists(sourceDir))
            {
                double DirSize = DirectoryManager.GetDirSize(new DirectoryInfo(sourceDir));
                List<string> files = DirectoryManager.GetFiles(sourceDir, "*");
                long size;
                foreach (string s in files)
                {
                    fileName = s.Replace(sourcePath, targetPath + "\\");
                    FileInfo file = new FileInfo(s);
                    targetFile = Path.Combine(targetPath, fileName);
                    string targetSubDir = targetFile.Replace(file.Name, "");
                    if (!Directory.Exists(targetSubDir))
                    {
                        Directory.CreateDirectory(targetSubDir);
                    }
                    size = file.Length;
                    if (!File.Exists(targetFile))
                    {
                        File.Move(s, targetFile);
                    }
                }
                DeleteDirectory(sourceDir);
            }
            else if (File.Exists(sourceFile) && !File.Exists(targetFile))
            {
                Directory.CreateDirectory(targetPath);
                try
                {
                    File.Move(sourceFile, targetFile);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Access to path is denied!");
                }
            }

        }


        public static void AddEncryption(string FileName)
        {
             File.Encrypt(FileName);
        }
        public static void RemoveEncryption(string FileName)
        {
            File.Decrypt(FileName);
        }

        public static string[] GetAttributes(string path)
        {
            if (!File.Exists(path)) 
            {
                File.Create(path);
            } 

            FileAttributes attributes = File.GetAttributes(path);
            string[] attrArray = attributes.ToString().Split(',');
            for (int i = 0; i < attrArray.Length; i++)
            {
                attrArray[i] = attrArray[i].Trim();
            }
            return attrArray;
        }
        private static FileAttributes RemoveAttribute(FileAttributes attributes, FileAttributes attributesToRemove)
        {
            return attributes & ~attributesToRemove;
        }
        public static void SetAttributes(string path, string[] attrArray)
        {
            {
                if (attrArray.Contains(FileAttributes.Hidden.ToString()))
                {
                    File.SetAttributes(path, FileAttributes.Hidden);
                }
                else
                {
                    FileAttributes attributes = RemoveAttribute(File.GetAttributes(path), FileAttributes.Hidden);
                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
                }

                if (attrArray.Contains(FileAttributes.Archive.ToString()))
                {
                    File.SetAttributes(path, FileAttributes.Archive);
                }
                else
                {
                    FileAttributes attributes = RemoveAttribute(File.GetAttributes(path), FileAttributes.Archive);
                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Archive);
                }

                if (attrArray.Contains(FileAttributes.Compressed.ToString()))
                {
                    File.SetAttributes(path, FileAttributes.Compressed);
                }
                else
                {
                    FileAttributes attributes = RemoveAttribute(File.GetAttributes(path), FileAttributes.Compressed);
                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Compressed);
                }

                if (attrArray.Contains(FileAttributes.Directory.ToString()))
                {
                    File.SetAttributes(path, FileAttributes.Directory);
                }
                else
                {
                    FileAttributes attributes = RemoveAttribute(File.GetAttributes(path), FileAttributes.Directory);
                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Directory);
                }

                if (attrArray.Contains(FileAttributes.Encrypted.ToString()))
                {
                    File.SetAttributes(path, FileAttributes.Encrypted);
                }
                else
                {
                    FileAttributes attributes = RemoveAttribute(File.GetAttributes(path), FileAttributes.Encrypted);
                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Encrypted);
                }

                if (attrArray.Contains(FileAttributes.Normal.ToString()))
                {
                    File.SetAttributes(path, FileAttributes.Normal);
                }
                else
                {
                    FileAttributes attributes = RemoveAttribute(File.GetAttributes(path), FileAttributes.Normal);
                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Normal);
                }

                if (attrArray.Contains(FileAttributes.ReadOnly.ToString()))
                {
                    File.SetAttributes(path, FileAttributes.ReadOnly);
                }
                else
                {
                    FileAttributes attributes = RemoveAttribute(File.GetAttributes(path), FileAttributes.ReadOnly);
                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.ReadOnly);
                }

                if (attrArray.Contains(FileAttributes.System.ToString()))
                {
                    File.SetAttributes(path, FileAttributes.System);
                }
                else
                {
                    FileAttributes attributes = RemoveAttribute(File.GetAttributes(path), FileAttributes.System);
                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.System);
                }

                if (attrArray.Contains(FileAttributes.Temporary.ToString()))
                {
                    File.SetAttributes(path, FileAttributes.Temporary);
                }
                else
                {
                    FileAttributes attributes = RemoveAttribute(File.GetAttributes(path), FileAttributes.Temporary);
                    File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Temporary);
                }
            }
        }

        public static void CompressFile(FileInfo toCompress, string path)
        {
            using (FileStream input = toCompress.OpenRead())
            {
                FileStream output = File.Create(path + toCompress.Name + ".gz");
                GZipStream compressor = new GZipStream(output, CompressionMode.Compress);
                int b = input.ReadByte();
                while (b != -1)
                {
                    compressor.WriteByte((byte)b);
                    b = input.ReadByte();
                }
                compressor.Close();
                input.Close();
                output.Close();





            }

        }
        public static void ExtractFile(FileInfo toExtract, string path)
        {
            using  (FileStream baseFileStream = toExtract.OpenRead())
            {
                string currentFileName = toExtract.Name;
                string newFileName = Path.Combine(path, toExtract.Name.Substring(0, toExtract.Name.Length - 3));
                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(baseFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                    }
                }
            }
        }
        private static void Pump(Stream input, Stream output)
        {
            byte[] bytes = new byte[4096];
            int n;
            while ((n = input.Read(bytes, 0, bytes.Length)) != 0)
            {
                output.Write(bytes, 0, n);
            }
        }

 









    }
}
