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

            if (clickedItem.SubItems[1].Text == "DIR")
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
            else if (clickedItem.SubItems[1].Text != "DIR")
            {
                string fullFileName = DirectoryManager.GetFilePath(clickedItem.SubItems[0].Text + "." + clickedItem.SubItems[1].Text);
                FileInfo thisFile = new FileInfo(fullFileName);

                System.Diagnostics.Process.Start(thisFile.FullName);
            }
        }




        private void label1_Click(object sender, EventArgs e)
        {
            string leftRootDirName = Path.GetFullPath(leftRootString);
            string rightRootDirName = Path.GetFullPath(rightRootString);
            DirectoryManager.PopulateListView(listViewLeft, leftRootDirName, TextLeft);
            DirectoryManager.PopulateListView(listViewRight, rightRootDirName, TextRight);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void maskedTextBox1_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void BackButton2_Click(object sender, EventArgs e)
        {

        }
    }
}
