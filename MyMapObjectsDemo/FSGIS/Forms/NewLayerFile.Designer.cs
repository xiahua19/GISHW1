
namespace FSGIS.Forms
{
    partial class NewLayerFile
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewLayerFile));
            this.NO = new System.Windows.Forms.Button();
            this.YES = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.layerType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.openFile = new System.Windows.Forms.Button();
            this.layerPath = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // NO
            // 
            this.NO.Location = new System.Drawing.Point(319, 189);
            this.NO.Margin = new System.Windows.Forms.Padding(4);
            this.NO.Name = "NO";
            this.NO.Size = new System.Drawing.Size(77, 29);
            this.NO.TabIndex = 7;
            this.NO.Text = "取消";
            this.NO.UseVisualStyleBackColor = true;
            this.NO.Click += new System.EventHandler(this.NO_Click);
            // 
            // YES
            // 
            this.YES.Location = new System.Drawing.Point(404, 189);
            this.YES.Margin = new System.Windows.Forms.Padding(4);
            this.YES.Name = "YES";
            this.YES.Size = new System.Drawing.Size(77, 29);
            this.YES.TabIndex = 6;
            this.YES.Text = "确定";
            this.YES.UseVisualStyleBackColor = true;
            this.YES.Click += new System.EventHandler(this.YES_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "请选择图层类型";
            // 
            // layerType
            // 
            this.layerType.FormattingEnabled = true;
            this.layerType.Items.AddRange(new object[] {
            "点",
            "线",
            "面"});
            this.layerType.Location = new System.Drawing.Point(40, 59);
            this.layerType.Margin = new System.Windows.Forms.Padding(4);
            this.layerType.Name = "layerType";
            this.layerType.Size = new System.Drawing.Size(322, 23);
            this.layerType.TabIndex = 4;
            this.layerType.Text = "下拉以选择...";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 98);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(142, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "请选择图层保存路径";
            // 
            // openFile
            // 
            this.openFile.Location = new System.Drawing.Point(368, 138);
            this.openFile.Name = "openFile";
            this.openFile.Size = new System.Drawing.Size(113, 32);
            this.openFile.TabIndex = 9;
            this.openFile.Text = "浏览文件...";
            this.openFile.UseVisualStyleBackColor = true;
            this.openFile.Click += new System.EventHandler(this.openFile_Click);
            // 
            // layerPath
            // 
            this.layerPath.Location = new System.Drawing.Point(40, 138);
            this.layerPath.Name = "layerPath";
            this.layerPath.ReadOnly = true;
            this.layerPath.Size = new System.Drawing.Size(322, 25);
            this.layerPath.TabIndex = 10;
            // 
            // NewLayerFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 235);
            this.Controls.Add(this.layerPath);
            this.Controls.Add(this.openFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.NO);
            this.Controls.Add(this.YES);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.layerType);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewLayerFile";
            this.Text = "新建图层文件";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button NO;
        private System.Windows.Forms.Button YES;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox layerType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button openFile;
        private System.Windows.Forms.TextBox layerPath;
    }
}