using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FSGIS.Forms
{
    public partial class LabelRender : Form
    {
        public MainForm frmContainer;  //父窗体
        public MyMapObjects.moMapLayer _Layer;            //当前处理的图层
        private MyMapObjects.moFields _AttributeFields;   //当前图层的字段信息

        private Font _Font = new Font("微软雅黑", 8);

        public LabelRender()
        {
            InitializeComponent();
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void btn符号_Click(object sender, EventArgs e)
        {
            DialogResult dr = fontDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            { 
                //设置注记符号
                _Font = fontDialog1.Font;

            }
        }

        private void TODO_Click(object sender, EventArgs e)
        {
            DoLabelRender();
            ///重绘地图
            frmContainer.myMapControl.RedrawMap();
        }

        private void Confirm_Click(object sender, EventArgs e)
        {
            DoLabelRender();
            ///重绘地图
            frmContainer.myMapControl.RedrawMap();
            this.Close();
        }

        private void DoLabelRender()
        {
            MyMapObjects.moLabelRenderer sLabelRenderer = new MyMapObjects.moLabelRenderer();

            //设定绑定字段为索引号为index的字段
            sLabelRenderer.Field = comboBox1.SelectedItem.ToString();
            //设置注记符号
            Font sOldFont = fontDialog1.Font;
            sLabelRenderer.TextSymbol.Font = new Font(sOldFont.Name, sOldFont.Size);
            sLabelRenderer.TextSymbol.UseMask = true;
            sLabelRenderer.LabelFeatures = true;
            // 赋给图层
            _Layer.LabelRenderer = sLabelRenderer;
        }
        private void LabelRender_Load(object sender, EventArgs e)
        {
            _AttributeFields = _Layer.AttributeFields;  //图层的字段信息
            for (Int32 i = 0; i < _AttributeFields.Count; ++i)
            {
                comboBox1.Items.Add(_AttributeFields.GetItem(i).Name);
            }
            comboBox1.SelectedIndex = 0;
        }
    }
}
