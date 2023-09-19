using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyMapObjects;

namespace FSGIS.Forms
{
    /// <summary>
    /// 选中要素后显示其属性，由于可以选择多个要素，因此这里设置下拉框以供选择
    /// </summary>
    public partial class SelectedAttri : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SelectedAttri()
        {
            // 初始化组件
            InitializeComponent();
        }

        /// <summary>
        /// 根据当前GIS系统中的图层数据来更新显示窗口
        /// </summary>
        public void UpdateContent()
        {
            // 首先清除已添加的内容
            选中图层.Items.Clear();
            选中要素.Items.Clear();

            // 获取当前Layers中的图层数量
            int layersNum = MainForm.form.myMapControl.Layers.Count;

            // 将所有Layer加入图层下拉框中，命名前加上序号，方便其他函数进行解析
            for (int i = 0; i < layersNum; ++i)
            {
                选中图层.Items.Add(i.ToString() + ". " + MainForm.form.myMapControl.Layers.GetItem(i).Name);
            }

            // 如果数量不为0，则将第一个作为选中图层
            if (layersNum > 0)
            {
                选中图层.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 选中图层下拉框选中内容改变后触发的事件
        /// </summary>
        private void 选中图层_SelectedValueChanged(object sender, EventArgs e)
        {
            // 对下拉框内容进行解析，'.'之前的字符串中标识了当前图层在Layers中的index
            int selectedLayerIndex = int.Parse(选中图层.SelectedItem.ToString().Split('.')[0]);

            // 根据index获取当前选中的图层对象
            moMapLayer selectedLayer = MainForm.form.myMapControl.Layers.GetItem(selectedLayerIndex);

            // 读取当前选中图层对象中被框选要素的数量
            int selectedFeaturesNum = selectedLayer.SelectedFeatures.Count;

            // 依次将被框选要素加入到选中要素下拉框中，命名中包含序号和要素的第一个属性值
            for(int i = 0; i < selectedFeaturesNum; ++i)
            {
                选中要素.Items.Add(i.ToString() + ". " + selectedLayer.SelectedFeatures.GetItem(i).Attributes.GetItem(0).ToString());
            }

            // 如果选中要素的数量不为0，则将第一个要素作为被选中要素
            if(selectedFeaturesNum > 0)
            {
                选中要素.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 选中要素下拉框内容改变后触发的事件
        /// </summary>
        private void 选中要素_SelectedValueChanged(object sender, EventArgs e)
        {
            // 获取当前选中的图层的index，并据此获得该图层对象
            int selectedLayerIndex = int.Parse(选中图层.SelectedItem.ToString().Split('.')[0]);
            moMapLayer selectedLayer = MainForm.form.myMapControl.Layers.GetItem(selectedLayerIndex);
            
            // 获取当前选中的图层中选中要素的index，并据此获得该要素对象
            int selectedFeatureIndex = int.Parse(选中要素.SelectedItem.ToString().Split('.')[0]);
            moFeature selectedFeature = selectedLayer.SelectedFeatures.GetItem(selectedFeatureIndex);
            
            // 获取被选中图层的字段集合和数量
            moFields featureFields = selectedLayer.AttributeFields;
            int fieldsNum = featureFields.Count;

            // 获取被选中要素的属性变量
            moAttributes selectedAttribute = selectedFeature.Attributes;

            // 新建一个数据表，用来显示选中要素的属性表。第一列存放字段名，第二列存放字段值
            DataTable dataTable = new DataTable();
            DataColumn nameColumn = new DataColumn("字段名");
            DataColumn dataColumn = new DataColumn("字段值");
            dataTable.Columns.Add(nameColumn);
            dataTable.Columns.Add(dataColumn);

            // 遍历当前选中要素的所有字段
            for(int i = 0; i < fieldsNum; ++i)
            {
                // 新建一行用来存放当前字段的数据
                DataRow dataRow = dataTable.NewRow();
                dataTable.Rows.Add(dataRow);

                // 当前行第一列存放字段名，第二列存放属性值
                dataTable.Rows[i][0] = featureFields.GetItem(i).Name.ToString();
                dataTable.Rows[i][1] = selectedAttribute.GetItem(i).ToString();
            }

            // 将该数据表作为控件的数据源
            dataGridView.DataSource = dataTable;
        }
    }
}
