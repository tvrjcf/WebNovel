using System.Collections.Generic;

namespace WebBookManage.Model
{
    public class SiteSign //: CYQ.Data.Orm.OrmBase
    {
        public string name { get; set; }
        public string url { get; set; }
        public string ListStart { get; set; }
        public string ListEnd { get; set; }
        public string ContentStart { get; set; }
        public string ContentEnd { get; set; }
        public string NeedDelStr { get; set; }
        public string VolumeStart { get; set; }
        public string VolumeEnd { get; set; }
        public string BriefUrlStart { get; set; }
        public string BriefUrlEnd { get; set; }
        public string AuthorStart { get; set; }
        public string AuthorEnd { get; set; }
        public string BriefStart { get; set; }
        public string BriefEnd { get; set; }
        public string BookImgUrlStart { get; set; }
        public string BookImgUrlEnd { get; set; }
    }

    public class Message
    {
        public List<SiteSign> SiteSign;
    }
    public class SiteSignData
    {
        public SiteSignData()
        {
            MESSAGE = new Message() { SiteSign = new List<SiteSign>() };
        }
        public Message MESSAGE;
    }
}
