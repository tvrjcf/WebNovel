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
            //Thread.Sleep(4000);
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
                var novel = JsonConvert.DeserializeObject<Novel>(data);
                //var novel = JsonHelper.ToEntity<Novel>(data);
                if (novel.ListUrl.Length == 0)
                    throw new ValidationException("网站地址不能为空");
                var save = BH.SaveNovel(novel);

                //保存正文标记
                string rootUrl = CommonHelper.GetWebRootUrl(novel.ListUrl);
                //var sign = JsonHelper.ToEntity<SiteSign>(data);
                var sign = JsonConvert.DeserializeObject<SiteSign>(data);
                sign.name = rootUrl;
                sign.url = rootUrl;
                BH.SignToJson(sign);

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
        /// 获取榜单列表
        /// </summary>
        [JSFunctin]
        public string GetChartsList(string url, string hrefPattern)
        {
            var result = new Result();
            try
            {
                //var url = "http://www.hebao22.com/qitaleixing/";
                //var hrefPattern = @"/book/\d+/";
                var list = BH.GetChartsList(url, hrefPattern);
                var data = JsonHelper.ToJson(list, false).Replace("null", "\"\"");
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

        /// <summary>
        /// 制作EBook
        /// </summary>
        /// <param name="novelId"></param>
        /// <returns></returns>
        [JSFunctin]
        public string CreateEBook(string novelId)
        {
            var txt = BH.CreateEBook(novelId, EbookType.TXT);
            return txt;
        }

        private int CompletedCount = 0;
        private int TotalCount = 0;
        private string chapterModel = string.Empty;
        private string listModel = string.Empty;
        private Novel currNovel;
        private List<NovelContent> menuList;    //章节列表
        private List<Result> erroList;    //下载异常列表
        private string[] arrContent;    //章节内容数据
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
                currNovel = BH.GetNovel(novelId);
                menuList = BH.GetNovelContents(novelId).ToList<NovelContent>();
                erroList = new List<Result>();

                #region 生成章节列表            
                //if (MessageBox.Show("是否生成章节目录?", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                //{
                //    CommonHelper.CreateDirectory(string.Format(@"chm\{0}", currNovel.NovelID));
                //    listModel = BH.GetMenuListModel();

                //    BH.SaveNovelListToHtml(currNovel, menuList, listModel);
                //    if (MessageBox.Show("章节目录生成完成,是否继续下载章节内容?", "提示", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                //    {
                //        return "-1";
                //    };
                //}
                //else
                //{
                //    //return "-1";
                //}
                CommonHelper.CreateDirectory(string.Format(@"chm\{0}", currNovel.NovelID));
                listModel = BH.GetMenuListModel();
                BH.SaveNovelListToHtml(currNovel, menuList, listModel);
                Console.WriteLine(string.Format("已生成[{0}]目录", currNovel.NovelName));

                #endregion

                //int tepmId = BH.GetNovelContentMaxId();
                foreach (var item in downList)
                {
                    //item.Id = tepmId++;
                    ThreadPool.QueueUserWorkItem(new WaitCallback(Thread_DownLoadContent), item);
                }
                //MB.InvokeJS("SetProgressValue(50)");

            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }

            return "0";
        }
        private void Thread_DownLoadContent(object o)
        {
            var menu = o as NovelContent;
            try
            {
                var dlmsg = string.Empty;
                var prior = BH.GetPrior(menuList, menu.Id);
                var next = BH.GetNext(menuList, menu.Id);
                var result = BH.SaveNovelContentToHtml(currNovel, ref menu, chapterModel, prior, next, ref dlmsg, true);

                if (!string.IsNullOrEmpty(dlmsg))
                    throw new ValidationException(dlmsg);
            }
            catch (Exception ex)
            {
                erroList.Add(new Result() { Data = JsonHelper.ToJson(menu), Key = menu.Id, Success = false, Message = ex.GetBaseException().Message });
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
        /// 下载榜单
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [JSFunctin]
        public string DownLoadCharts(string data)
        {

            try
            {
                var novelList = JsonHelper.ToList<Novel>(data);
                CommonHelper.CreateDirectory(@"charts");
                CommonHelper.SaveToFile(@"charts\charts.json", data, Encoding.UTF8);
                //BH.SaveNovelContents(downList);
                foreach (var novel in novelList)
                {
                    CommonHelper.CreateDirectory(string.Format(@"charts\{0}", novel.NovelID));
                    var html = CommonHelper.GetHtml(novel.ListUrl);
                    novel.ChapterList = BH.GetMenuList(html, novel.NovelID, novel.ListUrl);
                    int tepmId = 1;
                    foreach (var item in novel.ChapterList)
                    {
                        item.Id = tepmId++;
                    }
                    CompletedCount = 0;
                    TotalCount = novel.ChapterList.Count;
                    arrContent = new string[TotalCount];
                    if (TotalCount == 0)
                        return "-1";
                    chapterModel = BH.GetChapterModel();
                    var novelId = novel.NovelID;
                    currNovel = novel;
                    menuList = novel.ChapterList;
                    erroList = new List<Result>();

                    #region 生成章节列表
                    var saveFileName = string.Format(@"charts\{0}\0 list", novel.NovelID, novel.NovelName);
                    CommonHelper.SaveToFile(saveFileName, JsonHelper.ToJson(novel), Encoding.UTF8);
                    Console.WriteLine(string.Format("已生成[{0}]目录", novel.NovelName));
                    #endregion

                    foreach (var item in novel.ChapterList)
                    {
                        ThreadPool.QueueUserWorkItem(new WaitCallback(Thread_DownLoadChartsContent), item);
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.GetBaseException().Message;
            }

            return "0";
        }

        private void Thread_DownLoadChartsContent(object o)
        {
            var menu = o as NovelContent;
            var dlmsg = string.Empty;
            var content = string.Empty;
            try
            {
                var result = BH.SaveChartsContentToJson(ref menu, ref content, ref dlmsg, true);

                if (!string.IsNullOrEmpty(dlmsg))
                    throw new ValidationException(dlmsg);
            }
            catch (Exception ex)
            {
                erroList.Add(new Result() { Data = JsonHelper.ToJson(menu), Key = menu.Id, Success = false, Message = ex.GetBaseException().Message });
                Console.WriteLine(string.Format("下载异常-> <<{0}>> -> {1}", menu.Title, ex.GetBaseException().Message));
                //this.Invoke(new Action<int, string>(UpdateResult), menu.Id, string.Format("异常 {0}", ex.Message));
            }
            finally
            {
                Interlocked.Increment(ref CompletedCount);
                //CompleteEvent(menu);
                arrContent[menu.Id - 1] = content;
                if (CompletedCount == TotalCount)
                {
                    //生成txt
                    var saveFileName = string.Format(@"charts\{0}\{1}.txt", currNovel.NovelID, currNovel.NovelName);
                    CommonHelper.SaveToFile(saveFileName, string.Join("\r\n", arrContent), Encoding.UTF8);
                }
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
        public int ChangeHttpMode(string url, string novelId)
        {

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
                //var sign = JsonHelper.ToEntity<SiteSign>(data);
                var sign = JsonConvert.DeserializeObject<SiteSign>(data);
                if (sign.url.Length == 0)
                    throw new ValidationException("网站地址不能为空");
                string rootUrl = CommonHelper.GetWebRootUrl(sign.url);
                if (rootUrl.Length == 0)
                    throw new ValidationException("该网址无效");

                //sign.name = rootUrl;
                sign.url = rootUrl;

                BH.SignToJson(sign);
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
        public int IsExists(string filePath)
        {
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

    public class ThreadArgs
    {
        public object arg1 { get; set; }
        public object arg2 { get; set; }
    }
}
