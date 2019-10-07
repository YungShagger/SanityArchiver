using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SanityArchiver;

namespace SanityArchiver
{
    public partial class AttributsForm : Form
    {
        public AttributsForm(string path)
        {
            InitializeComponent();
            FillAttributesList(path);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void FillAttributesList(string path)
        {
            string[] attributeArray = FileManager.GetAttributes(path);
            pathLabel.Text = path;
            for (int i = 0; i < AtributbeCheckList.Items.Count; i++)
            {
                if (attributeArray.Contains(AtributbeCheckList.Items[i].ToString()))
                {
                    AtributbeCheckList.SetItemChecked(i, true);
                }
            }
        }
        private void AcceptButton_Click(object sender, EventArgs e)
        {
            string[] attrArray = new string[AtributbeCheckList.CheckedItems.Count];
            string path = pathLabel.Text;

            for (int i = 0; i < attrArray.Length; i++)
            {
                attrArray[i] = AtributbeCheckList.CheckedItems[i].ToString();
            }
            FileManager.SetAttributes();
            MessageBox.Show("Attributes changed!");
            this.Close();
        }
        private void AtributbeCheckList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
