using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WebBookManage.Common;
using WebBookManage.Model;

namespace WebBookManage
{
    public partial class frmSetting : Form
    {
        private BookHelper BH = new BookHelper();
        private string novelId = string.Empty;
        public delegate void RefreshUI(object o);
        public frmSetting()
        {
            InitializeComponent();
        }
        public frmSetting(string _novelId)
        {
            InitializeComponent();
            novelId = _novelId;
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            LoadThemeModel();
        }

        private void LoadThemeModel()
        {
            cbbTheme.Items.Clear();
            var listThemeModel = new List<ThemeModel>();
            DirectoryInfo dir = new DirectoryInfo(@"usermodel");
            foreach (var item in dir.GetDirectories())
            {
                //var themeModel = new ThemeModel()
                //{
                //    ThemeName = item.Name,
                //    ListModelPath = string.Format(@"usermodel\{0}\listmodel.htm", item.Name),
                //    ChapterModelPath = string.Format(@"usermodel\{0}\chaptermodel.htm", item.Name),
                //};
                //listThemeModel.Add(themeModel);
                cbbTheme.Items.Add(item.Name);
            }
            //cbbTheme.DataSource = listThemeModel;
            //cbbTheme.DisplayMember = "ThemeName";
            //cbbTheme.ValueMember = "ListModelPath";

        }

        private void cbbTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbbTheme.Text != null)
            {
                SysSetting.GetInstance().ThemeModel = new ThemeModel(cbbTheme.Text);

            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                var novel = BH.GetNovel(novelId);
                var menuList = BH.GetNovelContents(novelId).ToList<NovelContent>();

                var theme = SysSetting.GetInstance().ThemeModel;
                var dirInfo = new DirectoryInfo(string.Format(BookHelper.DIR_PATH_NOVEL, novel.NovelID));
                var fileInfos = dirInfo.GetFiles();
                var listModelContent = CommonHelper.LoadHtmlFile(theme.ListModelPath);
                var chapterModelContent = CommonHelper.LoadHtmlFile(theme.ChapterModelPath);
                int i = 0;
                btnAccept.Enabled = false;
                progressBar1.Minimum = 0;
                progressBar1.Maximum = fileInfos.Length;
                progressBar1.Visible = true;


                var _ManualEvents = new MutipleThreadResetEvent(fileInfos.Length);
                ThreadPool.SetMinThreads(1, 1);
                ThreadPool.SetMaxThreads(5, 10);

                foreach (var fileInfo in fileInfos)
                {
                    if (fileInfo.Name.Contains("List.htm"))
                    {
                        ThreadPool.QueueUserWorkItem(state=> BH.SaveNovelListToHtml(novel, menuList, listModelContent));
                        _ManualEvents.SetOne();
                        //BH.SaveNovelListToHtml(novel, menuList, listModelContent);
                    }
                    else
                    {
                        ThreadPool.QueueUserWorkItem(state => ChangeModel(fileInfo, chapterModelContent, _ManualEvents));
                        //BH.ChangeThemeModel(fileInfo, chapterModelContent);
                    }
                    i++;
                    //this.Invoke(new RefreshUI(refreshUI), i);
                    //this.Invoke(new Action<int>((value) =>
                    //{
                    //    progressBar1.Value = value;
                    //    if (progressBar1.Maximum == value)
                    //    {
                    //        progressBar1.Visible = false;
                    //        btnAccept.Enabled = true;
                    //    }
                    //}), i);
                }
                //_ManualEvents.WaitAll();
                //MessageBox.Show("应用完成！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                progressBar1.Visible = false;
                btnAccept.Enabled = true;
            }


        }
        public void refreshUI(Object o)
        {
            var value = (int)o;
            progressBar1.Value = value;
            if (progressBar1.Maximum == value)
            {
                Thread.Sleep(2000);
                progressBar1.Visible = false;
                btnAccept.Enabled = true;
            }
        }

        public void ChangeModel(FileInfo fileInfo, string modelContent, MutipleThreadResetEvent e) {
            BH.ChangeThemeModel(fileInfo, modelContent, null);
            e.SetOne();
            this.Invoke(new RefreshUI(refreshUI), e.index);
        }
    }
}