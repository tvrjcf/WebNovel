using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WebBookManage.Common;

namespace WebBookManage
{
    public partial class frmWebReader : Form
    {
        private BookHelper BH = new BookHelper();
        private string fileName = "";
        public frmWebReader(string _fileName)
        {
            InitializeComponent();
            this.fileName = _fileName;
        }

        private void frmWebReader_Load(object sender, EventArgs e)
        {
            var html = CommonHelper.LoadHtmlFile(@"chm\000046\list.htm");
            wbReader.DocumentText = html;
        }
    }
}
