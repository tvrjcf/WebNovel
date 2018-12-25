using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using LTFrameNet;
using Newtonsoft.Json;

namespace Canvas
{
    public class UIClass
    {
        public LTFrameNetClass ltf;
       
        public void QuitApp()
        {
            ltf.CloseWindow();
        }

        public long DragWindowFun(IntPtr es)
        {
            Lib.ReleaseCapture();
            Point DownPoint =new Point();
            Lib.GetCursorPos(ref DownPoint);
            Lib.SendMessage(ltf.windowHandle(), 0x00A1, 2, (DownPoint.X & 0xFFFF) + (DownPoint.Y & 0xFFFF) * 0x10000);
            return ltf.JsUndefined();
        }

        public long QuitAppFun(IntPtr es)
        {
            ltf.QuitApp();
            return ltf.JsUndefined();
        }
        
        public long MinWindowFun(IntPtr es)
        {
            Lib.ShowWindow(ltf.windowHandle(), Win32DataType.SW_MINIMIZE);
            return ltf.JsUndefined();
        }
        public UIClass()
        {
            int ww = 1024, wh = 700;
            Rectangle rect = Lib.CenterWindow(ww, wh);
            ltf = new LTFrameNetClass("LTFrame-BookReader","JavaScript", IntPtr.Zero, Win32DataType.WS_POPUP | Win32DataType.WS_VISIBLE, rect.Left, rect.Top, ww, wh, IntPtr.Zero);
            ltf.BindUserFunction("DragWindow", DragWindowFun, 1);
            ltf.BindUserFunction("QuitApp", QuitAppFun, 0);
            ltf.BindUserFunction("MinWindow", MinWindowFun, 0);
            
            LTFApiHelper ApiHelper = new LTFApiHelper(ltf);
            ltf.BindUserFunction("f1", f1Fun, 0);
            ltf.BindUserFunction("f2", f2Fun, 0);
            ltf.BindUserFunction("f3", f3Fun, 0);
            ltf.BindUserFunction("f4", f4Fun, 0);
            ltf.BindUserFunction("f5", f5Fun, 1);
            ltf.BindUserFunction("GetNovelTypes", ApiHelper.GetNovelTypes, 0);
            ltf.BindUserFunction("GetNovels", ApiHelper.GetNovels, 0);
            //ltf.loadFile(Lib.GetAppPath + @"./template/javascript.html");
            ltf.loadFile(Lib.GetAppPath + ConfigurationManager.AppSettings["Web.index"].ToString());
            ltf.EnableDragFrameChangeSize(false);
            ltf.MessageLoop();
        }

        WebBookManage.Common.BookHelper BH = new WebBookManage.Common.BookHelper();
        public long GetNovelTypes(IntPtr es)
        {
            var data = BH.GetNovelTypes().ToJson(false, false).Replace("null", "\"\"");
            return ltf.String2JsValue(es, data);
        }

        public long GetNovels(IntPtr es)
        {
            var data = BH.GetNovelTypes().ToJson(false, false).Replace("null", "\"\"");
            //var dt = BH.GetNovels().ToDataTable();
            //var data = JsonConvert.SerializeObject(dt, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Include}).Replace("null", "\"\"");
            //var data = dt.ToJson(false, false).Replace("null", "\"\"");
            //var book = new WebBookManage.Model.Novel(); 
            // var str = @"[{'NovelID': '000001','NeedDelUrl':'', 'NovelName': '莽荒纪-青帝文学网','Author': '','bIsEnd': '0','ListUrl': 'http://www.qingdiba.com/read/294/index.html','ListStart': '</h2>','ListEnd': '<SPAN id=admu_6>','UrlChange': '','ContentStart': '<div id=content>','ContentEnd': '<br>','NeedDelStr': '[[标题]]||()提供本文最新章节在线阅读!','LB': '07','Displayorder': 10,'VolumeStart': '','VolumeEnd': '','Brief': '','BookImg': ''}]";
            return ltf.String2JsValue(es, data);    //"[]"
        }

        public long f1Fun(IntPtr es)
        {
            ltf.RunJavaScript("alert('C#执行了JS')");
            return ltf.JsUndefined();
        }
        public long f2Fun(IntPtr es)
        {
            return ltf.String2JsValue(es, "JS 调用C# 返回了值");
        }
        public long f3Fun(IntPtr es)
        {
            long t = ltf.CreateObject(es);

            IntPtr str1 = Marshal.StringToHGlobalAnsi("avalue");
            ltf.CreateJsObject(es, t, str1,ltf.Int2JsValue(12));
            IntPtr str2 = Marshal.StringToHGlobalAnsi("bvalue");
            ltf.CreateJsObject(es, t, str2,  ltf.Int2JsValue(22));
            return t;
        }
        public long f4Fun(IntPtr es)
        {
	        long fun = ltf.GetJsParameter(es,0);
	        long arg =ltf.String2JsValue(es,"C#执行了回掉函数");
	        long[] ptarg = {arg};
            IntPtr arrayStore = Marshal.AllocHGlobal(ptarg.Length * sizeof(long));
            Marshal.Copy(ptarg, 0, arrayStore, ptarg.Length);
            ltf.JsCall(es, fun, arrayStore, 1); 
	        return ltf.JsUndefined();
        }
        public long f5Fun(IntPtr es)
        {
            long obj = ltf.GetJsParameter(es,0);
	        IntPtr exec = ltf.GetGlobalExecState();
            long v = ltf.GetJsObjectAttribute(exec, obj, "a");
            long xc = ltf.GetJsObjectAttribute(exec, obj, "b");
	        int i1 = ltf.JsValue2Int(exec,v);
	        int i2 = ltf.JsValue2Int(exec,xc);
	        string str = string.Format("alert('a:{0},b:{1}')",i1,i2);
	        ltf.RunJavaScript(str);
            return ltf.JsUndefined();
        }
 
    }
}
