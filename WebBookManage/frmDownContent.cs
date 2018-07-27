using CYQ.Data.Table;
using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Taskbar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using WebBookManage.Common;
using WebBookManage.Model;

namespace WebBookManage
{
    public partial class frmDownContent : Form
    {
        #region 属性定义
        private BookHelper BH = new BookHelper();
        //private List<Thread> threadList;
        //新建ManualResetEvent对象并且初始化为无信号状态
        //ManualResetEvent eventX = new ManualResetEvent(false);
        private Novel novel;
        private List<NovelContent> menulist;    //章节列表
        private List<NovelContent> successlist;    //已下载列表
        private List<NovelContent> waitinglist;     //待下载列表
        //private int SuccessCount = 0;
        //private int ErrorCount = 0;
        private int CompletedCount = 0;
        private object lockObj = new object();
        //private bool IsGetCompleted = false;    //全部待下列表已取出
        private bool IsDownCompleted = true;   //全部列表已下载完成
        private bool IsBreak = false;  //停止标识
        private bool IsClose = false;   //关闭窗体
        private bool SkipExist = true;  //自动跳过已下载过章节
        private string chaptermodel = string.Empty;
        private string listmodel = string.Empty;
        #endregion
        private TaskbarManager taskbar = TaskbarManager.Instance;

        #region 窗体加载事件

        public frmDownContent(Novel _novel, List<NovelContent> _list)
        {
            InitializeComponent();
            DoubleBufferDataGridView.DoubleBufferedDataGirdView(dvList, true);
            novel = _novel;
            menulist = _list;
            //downLoadlist = _list.ToList();
        }
        private void frmDownContent_Load(object sender, EventArgs e)
        {
            taskbar.SetProgressState(TaskbarProgressBarState.Indeterminate);
            this.dvList.AutoGenerateColumns = false;
            BindData();
        }
        #endregion

        #region 数据绑定

        private void BindData()
        {
            var BList = new BindingList<NovelContent>(menulist);
            dvList.DataSource = BList;
            dvList.ClearSelection();
        }
        #endregion

        #region 下载相关方法
        /// <summary>
        /// 获取待下载章节
        /// </summary>
        /// <returns></returns>
        private NovelContent GetDownLoadReady()
        {
            lock (lockObj)
            {
                NovelContent currMenu = null;
                if (waitinglist.Count > 0)
                {
                    currMenu = waitinglist[0];
                    waitinglist.RemoveAt(0);
                }
                else
                {
                    //IsGetCompleted = true;
                    currMenu = null;
                }
                Console.WriteLine(string.Format("取章节-> {0}-{1}", currMenu?.Id, currMenu?.Title));
                return currMenu;
            }
        }

        /// <summary>
        /// 下载执行方法
        /// </summary>
        /// <param name="o"></param>
        private void DownLoad(object o)
        {
            var menu = o as NovelContent;
            try
            {
                if (IsBreak)
                {
                    //eventX.Set();
                    Console.WriteLine(string.Format("下载中止-> {0}//{1} - <<{2}>>", menulist.Count, CompletedCount, menu.Title));
                }
                else
                {
                    var prior = BH.GetPrior(menulist, menu.Id);
                    var next = BH.GetNext(menulist, menu.Id);
                    var result = BH.SaveNovelContentToHtml(novel, ref menu, chaptermodel, prior, next, SkipExist);
                    Console.WriteLine(result);
                    lock (lockObj)
                    {
                        successlist.Add(menu);
                    }
                    this.Invoke(new EventHandler(delegate { UpdateResult(menu.Id, string.Format("已下载 {0}", menu.Title, Thread.CurrentThread.Name)); }));
                    //this.Invoke(new Action<int, string>(UpdateResult), menu.Id, string.Format("已下载 {0}", menu.Title, Thread.CurrentThread.Name));

                    //Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("下载异常-> <<{0}>> -> {1}", menu.Title, ex.Message));
                //this.Invoke(new Action<int, string>(UpdateResult), menu.Id, string.Format("异常 {0}", ex.Message));
            }
            finally
            {
                Interlocked.Increment(ref CompletedCount);
                CompleteEvent(menu);
            }
        }

        /// <summary>
        /// 下载完成事件
        /// </summary>
        /// <param name="menu"></param>
        private void CompleteEvent(NovelContent menu)
        {
            if (!IsDownCompleted && (waitinglist.Count == CompletedCount))
            {
                IsDownCompleted = true;
                Console.WriteLine(string.Format("下载完成-> {0}//{1}", waitinglist.Count, CompletedCount));

                if (successlist.Count > 0)
                {
                    //更新已下载的章节
                    var dt = MDataTable.CreateFrom(successlist);
                    dt.TableName = successlist[0].TableName;
                    dt.AcceptChanges(AcceptOp.Update);
                    Console.WriteLine(string.Format("更新保存-> {0}//{1}", successlist.Count, CompletedCount));
                    this.Invoke(new EventHandler(delegate { UpdateResult(menu.Id, string.Format("更新保存 {0}", menu.Title, Thread.CurrentThread.Name)); }));
                    //this.Invoke(new Action<int, string>(UpdateResult), menu.Id, string.Format("更新保存 {0}", menu.Title, Thread.CurrentThread.Name));
                    //eventX.Set();
                }

                //MessageBox.Show(arg2, "提示");
            }

        }

        private void StartDownLoad(List<NovelContent> list)
        {
            if (list.Count == 0)
            {
                MessageBox.Show("没有要下载的章节", "提示");
                return;
            }

            #region 生成章节列表            
            if (MessageBox.Show("是否生成章节目录?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                CommonHelper.CreateDirectory(string.Format(@"chm\{0}", novel.NovelID));
                listmodel = BH.GetMenuListModel();

                BH.SaveNovelListToHtml(novel, menulist, listmodel);
                if (MessageBox.Show("章节目录生成完成,是否继续下载章节内容?", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                {
                    return;
                };
            };

            #endregion

            #region 下载并生成章节内容

            waitinglist = list;
            successlist = new List<NovelContent>();
            pbComplete.Maximum = waitinglist.Count;
            pbComplete.Value = 0;
            //SuccessCount = 0;
            //ErrorCount = 0;
            CompletedCount = 0;
            //IsGetCompleted = false;
            IsDownCompleted = false;
            IsBreak = false;
            btnDownLoadSelect.Enabled = false;
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            SkipExist = ckbSkip.Checked;
            //eventX.Reset();
            chaptermodel = BH.GetChapterModel();

            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 10);
            for (int i = 0, j = waitinglist.Count; i < j; i++)
            {
                //int nWorkThreads;
                //int nCompletionPortThreads;
                //ThreadPool.GetAvailableThreads(out nWorkThreads,out nCompletionPortThreads);
                //Console.WriteLine(nWorkThreads.ToString() + "   " + nCompletionPortThreads.ToString()); //默认都是1000
                if (!IsBreak)
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DownLoad), waitinglist[i]);
            }

            #endregion
        }

        /// <summary>
        /// UI刷新方法
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        private void UpdateResult(int arg1, string arg2)
        {
            pbComplete.Value = CompletedCount;
            this.lbTip.Text = arg2;
            if (IsDownCompleted)
            {
                arg2 = "全部章节已下载完成";
                this.lbTip.Text = arg2;
                this.btnDownLoadSelect.Enabled = true;
                this.btnStart.Enabled = true;
                this.btnStop.Enabled = false;
                this.btnStop.Text = "停止";
                if (IsClose)
                {
                    Thread.Sleep(1000);
                    this.Close();
                }
                //MessageBox.Show(arg2, "提示");
            }
            taskbar.SetProgressValue(CompletedCount, waitinglist.Count);
        }
        #endregion

        #region 按钮事件

        private void btnDownLoadSelect_Click(object sender, EventArgs e)
        {
            if (dvList.SelectedRows.Count > 0)
            {
                var ids = new List<int>();
                //List<DataGridViewRow> tmpList = new List<DataGridViewRow>();
                for (int i = 0, j = dvList.SelectedRows.Count; i < j; i++)
                {
                    var id = dvList.SelectedRows[i].Cells["Id"].Value;
                    ids.Add(Convert.ToInt32(id));
                    //tmpList.Add(dvList.SelectedRows[i]);
                }
                if (ids.Count > 0)
                {
                    var list = menulist.FindAll(p => ids.Contains(p.Id));
                    StartDownLoad(list);
                }
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.ckbSkip.Checked)
            {
                var list = menulist.FindAll(p => !BH.IsDownLoad(novel.NovelID, p.Id));
                StartDownLoad(list);
            }
            else
                StartDownLoad(menulist);
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dvList.SelectedRows.Count > 0)
            {
                List<int> ids = new List<int>();
                List<DataGridViewRow> tmpList = new List<DataGridViewRow>();
                for (int i = 0, j = dvList.SelectedRows.Count; i < j; i++)
                {
                    var novelId = Convert.ToInt32(dvList.SelectedRows[i].Cells["Id"].Value.ToString());
                    ids.Add(novelId);
                    tmpList.Add(dvList.SelectedRows[i]);
                }
                if (ids.Count > 0)
                    if (MessageBox.Show("确定要删除所选项吗，共 [" + ids.Count.ToString() + "] 条记录?", "确认", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        if (BH.DelNovelContents(ids))
                        {
                            for (int i = 0, j = tmpList.Count; i < j; i++)
                            {
                                dvList.Rows.Remove(tmpList[i]);
                            }
                            tmpList.Clear();
                        }
                        else
                        {
                            MessageBox.Show("删除失败", "错误");
                        }
                    }
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            IsBreak = true;
            this.btnStop.Text = "正在停止";
        }
        private void btnSelectRepeat_Click(object sender, EventArgs e)
        {
            dvList.ClearSelection();
            var dicList = new Dictionary<string, NovelContent>();
            var repeatList = new List<NovelContent>();
            var repeatIndexList = new List<int>();
            int i = 0;
            foreach (var item in menulist)
            {
                if (!dicList.ContainsKey(item.ComeFrom))
                {
                    dicList.Add(item.ComeFrom, item);
                }
                else
                {
                    repeatList.Add(item);
                    repeatIndexList.Add(i);
                }
                i++;
            }
            if (repeatIndexList.Count > 0)
            {
                //menulist.RemoveAll(p=> repeatList.Contains(p));
                foreach (var index in repeatIndexList)
                {
                    dvList.Rows[index].Selected = true;
                }
                dvList.FirstDisplayedScrollingRowIndex = repeatIndexList[0];
            }
        }
        #endregion

        #region 窗体关闭事件

        private void frmDownContent_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsDownCompleted && !IsBreak)
            {
                //MessageBox.Show("请等待下载完成或者中止下载后再操作！", "提示");
                if (MessageBox.Show("下载未全部完成，是否继续关闭?", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    IsBreak = true;
                    IsClose = true;
                }
                e.Cancel = true;
            }
        }
        #endregion

        private void dvList_SelectionChanged(object sender, EventArgs e)
        {
            btnDownLoadSelect.Text = string.Format("{0}({1})", "下载", dvList.SelectedRows.Count);
            btnDelete.Text = string.Format("{0}({1})", "删除", dvList.SelectedRows.Count);
        }
    }
}
