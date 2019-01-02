using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Xml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using WebBookManage.Model;

namespace WebBookManage.Common
{
    public class BookHelper
    {
        public static string DIR_PATH_NOVEL = @"chm\{0}";    //
        public static string FILE_PATH_LIST_MODEL = @"chm\{0}\List.htm";    //
        public static string FILE_PATH_CHAPTER_MODEL = @"chm\{0}\{1}.htm";  //
        private string T_Novel = "book_Novel";
        private string T_NovelContent = "book_NovelContent";

        /// <summary>
        /// 查询书籍
        /// </summary>
        /// <returns></returns>
        public Novel GetNovel(string novelId)
        {
            var model = new Novel();
            if (model.Fill(novelId))
            {
                return model;
            }
            return null;
        }

        /// <summary>
        /// 查询书籍
        /// </summary>
        /// <returns></returns>
        public MDataTable GetNovels()
        {
            var model = new Novel();
            var dt = model.Select("order by novelid");
            return dt;
        }

        /// <summary>
        /// 查询书籍ID最大值
        /// </summary>
        /// <returns></returns>
        public int GetNovelMaxId()
        {
            int maxid = 0;
            using (MAction actionMax = new MAction("select max(NovelID) as [maxId] from " + T_Novel))
            {
                var dtMax = actionMax.Select();
                if (dtMax != null && dtMax.Rows.Count > 0)
                {
                    try
                    {
                        maxid = Convert.ToInt32(dtMax.Rows[0][0].Value);
                        maxid++;
                    }
                    catch (Exception)
                    {
                        maxid = 0;
                    }
                }
            }
            return maxid;
        }


        public Novel SaveNovel(Novel novel)
        {
            bool ret = false;
            if (novel.NovelID == "0")
            {
                novel.NovelID = GetNovelMaxId().ToString().PadLeft(6, '0');
                ret = novel.Insert(true, CYQ.Data.InsertOp.ID);
            }
            else
            {
                ret = novel.Update(string.Format("NovelID='{0}'", novel.NovelID));
            }
            if (!ret)
                throw new ValidationException(novel.DebugInfo);
            return novel;
        }

        /// <summary>
        /// 删除书籍
        /// </summary>
        /// <param name="novelId">书籍ID</param>
        /// <returns></returns>
        public bool DelNovel(string novelId)
        {
            using (MAction action = new MAction(T_Novel))
            {
                return action.Delete(string.Format("NovelID='{0}'", novelId));
            }
        }

        /// <summary>
        /// 查询目录信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NovelContent GetNovelContent(string id)
        {
            var model = new NovelContent();
            if (model.Fill(id))
            {
                return model;
            }
            return null;
        }
        /// <summary>
        /// 根据书籍ID查询目录
        /// </summary>
        /// <returns></returns>
        public MDataTable GetNovelContents(string novelId)
        {
            var model = new NovelContent();
            var dt = model.Select(string.Format("NovelID='{0}' order by id", novelId));
            return dt;
        }

        /// <summary>
        /// 查询目录ID最大值
        /// </summary>
        /// <returns></returns>
        public int GetNovelContentMaxId()
        {
            int maxid = 0;
            using (MAction actionMax = new MAction("select max(ID) as [maxId] from " + T_NovelContent))
            {
                var dtMax = actionMax.Select();
                if (dtMax != null && dtMax.Rows.Count > 0)
                {
                    try
                    {
                        maxid = Convert.ToInt32(dtMax.Rows[0][0].Value);
                        maxid++;
                    }
                    catch (Exception)
                    {
                        maxid = 0;
                    }
                }
            }
            return maxid;
        }

        /// <summary>
        /// 根据书籍ID删除目录
        /// </summary>
        /// <param name="novelId">书籍ID</param>
        /// <returns></returns>
        public bool DelNovelContents(string novelId)
        {
            using (MAction action = new MAction(T_NovelContent))
            {
                return action.Delete(string.Format("NovelID='{0}'", novelId));
            }
        }
        /// <summary>
        /// 根据目录ID列表删除目录
        /// </summary>
        /// <param name="novelId">书籍ID</param>
        /// <returns></returns>
        public bool DelNovelContents(List<int> ids)
        {
            using (MAction action = new MAction(T_NovelContent))
            {
                //string strIds = string.Join("','", ids.ConvertAll(p => p.ToString()).ToArray());    //MS SQL
                //var del = action.Delete(string.Format("ID in ('{0}')", strIds));  //MS SQL
                string strIds = string.Join(",", ids.ConvertAll(p => p.ToString()).ToArray());    //Access
                var del = action.Delete(string.Format("ID in ({0})", strIds));  //Access
                if (!del) throw new ValidationException(action.DebugInfo);
                return del;
            }
        }
        /// <summary>
        /// 保存目录列表
        /// </summary>
        /// <param name="menulist"></param>
        /// <returns></returns>
        public bool SaveNovelContents(List<NovelContent> menulist)
        {
            int maxid = GetNovelContentMaxId();
            foreach (var menu in menulist)
            {
                if (menu.Id > 0) continue;
                menu.Id = maxid++;
            }
            var dt = MDataTable.CreateFrom(menulist);
            dt.TableName = menulist[0].TableName;
            return dt.AcceptChanges(AcceptOp.Insert);
        }
        /// <summary>
        /// 从HTML中获取目录列表
        /// </summary>
        /// <param name="html"></param>
        /// <param name="listUrl"></param>
        /// <returns></returns>
        public List<NovelContent> GetMenuList(string html, string novelId, string listUrl)
        {
            if (listUrl.Substring(listUrl.Length - 1, 1) != "/")
                listUrl += "/";
            string rootUrl = listUrl.Substring(0, listUrl.IndexOf("/", 8)) + "/";

            var list = new List<NovelContent>();
            string prttern = "<a(\\s+(href=\"(?<url>([^\"])*)\"|'([^'])*'|\\w+=\"(([^\"])*)\"|'([^'])*'))+>(?<text>(.*?))</a>";
            var maths = Regex.Matches(html, prttern);

            for (int i = 0; i < maths.Count; i++)
            {
                string url = maths[i].Groups["url"].Value;
                string text = maths[i].Groups["text"].Value;
                if (!url.Contains(".htm") && !url.Contains(".html"))
                    continue;

                if (url.IndexOf("http") == -1)
                {
                    if (url.StartsWith("/"))
                    {
                        url = rootUrl + url.Substring(1, url.Length - 1);
                    }
                    else
                    {
                        url = listUrl + url;
                    }
                }
                var menu = new NovelContent()
                {
                    NovelID = novelId,
                    ComeFrom = url.Trim(),
                    Title = text,
                    DownDate = DateTime.Now,
                    DownTime = DateTime.Now,
                    bIsRead = 0,
                    ChpType = 1
                };
                list.Add(menu);
            }
            return list;
        }

        /// <summary>
        /// 下载书籍目录
        /// </summary>
        /// <param name="novelId"></param>
        /// <returns></returns>
        public List<NovelContent> DownMenuList(string novelId)
        {
            var novel = GetNovel(novelId);
            if (novel == null || !CommonHelper.IsUrl(novel.ListUrl))
            {
                return new List<NovelContent>();
            }
            if (novel.ListStart == null || novel.ListEnd == null)
                throw new ValidationException("请维护目录匹配规则");
            Console.WriteLine("->正在连接目录...");
            string html = CommonHelper.GetHtml(novel.ListUrl);

            Console.WriteLine("->正在解析目录...");
            string menuHtml = CommonHelper.GetValue(html, novel.ListStart, novel.ListEnd);
            var list = GetMenuList(menuHtml, novelId, novel.ListUrl);

            int totalCount = list.Count;
            Console.WriteLine(string.Format("->解析完成,共{0}", totalCount));
            if (totalCount == 0)
                throw new ValidationException("同步失败:\n-->>" + html);
            int filterCount = 0;
            var listExist = new List<NovelContent>();
            //var listUpdate = new List<NovelContent>();            

            #region 过滤链接列表
            //过滤链接列表

            Console.WriteLine("->过滤链接列表...");
            string strUrl = novel.NeedDelUrl ?? string.Empty;
            list.RemoveAll(p => strUrl.Contains(p.ComeFrom));
            filterCount = totalCount - list.Count;
            #endregion

            #region 过滤已下载列表
            //过滤已下载列表

            Console.WriteLine("->过滤已下载列表...");
            var dt = GetNovelContents(novelId);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = list.Count - 1, j = 0; i >= j; i--)
                {
                    if (dt.FindRow(string.Format("ComeFrom = '{0}'", list[i].ComeFrom)) != null)
                    {
                        list.RemoveAt(i);
                    }
                }
                //foreach (var item1 in list)
                //{
                //    if (dt.FindRow(string.Format("ComeFrom = '{0}'", item1.ComeFrom)) != null)
                //    {
                //        listExist.Add(item1);
                //    }
                //    //foreach (var row in dt.Rows)
                //    //{
                //    //    if (row["ComeFrom"].Value.ToString() == item1.ComeFrom)
                //    //    {
                //    //        listExist.Add(item1);
                //    //        break;
                //    //    }
                //    //}
                //}
                //list.RemoveAll(p => listExist.Exists(x => x.ComeFrom == p.ComeFrom));
            }
            #endregion

            Console.WriteLine("->列表处理完成.");
            return list;
        }
        /// <summary>
        /// 查询小说类型
        /// </summary>
        /// <returns></returns>
        public MDataTable GetNovelTypes()
        {
            var model = new NovelType();
            var dt = model.Select();
            return dt;
        }

        /// <summary>
        /// 网址http/https模式切换
        /// </summary>
        /// <param name="novelId"></param>
        /// <returns></returns>
        public bool ChangeHttpMode(string novelId)
        {
            var novel = GetNovel(novelId);
            if (novelId == null)
                return false;
            string modeOld = "http", modeNew = "https";
            if (novel.ListUrl.Contains("https"))
            {
                modeOld = "https";
                modeNew = "http";
            }
            novel.ListUrl = novel.ListUrl.Replace(modeOld, modeNew);
            var dt = GetNovelContents(novelId);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (var row in dt.Rows)
                {
                    row.Set("ComeFrom", row.Get<string>("ComeFrom").Replace(modeOld, modeNew));
                }
                dt.AcceptChanges(AcceptOp.Update);
            }
            return novel.Update();
        }

        /// <summary>
        /// 查询网站正文设定标志列表
        /// </summary>
        /// <returns></returns>
        public List<SiteSign> GetSiteSignList()
        {
            var list = new List<SiteSign>();
            XHtmlAction xml = new XHtmlAction();
            if (xml.Load(@"SiteSign.xml"))
            {
                var nodeList = xml.GetList("SiteSign");
                foreach (XmlNode item in nodeList)
                {
                    var sign = new SiteSign();
                    foreach (XmlNode node in item.ChildNodes)
                    {
                        string value = node.InnerXml
                            .Replace("&amp;", "&")
                            .Replace("&lt;", "<")
                            .Replace("&gt;", ">")
                            .Replace("&quot;", "\"")
                            .Replace("&apos;", "\'");

                        #region 赋值

                        switch (node.Name)
                        {
                            case "name":
                                sign.name = value;
                                break;
                            case "url":
                                sign.url = value;
                                break;
                            case "ListStart":
                                sign.ListStart = value;
                                break;
                            case "ListEnd":
                                sign.ListEnd = value;
                                break;
                            case "ContentStart":
                                sign.ContentStart = value;
                                break;
                            case "ContentEnd":
                                sign.ContentEnd = value;
                                break;
                            case "NeedDelStr":
                                sign.NeedDelStr = value;
                                break;
                            case "VolumeStart":
                                sign.VolumeStart = value;
                                break;
                            case "VolumeEnd":
                                sign.VolumeEnd = value;
                                break;
                            case "BriefUrlStart":
                                sign.BriefUrlStart = value;
                                break;
                            case "BriefUrlEnd":
                                sign.BriefUrlEnd = value;
                                break;
                            case "AuthorStart":
                                sign.AuthorStart = value;
                                break;
                            case "AuthorEnd":
                                sign.AuthorEnd = value;
                                break;
                            case "BriefStart":
                                sign.BriefStart = value;
                                break;
                            case "BriefEnd":
                                sign.BriefEnd = value;
                                break;
                            case "BookImgUrlStart":
                                sign.BookImgUrlStart = value;
                                break;
                            case "BookImgUrlEnd":
                                sign.BookImgUrlEnd = value;
                                break;
                            default:
                                break;
                        }
                        #endregion
                    }
                    list.Add(sign);

                }
            };
            return list;
        }

        public string SignToHtmlCode(string value)
        {
            return value;
            //.Replace("&", "&lt;")
            //.Replace("<", "&lt;")
            //.Replace(">", "&gt;")
            //.Replace("\"", "&quot;")
            //.Replace("\'", "&apos;");
        }
        public XmlNode ToXmlNode(SiteSign sign)
        {
            XmlNode oldNode = null;
            XmlNode newNode = null;
            var xml = new XmlDocument();
            xml.Load(@"SiteSign.xml");
            {
                var root = xml.DocumentElement;

                foreach (XmlNode item in root.ChildNodes)
                {
                    if (item.InnerText.Contains(sign.url))
                    {
                        oldNode = item;
                        break;
                    }
                }
                newNode = xml.CreateElement("SiteSign");

                Type t = sign.GetType();
                PropertyInfo[] PropertyList = t.GetProperties();
                foreach (PropertyInfo item in PropertyList)
                {
                    string name = item.Name;
                    object value = item.GetValue(sign, null);

                    var xnode = xml.CreateElement(name);
                    xnode.InnerText = SignToHtmlCode(value.ToString());
                    newNode.AppendChild(xnode);
                }
                //root.AppendChild(NewNode);
                if (oldNode == null)
                    root.InsertBefore(newNode, root.ChildNodes[0]);
                else
                    root.ReplaceChild(newNode, oldNode);
                xml.Save(@"SiteSign.xml");
            }

            return newNode;
        }

        /// <summary>
        /// 根据网址取出标志
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public SiteSign GetSiteSign(string url)
        {
            var rootUrl = CommonHelper.GetWebRootUrl(url);
            if (rootUrl.Length == 0)
                return null;
            var list = GetSiteSignList();
            return list.Find(p => p.url.Contains(rootUrl));
        }

        /// <summary>
        /// 获取目录模板内容
        /// </summary>
        /// <returns></returns>
        public string GetMenuListModel()
        {

            //string listmodel = CommonHelper.LoadHtmlFile(@"usermodel\素白\listmodel.htm");
            var listmodel = CommonHelper.LoadHtmlFile(SysSetting.GetInstance().ThemeModel.ListModelPath);
            return listmodel;
        }

        /// <summary>
        /// 获取章节模板内容
        /// </summary>
        /// <returns></returns>
        public string GetChapterModel()
        {
            //string chaptermodel = CommonHelper.LoadHtmlFile(@"usermodel\素白\chaptermodel.htm");
            string chaptermodel = CommonHelper.LoadHtmlFile(SysSetting.GetInstance().ThemeModel.ChapterModelPath);
            return chaptermodel;
        }

        /// <summary>
        /// 切换主题
        /// </summary>
        /// <param name="theme"></param>
        public void ChangeThemeModel(Novel novel, List<NovelContent> menulist, ThemeModel theme)
        {
            var dirInfo = new DirectoryInfo(string.Format(BookHelper.DIR_PATH_NOVEL, novel.NovelID));
            var listModelContent = CommonHelper.LoadHtmlFile(theme.ListModelPath);
            var chapterModelContent = CommonHelper.LoadHtmlFile(theme.ChapterModelPath);
            foreach (var fileInfo in dirInfo.GetFiles())
            {
                string fileName = fileInfo.FullName;
                var doc = CommonHelper.LoadHtmlFile(fileName);
                if (fileName.Contains("List.htm"))
                {
                    SaveNovelListToHtml(novel, menulist, listModelContent);
                }
                else
                {
                    var title = CommonHelper.GetValue(doc, "<font class=\"article_title\">", "</font>");
                    var content = CommonHelper.GetValue(doc, "<!--BookContent Start-->", "<!--BookContent End-->");

                    CommonHelper.SaveToFile(
                        fileName,
                        chapterModelContent
                        .Replace(@"[[title]]", title.Replace("<br>", ""))
                        .Replace(@"[[Content]]", content));
                }
            }
        }

        /// <summary>
        /// 切换模板主题
        /// </summary>
        /// <param name="fileInfo">要切换的章节文件</param>
        /// <param name="modelContent">章节模板</param>
        public void ChangeThemeModel(FileInfo fileInfo, string modelContent, object o = null)
        {
            string fileName = fileInfo.FullName;
            var doc = CommonHelper.LoadHtmlFile(fileName);

            var title = CommonHelper.GetValue(doc, "<font class=\"article_title\">", "</font>").Replace("<br>", "").TrimStart().TrimEnd();
            var content = CommonHelper.GetValue(doc, "<!--BookContent Start-->", "<!--BookContent End-->").Replace("\r\n", "").TrimStart().TrimEnd();

            CommonHelper.SaveToFile(
                fileName,
                modelContent
                .Replace(@"[[title]]", title)
                .Replace(@"[[Content]]", content));

            //if (o != null && o is MutipleThreadResetEvent)
            //{
            //    var e = (MutipleThreadResetEvent)o;
            //    e.SetOne();
            //}
        }

        /// <summary>
        /// 保存章节列表为HTML文件
        /// </summary>
        /// <param name="novel"></param>
        /// <param name="menulist"></param>
        /// <param name="modelContent"></param>
        /// <returns></returns>
        public string SaveNovelListToHtml(Novel novel, List<NovelContent> menulist, string modelContent)
        {
            #region 生成章节列表
            string content = "";
            for (int i = 0, j = menulist.Count; i < j; i++)
            {
                content += string.Format("<tr>");
                var menu1 = menulist[i];

                content += string.Format("<td width=50% align=left>&nbsp;<a href=\"{0}.htm\">{1}</a></td>", menu1.Id, menu1.Title.Replace(" ", "&nbsp;"));
                if (++i < j)
                {
                    var menu2 = menulist[i];
                    content += string.Format("<td width=50% align=left>&nbsp;<a href=\"{0}.htm\">{1}</a></td>", menu2.Id, menu2.Title.Replace(" ", "&nbsp;"));
                }
                content += string.Format("</tr>\r");

            }

            string listHtml = modelContent
                .Replace("[[title]]", "目录")
                .Replace("[[BookName]]", novel.NovelName)
                .Replace("[[author]]", novel.Author)
                .Replace("[[BookImgName]]", string.IsNullOrEmpty(novel.BookImg) ? "../nocover.gif" : novel.BookImg)
                .Replace("[[LBtitle]]", novel.LB)
                .Replace("[[bIsEnd]]", novel.bIsEnd ? "已完结" : "连载中")
                .Replace("[[ListUrl]]", novel.ListUrl)
                .Replace("[[brief]]", novel.Brief)
                .Replace("[[Content]]", content)
                ;
            string saveFileName = string.Format(FILE_PATH_LIST_MODEL, novel.NovelID);
            CommonHelper.SaveToFile(saveFileName, listHtml);

            //UpdateResult(0, "章节列表生成完成");
            #endregion

            return Path.GetFullPath(saveFileName);
        }

        /// <summary>
        /// 保存目录内容为HTML文件
        /// </summary>
        /// <param name="novel">书籍</param>
        /// <param name="menu">目录实体</param>
        /// <param name="modelContent">模板</param>
        /// <param name="priorMenu">上一目录实体</param>
        /// <param name="nextMenu">下一目录实体</param>
        /// <param name="skipExist">跳过已下载章节，默认true</param>
        /// <returns></returns>
        public string SaveNovelContentToHtml(Novel novel, ref NovelContent menu, string modelContent, NovelContent priorMenu, NovelContent nextMenu, bool skipExist = true)
        {
            string saveFileName = string.Format(FILE_PATH_CHAPTER_MODEL, menu.NovelID, menu.Id);
            //跳过已下载的章节
            if (skipExist && File.Exists(saveFileName))
            {
                return string.Format("已跳过 《{0}》", menu.Title);
            }
            string html = CommonHelper.GetHtml(menu.ComeFrom);
            string content = CommonHelper.GetValue(html, novel.ContentStart, novel.ContentEnd);

            if (html.Contains("操作超时") && content.Length == 0)
            {
                content = "操作超时";
                Console.WriteLine(string.Format("操作超时 - > {0}-{1}", menu.Id, menu.Title));
            }

            CommonHelper.SaveToFile(
                saveFileName,
                modelContent
                .Replace(@"[[title]]", menu.Title)
                .Replace(@"[[Content]]", Regex.Replace(content, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase))
                .Replace(@"Prior.htm", string.Format("{0}.htm", priorMenu == null ? "List" : priorMenu.Id.ToString()))
                .Replace(@"Next.htm", string.Format("{0}.htm", nextMenu == null ? "List" : nextMenu.Id.ToString()))
                );

            menu.DownDate = DateTime.Now;
            menu.DownTime = DateTime.Now;
            menu.WordNums = content.Length;

            return Path.GetFullPath(saveFileName);
        }

        /// <summary>
        /// 查找上一章节
        /// </summary>
        /// <param name="list"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public NovelContent GetPrior(List<NovelContent> list, int menuId)
        {
            var currIndex = list.FindIndex(p => p.Id == menuId);
            if (currIndex <= 0) return null;
            return list[currIndex - 1];
        }
        /// <summary>
        /// 查找下一章节
        /// </summary>
        /// <param name="list"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public NovelContent GetNext(List<NovelContent> list, int menuId)
        {
            var currIndex = list.FindIndex(p => p.Id == menuId);
            if (currIndex < 0) return null;
            if (currIndex >= list.Count - 1) return null;
            return list[currIndex + 1];
        }

        /// <summary>
        /// 查询异常章节列表
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<NovelContent> GetExceptionList(List<NovelContent> list)
        {
            var exlist = new List<NovelContent>();
            if (list.Count <= 2) return exlist;//排除首尾（无需调整）章节
            for (int i = 1, j = list.Count - 1; i < j; i++)
            {
                var menu = list[i];
                var prior = GetPrior(list, menu.Id);
                var next = GetNext(list, menu.Id);
                string fileName = string.Format(FILE_PATH_CHAPTER_MODEL, menu.NovelID, menu.Id);
                var content = CommonHelper.LoadHtmlFile(fileName);
                if (content.Contains("if (event.keyCode==37) document.location= \"List.htm\"")
                    || content.Contains("if (event.keyCode==39) document.location= \"List.htm\""))
                {
                    exlist.Add(menu);
                    CommonHelper.SaveToFile(
                    fileName,
                    content
                    .Replace("if (event.keyCode==37) document.location= \"List.htm\"", string.Format("if (event.keyCode==37) document.location= \"{0}.htm\"", prior == null ? "List" : prior.Id.ToString()))
                    .Replace("if (event.keyCode==39) document.location= \"List.htm\"", string.Format("if (event.keyCode==39) document.location= \"{0}.htm\"", next == null ? "List" : next.Id.ToString()))
                    .Replace("<a href=\"List.htm\" target=\"_top\">上一页</a>", string.Format("<a href=\"{0}.htm\" target=\"_top\">上一页</a>", prior == null ? "List" : prior.Id.ToString()))
                    .Replace("<a href=\"List.htm\" target=\"_top\">下一页</a>", string.Format("<a href=\"{0}.htm\" target=\"_top\">下一页</a>", next == null ? "List" : next.Id.ToString()))
                    );
                }
            }
            return exlist;
        }

        /// <summary>
        /// 判断章节是否已下载存在
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="novelID"></param>
        /// <returns></returns>
        public bool IsDownLoad(string novelID, int menuId)
        {
            string fileName = string.Format(FILE_PATH_CHAPTER_MODEL, novelID, menuId);
            return File.Exists(fileName);
        }

        /// <summary>
        /// 制作电子书
        /// </summary>
        /// <param name="novelId">书籍ID</param>
        /// <returns></returns>
        public string CreateEBook(string novelId, EbookType ebookType)
        {
            string ret = string.Empty;
            switch (ebookType)
            {
                case EbookType.TXT:
                    ret = CreateTXT(novelId);
                    break;
                case EbookType.CHM:
                case EbookType.DOC:
                case EbookType.JAR:
                case EbookType.UMD:
                case EbookType.PDF:
                    throw new ValidationException("暂未支持该功能");
                //break;
                default:
                    break;
            }

            return ret;
        }

        /// <summary>
        /// 生成TXT
        /// </summary>
        /// <param name="novelId"></param>
        /// <returns></returns>
        public string CreateTXT(string novelId)
        {
            var novel = GetNovel(novelId);
            if (novel != null)
            {
                var txtBuilder = new StringBuilder();
                txtBuilder.Append(novel.NovelName);

                var dt = GetNovelContents(novelId);
                var listNovelContent = dt.ToList<NovelContent>();
                int totalCount = listNovelContent.Count;    //总数量
                int processedCount = 0; //已处理数
                foreach (NovelContent menu in listNovelContent)
                {
                    string strFileName = string.Format(FILE_PATH_CHAPTER_MODEL, menu.NovelID, menu.Id);
                    if (File.Exists(strFileName))
                    {
                        string fileContent = CommonHelper.LoadHtmlFile(strFileName);
                        //获取正文内容，处理换行、空格符等
                        string txtContent = CommonHelper.GetValue(fileContent, @"<!--BookContent Start-->", "<!--BookContent End-->")
                            //.Replace("<p>", "\t")
                            .Replace("</p>", "\r\n")
                            .Replace("<br />", "\r\n")
                            .Replace("<br/>", "\r\n")
                            .Replace("&nbsp;", " ")
                            .Replace("\t", "")
                            ;
                        //使用正则剔除html标签
                        txtContent = Regex.Replace(txtContent, "(?is)<.*?>", "");

                        txtBuilder.Append("\r\n");
                        txtBuilder.Append("\r\n");
                        txtBuilder.Append(menu.Title);
                        txtBuilder.Append("\r\n");
                        txtBuilder.Append(txtContent);
                        //break;
                    }
                    processedCount++;
                    //processd = processedCount * 100 / totalCount;
                }
                CommonHelper.CreateDirectory(string.Format(@"txt", ""));
                string fileName = string.Format(@"txt\{0}.txt", novel.NovelName);
                CommonHelper.SaveToFile(fileName, txtBuilder.ToString());
                return Path.GetFullPath(fileName);
            }
            return "";
        }
    }
}
