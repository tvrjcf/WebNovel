namespace WebBookManage
{
    partial class frmDownContent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDownContent));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ckbSkip = new System.Windows.Forms.CheckBox();
            this.pbComplete = new System.Windows.Forms.ProgressBar();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnDownLoadSelect = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnSelectRepeat = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbTip = new System.Windows.Forms.Label();
            this.dvList = new System.Windows.Forms.DataGridView();
            this.ckbIsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Volume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComeFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnArrange = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ckbSkip);
            this.groupBox1.Controls.Add(this.pbComplete);
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.btnArrange);
            this.groupBox1.Controls.Add(this.btnDownLoadSelect);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.btnSelectRepeat);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lbTip);
            this.groupBox1.Controls.Add(this.dvList);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(707, 384);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // ckbSkip
            // 
            this.ckbSkip.AutoSize = true;
            this.ckbSkip.Checked = true;
            this.ckbSkip.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckbSkip.Location = new System.Drawing.Point(626, 262);
            this.ckbSkip.Name = "ckbSkip";
            this.ckbSkip.Size = new System.Drawing.Size(72, 16);
            this.ckbSkip.TabIndex = 8;
            this.ckbSkip.Text = "跳过已有";
            this.ckbSkip.UseVisualStyleBackColor = true;
            // 
            // pbComplete
            // 
            this.pbComplete.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pbComplete.Location = new System.Drawing.Point(3, 371);
            this.pbComplete.Name = "pbComplete";
            this.pbComplete.Size = new System.Drawing.Size(701, 10);
            this.pbComplete.TabIndex = 7;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(624, 342);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 6;
            this.btnStop.Text = "停止";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnDownLoadSelect
            // 
            this.btnDownLoadSelect.Location = new System.Drawing.Point(624, 284);
            this.btnDownLoadSelect.Name = "btnDownLoadSelect";
            this.btnDownLoadSelect.Size = new System.Drawing.Size(75, 23);
            this.btnDownLoadSelect.TabIndex = 6;
            this.btnDownLoadSelect.Text = "下载选中";
            this.btnDownLoadSelect.UseVisualStyleBackColor = true;
            this.btnDownLoadSelect.Click += new System.EventHandler(this.btnDownLoadSelect_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(624, 313);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 6;
            this.btnStart.Text = "下载全部";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnSelectRepeat
            // 
            this.btnSelectRepeat.Location = new System.Drawing.Point(624, 101);
            this.btnSelectRepeat.Name = "btnSelectRepeat";
            this.btnSelectRepeat.Size = new System.Drawing.Size(75, 23);
            this.btnSelectRepeat.TabIndex = 4;
            this.btnSelectRepeat.Text = "选择重复";
            this.btnSelectRepeat.UseVisualStyleBackColor = true;
            this.btnSelectRepeat.Click += new System.EventHandler(this.btnSelectRepeat_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(624, 133);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "删除选中";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(624, 72);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "不选";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(624, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "选择";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(463, 15);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(146, 21);
            this.txtSearch.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(388, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "按标题查找:";
            // 
            // lbTip
            // 
            this.lbTip.AutoSize = true;
            this.lbTip.Location = new System.Drawing.Point(8, 19);
            this.lbTip.Name = "lbTip";
            this.lbTip.Size = new System.Drawing.Size(305, 12);
            this.lbTip.TabIndex = 1;
            this.lbTip.Text = "在这里你可以选择需要下载的链接, 点击左边的方框选择\r\n";
            // 
            // dvList
            // 
            this.dvList.AllowUserToAddRows = false;
            this.dvList.AllowUserToDeleteRows = false;
            this.dvList.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dvList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dvList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dvList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ckbIsSelect,
            this.State,
            this.Id,
            this.Title,
            this.Volume,
            this.ComeFrom});
            this.dvList.GridColor = System.Drawing.SystemColors.Control;
            this.dvList.Location = new System.Drawing.Point(3, 43);
            this.dvList.Name = "dvList";
            this.dvList.RowTemplate.Height = 23;
            this.dvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dvList.Size = new System.Drawing.Size(609, 326);
            this.dvList.TabIndex = 0;
            this.dvList.SelectionChanged += new System.EventHandler(this.dvList_SelectionChanged);
            // 
            // ckbIsSelect
            // 
            this.ckbIsSelect.DataPropertyName = "IsSelect";
            this.ckbIsSelect.HeaderText = "";
            this.ckbIsSelect.Name = "ckbIsSelect";
            this.ckbIsSelect.Width = 30;
            // 
            // State
            // 
            this.State.DataPropertyName = "State";
            this.State.HeaderText = "状态";
            this.State.Name = "State";
            this.State.Width = 60;
            // 
            // Id
            // 
            this.Id.DataPropertyName = "Id";
            this.Id.HeaderText = "Id";
            this.Id.Name = "Id";
            this.Id.Visible = false;
            // 
            // Title
            // 
            this.Title.DataPropertyName = "Title";
            this.Title.HeaderText = "标题";
            this.Title.Name = "Title";
            this.Title.Width = 200;
            // 
            // Volume
            // 
            this.Volume.DataPropertyName = "Volume";
            this.Volume.HeaderText = "分卷名";
            this.Volume.Name = "Volume";
            this.Volume.Width = 200;
            // 
            // ComeFrom
            // 
            this.ComeFrom.DataPropertyName = "ComeFrom";
            this.ComeFrom.HeaderText = "网址";
            this.ComeFrom.Name = "ComeFrom";
            this.ComeFrom.Width = 400;
            // 
            // btnArrange
            // 
            this.btnArrange.Location = new System.Drawing.Point(623, 198);
            this.btnArrange.Name = "btnArrange";
            this.btnArrange.Size = new System.Drawing.Size(75, 23);
            this.btnArrange.TabIndex = 6;
            this.btnArrange.Text = "整理目录";
            this.btnArrange.UseVisualStyleBackColor = true;
            this.btnArrange.Click += new System.EventHandler(this.btnArrange_Click);
            // 
            // frmDownContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 384);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmDownContent";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "下载内容";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownContent_FormClosing);
            this.Load += new System.EventHandler(this.frmDownContent_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvList)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbTip;
        private System.Windows.Forms.DataGridView dvList;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ckbIsSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Volume;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComeFrom;
        private System.Windows.Forms.ProgressBar pbComplete;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox ckbSkip;
        private System.Windows.Forms.Button btnDownLoadSelect;
        private System.Windows.Forms.Button btnSelectRepeat;
        private System.Windows.Forms.Button btnArrange;
    }
}