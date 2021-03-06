﻿using CYQ.Data;
using CYQ.Data.Table;
using CYQ.Data.Tool;
using CYQ.Data.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using BR.Model;

namespace BR.Common
{
    public class BookHelper
    {
        public static SaveType SAVE_TYPE = SaveType.Txt;
        public static string DIR_PATH_NOVEL = @"chm\{0}";    //
        public static string FILE_PATH_LIST_MODEL = @"chm\{0}\List.htm";    //
        public static string FILE_PATH_CHAPTER_MODEL = @"chm\{0}\{1}.htm";  //
        public static string FILE_PATH_LIST_JOSN = @"chm\{0}\list.json";    //
        public static string FILE_PATH_CHAPTER_JOSN = @"chm\{0}\{1}.json";  //
        public static string FILE_PATH_LIST_TXT = @"chm\{0}\list.json";    //
        public static string FILE_PATH_CHAPTER_TXT = @"chm\{0}\{1}.txt";  //
        public static string FILE_PATH_CHARTS_LIST_JSON = @"charts\{0}\list";    //
        public static string FILE_PATH_CHARTS_CHAPTER_JSON = @"charts\{0}\{1}";  //
        private string T_Novel = "book_Novel";
        private string T_NovelContent = "book_NovelContent";

        public BookHelper()
        {
            JsonConvert.DefaultSettings = () =>
            {
                var setting = new JsonSerializerSettings();
                setting.TypeNameHandling = TypeNameHandling.Auto;
                setting.TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Simple;
                setting.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                setting.NullValueHandling = NullValueHandling.Ignore;
                setting.Formatting = Newtonsoft.Json.Formatting.Indented;
                setting.Converters.Add(new JavaScriptDateTimeConverter());
                //setting.Converters.Add(new DomainJsonConverter());
                return setting;
            };
            AppConfig.SetConn("Conn", @"Provider=Microsoft.Jet.OLEDB.4.0; Data Source=db\pim.mdb");// providerName = "System.Data.OleDb";
            Console.WriteLine("new BookHelper()");
        }

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
            var dt = model.Select(string.Format("NovelID='{0}' order by Id", novelId));
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
        /// 
        /// </summary>
        /// <param name="chartsUrl"></param>
        /// <param name="hrefReg">链接特征</param>
        /// <returns></returns>
        public List<Novel> GetChartsList(string chartsUrl, string hrefPattern)
        {
            var rootUrl = CommonHelper.GetWebRootUrl(chartsUrl);
            string html = CommonHelper.GetHtml(chartsUrl);
            var list = new List<Novel>();
            //string prttern = "<a(\\s+(href=\"(?<url>([^\"])*)\"|'([^'])*'|\\w+=\"(([^\"])*)\"|'([^'])*'))+>(?<text>(.*?))</a>";
            string prttern = "<a(\\s+(href=\"(?<url>([^\"])*)\"|'([^'])*'|\\w+=\"(([^\"])*)\"|'([^'])*'))+>(?<text>(.*?))</a>";
            var maths = Regex.Matches(html, prttern);
            var hrefReg = new Regex(hrefPattern, RegexOptions.IgnoreCase);
            for (int i = 0; i < maths.Count; i++)
            {
                string url = new Uri(new Uri(rootUrl), maths[i].Groups["url"].Value).ToString();
                string text = maths[i].Groups["text"].Value;
                if (url.Contains(".htm") || url.Contains(".html"))
                    continue;
                if (url.LastIndexOf('/') != url.Length - 1)
                    continue;
                if (!string.IsNullOrEmpty(hrefPattern) && !hrefReg.IsMatch(url) || string.IsNullOrEmpty(CommonHelper.HtmlToText(text)))
                    continue;
                if (list.FindIndex(p => p.ListUrl == url.Trim()) >= 0)
                    continue;
                var novel = new Novel()
                {
                    NovelID = Guid.NewGuid().ToString("N"),
                    ListUrl = url.Trim(),
                    NovelName = text,
                    Author = "",
                    LB = "07",
                    Brief = text
                };
                list.Add(novel);

            }
            return list;

            throw new NotImplementedException();
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
            return dt.AcceptChanges(AcceptOp.Auto);
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
            string prttern = "<a(\\s+(href\\s*=\"(?<url>([^\"])*)\"|'([^'])*'|\\w+=\"(([^\"])*)\"|'([^'])*'))+>(?<text>(.*?))</a>";
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
        /// 查询网站正文设定标志列表(xml文件)
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
                GC.Collect();
            }
            xml = null;
            return newNode;
        }

        /// <summary>
        /// 查询网站正文设定标志数据(json文件)
        /// </summary>
        /// <returns></returns>
        public SiteSignData GetSiteSigns()
        {
            var saveFile = @"SiteSign.json";
            var list = new SiteSignData();
            if (!File.Exists(saveFile)) return list;
            //var xml = new XmlDocument();
            //xml.Load(@"SiteSign.xml");
            //var json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(xml.DocumentElement);
            var json = File.ReadAllText(saveFile, Encoding.UTF8);
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<SiteSignData>(json);
            return data;
        }

        /// <summary>
        /// 保存正文标志(json文件)
        /// </summary>
        /// <param name="sign"></param>
        public void SignToJson(SiteSign sign)
        {
            var data = GetSiteSigns();
            var index = data.MESSAGE.SiteSign.FindIndex(p => p.url.Contains(sign.url));
            if (index != -1)
            {
                data.MESSAGE.SiteSign.RemoveAt(index);
                //data.MESSAGE.SiteSign.Insert(index, sign);
            }
            else
                index = 0;
            data.MESSAGE.SiteSign.Insert(index, sign);
            //var setting = new Newtonsoft.Json.JsonSerializerSettings() { Formatting = Newtonsoft.Json.Formatting.Indented };
            var json = JsonConvert.SerializeObject(data);
            CommonHelper.SaveToFile(@"SiteSign.json", json, Encoding.UTF8);
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
            var list = GetSiteSigns().MESSAGE.SiteSign;// GetSiteSignList();
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

            //保存为json文件
            novel.ChapterList = new List<NovelContent>();
            novel.ChapterList.AddRange(menulist);
            saveFileName = GetListFileName(SAVE_TYPE, novel.NovelID);
            CommonHelper.SaveToFile(saveFileName, JsonHelper.ToJson(novel), Encoding.UTF8);
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
        public string SaveNovelContentToHtml(Novel novel, ref NovelContent menu, string modelContent, NovelContent priorMenu, NovelContent nextMenu, ref string downLoadMsg, bool skipExist = true)
        {
            //string saveFileName = string.Format(FILE_PATH_CHAPTER_MODEL, menu.NovelID, menu.Id);
            string saveFileName = GetChapterFileName(SAVE_TYPE, menu.NovelID, menu.Id);
            //跳过已下载的章节
            if (skipExist && File.Exists(saveFileName))
            {
                downLoadMsg = "已跳过";
                return string.Format("已跳过 《{0}》", menu.Title);
            }
            string html = CommonHelper.GetHtml(menu.ComeFrom);
            string content = CommonHelper.GetValue(html, novel.ContentStart, novel.ContentEnd);
            content = Regex.Replace(content, novel.NeedDelStr, "", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            if (html.Contains("操作超时") && content.Length == 0)
            {
                downLoadMsg = "操作超时";
                content = "操作超时";
                Console.WriteLine(string.Format("操作超时 - > {0}-{1}", menu.Id, menu.Title));
            }


            menu.DownDate = DateTime.Now;
            menu.DownTime = DateTime.Now;
            menu.WordNums = content.Length;

            CommonHelper.SaveToFile(
                saveFileName,
                modelContent
                .Replace(@"[[title]]", menu.Title)
                .Replace(@"[[Content]]", content)
                .Replace(@"Prior.htm", string.Format("{0}.htm", priorMenu == null ? "List" : priorMenu.Id.ToString()))
                .Replace(@"Next.htm", string.Format("{0}.htm", nextMenu == null ? "List" : nextMenu.Id.ToString()))
                );
            //保存为json文件

            content = CommonHelper.HtmlToText(content);

            //content = Regex.Replace(content, @"<\s*/p>", "\r\n", RegexOptions.IgnoreCase);
            //content = Regex.Replace(content, @"<br\s*/>", "\r\n", RegexOptions.IgnoreCase);
            //content = Regex.Replace(content, @"&nbsp;", " ", RegexOptions.IgnoreCase);
            //content = Regex.Replace(content, @"\t", "", RegexOptions.IgnoreCase);
            CommonHelper.SaveToFile(saveFileName, content, Encoding.UTF8);

            return Path.GetFullPath(saveFileName);
        }

        public string SaveChartsContentToJson(ref NovelContent menu, ref string content, ref string downLoadMsg, bool skipExist = true)
        {
            //string saveFileName = string.Format(FILE_PATH_CHAPTER_MODEL, menu.NovelID, menu.Id);
            string saveFileName = GetChapterFileName(SAVE_TYPE, menu.NovelID, menu.Id, true);
            saveFileName = saveFileName +" "+ menu.Title;
            //跳过已下载的章节
            if (skipExist && File.Exists(saveFileName))
            {
                downLoadMsg = "已跳过";
                return string.Format("已跳过 《{0}》", menu.Title);
            }
            string html = CommonHelper.GetHtml(menu.ComeFrom);
            var sign = GetSiteSign(menu.ComeFrom);
            if (sign == null)
            {
                downLoadMsg = string.Format("未维护章节标志 《{0}》", menu.Title);
                return string.Format("未维护章节标志 《{0}》", menu.Title);
            }
            content = CommonHelper.GetValue(html, sign.ContentStart, sign.ContentEnd);
            content = Regex.Replace(content, sign.NeedDelStr, "", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);

            if (html.Contains("操作超时") && content.Length == 0)
            {
                downLoadMsg = "操作超时";
                content = "操作超时";
                Console.WriteLine(string.Format("操作超时 - > {0}-{1}", menu.Id, menu.Title));
            }

            menu.DownDate = DateTime.Now;
            menu.DownTime = DateTime.Now;
            menu.WordNums = content.Length;

            //保存为json文件
            content = CommonHelper.HtmlToText(content);
            content = "\r\n\r\n" + menu.Title + "\r\n\r\n" + content;
            CommonHelper.SaveToFile(saveFileName, content, Encoding.UTF8);

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
            string fileName = GetChapterFileName(SAVE_TYPE, novelID, menuId);

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
                    //string strFileName = string.Format(FILE_PATH_CHAPTER_MODEL, menu.NovelID, menu.Id);
                    string strFileName = GetChapterFileName(SAVE_TYPE, menu.NovelID, menu.Id);
                    if (File.Exists(strFileName))
                    {
                        string fileContent = CommonHelper.LoadHtmlFile(strFileName);
                        ////获取正文内容，处理换行、空格符等
                        //string txtContent = CommonHelper.GetValue(fileContent, @"<!--BookContent Start-->", "<!--BookContent End-->");
                        //    //.Replace("<p>", "\t")
                        //    .Replace("</p>", "\r\n")
                        //    .Replace("<br />", "\r\n")
                        //    .Replace("<br/>", "\r\n")
                        //    .Replace("&nbsp;", " ")
                        //    .Replace("\t", "")
                        //    ;
                        ////使用正则剔除html标签
                        //txtContent = Regex.Replace(txtContent, "(?is)<.*?>", "");
                        string txtContent = CommonHelper.HtmlToText(fileContent);
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
                CommonHelper.SaveToFile(fileName, txtBuilder.ToString(), Encoding.UTF8);
                return Path.GetFullPath(fileName);
            }
            return "";
        }

        /// <summary>
        /// 获取目录文件路径
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="novelID"></param>
        /// <returns></returns>
        public string GetListFileName(SaveType saveType, string novelID, bool isCharts = false)
        {
            string fileName = string.Empty;
            switch (saveType)
            {
                case SaveType.Html:
                    fileName = string.Format(FILE_PATH_LIST_MODEL, novelID); ;
                    break;
                case SaveType.Txt:
                    fileName = string.Format(FILE_PATH_LIST_TXT, novelID);
                    break;
                case SaveType.Json:
                    fileName = string.Format(FILE_PATH_LIST_JOSN, novelID);
                    break;
                default:
                    break;
            }
            if (isCharts)
            {

                switch (saveType)
                {
                    case SaveType.Html:
                        fileName = string.Format(FILE_PATH_LIST_MODEL, novelID); ;
                        break;
                    case SaveType.Txt:
                        fileName = string.Format(FILE_PATH_CHARTS_LIST_JSON, novelID);
                        break;
                    case SaveType.Json:
                        fileName = string.Format(FILE_PATH_CHARTS_LIST_JSON, novelID);
                        break;
                    default:
                        break;
                }
            }
            return fileName;
        }

        /// <summary>
        /// 获取章节文件路径
        /// </summary>
        /// <param name="saveType"></param>
        /// <param name="novelID"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public string GetChapterFileName(SaveType saveType, string novelID, int menuId, bool isCharts = false)
        {
            string fileName = string.Empty;
            switch (saveType)
            {
                case SaveType.Html:
                    fileName = string.Format(FILE_PATH_CHAPTER_MODEL, novelID, menuId);
                    break;
                case SaveType.Txt:
                    fileName = string.Format(FILE_PATH_CHAPTER_TXT, novelID, menuId);
                    break;
                case SaveType.Json:
                    fileName = string.Format(FILE_PATH_CHAPTER_JOSN, novelID, menuId);
                    break;
                default:
                    break;
            }
            if (isCharts)
            {

                switch (saveType)
                {
                    case SaveType.Html:
                        fileName = string.Format(FILE_PATH_CHAPTER_MODEL, novelID, menuId);
                        break;
                    case SaveType.Txt:
                        fileName = string.Format(FILE_PATH_CHARTS_CHAPTER_JSON, novelID, menuId);
                        break;
                    case SaveType.Json:
                        fileName = string.Format(FILE_PATH_CHARTS_CHAPTER_JSON, novelID, menuId);
                        break;
                    default:
                        break;
                }
            }
            return fileName;
        }
    }

    public enum SaveType
    {
        Html = 0,
        Txt = 1,
        Json = 2
    }
}
