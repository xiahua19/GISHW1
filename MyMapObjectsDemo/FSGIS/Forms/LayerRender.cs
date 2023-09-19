using MyMapObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSGIS.SubSystems;
using System.Drawing.Drawing2D;

namespace FSGIS.Forms
{
    public partial class LayerRender : Form
    {
        public MainForm frmContainer;  //父窗体
        public MyMapObjects.moMapLayer _Layer;            //当前处理的图层

        private MyMapObjects.moRenderer _Renderer;        //当前图层的渲染方式
        private MyMapObjects.moSymbol _Symbol;            //当前图层的符号样式



        //便于程序运行的变量
        private MyMapObjects.moSimpleRenderer _SimpleRenderer;
        private MyMapObjects.moUniqueValueRenderer _UniqueValueRenderer;
        private MyMapObjects.moClassBreaksRenderer _ClassBreakRenderer;

        private MyMapObjects.moSimpleMarkerSymbol _SimpleMarkerSymbol;
        private MyMapObjects.moSimpleLineSymbol _SimpleLineSymbol;
        private MyMapObjects.moSimpleFillSymbol _SimpleFillSymbol;
        

        

        private MyMapObjects.moFields _AttributeFields;   //当前图层的字段信息


        bool _DataGridViewBtnChanged = false;             //手动修改样式
        bool _DataGirdViewCBKChanged = false;            //手动修改节点

        //收集按钮点击的信息
        String _RenderType;
        String _GeometryType;
        moSimpleMarkerSymbolStyleConstant _MarkerSymbol;   //点符号样式
        moSimpleLineSymbolStyleConstant _PolylineSymbol;   //面符号样式


        public LayerRender()
        {
            frmContainer = (MainForm)this.Owner; //设置父窗体
            InitializeComponent();
            _RenderType = "tabSimple";
            _GeometryType = "tabPoint";
        }

        #region  窗口载入预处理
        private void LayerRender_Load(object sender, EventArgs e)
        {
            _AttributeFields = _Layer.AttributeFields;  //图层的字段信息
            _Renderer = _Layer.Renderer;               //图层渲染信息
            //读取当前图层的渲染信息，根据渲染方式和符号类型读取
            LoadLayRenderAndSymbol();
            //将字段信息写入唯一值、分级渲染绑定选项
            comboBox1.Items.Clear();
            comboBox3.Items.Clear();
            for (Int32 i = 0; i < _AttributeFields.Count; ++i)
            {
                comboBox1.Items.Add(_AttributeFields.GetItem(i).Name);
                comboBox3.Items.Add(_AttributeFields.GetItem(i).Name);
            }
            comboBox1.SelectedIndex = 0;
            comboBox3.SelectedIndex = 0;
            //隐藏单一符号渲染窗口
            if (_Layer.ShapeType == moGeometryTypeConstant.Point)
            {
                tabPolyline.Parent = null;
                tabPolygon.Parent = null;
            }
            else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolyline)
            {
                tabPolygon.Parent = null;
                tabPoint.Parent = null;
            }
            else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolygon)
            {
                tabPolyline.Parent = null;
                tabPoint.Parent = null;
            }
        }
        //读取当前图层的渲染信息，根据渲染方式和符号类型读取
        private void LoadLayRenderAndSymbol()
        {
            
            if (_Layer.Renderer.RendererType == moRendererTypeConstant.Simple)
            {
                _SimpleRenderer = (moSimpleRenderer)_Renderer;
                if (_Layer.ShapeType == moGeometryTypeConstant.Point)
                {
                    _SimpleMarkerSymbol = (moSimpleMarkerSymbol)_SimpleRenderer.Symbol;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    _SimpleLineSymbol = (moSimpleLineSymbol)_SimpleRenderer.Symbol;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    _SimpleFillSymbol = (moSimpleFillSymbol)_SimpleRenderer.Symbol;
                }
            }
            else if (_Layer.Renderer.RendererType == moRendererTypeConstant.UniqueValue)
            {
                _UniqueValueRenderer = (moUniqueValueRenderer)_Renderer;
                if (_Layer.ShapeType == moGeometryTypeConstant.Point)
                {
                    _SimpleMarkerSymbol = (moSimpleMarkerSymbol)_UniqueValueRenderer.DefaultSymbol;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    _SimpleLineSymbol = (moSimpleLineSymbol)_UniqueValueRenderer.DefaultSymbol;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    _SimpleFillSymbol = (moSimpleFillSymbol)_UniqueValueRenderer.DefaultSymbol;
                }

            }
            else if (_Layer.Renderer.RendererType == moRendererTypeConstant.ClassBreaks)
            {
                _ClassBreakRenderer = (moClassBreaksRenderer)_Renderer;
                if (_Layer.ShapeType == moGeometryTypeConstant.Point)
                {
                    _SimpleMarkerSymbol = (moSimpleMarkerSymbol)_ClassBreakRenderer.DefaultSymbol;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    _SimpleLineSymbol = (moSimpleLineSymbol)_ClassBreakRenderer.DefaultSymbol;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    _SimpleFillSymbol = (moSimpleFillSymbol)_ClassBreakRenderer.DefaultSymbol;
                }
            }
        }
        #endregion

        #region 单一符号渲染样式
        private void button3_Click(object sender, EventArgs e)
        {
            button11.Text = button3.Text;
            button11.Image = button3.Image;
            _MarkerSymbol = moSimpleMarkerSymbolStyleConstant.Circle;
            //etc
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button11.Text = button4.Text;
            button11.Image = button4.Image;
            _MarkerSymbol = moSimpleMarkerSymbolStyleConstant.SolidCircle;
            //etc
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button11.Text = button5.Text;
            button11.Image = button5.Image;
            _MarkerSymbol = moSimpleMarkerSymbolStyleConstant.Square;
            //etc
        }

        private void button6_Click(object sender, EventArgs e)
        {
            button11.Text = button6.Text;
            button11.Image = button6.Image;
            _MarkerSymbol = moSimpleMarkerSymbolStyleConstant.SolidSquare;
            //etc
        }

        private void button7_Click(object sender, EventArgs e)
        {
            button11.Text = button7.Text;
            button11.Image = button7.Image;
            _MarkerSymbol = moSimpleMarkerSymbolStyleConstant.Triangle;
            //etc
        }

        private void button8_Click(object sender, EventArgs e)
        {
            button11.Text = button8.Text;
            button11.Image = button8.Image;
            _MarkerSymbol = moSimpleMarkerSymbolStyleConstant.SolidTriangle;
            //etc
        }

        private void button9_Click(object sender, EventArgs e)
        {
            button11.Text = button9.Text;
            button11.Image = button9.Image;
            _MarkerSymbol = moSimpleMarkerSymbolStyleConstant.CircleDot;
            //etc
        }

        private void button10_Click(object sender, EventArgs e)
        {
            button11.Text = button10.Text;
            button11.Image = button10.Image;
            _MarkerSymbol = moSimpleMarkerSymbolStyleConstant.CircleCircle;
            //etc
        }

        private void btnColor_Click(object sender, EventArgs e)
        {
            //显示颜色对话框
            DialogResult dr = colorDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            {
                btnColorPoint.BackColor = colorDialog1.Color;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            button15.Text = button12.Text;
            button15.Image = button12.Image;
            _PolylineSymbol = moSimpleLineSymbolStyleConstant.Dash;
            //etc
        }

        private void button13_Click(object sender, EventArgs e)
        {
            button15.Text = button13.Text;
            button15.Image = button13.Image;
            _PolylineSymbol = moSimpleLineSymbolStyleConstant.Solid;
            //etc
        }

        private void button18_Click(object sender, EventArgs e)
        {
            button20.Text = button12.Text;
            button20.Image = button12.Image;
            _PolylineSymbol = moSimpleLineSymbolStyleConstant.Dash;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            button20.Text = button13.Text;
            button20.Image = button13.Image;
            _PolylineSymbol = moSimpleLineSymbolStyleConstant.Solid;
        }

        #endregion

        #region 执行渲染

        /// <summary>
        /// 确认渲染
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            DoLayerRender();
            frmContainer.myMapControl.RedrawMap();
            this.Close();

        }
        /// <summary>
        /// 应用渲染
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            DoLayerRender();
            frmContainer.myMapControl.RedrawMap();
        }
        /// <summary>
        /// 执行渲染的函数
        /// </summary>
        private void DoLayerRender()
        {
            //首先判断所处的活动页面
            if (_RenderType == "tabSimple".ToString())  //单一符号渲染
            {

                if (_Layer.ShapeType == moGeometryTypeConstant.Point)
                {
                    DoPointSimpleRender(_MarkerSymbol, Convert.ToDouble(numPoint.Value), btnColorPoint.BackColor);
                    _DataGridViewBtnChanged = false;
                    _DataGirdViewCBKChanged = false;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    DoPolylineSimpleRender(_PolylineSymbol, Convert.ToDouble(numPolyline.Value), btnColorPolyline.BackColor);
                    _DataGridViewBtnChanged = false;
                    _DataGirdViewCBKChanged = false;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    DoPolygonSimpleRender(_PolylineSymbol,Convert.ToDouble(numOutSize.Value), btnOutColor.BackColor,btnColorPolygon.BackColor);
                    _DataGridViewBtnChanged = false;
                    _DataGirdViewCBKChanged = false;
                }
            }
            else if (_RenderType == "tabUnique".ToString())
            {
                if (_Layer.ShapeType == moGeometryTypeConstant.Point)
                {
                    DoPointUniqueValueRender();
                    _DataGirdViewCBKChanged = false;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    DoPolylineUniqueValueRender();
                    _DataGirdViewCBKChanged = false;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    DoPolygonUniqueValueRender();
                    _DataGirdViewCBKChanged = false;
                }
            }
            else if (_RenderType == "tabGriding".ToString())
            {
                if (_Layer.ShapeType == moGeometryTypeConstant.Point)
                {
                    DoPointClassBreaksRender();
                    _DataGridViewBtnChanged = false;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    DoPolylineClassBreaksRender();
                    _DataGridViewBtnChanged = false;
                }
                else if (_Layer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    DoPolygonClassBreaksRender();
                    _DataGridViewBtnChanged = false;
                }
            }
        }
        #endregion 

        #region  单一符号渲染
        public void DoPointSimpleRender(moSimpleMarkerSymbolStyleConstant markerSymbol, double size, Color color)
        {
            if (_Layer == null)
            {
                return;
            }
            
            _SimpleMarkerSymbol.Color = color;
            _SimpleMarkerSymbol.Size = size;
            _SimpleMarkerSymbol.Style = markerSymbol;
            if (_SimpleRenderer == null)
            {
                _SimpleRenderer = new moSimpleRenderer();
            }
            _SimpleRenderer.Symbol = _SimpleMarkerSymbol;

            _Layer.Renderer = _SimpleRenderer;
        }
        public void DoPolylineSimpleRender(moSimpleLineSymbolStyleConstant lineSymbol, double size, Color color)
        {

            if (_Layer == null)
            {
                return;
            }
            _SimpleLineSymbol.Color = color;
            _SimpleLineSymbol.Size = size;
            _SimpleLineSymbol.Style = lineSymbol;
            if (_SimpleRenderer == null)
            {
                _SimpleRenderer = new moSimpleRenderer();
            }
            _SimpleRenderer.Symbol = _SimpleLineSymbol;
            _Layer.Renderer = _SimpleRenderer;
            LoadLayRenderAndSymbol();
        }
        public void DoPolygonSimpleRender(moSimpleLineSymbolStyleConstant lineSymbol,double size,Color color,Color color0)
        {
            if (_Layer == null)
            {
                return;
            }
            _SimpleFillSymbol.Outline.Style = lineSymbol;
            _SimpleFillSymbol.Outline.Color = color;
            _SimpleFillSymbol.Outline.Size = size;

            _SimpleFillSymbol.Color = color0;
            if(_SimpleRenderer == null)
            {
                _SimpleRenderer = new moSimpleRenderer();
            }
            _SimpleRenderer.Symbol = _SimpleFillSymbol;
            _Layer.Renderer = _SimpleRenderer;
           
        }
        #endregion

        #region 唯一值渲染
        public void DoPointUniqueValueRender()
        {
            if (_Layer == null)
            {
                return;
            }
            if(_DataGridViewBtnChanged == false)
            {
                MyMapObjects.moUniqueValueRenderer sRenderer = new MyMapObjects.moUniqueValueRenderer();
                sRenderer.Field = comboBox1.SelectedItem.ToString();  //绑定选中的字段
                                                                      //读取该字段index
                Int32 sFieldIndex = _Layer.AttributeFields.FindField(sRenderer.Field);
                if (sFieldIndex < 0)  //不存在该字段
                    return;
                Int32 sFeatureCount = _Layer.Features.Count;

                List<string> sValues = new List<string>();   //MyMapObjects组件提供的唯一值渲染都是针对string类型，因而需要转化
                for (Int32 i = 0; i <= sFeatureCount - 1; i++)
                {
                    string sValue = Convert.ToString(_Layer.Features.GetItem(i).Attributes.GetItem(sFieldIndex));
                    sValues.Add(sValue);
                }
                sValues.Distinct().ToList();//去除重复
                Int32 sValueCount = sValues.Count;
                for (Int32 i = 0; i <= sValueCount - 1; i++)
                {
                    MyMapObjects.moSimpleMarkerSymbol sSymbol = new MyMapObjects.moSimpleMarkerSymbol();
                    sSymbol.Style = _SimpleMarkerSymbol.Style;
                    sSymbol.Size = _SimpleMarkerSymbol.Size;
                    sRenderer.AddUniqueValue(sValues[i], sSymbol);
                }
                GenerateDataGirdView(sValueCount, sRenderer, sValues, moSymbolTypeConstant.SimpleMarkerSymbol);
                sRenderer.DefaultSymbol = _SimpleMarkerSymbol;
                _Layer.Renderer = sRenderer;
            }
            else if (_Layer.Renderer.RendererType == moRendererTypeConstant.UniqueValue)
            {
                MyMapObjects.moUniqueValueRenderer sRenderer = (moUniqueValueRenderer)_Layer.Renderer;
                Int32 sValueCount = sRenderer.ValueCount;
                for(Int32 i=0;i<sValueCount;i++)
                {
                    MyMapObjects.moSimpleMarkerSymbol sSymbol = (moSimpleMarkerSymbol)sRenderer.GetSymbol(i);
                    sSymbol.Color = dataGridView1.Rows[i].Cells[0].Style.BackColor;
                }
            }
           
        }   
        public void DoPolylineUniqueValueRender()
        {
            if (_Layer == null)
            {
                return;
            }
            if(_DataGridViewBtnChanged == false)
            {
                MyMapObjects.moUniqueValueRenderer sRenderer = new MyMapObjects.moUniqueValueRenderer();
                sRenderer.Field = comboBox1.SelectedItem.ToString();  //绑定选中的字段
                                                                      //读取该字段index
                Int32 sFieldIndex = _Layer.AttributeFields.FindField(sRenderer.Field);
                if (sFieldIndex < 0)  //不存在该字段
                    return;
                Int32 sFeatureCount = _Layer.Features.Count;

                List<string> sValues = new List<string>();   //MyMapObjects组件提供的唯一值渲染都是针对string类型，因而需要转化
                for (Int32 i = 0; i <= sFeatureCount - 1; i++)
                {
                    string sValue = Convert.ToString(_Layer.Features.GetItem(i).Attributes.GetItem(sFieldIndex));
                    sValues.Add(sValue);
                }
                sValues.Distinct().ToList();//去除重复
                Int32 sValueCount = sValues.Count;
                for (Int32 i = 0; i <= sValueCount - 1; i++)
                {
                    MyMapObjects.moSimpleLineSymbol sSymbol = new MyMapObjects.moSimpleLineSymbol();
                    sSymbol.Style = _SimpleLineSymbol.Style;
                    sSymbol.Size = _SimpleLineSymbol.Size;
                    sRenderer.AddUniqueValue(sValues[i], sSymbol);
                }
                GenerateDataGirdView(sValueCount, sRenderer, sValues, moSymbolTypeConstant.SimpleLineSymbol);
                sRenderer.DefaultSymbol = _SimpleLineSymbol;
                _Layer.Renderer = sRenderer;
            }
            else if (_Layer.Renderer.RendererType == moRendererTypeConstant.UniqueValue)
            {
                MyMapObjects.moUniqueValueRenderer sRenderer = (moUniqueValueRenderer)_Layer.Renderer;
                Int32 sValueCount = sRenderer.ValueCount;
                for (Int32 i = 0; i < sValueCount; i++)
                {
                    MyMapObjects.moSimpleLineSymbol sSymbol = (moSimpleLineSymbol)sRenderer.GetSymbol(i);
                    sSymbol.Color = dataGridView1.Rows[i].Cells[0].Style.BackColor;
                }
            }
           
        }
        public void DoPolygonUniqueValueRender()
        {
            if (_Layer == null)
            {
                return;
            }
            if (_DataGridViewBtnChanged == false)
            {
                MyMapObjects.moUniqueValueRenderer sRenderer = new MyMapObjects.moUniqueValueRenderer();
                sRenderer.Field = comboBox1.SelectedItem.ToString();  //绑定选中的字段
                                                                      //读取该字段index
                Int32 sFieldIndex = _Layer.AttributeFields.FindField(sRenderer.Field);
                if (sFieldIndex < 0)  //不存在该字段
                    return;
                Int32 sFeatureCount = _Layer.Features.Count;

                List<string> sValues = new List<string>();   //MyMapObjects组件提供的唯一值渲染都是针对string类型，因而需要转化
                for (Int32 i = 0; i <= sFeatureCount - 1; i++)
                {
                    string sValue = Convert.ToString(_Layer.Features.GetItem(i).Attributes.GetItem(sFieldIndex));
                    sValues.Add(sValue);
                }
                sValues.Distinct().ToList();//去除重复
                Int32 sValueCount = sValues.Count;
                for (Int32 i = 0; i <= sValueCount - 1; i++)
                {
                    MyMapObjects.moSimpleFillSymbol sSymbol = new MyMapObjects.moSimpleFillSymbol();
                    sSymbol.Outline.Style = _SimpleFillSymbol.Outline.Style;
                    sSymbol.Outline.Size = _SimpleFillSymbol.Outline.Size;
                    sRenderer.AddUniqueValue(sValues[i], sSymbol);
                }
                GenerateDataGirdView(sValueCount, sRenderer, sValues, moSymbolTypeConstant.SimpleFillSymbol);
                sRenderer.DefaultSymbol = _SimpleFillSymbol;
                _Layer.Renderer = sRenderer;
            }
            else if (_Layer.Renderer.RendererType == moRendererTypeConstant.UniqueValue)
            {
                MyMapObjects.moUniqueValueRenderer sRenderer = (moUniqueValueRenderer)_Layer.Renderer;
                Int32 sValueCount = sRenderer.ValueCount;
                for (Int32 i = 0; i < sValueCount; i++)
                {
                    MyMapObjects.moSimpleFillSymbol sSymbol = (moSimpleFillSymbol)sRenderer.GetSymbol(i);
                    sSymbol.Color = dataGridView1.Rows[i].Cells[0].Style.BackColor;
                }
            }
           
        }
        #endregion

        #region 分级渲染
        private void DoPointClassBreaksRender()
        {
            if (_Layer == null)
            {
                return;
            }
            if(_DataGirdViewCBKChanged == false)
            {
                Int32 classNum = Convert.ToInt32(comboBox2.SelectedItem);
                MyMapObjects.moClassBreaksRenderer sRenderer = new MyMapObjects.moClassBreaksRenderer();
                sRenderer.Field = comboBox3.SelectedItem.ToString();
                //读出所有值
                Int32 sFieldIndex = _Layer.AttributeFields.FindField(sRenderer.Field);
                if (sFieldIndex < 0)
                    return;
                if (_Layer.AttributeFields.GetItem(sFieldIndex).ValueType == MyMapObjects.moValueTypeConstant.dText)
                    return;
                Int32 sFeatureCount = _Layer.Features.Count;  //图层所有要素
                List<double> sValues = new List<double>();    //图层要素某字段值的集合
                List<string> sValueString = new List<string>(); // datagirdview表内容
                List<string> sValueScope = new List<string>();  //分级范围
                for (Int32 i = 0; i < sFeatureCount; i++)
                {
                    double sValue = Convert.ToDouble(_Layer.Features.GetItem(i).Attributes.GetItem(sFieldIndex));
                    sValues.Add(sValue);
                }
                //获取最小最大值，并分5级
                double sMinValue = sValues.Min();
                double sMaxValue = sValues.Max();
                for (Int32 i = 0; i <= classNum; i++)
                {
                    double sValue0 = sMinValue + (sMaxValue - sMinValue) * (i) / classNum;
                    double sValue = sMinValue + (sMaxValue - sMinValue) * (i + 1) / classNum;
                    sValueString.Add(sValue.ToString());
                    sValueScope.Add(Math.Round(sValue0, 2).ToString() + " - " + Math.Round(sValue, 2).ToString());
                    MyMapObjects.moSimpleMarkerSymbol sSymbol = new MyMapObjects.moSimpleMarkerSymbol();
                    sSymbol.Style = _SimpleMarkerSymbol.Style;
                    sSymbol.Size = _SimpleMarkerSymbol.Size;
                    sRenderer.AddBreakValue(sValue, sSymbol);
                }
                //设置起始末尾色
                Color sStartColor = btnStartColor.BackColor;
                Color sEndColor = btnEndColor.BackColor;
                sRenderer.RampColor(sStartColor, sEndColor);
                ////配置datagirdview表格
                GenerateDataGirdView(classNum, sRenderer, sValueString, moSymbolTypeConstant.SimpleMarkerSymbol, sValueScope);
                //赋给图层
                sRenderer.DefaultSymbol = _SimpleMarkerSymbol;
                _Layer.Renderer = sRenderer;
            }
            else if(_Layer.Renderer.RendererType == moRendererTypeConstant.ClassBreaks)
            {
                MyMapObjects.moClassBreaksRenderer sRenderer = (moClassBreaksRenderer)_Layer.Renderer;
                Int32 sValuesCount = sRenderer.BreakCount;
                for(Int32 i = 0;i<sValuesCount;i++)
                {
                    sRenderer.SetBreakValue(i, Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value));
                }

            }
            
        }
        private void DoPolylineClassBreaksRender()
        {
            if(_Layer == null)
            {
                return;
            }
            if(_DataGirdViewCBKChanged == false)
            {
                Int32 classNum = Convert.ToInt32(comboBox2.SelectedItem);
                MyMapObjects.moClassBreaksRenderer sRenderer = new MyMapObjects.moClassBreaksRenderer();
                sRenderer.Field = comboBox3.SelectedItem.ToString();
                //读出所有值
                Int32 sFieldIndex = _Layer.AttributeFields.FindField(sRenderer.Field);
                if (sFieldIndex < 0)
                    return;
                if (_Layer.AttributeFields.GetItem(sFieldIndex).ValueType == MyMapObjects.moValueTypeConstant.dText)
                    return;
                Int32 sFeatureCount = _Layer.Features.Count;  //图层所有要素
                List<double> sValues = new List<double>();    //图层要素某字段值的集合
                List<string> sValueString = new List<string>(); // datagirdview表内容
                List<string> sValueScope = new List<string>();
                for (Int32 i = 0; i < sFeatureCount; i++)
                {
                    double sValue = Convert.ToDouble(_Layer.Features.GetItem(i).Attributes.GetItem(sFieldIndex));
                    sValues.Add(sValue);
                }
                //获取最小最大值，并分5级
                double sMinValue = sValues.Min();
                double sMaxValue = sValues.Max();
                for (Int32 i = 0; i <= classNum; i++)
                {
                    double sValue0 = sMinValue + (sMaxValue - sMinValue) * (i) / classNum;
                    double sValue = sMinValue + (sMaxValue - sMinValue) * (i + 1) / classNum;
                    sValueString.Add(sValue.ToString());
                    sValueScope.Add(Math.Round(sValue0, 2).ToString() + " - " + Math.Round(sValue, 2).ToString());
                    MyMapObjects.moSimpleLineSymbol sSymbol = new MyMapObjects.moSimpleLineSymbol();
                    sSymbol.Style = _SimpleLineSymbol.Style;
                    sSymbol.Size = _SimpleLineSymbol.Size;
                    sRenderer.AddBreakValue(sValue, sSymbol);
                }
                //设置起始末尾色
                Color sStartColor = btnStartColor.BackColor;
                Color sEndColor = btnEndColor.BackColor;
                sRenderer.RampColor(sStartColor, sEndColor);
                ////配置datagirdview表格
                GenerateDataGirdView(classNum, sRenderer, sValueString, moSymbolTypeConstant.SimpleLineSymbol, sValueScope);
                //赋给图层
                sRenderer.DefaultSymbol = _SimpleLineSymbol;
                _Layer.Renderer = sRenderer;
            }
            else if (_Layer.Renderer.RendererType == moRendererTypeConstant.ClassBreaks)
            {
                MyMapObjects.moClassBreaksRenderer sRenderer = (moClassBreaksRenderer)_Layer.Renderer;
                Int32 sValuesCount = sRenderer.BreakCount;
                for (Int32 i = 0; i < sValuesCount; i++)
                {
                    sRenderer.SetBreakValue(i, Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value));
                }

            }
           
        }
        private void DoPolygonClassBreaksRender()
        {
            if(_Layer == null)
            {
                return;
            }
            if(_DataGirdViewCBKChanged == false)
            {
                Int32 classNum = Convert.ToInt32(comboBox2.SelectedItem);
                MyMapObjects.moClassBreaksRenderer sRenderer = new MyMapObjects.moClassBreaksRenderer();
                sRenderer.Field = comboBox3.SelectedItem.ToString();
                //读出所有值
                Int32 sFieldIndex = _Layer.AttributeFields.FindField(sRenderer.Field);
                if (sFieldIndex < 0)
                    return;
                if (_Layer.AttributeFields.GetItem(sFieldIndex).ValueType == MyMapObjects.moValueTypeConstant.dText)
                    return;
                Int32 sFeatureCount = _Layer.Features.Count;  //图层所有要素
                List<double> sValues = new List<double>();    //图层要素某字段值的集合
                List<string> sValueString = new List<string>(); // datagirdview表内容
                List<string> sValueScope = new List<string>();
                for (Int32 i = 0; i < sFeatureCount; i++)
                {
                    double sValue = Convert.ToDouble(_Layer.Features.GetItem(i).Attributes.GetItem(sFieldIndex));
                    sValues.Add(sValue);
                }
                //获取最小最大值，并分5级
                double sMinValue = sValues.Min();
                double sMaxValue = sValues.Max();
                for (Int32 i = 0; i <= classNum; i++)
                {
                    double sValue0 = sMinValue + (sMaxValue - sMinValue) * (i) / classNum;
                    double sValue = sMinValue + (sMaxValue - sMinValue) * (i + 1) / classNum;
                    sValueString.Add(sValue.ToString());
                    sValueScope.Add(Math.Round(sValue0, 2).ToString() + " - " + Math.Round(sValue, 2).ToString());
                    MyMapObjects.moSimpleFillSymbol sSymbol = new MyMapObjects.moSimpleFillSymbol();
                    sSymbol.Outline.Style = _SimpleFillSymbol.Outline.Style;
                    sSymbol.Outline.Color = _SimpleFillSymbol.Outline.Color;
                    sSymbol.Outline.Size = _SimpleFillSymbol.Outline.Size;
                    sRenderer.AddBreakValue(sValue, sSymbol);
                }
                //设置起始末尾色
                Color sStartColor = btnStartColor.BackColor;
                Color sEndColor = btnEndColor.BackColor;
                sRenderer.RampColor(sStartColor, sEndColor);
                ////配置datagirdview表格
                GenerateDataGirdView(classNum, sRenderer, sValueString, moSymbolTypeConstant.SimpleFillSymbol, sValueScope);
                //赋给图层
                sRenderer.DefaultSymbol = _SimpleFillSymbol;
                _Layer.Renderer = sRenderer;
            }
            else if (_Layer.Renderer.RendererType == moRendererTypeConstant.ClassBreaks)
            {
                MyMapObjects.moClassBreaksRenderer sRenderer = (moClassBreaksRenderer)_Layer.Renderer;
                Int32 sValuesCount = sRenderer.BreakCount;
                for (Int32 i = 0; i < sValuesCount; i++)
                {
                    sRenderer.SetBreakValue(i, Convert.ToDouble(dataGridView2.Rows[i].Cells[1].Value));
                }

            }
        }

        #endregion

        #region 部分私有函数

        private void tabControl_LayerRender_SelectedIndexChanged(object sender, EventArgs e)
        {
            _RenderType = tabControl_LayerRender.SelectedTab.Name;
        }

        private void subSimple_SelectedIndexChanged(object sender, EventArgs e)
        {
            _GeometryType = subSimple.SelectedTab.Name;
        }

        

        private void comboBox2_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        ////配置datagirdview表格
        private void GenerateDataGirdView(Int32 valueCount, moRenderer renderer, List<string> values, moSymbolTypeConstant symbolType, List<string> valuescope = null)
        {
            //表格类型
            if (renderer.RendererType == moRendererTypeConstant.UniqueValue)
            {
                dataGridView1.Rows.Clear();
                MyMapObjects.moUniqueValueRenderer sRenderer = (moUniqueValueRenderer)renderer;
                for (Int32 i = 0; i <= valueCount - 1; i++)
                {
                    DataGridViewRow sRow = new DataGridViewRow();
                    DataGridViewButtonCell sBtnCell = new DataGridViewButtonCell();
                    DataGridViewTextBoxCell sTextCell1 = new DataGridViewTextBoxCell();
                    DataGridViewTextBoxCell sTextCell2 = new DataGridViewTextBoxCell();
                    DataGridViewTextBoxCell sTextCell3 = new DataGridViewTextBoxCell();
                    //判断图层类型
                    if (symbolType == moSymbolTypeConstant.SimpleMarkerSymbol)  //点渲染
                    {
                        MyMapObjects.moSimpleMarkerSymbol sSymbol = (moSimpleMarkerSymbol)sRenderer.GetSymbol(i);
                        sBtnCell.Style.BackColor = sSymbol.Color;
                    }
                    else if (symbolType == moSymbolTypeConstant.SimpleLineSymbol)
                    {
                        MyMapObjects.moSimpleLineSymbol sSymbol = (moSimpleLineSymbol)sRenderer.GetSymbol(i);
                        sBtnCell.Style.BackColor = sSymbol.Color;
                    }
                    else if (symbolType == moSymbolTypeConstant.SimpleFillSymbol)
                    {
                        MyMapObjects.moSimpleFillSymbol sSymbol = (moSimpleFillSymbol)sRenderer.GetSymbol(i);
                        sBtnCell.Style.BackColor = sSymbol.Color;
                    }
                    sBtnCell.FlatStyle = FlatStyle.Flat;
                    sBtnCell.Style.ForeColor = Color.White;
                    sBtnCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    sTextCell1.Value = values[i];
                    sRow.Cells.Add(sBtnCell);
                    sRow.Cells.Add(sTextCell1);
                    sRow.Cells.Add(sTextCell2);
                    sRow.Cells.Add(sTextCell3);
                    dataGridView1.Rows.Add(sRow);
                }
            }
            else if (renderer.RendererType == moRendererTypeConstant.ClassBreaks)
            {
                MyMapObjects.moClassBreaksRenderer sRenderer = (moClassBreaksRenderer)renderer;
                dataGridView2.Rows.Clear();
                for (Int32 i = 0; i <= valueCount-1; i++)
                {
                    DataGridViewRow sRow = new DataGridViewRow();
                    DataGridViewButtonCell sBtnCell = new DataGridViewButtonCell();
                    DataGridViewTextBoxCell sTextCell1 = new DataGridViewTextBoxCell();
                    DataGridViewTextBoxCell sTextCell2 = new DataGridViewTextBoxCell();
                    DataGridViewTextBoxCell sTextCell3 = new DataGridViewTextBoxCell();
                    //判断图层类型
                    if (symbolType == moSymbolTypeConstant.SimpleMarkerSymbol)  //点渲染
                    {
                        MyMapObjects.moSimpleMarkerSymbol sSymbol = (moSimpleMarkerSymbol)sRenderer.GetSymbol(i);
                        sBtnCell.Style.BackColor = sSymbol.Color;
                    }
                    else if (symbolType == moSymbolTypeConstant.SimpleLineSymbol)
                    {
                        MyMapObjects.moSimpleLineSymbol sSymbol = (moSimpleLineSymbol)sRenderer.GetSymbol(i);
                        sBtnCell.Style.BackColor = sSymbol.Color;
                    }
                    else if (symbolType == moSymbolTypeConstant.SimpleFillSymbol)
                    {
                        MyMapObjects.moSimpleFillSymbol sSymbol = (moSimpleFillSymbol)sRenderer.GetSymbol(i);
                        sBtnCell.Style.BackColor = sSymbol.Color;
                    }
                    sBtnCell.FlatStyle = FlatStyle.Flat;
                    sBtnCell.Style.ForeColor = Color.White;
                    sBtnCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    sTextCell1.Value = values[i];
                    sTextCell2.Value = valuescope[i];
                    sRow.Cells.Add(sBtnCell);
                    sRow.Cells.Add(sTextCell1);
                    sRow.Cells.Add(sTextCell2);
                    sRow.Cells.Add(sTextCell3);
                    dataGridView2.Rows.Add(sRow);
                }

            }

        }

        //手动修改value样式
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                DialogResult dr = colorDialog1.ShowDialog();
                //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
                if (dr == DialogResult.OK)
                {
                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = colorDialog1.Color;
                    dataGridView1.Update();
                }
            }
            _DataGridViewBtnChanged = true;//已进行修改
            //为避免bug，需要
        }
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                //T0DO
            }
            _DataGirdViewCBKChanged = true;
            //为避免bug，需要
        }
        #endregion

        #region 设置颜色
        private void btnStartColor_Click(object sender, EventArgs e)
        {
            //显示颜色对话框
            DialogResult dr = colorDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            {
                btnStartColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnEndColor_Click(object sender, EventArgs e)
        {
            //显示颜色对话框
            DialogResult dr = colorDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            {
                btnEndColor.BackColor = colorDialog1.Color;
            }
        }
        //设置颜色
        private void btnColor_Click_1(object sender, EventArgs e)
        {
            //显示颜色对话框
            DialogResult dr = colorDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            {
                btnColorPoint.BackColor = colorDialog1.Color;
            }
        }

        private void btnColorPolygon_Click(object sender, EventArgs e)
        {
            DialogResult dr = colorDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            {
                btnColorPolygon.BackColor = colorDialog1.Color;
            }
        }

        private void btnOutColor_Click(object sender, EventArgs e)
        {
            DialogResult dr = colorDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            {
                btnOutColor.BackColor = colorDialog1.Color;
            }
        }

        private void btnColorPolyline_Click(object sender, EventArgs e)
        {
            DialogResult dr = colorDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            {
                btnColorPolyline.BackColor = colorDialog1.Color;
            }
        }

        private void btnColorPoint_Click(object sender, EventArgs e)
        {
            DialogResult dr = colorDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            {
                btnColorPoint.BackColor = colorDialog1.Color;
            }
        }
        #endregion

    }
}
