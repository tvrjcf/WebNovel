using System;
using System.Windows.Forms;
using WebBookManage.Common;

namespace WebBookManage
{
    public partial class frmCreateEBook : Form
    {
        private BookHelper BH = new BookHelper();
        private string NovelID;
        public frmCreateEBook(string _novelId)
        {
            InitializeComponent();
            NovelID = _novelId;
        }

        private void frmCreateEBook_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            var ebookType = EbookType.TXT;
            if (rbtnTxt.Checked) { ebookType = EbookType.TXT; }
            else if (rbtnChm.Checked) { ebookType = EbookType.CHM; }
            else if (rbtnDoc.Checked) { ebookType = EbookType.DOC; }
            else if (rbtnJar.Checked) { ebookType = EbookType.JAR; }
            else if (rbtnUmd.Checked) { ebookType = EbookType.UMD; }
            else if (rbtnPdf.Checked) { ebookType = EbookType.PDF; }

            string file = BH.CreateEBook(NovelID, ebookType);
            if (file.Length > 0)
            {
                MessageBox.Show("制作电子书已完成!\n" + file, "提示");
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSetting_Click(object sender, EventArgs e)
        {

        }
    }
}
