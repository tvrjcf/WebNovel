namespace WebBookManage
{
    partial class frmCreateEBook
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.rbtnTxt = new System.Windows.Forms.RadioButton();
            this.rbtnChm = new System.Windows.Forms.RadioButton();
            this.rbtnDoc = new System.Windows.Forms.RadioButton();
            this.rbtnJar = new System.Windows.Forms.RadioButton();
            this.rbtnUmd = new System.Windows.Forms.RadioButton();
            this.rbtnPdf = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbtnPdf);
            this.groupBox1.Controls.Add(this.rbtnUmd);
            this.groupBox1.Controls.Add(this.rbtnJar);
            this.groupBox1.Controls.Add(this.rbtnDoc);
            this.groupBox1.Controls.Add(this.rbtnChm);
            this.groupBox1.Controls.Add(this.rbtnTxt);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(188, 237);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "电子书格式";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(206, 22);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(206, 51);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Location = new System.Drawing.Point(206, 226);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(75, 23);
            this.btnSetting.TabIndex = 1;
            this.btnSetting.Text = "设置";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // rbtnTxt
            // 
            this.rbtnTxt.AutoSize = true;
            this.rbtnTxt.Checked = true;
            this.rbtnTxt.Location = new System.Drawing.Point(7, 21);
            this.rbtnTxt.Name = "rbtnTxt";
            this.rbtnTxt.Size = new System.Drawing.Size(65, 16);
            this.rbtnTxt.TabIndex = 0;
            this.rbtnTxt.Text = "TXT格式";
            this.rbtnTxt.UseVisualStyleBackColor = true;
            // 
            // rbtnChm
            // 
            this.rbtnChm.AutoSize = true;
            this.rbtnChm.Location = new System.Drawing.Point(6, 59);
            this.rbtnChm.Name = "rbtnChm";
            this.rbtnChm.Size = new System.Drawing.Size(65, 16);
            this.rbtnChm.TabIndex = 0;
            this.rbtnChm.Text = "CHM格式";
            this.rbtnChm.UseVisualStyleBackColor = true;
            // 
            // rbtnDoc
            // 
            this.rbtnDoc.AutoSize = true;
            this.rbtnDoc.Location = new System.Drawing.Point(6, 97);
            this.rbtnDoc.Name = "rbtnDoc";
            this.rbtnDoc.Size = new System.Drawing.Size(65, 16);
            this.rbtnDoc.TabIndex = 0;
            this.rbtnDoc.Text = "DOC格式";
            this.rbtnDoc.UseVisualStyleBackColor = true;
            // 
            // rbtnJar
            // 
            this.rbtnJar.AutoSize = true;
            this.rbtnJar.Location = new System.Drawing.Point(7, 135);
            this.rbtnJar.Name = "rbtnJar";
            this.rbtnJar.Size = new System.Drawing.Size(65, 16);
            this.rbtnJar.TabIndex = 0;
            this.rbtnJar.Text = "JAR格式";
            this.rbtnJar.UseVisualStyleBackColor = true;
            // 
            // rbtnUmd
            // 
            this.rbtnUmd.AutoSize = true;
            this.rbtnUmd.Location = new System.Drawing.Point(7, 173);
            this.rbtnUmd.Name = "rbtnUmd";
            this.rbtnUmd.Size = new System.Drawing.Size(65, 16);
            this.rbtnUmd.TabIndex = 0;
            this.rbtnUmd.Text = "UMD格式";
            this.rbtnUmd.UseVisualStyleBackColor = true;
            // 
            // rbtnPdf
            // 
            this.rbtnPdf.AutoSize = true;
            this.rbtnPdf.Location = new System.Drawing.Point(6, 211);
            this.rbtnPdf.Name = "rbtnPdf";
            this.rbtnPdf.Size = new System.Drawing.Size(65, 16);
            this.rbtnPdf.TabIndex = 0;
            this.rbtnPdf.Text = "PDF格式";
            this.rbtnPdf.UseVisualStyleBackColor = true;
            // 
            // frmCreateEBook
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 261);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSetting);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmCreateEBook";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "制电子书";
            this.Load += new System.EventHandler(this.frmCreateEBook_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtnPdf;
        private System.Windows.Forms.RadioButton rbtnUmd;
        private System.Windows.Forms.RadioButton rbtnJar;
        private System.Windows.Forms.RadioButton rbtnDoc;
        private System.Windows.Forms.RadioButton rbtnChm;
        private System.Windows.Forms.RadioButton rbtnTxt;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSetting;
    }
}