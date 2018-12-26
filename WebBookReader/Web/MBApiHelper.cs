using CYQ.Data.Table;
using CYQ.Data.Tool;
using MiniBlinkPinvoke;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using WebBookManage.Common;
using WebBookManage.Model;

namespace WebBookReader.Web
{
    public class MBApiHelper
    {
        BookHelper BH = null;
        BlinkBrowser MB = null;
        public MBApiHelper(BlinkBrowser mb)
        {
            BH = new BookHelper();
            MB = mb;
        }

        [JSFunctin]
        public void ApiTest(string msg)
        {
            MessageBox.Show("ApiTest : " + msg);
        }

        [JSFunctin]
        public string GetNovelTypes()
        {
            var data = BH.GetNovelTypes().ToJson(false, false).Replace("null", "\"\"");
            return data;
        }

        [JSFunctin]
        public string GetNovels()
        {
            var data = BH.GetNovels().ToJson(false, false).Replace("null", "\"\"");
            return data;
        }

        [JSFunctin]
        public string UpdateNovel(string novelId)
        {
            var result = new Result();
            try
            {
                var list = BH.DownMenuList(novelId);
                var data = JsonHelper.ToJson(list);// JsonConvert.SerializeObject(list).Replace("null", "\"\"");
                result.Success = true;
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.GetBaseException().Message;
            }
            //var jsonData = JsonHelper.ToJson(result);
            var jsonData = JsonConvert.SerializeObject(result);
            return jsonData;
        }

        private int CompletedCount = 0;
        private int TotalCount = 0;
        private string chapterModel = string.Empty;
        private string listModel = string.Empty;
        private Novel novel;
        private List<NovelContent> menuList;    //章节列表
        [JSFunctin]
        public string DownLoadContent(string data)
        {
            try
            {
                var downList = JsonHelper.ToList<NovelContent>(data);
                CompletedCount = 0;
                TotalCount = downList.Count;
                if (TotalCount == 0)
                    return "-1";
                chapterModel = BH.GetChapterModel();
                var novelId = downList[0].NovelID;
                novel = BH.GetNovel(novelId);
                menuList = BH.GetNovelContents(novelId).ToList<NovelContent>();

                #region 生成章节列表            
                if (MessageBox.Show("是否生成章节目录?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    CommonHelper.CreateDirectory(string.Format(@"chm\{0}", novel.NovelID));
                    listModel = BH.GetMenuListModel();

                    BH.SaveNovelListToHtml(novel, menuList, listModel);
                    if (MessageBox.Show("章节目录生成完成,是否继续下载章节内容?", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return "-1";
                    };
                }
                else
                {
                    return "-1";
                }

                #endregion

                int tepmId = BH.GetNovelContentMaxId();
                foreach (var item in downList)
                {
                    item.Id = tepmId++;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DownLoad), item);
                }
                //MB.InvokeJS("SetProgressValue(50)");
                //BH.SaveNovelContents(downList);
                //var novelId = downList[0].NovelID;
                //var novel = BH.GetNovel(novelId);
                ////if (novel != null)
                ////{
                ////    var dt = BH.GetNovelContents(novelId);
                ////    new frmDownContent(novel, dt.ToList<NovelContent>()).ShowDialog();
                ////}
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }
            
            return "0";
        }
        private void DownLoad(object o)
        {
            var menu = o as NovelContent;
            try
            {
                //if (IsBreak)
                //{
                //    //eventX.Set();
                //    Console.WriteLine(string.Format("下载中止-> {0}//{1} - <<{2}>>", menuList.Count, CompletedCount, menu.Title));
                //}
                //else
                //{
                var prior = BH.GetPrior(menuList, menu.Id);
                var next = BH.GetNext(menuList, menu.Id);
                var result = BH.SaveNovelContentToHtml(novel, ref menu, chapterModel, next, null, true);
                //    Console.WriteLine(result);
                //    lock (lockObj)
                //    {
                //        successList.Add(menu);
                //    }
                //    this.Invoke(new EventHandler(delegate { UpdateResult(menu.Id, string.Format("已下载 {0}", menu.Title, Thread.CurrentThread.Name)); }));
                //    //this.Invoke(new Action<int, string>(UpdateResult), menu.Id, string.Format("已下载 {0}", menu.Title, Thread.CurrentThread.Name));

                //Thread.Sleep(500);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("下载异常-> <<{0}>> -> {1}", menu.Title, ex.Message));
                //this.Invoke(new Action<int, string>(UpdateResult), menu.Id, string.Format("异常 {0}", ex.Message));
            }
            finally
            {
                Interlocked.Increment(ref CompletedCount);
                
                //CompleteEvent(menu);
            }
        }

        [JSFunctin]
        public string UpdateProgressValue()
        {
            var value = (int)(CompletedCount * 100 / TotalCount);
            //if (value >= 100)
            //    MB.InvokeJS("SetProgressValue(" + value + ")"); ;
            Console.WriteLine(string.Format("{0}/{1} -> {2}%", CompletedCount, TotalCount, value));
            //return string.Format("{0}/{1}", CompletedCount, TotalCount);
            return value.ToString();
        }

        public string GetBookListHtml()
        {
            var html = @"<div class='yd-book-item yd-book-item-pull-left'>
                                    <a href='#' target='_blank'> <img src='./res/Default.png' alt='人山人海里，你不必记得我' class='pull-left cover-container' width='114' height='160' /> <h2>人山人海里，你不必记得我</h2> </a>
                                    <div class='author-container'>
                                        <dl class='dl-horizontal-inline'>
                                            <dt>
                                                作者：
                                            </dt>
                                            <dd>
                                                金浩森
                                            </dd>
                                        </dl>
                                    </div>
                                    <div class='rate w-star w-star1'>
                                        <span>&nbsp;</span>
                                        <span>&nbsp;</span>
                                        <span>&nbsp;</span>
                                        <span>&nbsp;</span>
                                        <span class='no'>&nbsp;</span>
                                    </div>
                                    <div class='price-container f-invi'>
                                        <b class='price'> ￥9.90 </b>
                                    </div>
                                    <div class='summery'>
                                        <p>《人山人海里，你不必记得我》是金浩森在七年沉淀后的内心之作。十篇异国旅途日记、十个国家的写真及摄影作品，描述出这些年在旅行路上对生活的反思，以及收获到对世界的认知。同时，通过对自我的发问，写给六个“你”的真诚书信，全书以文字作品、个人写真、摄影作品的立体形式，呈现了一段褪去浮躁与复杂、归于简单与纯真的成长路途。我想一直这样走下去，去看世界上所有的魅力与孤独，无论在哪里，做什么样的事情，只要能和喜欢的人在一起。</p>
                                    </div>
                                    <div class='badge badge-complete png'></div>
                                </div>";

            return html;
        }
    }

    public class Result
    {
        public bool Success = false;
        public string Message = string.Empty;
        public object Data = false;
    }
}
