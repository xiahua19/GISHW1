using MyMapObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Imaging;
using Encoder = System.Drawing.Imaging.Encoder;
using System.IO;
using System.Runtime.Serialization;
using Gif.Components;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace FSGIS.Forms
{
    public partial class SeriesRender : Form
    {
        public MainForm frmContainer;  //父窗体
        public MyMapObjects.moMapLayer _Layer;            //当前处理的图层

        private MyMapObjects.moRenderer _Renderer;        //当前图层的渲染方式
        private MyMapObjects.moSymbol _Symbol;            //当前图层的符号样式



        //便于程序运行的变量
        private MyMapObjects.moSimpleRenderer _SimpleRenderer;
        private MyMapObjects.moUniqueValueRenderer _UniqueValueRenderer;
        private MyMapObjects.moClassBreaksRenderer _ClassBreakRenderer = new moClassBreaksRenderer();

        private MyMapObjects.moSimpleLineSymbol _SimpleLineSymbol;
        private MyMapObjects.moSimpleFillSymbol _SimpleFillSymbol = new moSimpleFillSymbol();




        private MyMapObjects.moFields _AttributeFields;   //当前图层的字段信息
        private List<MyMapObjects.moField> _UseAttributeFields = new List<MyMapObjects.moField>();
        private List<MyMapObjects.moField> _NotUseAttributeFields = new List<moField>();  //要渲染的字段


        bool _DataGridViewBtnChanged = false;             //手动修改样式
        bool _DataGirdViewCBKChanged = false;            //手动修改节点

        //收集按钮点击的信息
        String _RenderType;
        String _GeometryType;
        moSimpleMarkerSymbolStyleConstant _MarkerSymbol;   //点符号样式
        moSimpleLineSymbolStyleConstant _PolylineSymbol;   //面符号样式
        public SeriesRender()
        {
            frmContainer = (MainForm)this.Owner; //设置父窗体
            InitializeComponent();
        }

        private void SeriesRender_Load(object sender, EventArgs e)
        {
            _AttributeFields = _Layer.AttributeFields;  //图层的字段信息
            _Renderer = _Layer.Renderer;               //图层渲染信息
            //将字段信息写入唯一值、分级渲染绑定选项
            checkedListBox1.Items.Clear();
            checkedListBox2.Items.Clear();
            for (Int32 i = 0; i < _AttributeFields.Count; ++i)
            {
                _NotUseAttributeFields.Add(_AttributeFields.GetItem(i));
            }
            UpdateChecklistbox();
        }



        private void button7_Click(object sender, EventArgs e)
        {
            //显示颜色对话框
            DialogResult dr = colorDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            {
                button7.BackColor = colorDialog1.Color;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //显示颜色对话框
            DialogResult dr = colorDialog1.ShowDialog();
            //如果选中颜色，单击“确定”按钮则改变文本框的文本颜色
            if (dr == DialogResult.OK)
            {
                button6.BackColor = colorDialog1.Color;
            }
        }



        private void UpdateChecklistbox()
        {
            checkedListBox1.Items.Clear ();
            checkedListBox2.Items.Clear ();
            for (Int32 i = 0; i < _NotUseAttributeFields.Count; ++i)
            {
                checkedListBox2.Items.Add(_NotUseAttributeFields[i].Name);
            }
            for (Int32 i = 0; i < _UseAttributeFields.Count; ++i)
            {
                checkedListBox1.Items.Add(_UseAttributeFields[i].Name);
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedIndices.Count == 0) { return; }
            Int32 sMinus = 0;
            for (Int32 i = 0; i < checkedListBox1.CheckedIndices.Count; ++i)
            {
                Int32 sIdx = checkedListBox1.CheckedIndices[i] - sMinus;
                _NotUseAttributeFields.Add(_UseAttributeFields[sIdx]);
                _UseAttributeFields.RemoveAt(sIdx);
                sMinus += 1;
            }

            UpdateChecklistbox();
        }

        /// <summary>
        /// 向下挪动UseAttributeField
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedIndices.Count == 0 || checkedListBox1.CheckedIndices.Count >= 2)
            {
                MessageBox.Show("调整顺序时，请只给一个属性打勾。");
                return;
            }
            if (checkedListBox1.CheckedIndices[0] == _UseAttributeFields.Count - 1) return;
            moField sTempField = _UseAttributeFields[checkedListBox1.CheckedIndices[0]];
            _UseAttributeFields[checkedListBox1.CheckedIndices[0]] = _UseAttributeFields[checkedListBox1.CheckedIndices[0] + 1];
            _UseAttributeFields[checkedListBox1.CheckedIndices[0] + 1] = sTempField;
            UpdateChecklistbox();
        }

        /// <summary>
        /// 向上挪动UseAttributeField
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if (checkedListBox1.CheckedIndices.Count == 0 || checkedListBox1.CheckedIndices.Count >= 2)
            {
                MessageBox.Show("调整顺序时，请只给一个属性打勾。");
                return;
            }
            if (checkedListBox1.CheckedIndices[0] == 0) return;
            moField sTempField = _UseAttributeFields[checkedListBox1.CheckedIndices[0]];
            _UseAttributeFields[checkedListBox1.CheckedIndices[0]] = _UseAttributeFields[checkedListBox1.CheckedIndices[0] - 1];
            _UseAttributeFields[checkedListBox1.CheckedIndices[0] - 1] = sTempField;
            UpdateChecklistbox();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (checkedListBox2.SelectedIndices.Count == 0) { return; }
            Int32 sMinus = 0;
            for (Int32 i = 0; i < checkedListBox2.CheckedIndices.Count; ++i)
            {
                
                Int32 sIdx = checkedListBox2.CheckedIndices[i] - sMinus;
                if (_NotUseAttributeFields[sIdx].ValueType == moValueTypeConstant.dText) continue;
                _UseAttributeFields.Add(_NotUseAttributeFields[sIdx]);
                _NotUseAttributeFields.RemoveAt(sIdx);
                sMinus += 1;
            }

            UpdateChecklistbox();

        }

        /// <summary>
        /// 分级
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button9_Click(object sender, EventArgs e)
        {
            if(checkedListBox1.Items.Count == 0) { return; }
            Int32 classNum = Convert.ToInt32(numericUpDown1.Value);
            MyMapObjects.moClassBreaksRenderer sRenderer = new MyMapObjects.moClassBreaksRenderer();
            sRenderer.Field = checkedListBox1.Items[0].ToString();
            //读出所有值

            Int32 sFeatureCount = _Layer.Features.Count;  //图层所有要素
            List<double> sValues = new List<double>();    //图层要素某字段值的集合
            List<string> sValueString = new List<string>(); // datagirdview表内容
            List<string> sValueScope = new List<string>();  //分级范围
            for (Int32 i = 0; i < sFeatureCount; i++)
            {
                for (Int32 j = 0; j < _UseAttributeFields.Count; j++)
                {
                    Int32 sFieldIndex = _Layer.AttributeFields.FindField(_UseAttributeFields[j].Name);
                    double sValue = Convert.ToDouble(_Layer.Features.GetItem(i).Attributes.GetItem(sFieldIndex));
                    sValues.Add(sValue);
                }
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
            Color sStartColor = button6.BackColor;
            Color sEndColor = button7.BackColor;
            sRenderer.RampColor(sStartColor, sEndColor);
            _ClassBreakRenderer = sRenderer;
            ////配置datagirdview表格
            GenerateDataGirdView(classNum, sRenderer, sValueString, moSymbolTypeConstant.SimpleFillSymbol, sValueScope);
            //赋给图层
            sRenderer.DefaultSymbol = _SimpleFillSymbol;
            _Layer.Renderer = sRenderer;
        }

        private void GenerateDataGirdView(Int32 valueCount, moRenderer renderer, List<string> values, moSymbolTypeConstant symbolType, List<string> valuescope = null)
        {
            if (renderer.RendererType == moRendererTypeConstant.ClassBreaks)
            {
                MyMapObjects.moClassBreaksRenderer sRenderer = (moClassBreaksRenderer)renderer;
                dataGridView1.Rows.Clear();
                for (Int32 i = 0; i <= valueCount - 1; i++)
                {
                    DataGridViewRow sRow = new DataGridViewRow();
                    DataGridViewButtonCell sBtnCell = new DataGridViewButtonCell();
                    DataGridViewTextBoxCell sTextCell1 = new DataGridViewTextBoxCell();

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

                    dataGridView1.Rows.Add(sRow);
                }

            }
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if(_ClassBreakRenderer == null) { return; }
            _ClassBreakRenderer.Field = _UseAttributeFields[checkedListBox1.SelectedIndex].Name;
            _Layer.Renderer = _ClassBreakRenderer;
            frmContainer.myMapControl.RedrawMap();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfg = new SaveFileDialog();
            sfg.Title = "导出图片";
            sfg.Filter = "所有文件(*.*)|*.*";

            sfg.RestoreDirectory = true;
            if (sfg.ShowDialog() == DialogResult.OK)
            {
                List<Image> frames = new List<Image>();
                for (Int32 i = 0; i < _UseAttributeFields.Count; i++)
                {
                    _ClassBreakRenderer.Field = _UseAttributeFields[i].Name;
                    _Layer.Renderer = _ClassBreakRenderer;
                    frmContainer.myMapControl.RedrawMap();

                    Image image = frmContainer.myMapControl.FSPicture();
                    using (Graphics g = Graphics.FromImage(image))
                    {
                        Brush textBrush = new SolidBrush(Color.Black);
                        g.DrawString(_UseAttributeFields[i].Name, new Font("微软雅黑", 16), textBrush, 0, 0);
                    }
                    frames.Add(image);
                }
                CreateAnimatedGif(sfg.FileName, frames, 500);
                
            }
            
        }
        private const int PropertyTagFrameDelay = 0x5100;

        public static void CreateAnimatedGif(string outputFilePath, List<Image> imageFrames, int frameDelay)
        {

            try
            {
                AnimatedGifEncoder el = new AnimatedGifEncoder();
                el.Start(outputFilePath);
                el.SetDelay(frameDelay);
                //0:循环播放    -1:不循环播放
                el.SetRepeat(0);

                for (int i = 0; i < imageFrames.Count; i++)
                {
                    el.AddFrame(imageFrames[i]);
                }
                el.Finish();
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            _UseAttributeFields.Sort((a, b) => a.Name.CompareTo(b.Name));
            UpdateChecklistbox();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            for(Int32 i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, true);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            for (Int32 i = 0; i < checkedListBox2.Items.Count; i++)
            {
                checkedListBox2.SetItemChecked(i, !checkedListBox2.GetItemChecked(i));
            }
        }
    }
}
