
namespace FSGIS.Forms
{
    partial class LabelRender
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
            this.gbox_标注字段 = new System.Windows.Forms.GroupBox();
            this.gBox_符号样式 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.fontDialog1 = new System.Windows.Forms.FontDialog();
            this.btn符号 = new System.Windows.Forms.Button();
            this.Confirm = new System.Windows.Forms.Button();
            this.TODO = new System.Windows.Forms.Button();
            this.gbox_标注字段.SuspendLayout();
            this.gBox_符号样式.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbox_标注字段
            // 
            this.gbox_标注字段.Controls.Add(this.comboBox1);
            this.gbox_标注字段.Location = new System.Drawing.Point(47, 37);
            this.gbox_标注字段.Name = "gbox_标注字段";
            this.gbox_标注字段.Size = new System.Drawing.Size(405, 85);
            this.gbox_标注字段.TabIndex = 0;
            this.gbox_标注字段.TabStop = false;
            this.gbox_标注字段.Text = "标注字段";
            // 
            // gBox_符号样式
            // 
            this.gBox_符号样式.Controls.Add(this.btn符号);
            this.gBox_符号样式.Location = new System.Drawing.Point(47, 170);
            this.gBox_符号样式.Name = "gBox_符号样式";
            this.gBox_符号样式.Size = new System.Drawing.Size(405, 96);
            this.gBox_符号样式.TabIndex = 1;
            this.gBox_符号样式.TabStop = false;
            this.gBox_符号样式.Text = "符号样式";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(67, 38);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(261, 26);
            this.comboBox1.TabIndex = 0;
            // 
            // fontDialog1
            // 
            this.fontDialog1.Apply += new System.EventHandler(this.fontDialog1_Apply);
            // 
            // btn符号
            // 
            this.btn符号.Location = new System.Drawing.Point(67, 38);
            this.btn符号.Name = "btn符号";
            this.btn符号.Size = new System.Drawing.Size(261, 34);
            this.btn符号.TabIndex = 0;
            this.btn符号.Text = "符号样式";
            this.btn符号.UseVisualStyleBackColor = true;
            this.btn符号.Click += new System.EventHandler(this.btn符号_Click);
            // 
            // Confirm
            // 
            this.Confirm.Location = new System.Drawing.Point(76, 352);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(113, 38);
            this.Confirm.TabIndex = 2;
            this.Confirm.Text = "确认";
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.Click += new System.EventHandler(this.Confirm_Click);
            // 
            // TODO
            // 
            this.TODO.Location = new System.Drawing.Point(294, 352);
            this.TODO.Name = "TODO";
            this.TODO.Size = new System.Drawing.Size(110, 38);
            this.TODO.TabIndex = 3;
            this.TODO.Text = "应用";
            this.TODO.UseVisualStyleBackColor = true;
            this.TODO.Click += new System.EventHandler(this.TODO_Click);
            // 
            // LabelRender
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 455);
            this.Controls.Add(this.TODO);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.gBox_符号样式);
            this.Controls.Add(this.gbox_标注字段);
            this.Name = "LabelRender";
            this.Text = "LabelRender";
            this.Load += new System.EventHandler(this.LabelRender_Load);
            this.gbox_标注字段.ResumeLayout(false);
            this.gBox_符号样式.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbox_标注字段;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox gBox_符号样式;
        private System.Windows.Forms.FontDialog fontDialog1;
        private System.Windows.Forms.Button btn符号;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Button TODO;
    }
}