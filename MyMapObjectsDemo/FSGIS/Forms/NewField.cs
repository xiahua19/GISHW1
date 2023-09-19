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
    public delegate void AddFieldHandle(MyMapObjects.moMapLayer layer, string name, string alias, MyMapObjects.moValueTypeConstant type);
    public partial class NewField : Form
    {
        #region 事件
        internal event AddFieldHandle _AddField;    // 添加字段事件，由AttributesTableForm处理
        #endregion

        #region 字段
        private MyMapObjects.moMapLayer _Layer;
        #endregion

        #region 构造函数
        public NewField()
        {
            InitializeComponent();
        }

        public NewField(MyMapObjects.moMapLayer layer)
        {
            InitializeComponent();
            this._Layer = layer;
            this.LayerName.Text = layer.Name;
            this.LayerName.Enabled = false;
        }

        #endregion

        #region 控件事件处理函数
        private void YES_Click(object sender, EventArgs e)
        {
            string name = this.FieldNameBox.Text;
            string alias = this.FieldAliasBox.Text;
            int type_index = this.FieldTypeBox.SelectedIndex;
            List<MyMapObjects.moValueTypeConstant> typeList = new List<MyMapObjects.moValueTypeConstant>();
            typeList.Add(MyMapObjects.moValueTypeConstant.dInt16);
            typeList.Add(MyMapObjects.moValueTypeConstant.dInt32);
            typeList.Add(MyMapObjects.moValueTypeConstant.dInt64);
            typeList.Add(MyMapObjects.moValueTypeConstant.dSingle);
            typeList.Add(MyMapObjects.moValueTypeConstant.dDouble);
            typeList.Add(MyMapObjects.moValueTypeConstant.dText);
            if (this.FieldNameBox.Text!=""&&this.FieldAliasBox.Text!=""&&this.FieldTypeBox.SelectedIndex!=-1)
            {
                _AddField(_Layer, name, alias, typeList[type_index]);
                this.Dispose();
            }
        }

        private void NO_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        #endregion
    }
}
