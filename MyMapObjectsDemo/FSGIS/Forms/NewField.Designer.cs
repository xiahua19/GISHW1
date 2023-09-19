
namespace FSGIS.Forms
{
    partial class NewField
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewField));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FieldNameBox = new System.Windows.Forms.TextBox();
            this.FieldAliasBox = new System.Windows.Forms.TextBox();
            this.FieldTypeBox = new System.Windows.Forms.ComboBox();
            this.YES = new System.Windows.Forms.Button();
            this.NO = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.LayerName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 84);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "字段名称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 141);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "字段别名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 204);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "字段类型";
            // 
            // FieldNameBox
            // 
            this.FieldNameBox.Location = new System.Drawing.Point(103, 80);
            this.FieldNameBox.Margin = new System.Windows.Forms.Padding(4);
            this.FieldNameBox.Name = "FieldNameBox";
            this.FieldNameBox.Size = new System.Drawing.Size(160, 25);
            this.FieldNameBox.TabIndex = 3;
            // 
            // FieldAliasBox
            // 
            this.FieldAliasBox.Location = new System.Drawing.Point(103, 138);
            this.FieldAliasBox.Margin = new System.Windows.Forms.Padding(4);
            this.FieldAliasBox.Name = "FieldAliasBox";
            this.FieldAliasBox.Size = new System.Drawing.Size(160, 25);
            this.FieldAliasBox.TabIndex = 4;
            // 
            // FieldTypeBox
            // 
            this.FieldTypeBox.FormattingEnabled = true;
            this.FieldTypeBox.Items.AddRange(new object[] {
            "Int16",
            "Int32",
            "Int64",
            "Single",
            "Double",
            "Text"});
            this.FieldTypeBox.Location = new System.Drawing.Point(103, 200);
            this.FieldTypeBox.Margin = new System.Windows.Forms.Padding(4);
            this.FieldTypeBox.Name = "FieldTypeBox";
            this.FieldTypeBox.Size = new System.Drawing.Size(160, 23);
            this.FieldTypeBox.TabIndex = 5;
            // 
            // YES
            // 
            this.YES.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.YES.Location = new System.Drawing.Point(19, 269);
            this.YES.Margin = new System.Windows.Forms.Padding(4);
            this.YES.Name = "YES";
            this.YES.Size = new System.Drawing.Size(100, 29);
            this.YES.TabIndex = 6;
            this.YES.Text = "确定";
            this.YES.UseVisualStyleBackColor = true;
            this.YES.Click += new System.EventHandler(this.YES_Click);
            // 
            // NO
            // 
            this.NO.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.NO.Location = new System.Drawing.Point(164, 269);
            this.NO.Margin = new System.Windows.Forms.Padding(4);
            this.NO.Name = "NO";
            this.NO.Size = new System.Drawing.Size(100, 29);
            this.NO.TabIndex = 7;
            this.NO.Text = "取消";
            this.NO.UseVisualStyleBackColor = true;
            this.NO.Click += new System.EventHandler(this.NO_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 29);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(67, 15);
            this.label4.TabIndex = 8;
            this.label4.Text = "当前图层";
            // 
            // LayerName
            // 
            this.LayerName.Location = new System.Drawing.Point(103, 25);
            this.LayerName.Margin = new System.Windows.Forms.Padding(4);
            this.LayerName.Name = "LayerName";
            this.LayerName.Size = new System.Drawing.Size(160, 25);
            this.LayerName.TabIndex = 9;
            // 
            // NewField
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 338);
            this.Controls.Add(this.LayerName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.NO);
            this.Controls.Add(this.YES);
            this.Controls.Add(this.FieldTypeBox);
            this.Controls.Add(this.FieldAliasBox);
            this.Controls.Add(this.FieldNameBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewField";
            this.Text = "添加字段";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox FieldNameBox;
        private System.Windows.Forms.TextBox FieldAliasBox;
        private System.Windows.Forms.ComboBox FieldTypeBox;
        private System.Windows.Forms.Button YES;
        private System.Windows.Forms.Button NO;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox LayerName;
    }
}