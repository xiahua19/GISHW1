
namespace FSGIS.Forms
{
    partial class SelectByAttri
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectByAttri));
            this.LayerNameLbl = new System.Windows.Forms.Label();
            this.FieldsList = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.SQL = new System.Windows.Forms.Label();
            this.QuerySentence = new System.Windows.Forms.RichTextBox();
            this.APPLY = new System.Windows.Forms.Button();
            this.YES = new System.Windows.Forms.Button();
            this.NO = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.LayerName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LayerNameLbl
            // 
            this.LayerNameLbl.AutoSize = true;
            this.LayerNameLbl.Location = new System.Drawing.Point(25, 28);
            this.LayerNameLbl.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LayerNameLbl.Name = "LayerNameLbl";
            this.LayerNameLbl.Size = new System.Drawing.Size(67, 15);
            this.LayerNameLbl.TabIndex = 1;
            this.LayerNameLbl.Text = "当前图层";
            // 
            // FieldsList
            // 
            this.FieldsList.Alignment = System.Windows.Forms.ListViewAlignment.Default;
            this.FieldsList.GridLines = true;
            this.FieldsList.HideSelection = false;
            this.FieldsList.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.FieldsList.Location = new System.Drawing.Point(28, 131);
            this.FieldsList.Margin = new System.Windows.Forms.Padding(4);
            this.FieldsList.Name = "FieldsList";
            this.FieldsList.Size = new System.Drawing.Size(163, 192);
            this.FieldsList.TabIndex = 2;
            this.FieldsList.UseCompatibleStateImageBehavior = false;
            this.FieldsList.View = System.Windows.Forms.View.List;
            this.FieldsList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.FieldsList_MouseDoubleClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 101);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "字段集合";
            // 
            // SQL
            // 
            this.SQL.AutoSize = true;
            this.SQL.Location = new System.Drawing.Point(225, 101);
            this.SQL.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SQL.Name = "SQL";
            this.SQL.Size = new System.Drawing.Size(67, 15);
            this.SQL.TabIndex = 4;
            this.SQL.Text = "查询语句";
            // 
            // QuerySentence
            // 
            this.QuerySentence.Location = new System.Drawing.Point(228, 150);
            this.QuerySentence.Margin = new System.Windows.Forms.Padding(4);
            this.QuerySentence.Name = "QuerySentence";
            this.QuerySentence.Size = new System.Drawing.Size(256, 110);
            this.QuerySentence.TabIndex = 5;
            this.QuerySentence.Text = "";
            // 
            // APPLY
            // 
            this.APPLY.Location = new System.Drawing.Point(228, 282);
            this.APPLY.Margin = new System.Windows.Forms.Padding(4);
            this.APPLY.Name = "APPLY";
            this.APPLY.Size = new System.Drawing.Size(80, 41);
            this.APPLY.TabIndex = 8;
            this.APPLY.Text = "应用";
            this.APPLY.UseVisualStyleBackColor = true;
            this.APPLY.Click += new System.EventHandler(this.APPLY_Click);
            // 
            // YES
            // 
            this.YES.Location = new System.Drawing.Point(316, 282);
            this.YES.Margin = new System.Windows.Forms.Padding(4);
            this.YES.Name = "YES";
            this.YES.Size = new System.Drawing.Size(80, 41);
            this.YES.TabIndex = 9;
            this.YES.Text = "确定";
            this.YES.UseVisualStyleBackColor = true;
            this.YES.Click += new System.EventHandler(this.YES_Click);
            // 
            // NO
            // 
            this.NO.Location = new System.Drawing.Point(405, 282);
            this.NO.Margin = new System.Windows.Forms.Padding(4);
            this.NO.Name = "NO";
            this.NO.Size = new System.Drawing.Size(80, 41);
            this.NO.TabIndex = 10;
            this.NO.Text = "取消";
            this.NO.UseVisualStyleBackColor = true;
            this.NO.Click += new System.EventHandler(this.NO_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(225, 131);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 15);
            this.label3.TabIndex = 11;
            this.label3.Text = "SELECT * WHERE";
            // 
            // LayerName
            // 
            this.LayerName.Location = new System.Drawing.Point(28, 60);
            this.LayerName.Margin = new System.Windows.Forms.Padding(4);
            this.LayerName.Name = "LayerName";
            this.LayerName.Size = new System.Drawing.Size(456, 25);
            this.LayerName.TabIndex = 12;
            // 
            // SelectByAttri
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 360);
            this.Controls.Add(this.LayerName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.NO);
            this.Controls.Add(this.YES);
            this.Controls.Add(this.APPLY);
            this.Controls.Add(this.QuerySentence);
            this.Controls.Add(this.SQL);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FieldsList);
            this.Controls.Add(this.LayerNameLbl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SelectByAttri";
            this.Text = "按属性选择";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label LayerNameLbl;
        private System.Windows.Forms.ListView FieldsList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label SQL;
        private System.Windows.Forms.RichTextBox QuerySentence;
        private System.Windows.Forms.Button APPLY;
        private System.Windows.Forms.Button YES;
        private System.Windows.Forms.Button NO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox LayerName;
    }
}