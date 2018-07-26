using DotNet4.Utilities;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace WebBookManage.Common
{
    public static class CommonHelper
    {
        /// <summary>
        /// 通过网址获取网页内容
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtml(string url)
        {
            HttpHelper http = new HttpHelper();
            HttpItem item = new HttpItem()
            {
                URL = url,
                Timeout = 10000,
                ReadWriteTimeout = 10000
            };
            HttpResult result = http.GetHtml(item);
            string html = result.Html;
            //string cookie = result.Cookie;
            //string urlll = result.RedirectUrl;

            return html;
        }

        /// <summary>
        /// 获得字符串中开始和结束字符串中间得值
        /// </summary>
        /// <param name="str">字符串</param>
        /// <param name="s">开始</param>
        /// <param name="e">结束</param>
        /// <returns></returns> 
        public static string GetValue(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s.Replace("\r", "") + "))[.\\s\\S]*?(?=(" + e.Replace("\r", "") + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

        /// <summary>
        /// 判断一个字符串是否为url
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsUrl(string str)
        {
            try
            {
                string Url = @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$";
                bool isUrl = Regex.IsMatch(str, Url);
                return isUrl;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 获取根网址
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetWebRootUrl(string url)
        {
            if (!IsUrl(url))
                return "";
            if (url.Substring(url.Length - 1, 1) != "/")
                url += "/";
            string rootUrl = url.Substring(0, url.IndexOf("/", 8)) + "/";
            return rootUrl;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="folder"></param>
        public static void CreateDirectory(string folder)
        {
            if (!Directory.Exists(folder))//判断文件夹是否存在 
            {
                Directory.CreateDirectory(folder);//不存在则创建文件夹 
            }
        }
        /// <summary>
        /// 保存html内容
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileContent"></param>
        public static void SaveToFile(string fileName, string fileContent)
        {
            SaveToFile(fileName, fileContent, Encoding.Default);
        }
        public static void SaveToFile(string fileName, string fileContent, Encoding encoding)
        {
            File.WriteAllText(fileName, fileContent, encoding);
        }
        /// <summary>
        /// 读取html模板
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string LoadHtmlFile(string fileName)
        {
            return LoadHtmlFile(fileName, Encoding.Default);
        }
        public static string LoadHtmlFile(string fileName, Encoding encoding)
        {
            string html = "";
            try
            {
                //string path = String.Format(@"log\log_email{0:yyyyMM}.txt", DateTime.Now);

                //string folder = "log";
                //string path = string.Format(@"{1}\log_email_{0}.txt", DateTime.Now.ToString("yyyyMM"), folder);
                //if (!Directory.Exists(folder))//判断文件夹是否存在 
                //{
                //    Directory.CreateDirectory(folder);//不存在则创建文件夹 
                //}

                html = File.ReadAllText(fileName, encoding);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //MessageBox.Show(ex.Message);
                //this.tsslb_state.Text = ex.Message;
            }
            return html;
        }


    }
}
