using FSGIS.SubSystems;
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
    public delegate void RefreshTrackingShapeHandle();
    public delegate void UpdateAttributeValue(MyMapObjects.moMapLayer layer,DataGridView table);
    public partial class AttributesTableForm : Form
    {
        #region 字段

        private List<MyMapObjects.moMapLayer> _Layers =
            new List<MyMapObjects.moMapLayer>();                // 要显示属性表的图层
        private List<DataGridView> _Tables = new List<DataGridView>();  // 要显示的属性表对应的DataGridView控件
        private int _ShowLayerIndex = -1;                               // 当前显示属性表图层的索引
        private int _CurrentColumnIndex = -1;                           // 当前选中的列索引，用于对字段的删除操作
        
        #endregion

        #region 构造函数

        public AttributesTableForm()
        {
            InitializeComponent();

        }

        #endregion

        #region 方法

        /// <summary>
        /// 添加要显示属性表的图层
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="editingStatus"></param>
        public void AddLayerTable(MyMapObjects.moMapLayer layer,bool editingStatus)
        {

            _Layers.Add(layer);
            DataTable dt = GenerateTableFromLayer(layer);
            // 生成表格控件
            tabControl1.TabPages.Add(layer.Name);
            DataGridView dgv = new DataGridView()
            {
                Name = layer.Name,
                Dock = DockStyle.Fill,
                DataSource = dt,
                ReadOnly = !editingStatus,
                MultiSelect = true,
            };

            // 显示行号
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                DataGridViewRow r = dgv.Rows[i];
                r.HeaderCell.Value = string.Format("{0}", i + 1);
            }

            // 记录这个控件
            this._Tables.Add(dgv);
            _ShowLayerIndex = tabControl1.TabPages.Count - 1;

            // 设置相关参数
            dgv.BindingContext = new BindingContext();              // 设置背景，否则Rows的Count属性为0            
            tabControl1.TabPages[tabControl1.TabPages.Count - 1].
                Controls.Add(dgv);                                  // 添加到tab控件上
            tabControl1.SelectedTab = tabControl1.
                TabPages[tabControl1.TabPages.Count - 1];           // 设置当前显示选项卡为此属性表
            SetSelected(tabControl1.TabPages.Count - 1);            // 根据图层当前选择要素情况设置选择的行
            dgv.RowStateChanged += Dgv_RowStateChanged;             // 添加行状态变化事件
            dgv.ColumnHeaderMouseClick += 
                Dgv_ColumnHeaderMouseClick;                         // 添加表头右键事件
            dgv.CellValueChanged += Dgv_CellValueChanged;

            for(int i = 0;i<dgv.Columns.Count;++i)
            {
                dgv.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dgv.AllowUserToAddRows = false;//禁止添加行

        }

        /// <summary>
        /// 设置当前显示的属性表为指定图层的属性表
        /// </summary>
        /// <param name="layer"></param>
        public void SetShowLayer(MyMapObjects.moMapLayer layer)
        {
            int index = _Layers.IndexOf(layer);
            _ShowLayerIndex = index;
            tabControl1.SelectedTab = tabControl1.TabPages[index];
        }

        /// <summary>
        /// 根据图层要素选择状况刷新所有属性表的选择状况
        /// </summary>
        public void RefreshSelected()
        {
            for (int i = 0; i < _Tables.Count; ++i) SetSelected(i);
        }

        /// <summary>
        /// 已经显示的属性表中是否有指定图层的属性表
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public bool Exists(MyMapObjects.moMapLayer layer)
        {
            return _Layers.Exists(item => item == layer);
        }

        /// <summary>
        /// 设置属性表可修改
        /// </summary>
        public void SetAllEditable()
        {
            for (int i = 0; i < _Tables.Count; ++i) if (_Tables[i].ReadOnly) _Tables[i].ReadOnly = false;
        }

        public void SetLayerEditable(MyMapObjects.moMapLayer layer)
        {
            int index = this._Layers.IndexOf(layer);
            if (index != -1)
            {
                this._Tables[index].ReadOnly = false;
            }
        }

        /// <summary>
        /// 设置属性表不可修改
        /// </summary>
        public void SetAllNotEditable()
        {
            for (int i = 0; i < _Tables.Count; ++i) if (!_Tables[i].ReadOnly) _Tables[i].ReadOnly = true;
        }

        public void RemoveTable(MyMapObjects.moMapLayer layer)
        {

             int index = _Layers.IndexOf(layer);
            if(index !=-1)
            {
                _Layers.RemoveAt(index);
                tabControl1.TabPages.RemoveAt(index);
                _Tables.RemoveAt(index);
                if (_Layers.Count == 0) this.Dispose();
                else
                {
                    _ShowLayerIndex = _Layers.Count - 1;
                    tabControl1.SelectedIndex = _Layers.Count - 1;
                }
                
            }

        }
        public void RefreshLayerTable(MyMapObjects.moMapLayer layer)
        {
            int index = _Layers.FindIndex(e=> e==layer);
            if (index != -1)
            {
                DataTable dt = GenerateTableFromLayer(layer);
                _Tables[index].DataSource = dt;
                SetSelected(index);
            }

        }

        #endregion

        #region 私有函数

        /// <summary>
        /// 根据指定图层要素选择状况设置属性表选择状况
        /// </summary>
        /// <param name="index"></param>
        private void SetSelected(int index)
        {
            bool ro = _Tables[index].ReadOnly;
            _Tables[index].ReadOnly = false;

            for (int i = 0; i < _Layers[index].Features.Count; ++i)
            {
                bool selected = false;
                for (int j = 0; j < _Layers[index].SelectedFeatures.Count; ++j)
                {
                    if (_Layers[index].SelectedFeatures.GetItem(j) == _Layers[index].Features.GetItem(i))
                    {
                        _Tables[index].Rows[i].Selected = true;
                        selected = true;
                        break;
                    }
                }
                if (!selected)
                {
                    _Tables[index].Rows[i].Selected = false;
                }
            }
            _Tables[index].ReadOnly = ro;
        }

        /// <summary>
        /// 创建右键菜单
        /// </summary>
        /// <returns></returns>
        private ContextMenuStrip GenerateCMS()
        {
            ContextMenuStrip cms = new ContextMenuStrip();
            cms.Items.Add("删除字段");
            cms.Items[0].Click += DeleteField_Click;
            return cms;
        }

        /// <summary>
        /// 刷新指定索引号的属性表
        /// </summary>
        /// <param name="index"></param>
        private void RefreshTable(int index)
        {
            List<int> selectedIndex = new List<int>();
            MyMapObjects.moMapLayer layer = _Layers[index];
            DataTable dt = new DataTable(layer.Name);
            MyMapObjects.moFields sFields = layer.AttributeFields;

            // 生成列
            for (int j = 0; j < sFields.Count; ++j)
            {
                dt.Columns.Add(sFields.GetItem(j).AliasName, DataTypeTools.GetTypeFromConstant(sFields.GetItem(j).ValueType));
            }

            // 生成行
            MyMapObjects.moFeatures sFeatures = layer.Features;
            for (int j = 0; j < sFeatures.Count; ++j)
            {
                DataRow sRow = dt.NewRow();
                for (int k = 0; k < sFields.Count; ++k)
                {
                    sRow[sFields.GetItem(k).AliasName] = sFeatures.GetItem(j).Attributes.GetItem(k);
                }

                dt.Rows.Add(sRow);
                if (_Tables[index].Rows[j].Selected) selectedIndex.Add(j);
            }


            _Tables[index].DataSource = dt;
            for (int i = 0;i<_Tables[index].Columns.Count;++i)
                _Tables[index].Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            for (int i = 0; i < selectedIndex.Count; ++i)
                _Tables[index].Rows[selectedIndex[i]].Selected = true;
        }

        /// <summary>
        /// 向图层中加入字段
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="name"></param>
        /// <param name="alias"></param>
        /// <param name="type"></param>
        private void AddFieldToLayer(MyMapObjects.moMapLayer layer, string name, string alias, MyMapObjects.moValueTypeConstant type)
        {
            MyMapObjects.moField field = new MyMapObjects.moField(name, type);
            try
            {
                layer.AttributeFields.Append(field);
            }
            catch (Exception e)
            {
                MessageBox.Show("不允许出现重名字段");
            }
            RefreshTable(_ShowLayerIndex);
        }

        private DataTable GenerateTableFromLayer(MyMapObjects.moMapLayer layer)
        {
            DataTable dt = new DataTable(layer.Name);
            MyMapObjects.moFields sFields = layer.AttributeFields;
            // 生成列
            for (int j = 0; j < sFields.Count; ++j)
            {
                dt.Columns.Add(sFields.GetItem(j).AliasName, DataTypeTools.GetTypeFromConstant(sFields.GetItem(j).ValueType));
            }

            // 生成行
            MyMapObjects.moFeatures sFeatures = layer.Features;
            for (int j = 0; j < sFeatures.Count; ++j)
            {
                DataRow sRow = dt.NewRow();
                for (int k = 0; k < sFields.Count; ++k)
                {
                    sRow[sFields.GetItem(k).AliasName] = sFeatures.GetItem(j).Attributes.GetItem(k);
                }

                dt.Rows.Add(sRow);
            }
            return dt;
        }
        #endregion

        #region 事件

        internal event RefreshTrackingShapeHandle _RefreshTrackingShape;      // 重绘事件，由MainFrame处理

        #endregion

        #region 控件事件处理函数

        /// <summary>
        /// 添加字段按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 添加字段ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewField nfForm = new NewField(_Layers[_ShowLayerIndex]);
            nfForm._AddField += AddFieldToLayer;
            nfForm.ShowDialog();
        }

        /// <summary>
        /// 删除字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteField_Click(object sender, EventArgs e)
        {
            // 删除字段
            _Layers[_ShowLayerIndex].AttributeFields.RemoveAt(_CurrentColumnIndex);
            // 刷新属性表
            RefreshTable(_ShowLayerIndex);
        }

        /// <summary>
        /// 属性表行选择状态改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgv_RowStateChanged(object sender, DataGridViewRowStateChangedEventArgs e)
        {
            if (e.StateChanged != DataGridViewElementStates.Selected) return;
            if (e.Row.Selected)
                _Layers[_ShowLayerIndex].SelectedFeatures.Add(_Layers[_ShowLayerIndex].Features.GetItem(e.Row.Index));
            else
            {
                for (int i = 0; i < _Layers[_ShowLayerIndex].SelectedFeatures.Count; ++i)
                {
                    if (_Layers[_ShowLayerIndex].SelectedFeatures.GetItem(i) == _Layers[_ShowLayerIndex].Features.GetItem(e.Row.Index))
                    {
                        _Layers[_ShowLayerIndex].SelectedFeatures.RemoveAt(i);
                    }
                }
            }
            _RefreshTrackingShape();
            ShowSelectedNumber(_ShowLayerIndex);
        }

        /// <summary>
        /// 右键单击列头，可以删除字段
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgv_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _CurrentColumnIndex = e.ColumnIndex;
                ContextMenuStrip cms = GenerateCMS();
                cms.Show(MousePosition.X, MousePosition.Y);
            }
        }

        /// <summary>
        /// 表格单元被修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Dgv_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this._Layers[_ShowLayerIndex]
                    .Features.GetItem(e.RowIndex)
                    .Attributes
                    .SetItem(
                        e.ColumnIndex,
                        this._Tables[_ShowLayerIndex][e.ColumnIndex, e.RowIndex].Value
                    );
            }
            catch(Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        #endregion


        private void ShowSelectedNumber(int index)
        {
            int sSelectedNumber = _Layers[index].SelectedFeatures.Count;
            int sTotalFeatures = _Layers[index].Features.Count;
            this.tssSelectedNumber.Text = "选择： " + sSelectedNumber.ToString() + "/" + sTotalFeatures.ToString();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = tabControl1.SelectedIndex;
            _ShowLayerIndex = index;
            ShowSelectedNumber(index);
        }
    }
}
