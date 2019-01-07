namespace WebBookReader
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.blinkBrowser1 = new MiniBlinkPinvoke.BlinkBrowser();
            this.SuspendLayout();
            // 
            // blinkBrowser1
            // 
            this.blinkBrowser1._ZoomFactor = 0F;
            this.blinkBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blinkBrowser1.HTML = "";
            this.blinkBrowser1.JsGetValue = null;
            this.blinkBrowser1.Location = new System.Drawing.Point(0, 0);
            this.blinkBrowser1.Margin = new System.Windows.Forms.Padding(0);
            this.blinkBrowser1.Name = "blinkBrowser1";
            this.blinkBrowser1.Size = new System.Drawing.Size(819, 561);
            this.blinkBrowser1.TabIndex = 0;
            this.blinkBrowser1.Text = "blinkBrowser1";
            this.blinkBrowser1.Url = "";
            this.blinkBrowser1.ZoomFactor = 0F;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 561);
            this.Controls.Add(this.blinkBrowser1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.frmMain_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private MiniBlinkPinvoke.BlinkBrowser blinkBrowser1;
    }
}

