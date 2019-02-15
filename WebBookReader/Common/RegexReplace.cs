using System;
using System.Collections.Generic;
using System.Text;

namespace BR.Common
{
    public class RegexConfig
    {
        public List<RegexReplace> HtmlToString { get; set; }
        public List<RegexReplace> StringToHtml { get; set; }
    }

    public class RegexReplace
    {
        public string pattern { get; set; }
        public string replacement { get; set; }
        public int RegexOptions { get; set; }
    }
}
