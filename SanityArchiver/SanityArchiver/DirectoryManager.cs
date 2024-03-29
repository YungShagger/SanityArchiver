﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace SanityArchiver
{
    class DirectoryManager
    {
        private static DirectoryInfo[] Directories;
        private static FileInfo[] Files;

        private static string[] TextFiles = { "txt", "doc", "rtf", "log" };
        private static string[] MusicFiles = { "mp3", "waw" };
        private static string[] Videos = { "avi", "mp4", "mkv", "mpeg" };
        private static string[] CompFiles = { "zip", "gz", "rar" };
        private static string[] RunFiles = { "exe", "bat" };
        private static string[] Settings = { "ini", "config" };


        private static void FetchDirContents(String rootDirName)
        {
            var rootDir = new DirectoryInfo(rootDirName.Length >= 4 ? rootDirName : @"C:\");
            if (rootDir.Exists)
            {
                Directories = rootDir.GetDirectories();
                Files = rootDir.GetFiles();
            }
            else
            {
                FetchDirContents(Directory.GetParent(rootDirName).FullName);
            }
        }
        public static void PopulateListView(ListView listView, String rootDirName, TextBox textBox)
        {
            FetchDirContents(rootDirName);
            AddFilesToListView(listView, rootDirName, textBox);
        }

        private static void AddFilesToListView(ListView listView, String rootDirName, TextBox textBox)
        {
            listView.Clear();
            if (textBox != null)
            {
                textBox.Text = rootDirName;
            }
            listView.View = View.Details;
            listView.Columns.Add("Name");
            listView.Columns.Add("Type");
            listView.Columns.Add("Last Modified");
            listView.Columns.Add("Size");

            foreach (DirectoryInfo dir in Directories)
            {
                ListViewItem newItem = new ListViewItem(new string[]
                { dir.Name, "Directory", dir.LastWriteTime.ToString(), "" });
                newItem.ImageIndex = 2;
                listView.Items.Add(newItem);
            }
            foreach (FileInfo currentFile in Files)
            {
                string[] fileNameArray = currentFile.Name.Split('.');

                string name = fileNameArray[0];
                for (int i = 1; i < fileNameArray.Length - 1; i++)
                {
                    name += "." + fileNameArray[i];
                }
                string type = fileNameArray[fileNameArray.Length - 1];
                ListViewItem newItem = new ListViewItem(new string[]
                    { name, type, currentFile.LastWriteTime.ToString(), Math.Round(currentFile.Length*0.000001024, 5).ToString()});
                FileAttributes attributes = File.GetAttributes(currentFile.FullName);
                if ((attributes & FileAttributes.Encrypted) == FileAttributes.Encrypted)
                {
                    newItem.ImageIndex = 5;
                }
                else if (Settings.Contains(type.ToLower()))
                {
                    newItem.ImageIndex = 4;
                }
                else if (RunFiles.Contains(type.ToLower()))
                {
                    newItem.ImageIndex = 3;
                }
                else if (CompFiles.Contains(type.ToLower()))
                {
                    newItem.ImageIndex = 6;
                }
                else if (TextFiles.Contains(type.ToLower()))
                {
                    newItem.ImageIndex = 0;
                }
                else if (Videos.Contains(type.ToLower()))
                {
                    newItem.ImageIndex = 7;
                }
                else if (MusicFiles.Contains(type.ToLower()))
                {
                    newItem.ImageIndex = 8;
                }
                else
                {
                    newItem.ImageIndex = 1;
                }
                listView.Items.Add(newItem);
            }
            listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
        }
       
        public static List<string> GetFiles(string path, string regex)
        {
            var files = new List<string>();
            try
            {
                files.AddRange(Directory.GetFiles(path, regex, SearchOption.TopDirectoryOnly));
                foreach (var directory in Directory.GetDirectories(path))
                    files.AddRange(GetFiles(directory, regex));
            }
            catch (UnauthorizedAccessException) { }

            return files;
        }
        public static string GetFilePath(String FileName)
        {
            foreach (FileInfo currentFile in Files)
            {
                if (currentFile.Name == FileName) return currentFile.FullName;
            }
            return "";
        }
        public static string GetDirPath(String DirName)
        {
            DirectoryInfo dir = new DirectoryInfo(DirName);
            if (dir.Exists)
            {
                return dir.FullName;
            }
            return "";
        }
        public static double GetDirSize(DirectoryInfo dir)
        {
            string rootDir = dir.FullName;

            long dirSize = 0;
            var FilePaths = GetFiles(rootDir, "*");
            var files = (from f in FilePaths where !Directory.Exists(f) select new FileInfo(f)).ToArray();
            foreach (FileInfo f in files)
            {
                dirSize += f.Length;
            }
            return Math.Round(dirSize * 0.000001024, 5);
        }
    }
}
