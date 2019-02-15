using System.Collections.Generic;

namespace BR.Model
{
    public class Novel : CYQ.Data.Orm.OrmBase
    {
        public Novel()
        {
            base.SetInit(this, "book_Novel");
        }
        public string NovelID { get; set; }
        public string NovelName { get; set; }
        public string Author { get; set; }
        public bool bIsEnd { get; set; }
        public string ListUrl { get; set; }
        public string ListStart { get; set; }
        public string ListEnd { get; set; }
        public string UrlChange { get; set; }
        public string ContentStart { get; set; }
        public string ContentEnd { get; set; }
        public string NeedDelStr { get; set; }
        public string LB { get; set; }
        public int Displayorder { get; set; }
        public string VolumeStart { get; set; }
        public string VolumeEnd { get; set; }
        public string NeedDelUrl { get; set; }
        public string Brief { get; set; }
        public string BookImg { get; set; }
        public List<NovelContent> ChapterList { get; set; }
    }
}
