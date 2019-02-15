using System;

namespace BR.Model
{
    public class NovelContent : CYQ.Data.Orm.OrmBase
    {
        public NovelContent()
        {
            base.SetInit(this, "book_NovelContent");
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string NovelID { get; set; }
        public string ComeFrom { get; set; }
        public DateTime DownDate { get; set; }
        public DateTime DownTime { get; set; }
        public int bIsRead { get; set; }
        public int Displayorder { get; set; }
        public string Volume { get; set; }
        public int WordNums { get; set; }
        public string BookNote { get; set; }
        public DateTime LastReadTime { get; set; }
        public int Pos { get; set; }
        public int ChpType { get; set; }
    }
}
