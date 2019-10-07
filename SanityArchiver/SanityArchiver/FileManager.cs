﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;


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
        public static void SetAttributes()
        {
            ///not yet implemented
        }

        public static void CompressFile(FileInfo fileToArchive, string path)
        {
            ///not yet implemented
        }

        public static void ExtractFile(FileInfo file, string path)
        {
            ///not yet implemented
        }
    }
}