using CYQ.Data.Table;
using CYQ.Data.Tool;
using MiniBlinkPinvoke;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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

        /// <summary>
        /// 查询书籍类型
        /// </summary>
        /// <returns>JsonData</returns>
        [JSFunctin]
        public string GetNovelTypes()
        {
            var data = BH.GetNovelTypes().ToJson(false, false).Replace("null", "\"\"");
            return data;
        }

        /// <summary>
        /// 查询书籍
        /// </summary>
        /// <returns></returns>
        [JSFunctin]
        public string GetNovel(string novelId)
        {
            var data = JsonHelper.ToJson(BH.GetNovel(novelId)).Replace("null", "\"\"");
            return data;
        }

        /// <summary>
        /// 查询书籍
        /// </summary>
        /// <returns></returns>
        [JSFunctin]
        public string GetNovels()
        {
            var data = BH.GetNovels().ToJson(false, false).Replace("null", "\"\"");
            return data;
        }

        /// <summary>
        /// 保存书籍
        /// </summary>
        /// <param name="novelId">书籍ID</param>
        /// <returns></returns>
        [JSFunctin]
        public string SaveNovel(string data)
        {
            var result = new Result();
            try
            {
                var novel = JsonHelper.ToEntity<Novel>(data);
                //if (novel.NovelID == "0")
                //    result.Success = novel.Insert(true, CYQ.Data.InsertOp.ID);
                //else
                //    result.Success = novel.Update(string.Format("NovelID='{0}'", novel.NovelID));
                //if (!result.Success) throw new ValidationException(novel.DebugInfo);
                var save = BH.SaveNovel(novel);
                result.Success = true;
                result.Data = JsonHelper.ToJson(save, false);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.GetBaseException().Message;
            }
            var jsonData = JsonConvert.SerializeObject(result);
            return jsonData;
        }

        /// <summary>
        /// 删除书籍
        /// </summary>
        /// <param name="novelId">书籍ID</param>
        /// <returns></returns>
        [JSFunctin]
        public string DelNovel(string novelId)
        {
            var result = new Result();
            try
            {
                BH.DelNovelContents(novelId);
                BH.DelNovel(novelId);
                result.Success = true;
                result.Data = novelId;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.GetBaseException().Message;
            }
            var jsonData = JsonConvert.SerializeObject(result);
            return jsonData;
        }

        /// <summary>
        /// 删除书籍目录
        /// </summary>
        /// <param name="novelId">书籍ID</param>
        /// <returns></returns>
        [JSFunctin]
        public string DelNovelContents(string ids)
        {
            var result = new Result();
            try
            {
                var idList = JsonConvert.DeserializeObject<List<int>>(ids);
                BH.DelNovelContents(idList);
                
                result.Success = true;
                result.Data = ids;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.GetBaseException().Message;
            }
            var jsonData = JsonConvert.SerializeObject(result);
            return jsonData;
        }
        /// <summary>
        /// 根据书籍ID查询目录
        /// </summary>
        /// <param name="novelId">书籍ID</param>
        /// <returns></returns>
        [JSFunctin]
        public string GetNovelContents(string novelId)
        {
            var result = new Result();
            try
            {
                var data = JsonHelper.ToJson(BH.GetNovelContents(novelId).ToList<NovelContent>(), false).Replace("null", "\"\"");
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

        /// <summary>
        /// 更新书籍
        /// </summary>
        /// <param name="novelId"></param>
        /// <returns></returns>
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
        private List<Result> erroList;    //下载异常列表
        [JSFunctin]
        public string DownLoadContent(string data)
        {
            try
            {
                var downList = JsonHelper.ToList<NovelContent>(data);

                BH.SaveNovelContents(downList);

                CompletedCount = 0;
                TotalCount = downList.Count;
                if (TotalCount == 0)
                    return "-1";
                chapterModel = BH.GetChapterModel();
                var novelId = downList[0].NovelID;
                novel = BH.GetNovel(novelId);
                menuList = BH.GetNovelContents(novelId).ToList<NovelContent>();
                erroList = new List<Result>();

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
                    //return "-1";
                }

                #endregion

                //int tepmId = BH.GetNovelContentMaxId();
                foreach (var item in downList)
                {
                    //item.Id = tepmId++;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(DownLoad), item);
                }
                //MB.InvokeJS("SetProgressValue(50)");

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
                var dlmsg = string.Empty;
                var prior = BH.GetPrior(menuList, menu.Id);
                var next = BH.GetNext(menuList, menu.Id);
                var result = BH.SaveNovelContentToHtml(novel, ref menu, chapterModel, prior, next,ref dlmsg, true);
                //    Console.WriteLine(result);
                //    lock (lockObj)
                //    {
                //        successList.Add(menu);
                //    }
                //    this.Invoke(new EventHandler(delegate { UpdateResult(menu.Id, string.Format("已下载 {0}", menu.Title, Thread.CurrentThread.Name)); }));
                //    //this.Invoke(new Action<int, string>(UpdateResult), menu.Id, string.Format("已下载 {0}", menu.Title, Thread.CurrentThread.Name));

                //Thread.Sleep(500);
                //}
                if (!string.IsNullOrEmpty(dlmsg))
                    throw new ValidationException(dlmsg);
            }
            catch (Exception ex)
            {
                erroList.Add(new Result() { Data = JsonHelper.ToJson(menu),Key = menu.Id, Success = false, Message = ex.GetBaseException().Message });
                Console.WriteLine(string.Format("下载异常-> <<{0}>> -> {1}", menu.Title, ex.GetBaseException().Message));
                //this.Invoke(new Action<int, string>(UpdateResult), menu.Id, string.Format("异常 {0}", ex.Message));
            }
            finally
            {
                Interlocked.Increment(ref CompletedCount);

                //CompleteEvent(menu);
            }
        }

        /// <summary>
        /// 查询下载进度
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// 查询异常列表
        /// </summary>
        /// <returns></returns>
        [JSFunctin]
        public string GetErroList()
        {
            var data = erroList;
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// http/https切换
        /// </summary>
        /// <param name="url"></param>
        /// <param name="novelId"></param>
        [JSFunctin]
        public int ChangeHttpMode(string url, string novelId) {

            //var listUrl = txtListUrl.Text;
            if (string.IsNullOrEmpty(url))
            {
                //MessageBox.Show("请填写目录页网址! ", "提示");
                return -1;
            }
            if (BH.ChangeHttpMode(novelId))
            {
                //LoadData(NovelId);
                //MessageBox.Show("已切换完成! ", "提示");
                return 1;
            }
            return -1;
        }

        /// <summary>
        ///取出正文标志
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [JSFunctin]
        public string GetSiteSign(string url)
        {
            var data = JsonHelper.ToJson(BH.GetSiteSign(url)).Replace("null", "\"\"");
            return data;
        }

        /// <summary>
        /// 保存正文标志
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [JSFunctin]
        public string SaveSiteSign(string data)
        {
            var result = new Result();
            try
            {
                var novel = JsonHelper.ToEntity<Novel>(data);
                if (novel.ListUrl.Length == 0)
                    throw new ValidationException("网站地址不能为空");
                string rootUrl = CommonHelper.GetWebRootUrl(novel.ListUrl);
                var sign = BH.GetSiteSign(rootUrl);
                if (sign == null)
                {
                    sign = new WebBookManage.Model.SiteSign()
                    {
                        name = rootUrl,
                        url = rootUrl,
                        ListStart = novel.ListStart,
                        ListEnd = novel.ListEnd,
                        ContentStart = novel.ContentStart,
                        ContentEnd = novel.ContentEnd,
                        NeedDelStr = novel.NeedDelStr,
                        VolumeStart = novel.VolumeStart,
                        VolumeEnd = novel.VolumeEnd,
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
                BH.SignToJson(sign);
                //var save = BH.SaveNovel(novel);
                result.Success = true;
                result.Data = JsonHelper.ToJson(sign, false);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = ex.GetBaseException().Message;
            }
            var jsonData = JsonConvert.SerializeObject(result);
            return jsonData;
        }

        [JSFunctin]
        public int IsExists(string filePath) {
            if (File.Exists(filePath))
                return 1;
            return -1;
        }
    }

    public class Result
    {
        public bool Success = false;
        public string Message = string.Empty;
        public object Data = false;
        public object Key;
    }
}
