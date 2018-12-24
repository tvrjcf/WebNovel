namespace WebBookManage.Model
{
    public class NovelType : CYQ.Data.Orm.OrmBase
    {
        public NovelType()
        {
            base.SetInit(this, "dic_noveltype");
        }
        public string DM { get; set; }
        public string MC { get; set; }
        public string TopDM { get; set; }
    }
}
