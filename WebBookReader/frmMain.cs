using MiniBlinkPinvoke;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using BR.Web;

namespace WebBookReader
{
    public partial class frmMain : Form
    {
        static BlinkBrowser mb = null;
        public frmMain()
        {
            InitializeComponent();
            mb = blinkBrowser1;
            blinkBrowser1.GlobalObjectJs = new MBApiHelper(blinkBrowser1);
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //this.blinkBrowser1.Url = "http://www.baidu.com/";
            NavigationLocalUrl(ConfigurationManager.AppSettings["Web.index"].ToString());//@"Web/test.html";
            
        }

        [JSFunctin]
        public void NavigationLocalUrl(string url)
        {
            blinkBrowser1.Url = AppDomain.CurrentDomain.BaseDirectory + url;
        }

        private void frmMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            //MessageBox.Show(e.KeyChar.ToString());
        }

        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12) {
                //blinkBrowser1.ShowDevtools(blinkBrowser1.Url);
                blinkBrowser1.ShowDevtools(Application.StartupPath + @"\front_end\inspector.html");
            }
            //MessageBox.Show(e.KeyCode.ToString());
        }
    }
}
