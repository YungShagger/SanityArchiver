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
        int selectedRightIndex = 0;
        int selectedLeftIndex = 0;
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
            ///not yet implemented
        }
        private void MoveButton_Click(object sender, EventArgs e)
        {
            ///not yet implemented
        }
        private void CompressButton_Click(object sender, EventArgs e)
        {
            ///not yet implemented
        }
        private void ExtractButton_Click(object sender, EventArgs e)
        {
            ///not yet implemented
        }
        private void EncriptButton_Click(object sender, EventArgs e)
        {
            ///not yet implemented
        }
        private void DecryptButton_Click(object sender, EventArgs e)
        {
            ///not yet implemented
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
    }
}
