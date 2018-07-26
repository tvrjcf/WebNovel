using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WebBookManage.Common;
using WebBookManage.Model;

namespace WebBookManage
{
    public partial class frmDownMenuList : Form
    {
        private BookHelper BH = new BookHelper();
        private Novel novel;
        private string novelId;
        private List<NovelContent> menulist;
        private bool IsSave = false;
        public frmDownMenuList(string _novelId, List<NovelContent> _list)
        {
            InitializeComponent();
            DoubleBufferDataGridView.DoubleBufferedDataGirdView(dvList, true);
            novelId = _novelId;
            menulist = _list;
        }

        private void frmDownMenuList_Load(object sender, EventArgs e)
        {
            this.dvList.AutoGenerateColumns = false;
            BindData();
        }

        private void BindData()
        {
            //novel = BH.GetNovel(novelId);
            dvList.DataSource = menulist;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (menulist.Count == 0 || IsSave) return;
            try
            {
                if (BH.SaveNovelContents(menulist))
                {
                    MessageBox.Show("目录已保存", "提示");
                    IsSave = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误");
            }
        }



        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDownContent_Click(object sender, EventArgs e)
        {
            if (!IsSave)
            {
                if (BH.SaveNovelContents(menulist))
                    IsSave = true;
            }
            var novel = BH.GetNovel(novelId);
            if (novel != null)
            {
                var dt = BH.GetNovelContents(novelId);
                new frmDownContent(novel, dt.ToList<NovelContent>()).ShowDialog();
            }
        }
    }
}
