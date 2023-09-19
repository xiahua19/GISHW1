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
    public delegate void SelectByAttributeHandle(MyMapObjects.moMapLayer layer, string SQL);
    public partial class SelectByAttri : Form
    {
        #region 字段

        private MyMapObjects.moMapLayer _Layer; //执行选择的图层

        #endregion

        #region 事件
        internal event SelectByAttributeHandle ApplySearch;     // 执行查询
        #endregion

        #region 构造函数
        public SelectByAttri()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置图层参数的构造函数
        /// </summary>
        /// <param name="layer"></param>
        public SelectByAttri(MyMapObjects.moMapLayer layer)
        {
            InitializeComponent();
            _Layer = layer;
            this.LayerName.Text = layer.Name;
            this.LayerName.Enabled = false;
            InitializeFieldsList();
        }

        #endregion

        #region 私有函数
        private void InitializeFieldsList()
        {
            for(int i = 0;i<_Layer.AttributeFields.Count;++i)
            {
                this.FieldsList.Items.Add(_Layer.AttributeFields.GetItem(i).Name);
            }
        }
        #endregion

        #region 控件事件响应函数
        private void NO_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void APPLY_Click(object sender, EventArgs e)
        {
            ApplySearch( _Layer, this.QuerySentence.Text);
        }

        private void YES_Click(object sender, EventArgs e)
        {
            ApplySearch( _Layer, this.QuerySentence.Text);
            this.Dispose();
        }

        /// <summary>
        /// 双击属性以添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FieldsList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left)
            {
                Point localPoint = FieldsList.PointToClient(MousePosition);
                ListViewItem lvi = this.FieldsList.GetItemAt(localPoint.X, localPoint.Y);
                if (lvi != null)
                {
                    string fieldName = this.FieldsList.GetItemAt(localPoint.X, localPoint.Y).Text;
                    this.QuerySentence.Text += (" " + fieldName + " ");
                }
            }
        }
        #endregion
    }
}
