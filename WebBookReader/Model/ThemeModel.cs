using System;
using System.Collections.Generic;
using System.Text;

namespace BR.Model
{
    public class ThemeModel
    {
        public ThemeModel(string themeName) {
            ThemeName = themeName;
            ListModelPath = string.Format(@"usermodel\{0}\listmodel.htm", themeName);
            ChapterModelPath = string.Format(@"usermodel\{0}\chaptermodel.htm", themeName);
        }
        public string ThemeName { get; set; }
        public string ListModelPath { get; set; }
        public string ChapterModelPath { get; set; }
        //public string ListModelContent { get; set; }
        //public string ChapterModelContent { get; set; }
        
    }
}
