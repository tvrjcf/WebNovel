namespace WebBookManage
{
    partial class frmDownMenuList
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
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dvList = new System.Windows.Forms.DataGridView();
            this.ckbIsSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Title = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Volume = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ComeFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ckbSetToFilter = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnFilterList = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDownContent = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvList)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSearch);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dvList);
            this.groupBox1.Location = new System.Drawing.Point(3, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(615, 372);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(305, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "在这里你可以选择需要下载的链接, 点击左边的方框选择\r\n";
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
            this.Id,
            this.Title,
            this.Volume,
            this.ComeFrom});
            this.dvList.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dvList.GridColor = System.Drawing.SystemColors.Control;
            this.dvList.Location = new System.Drawing.Point(3, 43);
            this.dvList.Name = "dvList";
            this.dvList.RowTemplate.Height = 23;
            this.dvList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dvList.Size = new System.Drawing.Size(609, 326);
            this.dvList.TabIndex = 0;
            // 
            // ckbIsSelect
            // 
            this.ckbIsSelect.DataPropertyName = "IsSelect";
            this.ckbIsSelect.HeaderText = "";
            this.ckbIsSelect.Name = "ckbIsSelect";
            this.ckbIsSelect.Width = 30;
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
            // ckbSetToFilter
            // 
            this.ckbSetToFilter.AutoSize = true;
            this.ckbSetToFilter.Location = new System.Drawing.Point(6, 395);
            this.ckbSetToFilter.Name = "ckbSetToFilter";
            this.ckbSetToFilter.Size = new System.Drawing.Size(132, 16);
            this.ckbSetToFilter.TabIndex = 1;
            this.ckbSetToFilter.Text = "未选中链接自动过滤";
            this.ckbSetToFilter.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(627, 55);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "选择";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(627, 84);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "不选";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnFilterList
            // 
            this.btnFilterList.Location = new System.Drawing.Point(627, 358);
            this.btnFilterList.Name = "btnFilterList";
            this.btnFilterList.Size = new System.Drawing.Size(75, 23);
            this.btnFilterList.TabIndex = 2;
            this.btnFilterList.Text = "过滤列表";
            this.btnFilterList.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(443, 391);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(540, 391);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnDownContent
            // 
            this.btnDownContent.Location = new System.Drawing.Point(627, 329);
            this.btnDownContent.Name = "btnDownContent";
            this.btnDownContent.Size = new System.Drawing.Size(75, 23);
            this.btnDownContent.TabIndex = 2;
            this.btnDownContent.Text = "下载内容";
            this.btnDownContent.UseVisualStyleBackColor = true;
            this.btnDownContent.Click += new System.EventHandler(this.btnDownContent_Click);
            // 
            // frmDownMenuList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 427);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnDownContent);
            this.Controls.Add(this.btnFilterList);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ckbSetToFilter);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDownMenuList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "章节更新";
            this.Load += new System.EventHandler(this.frmDownMenuList_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dvList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dvList;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox ckbSetToFilter;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnFilterList;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ckbIsSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Title;
        private System.Windows.Forms.DataGridViewTextBoxColumn Volume;
        private System.Windows.Forms.DataGridViewTextBoxColumn ComeFrom;
        private System.Windows.Forms.Button btnDownContent;
    }
}