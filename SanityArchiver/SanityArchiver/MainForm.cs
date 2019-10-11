using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace SanityArchiver
{
    public partial class SanityCommanderMainWindow : Form
    {
        int selectedLeftIndex = 0;
        int selectedRightIndex = 0;
        string leftRootString = @"C:\";
        string rightRootString = @"C:\";
        ListViewItem lastItemSelected;
        public SanityCommanderMainWindow()
        {
            InitializeComponent();
            listViewLeft.ItemActivate += new System.EventHandler(this.LeftItemSingleClick);
            listViewRight.ItemActivate += new System.EventHandler(this.RightItemSingleClick);
            listViewLeft.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ItemDoubleClick);
            listViewRight.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ItemDoubleClick);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string leftRootDirName = Path.GetFullPath(leftRootString);
            string rightRootDirName = Path.GetFullPath(rightRootString);
            DirectoryManager.PopulateListView(listViewLeft, leftRootDirName, TextLeft);
            DirectoryManager.PopulateListView(listViewRight, rightRootDirName, TextRight);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }


        private void LeftItemSingleClick(object sender, EventArgs e)
        {
            lastItemSelected = listViewLeft.SelectedItems[0];
            selectedLeftIndex = lastItemSelected.Index;
        }
        private void RightItemSingleClick(object sender, EventArgs e)
        {
            lastItemSelected = listViewRight.SelectedItems[0];
            selectedRightIndex = lastItemSelected.Index;
        }
        private void ItemDoubleClick(object sender, EventArgs e)
        {
            ListView thisListView = (ListView)sender;
            ListViewItem clickedItem = null;
            bool isLeft = false;

            if (thisListView.Name == listViewLeft.Name)
            {
                clickedItem = thisListView.Items[selectedLeftIndex];
                isLeft = true;
            }
            else if (thisListView.Name == listViewRight.Name)
            {
                clickedItem = thisListView.Items[selectedRightIndex];
            }

            if (clickedItem.SubItems[1].Text == "Directory")
            {
                string dirName = "";
                TextBox textBox = null;
                if (isLeft)
                {
                    leftRootString += clickedItem.SubItems[0].Text + "\\";
                    textBox = TextLeft;
                    dirName = leftRootString;
                }
                else if (!isLeft)
                {
                    rightRootString += clickedItem.SubItems[0].Text + "\\";
                    textBox = TextRight;
                    dirName = rightRootString;
                }
                try
                {
                    DirectoryManager.PopulateListView(thisListView, dirName, textBox);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("Not Accessable!");
                    if (isLeft)
                    {
                        leftRootString = leftRootString.Replace(clickedItem.SubItems[0].Text + @"\", "");
                    }
                    else if (!isLeft)
                    {
                        rightRootString = rightRootString.Replace(clickedItem.SubItems[0].Text + @"\", "");
                    }
                }
            }
            else if (clickedItem.SubItems[1].Text != "Directory")
            {
                string fullFileName = DirectoryManager.GetFilePath(clickedItem.SubItems[0].Text + "." + clickedItem.SubItems[1].Text);
                FileInfo thisFile = new FileInfo(fullFileName);

                System.Diagnostics.Process.Start(thisFile.FullName);
            }
        }




            
        private void CopyButton_Click(object sender, EventArgs e)
        {
            string sourceDirectory = "";
            string destinationDirectory = "";
            FileInfo toCopy = null;

            try
            {
                if (lastItemSelected.ListView.Name == "listViewLeft")
                {
                    toCopy = new FileInfo(leftRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text);
                    sourceDirectory = leftRootString;
                    destinationDirectory = rightRootString;
                }
                else if (lastItemSelected.ListView.Name == "listViewRight")
                {
                    toCopy = new FileInfo(rightRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text);
                    sourceDirectory = rightRootString;
                    destinationDirectory = leftRootString;
                }
                bool confirmed = Confirm(toCopy.Name, destinationDirectory, "Do you really want to Copy ");
                if (confirmed)
                {
                    FileManager.CopyFile(toCopy.Name, sourceDirectory, destinationDirectory);
                    Form1_Load(sender, e);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a file to Copy!");
            }
        }
        private void MoveButton_Click(object sender, EventArgs e)
        {
            string sourceDirectory = "";
            string destinationDirectory = "";
            FileInfo toMove = null;
            try
            {
                if (lastItemSelected.ListView.Name == "listViewLeft")
                {
                    toMove = new FileInfo(leftRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text);
                    sourceDirectory = leftRootString;
                    destinationDirectory = rightRootString;
                }
                else if (lastItemSelected.ListView.Name == "listViewRight")
                {
                    toMove = new FileInfo(leftRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text);
                    sourceDirectory = rightRootString;
                    destinationDirectory = leftRootString;
                }
                bool confirmed = Confirm(toMove.Name, destinationDirectory, "Do you really want to Move ");
                if (confirmed)
                {
                    FileManager.MoveFile(toMove.Name, sourceDirectory, destinationDirectory);
                    Form1_Load(sender, e);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a file to Move!");
            }
        }
        private void CompressButton_Click(object sender, EventArgs e)
        {
            string destinationDirectory = "";
            FileInfo toCompress = null;
            try
            {
                if (lastItemSelected.ListView.Name == "listViewLeft")
                {
                    toCompress = new FileInfo(leftRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text);
                    destinationDirectory = leftRootString;
                }
                else if (lastItemSelected.ListView.Name == "listViewRight")
                {
                    toCompress = new FileInfo(rightRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text);
                    destinationDirectory = rightRootString;
                }
                bool confirmed = Confirm(toCompress.Name, destinationDirectory, "Do you really want to Compress ");
                if (confirmed)
                {
                    FileManager.CompressFile(toCompress, destinationDirectory);
                    Form1_Load(sender, e);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a file to Compress!");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("file is not found!");
            }
        }
        private void ExtractButton_Click(object sender, EventArgs e)
        {
            string destinationDirectory = "";
            FileInfo toExtract = null;
            try
            {
                if (lastItemSelected.ListView.Name == "listViewLeft")
                {
                    toExtract = new FileInfo(leftRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text);
                    destinationDirectory = leftRootString;
                }
                else if (lastItemSelected.ListView.Name == "listViewRight")
                {
                    toExtract = new FileInfo(rightRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text);
                    destinationDirectory = rightRootString;
                }
                bool confirmed = Confirm(toExtract.Name, destinationDirectory, "Do you really want to Extract ");
                if (confirmed)
                {
                    FileManager.ExtractFile(toExtract, destinationDirectory);
                    Form1_Load(sender, e);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a file to Compress!");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("file is not found!");
            }
        }
        private void ExtractToButton_Click(object sender, EventArgs e)
        {
            string destinationDirectory = "";
            FileInfo toExtract = null;
            try
            {
                if (lastItemSelected.ListView.Name == "listViewLeft")
                {
                    toExtract = new FileInfo(leftRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text);
                    destinationDirectory = rightRootString;
                }
                else if (lastItemSelected.ListView.Name == "listViewRight")
                {
                    toExtract = new FileInfo(rightRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text);
                    destinationDirectory = leftRootString;
                }
                bool confirmed = Confirm(toExtract.Name, destinationDirectory, "Do you really want to Extract ");
                if (confirmed)
                {
                    FileManager.ExtractFile(toExtract, destinationDirectory);
                    Form1_Load(sender, e);
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a file to Compress!");
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("file is not found!");
            }        }
        private void EncriptButton_Click(object sender, EventArgs e)
        {
            try
            {
                string FileName = lastItemSelected.ListView.Name == "listViewLeft" ? 
                               leftRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text :
                               rightRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text;
               
                FileManager.AddEncryption(FileName);
                Form1_Load(sender, e);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a file to Encrypt!");
            }
            catch (IOException)
            {
                MessageBox.Show("Your Windows license have no access to this function");
            }
        }
        private void DecryptButton_Click(object sender, EventArgs e)
        {
            try
            {
                string FileName = lastItemSelected.ListView.Name == "listViewLeft" ?
                               leftRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text :
                               rightRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text;

                FileManager.RemoveEncryption(FileName);
                Form1_Load(sender, e);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a file to Decrypt!");
            }
            catch (IOException)
            {
                MessageBox.Show("Your Windows license have no access to this function");
            }
        }
        private void AttributesButton_Click(object sender, EventArgs e)
        {
            try
            {
                string FileName = lastItemSelected.ListView.Name == "listViewLeft" ?
                               leftRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text :
                               rightRootString + lastItemSelected.SubItems[0].Text + "." + lastItemSelected.SubItems[1].Text;

                AttributsForm newForm = new AttributsForm(FileName);
                newForm.Show();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Please select a file to see its' attributes!");
            }
        }




        private void TextLeft_Click(object sender, EventArgs e)
        {
            ///not yet implemented  
        }   
        private void TextRight_Click(object sender, EventArgs e)
        {
            ///not yet implemented
        }



        private void BackButton1_Click(object sender, EventArgs e)
        {
            leftRootString = GetParentDirectoryString(leftRootString);
            DirectoryManager.PopulateListView(listViewLeft, leftRootString, TextLeft);
        }
        private void BackButton2_Click(object sender, EventArgs e)
        {
            rightRootString = GetParentDirectoryString(rightRootString);
            DirectoryManager.PopulateListView(listViewRight, rightRootString, TextRight);
        }




        private bool Confirm(String file, String path, String message)
        {
            if (MessageBox.Show(message + file + "\r\nto:\r\n" +
                    path + "?", "Yes", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                return true;
            }
            return false;
        }
        private string GetParentDirectoryString(string path)
        {
            string sub = path.Substring(0, path.Length - 1);
            if (Directory.GetParent(sub).Exists && sub.Length > 4)
            {
                path = Directory.GetParent(sub).ToString();
                if (path.Length > 4) path = path + @"\";
                return path;
            }
            else
            {
                return path;
            }
        }
    }
}
