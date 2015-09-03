﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WitcherScriptMerger.Forms
{
    public partial class RenameFileForm : Form
    {
        public string FilePath
        {
            get
            {
                string path = lblFilePath.Text;
                int nameStart = path.LastIndexOf('\\') + 1;
                path = path.Substring(0, nameStart) + txtNewName.Text + ".ws";
                return path;
            }
        }

        public RenameFileForm(string filePath)
        {
            InitializeComponent();

            lblFilePath.Text = filePath;
            txtNewName.Text = Path.GetFileNameWithoutExtension(filePath);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
