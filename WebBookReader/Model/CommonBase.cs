namespace WebBookManage.Model
{
    public abstract class CommonBase
    {
        public bool IsSelect { get; set; }
        public DownLoadSate DownLoadSate { get; set; }
    }

    public enum DownLoadSate {
        Ready = 0,
        Downloading = 1,
        Completed = 2
    }
}
