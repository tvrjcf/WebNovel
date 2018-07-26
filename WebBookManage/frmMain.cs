using CYQ.Data.Table;
using System;
using System.Windows.Forms;
using WebBookManage.Common;
using WebBookManage.Model;

namespace WebBookManage
{
    public partial class frmMain : Form
    {
        private BookHelper BH = new BookHelper();
        private MDataTable dt;//和表格同一引用，用于批量操作表格内容。
        public frmMain()
        {
            InitializeComponent();
            DoubleBufferDataGridView.DoubleBufferedDataGirdView(dgvList, true);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public void LoadData()
        {
            dt = BH.GetNovels();
            dt.Bind(dgvList);
        }

        public void RefreshNewRow(MDataRow data)
        {
            dt.Rows.Add(data);
            dgvList.Refresh();
            var cell = dgvList.Rows[dgvList.Rows.Count - 1].Cells[0];
            dgvList.CurrentCell = cell;
        }
        public void RefreshEditRow(MDataRow data)
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                var novelId = dgvList.SelectedRows[0].Cells["NovelID"].Value.ToString();
                //var row = dt.Rows.FirstOrDefault(p => p.Get<string>("NovelID") == novelId);
                var row = dt.FindRow(string.Format("NovelID = '{0}'", novelId));

                row.LoadFrom(data);
                dgvList.Refresh();
                //dgNovel.SelectedRows[0].SetValues(data.ItemArray);
                //data.SetToEntity(row);
            }
        }

        public string SelectPrev()
        {
            try
            {
                if (dgvList.SelectedRows.Count > 0)
                {
                    var selectIndex = dgvList.SelectedRows[0].Index;
                    if (selectIndex >= 1)
                    {
                        var cell = dgvList.Rows[selectIndex - 1].Cells[0];
                        if (cell.Value != null)
                        {
                            dgvList.CurrentCell = cell;
                            var novelId = dgvList.SelectedRows[0].Cells["NovelID"].Value.ToString();
                            return novelId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
            return "0";
        }

        public string SelectNext()
        {
            try
            {

                if (dgvList.SelectedRows.Count > 0)
                {
                    var selectIndex = dgvList.SelectedRows[0].Index;
                    if (selectIndex < dgvList.Rows.Count - 1)
                    {
                        var cell = dgvList.Rows[selectIndex + 1].Cells[0];
                        if (cell.Value != null)
                        {
                            dgvList.CurrentCell = cell;
                            var novelId = dgvList.SelectedRows[0].Cells["NovelID"].Value.ToString();
                            return novelId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "0";
            }
            return "0";
        }

        void AddNovel()
        {
            new frmNovelEdit(this, "0").ShowDialog();
        }
        void UpdateNovel()
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                var novelId = dgvList.SelectedRows[0].Cells["NovelID"].Value.ToString();
                new frmNovelEdit(this, novelId).ShowDialog();
            }
        }
        void DeleteNovel()
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                var novelId = dgvList.SelectedRows[0].Cells["NovelID"].Value.ToString();
                var novelName = dgvList.SelectedRows[0].Cells["NovelName"].Value.ToString();

                if (MessageBox.Show("确定要删除 [" + novelId + "][" + novelName + "] 吗?", "确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (BH.DelNovelContents(novelId))
                    {
                        if (MessageBox.Show("删除目录 [" + novelId + "][" + novelName + "] 成功!\n是否同时删除书籍?", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (BH.DelNovel(novelId))
                            {
                                dgvList.Rows.Remove(dgvList.SelectedRows[0]);
                            }
                        };

                    }
                };

            }
        }

        void DownNovel()
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                var novelId = dgvList.SelectedRows[0].Cells["NovelID"].Value.ToString();
                var result = MessageBox.Show("确认要进行章节目录同步吗？\n\n【是】同步目录\t【否】下载内容", "确认", MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    var list = BH.DownMenuList(novelId);
                    //MessageBox.Show(string.Format("总共:\t{0}\n已过滤:\t{1}\n已存在:\t{2}\n待更新:\t{3}", totalCount, filterCount, listExist.Count, list.Count), "获取结果");
                    if (list.Count > 0)
                    {
                        new frmDownMenuList(novelId, list).ShowDialog();
                        return;
                    }
                    else
                    {
                        MessageBox.Show(string.Format("没有要更新的内容"), "获取结果");
                    }
                }
                else if (result == DialogResult.No)
                {
                }
                else
                {
                    return;
                }

                var novel = BH.GetNovel(novelId);
                if (novel != null)
                {
                    var dt = BH.GetNovelContents(novelId);
                    new frmDownContent(novel, dt.ToList<NovelContent>()).ShowDialog();
                }
            }
        }

        private void CreateEBook()
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                var novelId = dgvList.SelectedRows[0].Cells["NovelID"].Value.ToString();
                new frmCreateEBook(novelId).ShowDialog();

            }
        }
        private void OpenReader()
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                var novelId = dgvList.SelectedRows[0].Cells["NovelID"].Value.ToString();
                new frmWebReader(novelId).ShowDialog();

            }
        }

        private void Setting()
        {
            if (dgvList.SelectedRows.Count > 0)
            {
                var novelId = dgvList.SelectedRows[0].Cells["NovelID"].Value.ToString();
                new frmSetting(novelId).ShowDialog();

            }
        }

        private void tsmi添加_Click(object sender, EventArgs e)
        {
            AddNovel();
        }
        private void tsmi修改_Click(object sender, EventArgs e)
        {
            UpdateNovel();
        }

        private void tsmi删除_Click(object sender, EventArgs e)
        {
            DeleteNovel();
        }
        private void tsmi刷新_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void tsbtnAdd_Click(object sender, EventArgs e)
        {
            AddNovel();
        }

        private void tsbtnEdit_Click(object sender, EventArgs e)
        {
            UpdateNovel();
        }

        private void tsbtnDelete_Click(object sender, EventArgs e)
        {
            DeleteNovel();
        }
        private void tsbtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void tsbtnDown_Click(object sender, EventArgs e)
        {
            DownNovel();
        }

        private void tsbtnBook_Click(object sender, EventArgs e)
        {
            CreateEBook();
        }

        private void dgvList_DoubleClick(object sender, EventArgs e)
        {
            OpenReader();
        }

        private void tsbtnSetting_Click(object sender, EventArgs e)
        {
            Setting();
        }

    }
}
