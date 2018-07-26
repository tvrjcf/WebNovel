using CYQ.Data;
using CYQ.Data.Table;
using System;
using System.Windows.Forms;
using WebBookManage.Common;

namespace WebBookManage
{
    public partial class frmNovelEdit : Form
    {
        private BookHelper BH = new BookHelper();
        private MDataTable dt;//和表格同一引用，用于批量操作表格内容。
        public string NovelId = "0";
        public frmMain frmMain = null;
        public frmNovelEdit(frmMain form = null, string novelId = "0")
        {
            InitializeComponent();
            NovelId = novelId;
            if (form != null)
                frmMain = form;
        }

        private void frmNovelEdit_Load(object sender, EventArgs e)
        {
            BindData();
            LoadData(NovelId);
        }

        private void BindData()
        {
            //ddlLB.ValueMember = "DM";
            //ddlLB.DisplayMember = "MC";
            //BH.GetNovelTypes().Bind(ddlLB);
            using (MAction action = new MAction("dic_noveltype"))
            {
                action.UI.Bind(ddlLB, "", "MC", "DM");
                //action.Select().Bind(ddlLB);
                //var list = action.Select().ToDataTable();//.ToList<NovelType>();
                //ddlLB.DataSource = list;
            }
        }
        private void LoadData(string novelId)
        {
            if (!string.IsNullOrEmpty(novelId) && novelId != "0")
            {
                //Console.WriteLine(ddlLB.GetType().Name);
                using (MAction action = new MAction("book_novel"))
                {
                    if (action.Fill(novelId))
                    {
                        action.UI.SetToAll(this);
                        //var lb = action.Get<string>("LB") ?? "01";
                        //ddlLB.SelectedValue = lb;
                        NovelId = novelId;
                    }
                }
            }
        }

        private void DownMenuList()
        {
            var list = BH.DownMenuList(NovelId);
            //MessageBox.Show(string.Format("总共:\t{0}\n已过滤:\t{1}\n已存在:\t{2}\n待更新:\t{3}", totalCount, filterCount, listExist.Count, list.Count), "获取结果");
            if (list.Count > 0)
            {
                new frmDownMenuList(NovelId, list).ShowDialog();
            }
            else
            {
                MessageBox.Show(string.Format("没有要更新的内容"), "获取结果");
            }
        }


        private void btnOk_Click(object sender, EventArgs e)
        {
            if (NovelId == "0")
            {
                int maxid = BH.GetNovelMaxId();
                if (maxid > 0)
                {
                    using (MAction action = new MAction("book_novel"))
                    {
                        action.UI.SetAutoParentControl(this);
                        var newId = maxid.ToString().PadLeft(6, '0');
                        action.Set("NovelID", newId);
                        if (action.Insert(true, InsertOp.ID))
                        {
                            action.UI.SetToAll(this);
                            NovelId = action.Get<string>("NovelID");
                            var novelName = action.Get<string>("NovelName");
                            MessageBox.Show("添加 [" + NovelId + "][" + novelName + "] 成功! ", "提示");
                            if (frmMain != null)
                                frmMain.RefreshEditRow(action.Data);
                        }
                        LoadData(NovelId);
                    }
                }

            }
            else
            {
                using (MAction action = new MAction("book_novel"))
                {
                    action.UI.SetAutoParentControl(this);
                    //action.Set("NovelID", NovelId);
                    //action.Set("LB", ddlLB.SelectedValue);
                    if (action.Update(NovelId, true))
                    {
                        MessageBox.Show("修改成功!", "提示");

                        if (frmMain != null)
                            frmMain.RefreshEditRow(action.Data);
                        //this.Close();
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDownList_Click(object sender, EventArgs e)
        {
            DownMenuList();
        }

        private void btnPrevi_Click(object sender, EventArgs e)
        {
            if (frmMain != null)
            {
                var nid = frmMain.SelectPrev();
                if (nid != "" && nid != "0")
                    LoadData(nid);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (frmMain != null)
            {
                var nid = frmMain.SelectNext();
                if (nid != "" && nid != "0")
                    LoadData(nid);
            }
        }

        private void btnChangeMode_Click(object sender, EventArgs e)
        {
            var listUrl = txtListUrl.Text;
            if (string.IsNullOrEmpty(listUrl))
            {
                MessageBox.Show("请填写目录页网址! ", "提示");
                return;
            }
            if (BH.ChangeHttpMode(NovelId))
            {
                LoadData(NovelId);
                MessageBox.Show("已切换完成! ", "提示");
            }

        }

        private void btnSiteSign_Click(object sender, EventArgs e)
        {

        }

        private void btnGetSiteSign_Click(object sender, EventArgs e)
        {
            var sign = BH.GetSiteSign(txtListUrl.Text);
            if (sign != null)
            {
                txtContentStart.Text = sign.ContentStart;
                txtContentEnd.Text = sign.ContentEnd;
                txtListStart.Text = sign.ListStart;
                txtListEnd.Text = sign.ListEnd;
                txtVolumeStart.Text = sign.VolumeStart;
                txtVolumeEnd.Text = sign.VolumeEnd;
                txtNeedDelStr.Text = sign.NeedDelStr;
            }
        }

        private void btnSaveToSiteSign_Click(object sender, EventArgs e)
        {
            string rootUrl = CommonHelper.GetWebRootUrl(txtListUrl.Text);
            var sign = BH.GetSiteSign(rootUrl);
            if (sign == null)
            {
                sign = new Model.SiteSign()
                {
                    name = rootUrl,
                    url = rootUrl,
                    ListStart = txtListStart.Text,
                    ListEnd = txtListEnd.Text,
                    ContentStart = txtContentStart.Text,
                    ContentEnd = txtContentEnd.Text,
                    NeedDelStr = txtNeedDelStr.Text,
                    VolumeStart = txtVolumeStart.Text,
                    VolumeEnd = txtVolumeEnd.Text,
                    BriefUrlStart = "",
                    BriefUrlEnd = "",
                    AuthorStart = "",
                    AuthorEnd = "",
                    BriefStart = "",
                    BriefEnd = "",
                    BookImgUrlStart = "",
                    BookImgUrlEnd = ""
                };
            }
            BH.ToXmlNode(sign);
        }
    }
}
