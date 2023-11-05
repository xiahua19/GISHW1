using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FSGIS.SubSystems;
using FSGIS.Forms;
using MyMapObjects;

namespace FSGIS
{
    public partial class MainForm : Form
    {
        #region 变量

        // （1）基本变量
        private bool showLngLat = false;        // 显示地理坐标
        private bool startEditing = false;      // 开始编辑要素
        private bool isInPaning = false;        // 漫游状态下
        private bool isInSelecting = false;     // 选择状态下
        private int selectedLayerIndex = -1;    // 在图层列表框中选中的图层索引
        private PointF selectBoxStart = new PointF();   // 框选/点选的起点

        private int panMousePositionX = 0;         // 鼠标按下后所处的X坐标位置
        private int panMousePositionY = 0;         // 鼠标按下后所处的Y坐标位置

        private AttributesTableForm attrTBL;    // 属性表窗口
        private SelectByAttri sbaForm;          // 属性查询窗口
        private LayerRender layerRender;         //符号化窗口 
        private LabelRender fontRender;         //标注设置窗口

        private SelectedAttri selectedAttri;    // 查看选中要素的属性

        private SelectEditLayer selectEditLayer;// 选择编辑的图层
        private MyMapObjects.moMapLayer editLayer;  // 编辑的图层

        private string projPath = "";           // 当前打开的工程文件的路径

        //
        // （2）图形操作相关字段
        // 操作几何图形的状态，
        //      0：选择；
        //      1：移动要素；
        //      2：新增要素；
        //      3：编辑结点；
        //      4：拖动；
        //      5：Identify
        private int geoOperationMode = 0;           
        // （2.1）移动图形相关
        private bool isInMovingShapes = false;      // 正在移动图形 
        // 正在移动的图形集合
        private List<MyMapObjects.moGeometry> movingGeometries = new List<MyMapObjects.moGeometry>();

        // 移动图形样式
        private MyMapObjects.moSimpleFillSymbol movingPolygonSymbol = new moSimpleFillSymbol();
        private MyMapObjects.moSimpleMarkerSymbol movingPointSymbol = new moSimpleMarkerSymbol();
        private MyMapObjects.moSimpleLineSymbol movingPolylineSymbol = new moSimpleLineSymbol();

        // 移动起点
        private PointF movingStart; // 移动起点
        private PointF movingPoint; // 移动过程中的点

        // （2.2） 创建要素图形相关
        private List<MyMapObjects.moPoints> sketchingShape;
        private MyMapObjects.moSimpleFillSymbol editingPolygonSymbol = new moSimpleFillSymbol();
        private MyMapObjects.moSimpleMarkerSymbol editingVertexSymbol = new moSimpleMarkerSymbol();
        private MyMapObjects.moSimpleLineSymbol editingLineSymbol = new moSimpleLineSymbol();
        private MyMapObjects.moSimpleLineSymbol elasticSymbol = new moSimpleLineSymbol();

        // （2.3） 编辑图形相关
        private PointF editingStart;
        private MyMapObjects.moGeometry editingGeometry = null;
        private bool isDragging = false;
        private MyMapObjects.moPoint draggingPoint;
        private Int32 partIndex;
        private Int32 pointIndex;
        private PointF addVertexClickPosition;

        #region 类内枚举

        /// <summary>
        /// 枚举状态变量
        /// </summary>
        //private enum Status
        //{
        //    Arrow = 1,  // 箭头
        //    Pan = 2,    // 手掌拖动
        //    Edit = 3    // 编辑状态
        //}

        #endregion

        public static MainForm form;            // 窗体变量
        #endregion

        #region 构造函数

        public MainForm()
        {
            form = this;                                // 窗体变量form指向当前窗体this
            this.MouseWheel += myMapControl_MouseWheel; // 给myMapControl添加鼠标滚轮滚动事件
            selectedAttri = new SelectedAttri();        // 初始化对象
            InitializeComponent();                      // 初始化组件
        }

        #endregion

        #region 事件处理函数

        #region 加载界面
        private void MainForm_Load(object sender,EventArgs e)
        {
            
            InitializeSketchingShape();
            InitializeSymbols();
            箭头.Checked = true;
            SetCurrentStatus();
        }
        #endregion

        #region 状态响应

        private void 开始编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startEditing = true;
            if (this.attrTBL != null && !this.attrTBL.IsDisposed) attrTBL.SetAllEditable();

        }

        private void 保存修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 停止编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startEditing = false;
            if (this.attrTBL != null && !this.attrTBL.IsDisposed) attrTBL.SetAllNotEditable();
        }

        /// <summary>
        /// 修改当前状态变量为箭头
        /// </summary>
        private void 箭头_Click(object sender, EventArgs e)
        {
            geoOperationMode = 0;
            箭头.Checked = true;
            拖动.Checked = false;
            移动要素.Checked = false;
            创建要素.Checked = false;
            编辑要素.Checked = false;
            Identify.Checked = false;
            SetCurrentStatus();
        }

        /// <summary>
        /// 修改当前状态变量为拖动
        /// </summary>
        private void 拖动_Click(object sender, EventArgs e)
        {
            geoOperationMode = 4;
            箭头.Checked = false;
            拖动.Checked = true;
            移动要素.Checked = false;
            创建要素.Checked = false;
            编辑要素.Checked = false;
            SetCurrentStatus();
        }

        /// <summary>
        /// 进入或退出编辑状态的操作
        /// </summary>
        private void 图形编辑器_Click(object sender, EventArgs e)
        {
            // 退出编辑状态
            if (startEditing == true)
            {
                this.startEditing = false;
                // 修改当前状态为选择
                if(geoOperationMode != 0&& geoOperationMode !=4)
                {
                    geoOperationMode = 0;
                    SetButtonsDefault();
                    箭头.Checked = true;
                }

                // 将创建要素、编辑要素和移动要素三个按钮设为不可用
                创建要素.Enabled = false;
                编辑要素.Enabled = false;
                移动要素.Enabled = false;

                editLayer = null;

                // 设置属性表不可编辑
                if (attrTBL != null && !attrTBL.IsDisposed) attrTBL.SetAllNotEditable();

                InitializeEditingShape();
                InitializeSketchingShape();
                myMapControl.Refresh();
                myMapControl.RedrawMap();
            }
            // 进入编辑状态
            else
            {
                // 判断是否存在可编辑的图层并选择之一
                MyMapObjects.moLayers sEditableFeatures = new moLayers();
                for(int i = 0;i<this.myMapControl.Layers.Count;++i)
                {
                    if (myMapControl.Layers.GetItem(i).Visible == true) 
                        sEditableFeatures.Add(myMapControl.Layers.GetItem(i)); 
                }

                if(sEditableFeatures.Count == 0)
                {
                    MessageBox.Show("无显示图层，请导入图层并设置图层可见！");
                    return;
                }
                // 存在可编辑的图层，打开选择框
                else
                {
                    if(this.selectEditLayer == null || this.selectEditLayer.IsDisposed)
                    {
                        this.selectEditLayer = new SelectEditLayer(this.myMapControl.Layers);
                        this.selectEditLayer.SetEditLayer += SelectEditLayer_SetEditLayer;
                        // 对话框只读
                        this.selectEditLayer.ShowDialog();
                    }
                }        
            }
            SetCurrentStatus();
        }

        /// <summary>
        /// 设置编辑图层
        /// </summary>
        /// <param name="layer"></param>
        private void SelectEditLayer_SetEditLayer(moMapLayer layer)
        {
            if (layer == null) return;
            this.editLayer = layer;

            // 修改编辑状态为真，geoOperationMode不变
            startEditing = true;

            // 将创建要素和编辑要素两个按钮设为可用
            创建要素.Enabled = true;
            编辑要素.Enabled = true;
            移动要素.Enabled = true;

            // 设置图层属性表可修改
            if (attrTBL != null && !attrTBL.IsDisposed)
            {
                    attrTBL.SetLayerEditable(layer);
            }

            // 设置图层肮脏
            layer.IsDirty = true;
        }

        /// <summary>
        /// 点击创建要素按钮后触发的事件：修改状态变量
        /// </summary>
        private void 创建要素_Click(object sender, EventArgs e)
        {
            if (!startEditing) return;
            if(!editLayer.Visible)
            {
                editLayer.Visible = true;
                int index = myMapControl.Layers.IndexOf(editLayer);
                if (index != -1) this.checkedListBox1.SetItemChecked(index, true) ;
            }
            if(geoOperationMode == 2)
            {
                geoOperationMode = 0;
                箭头.Checked = true;
                创建要素.Checked = false;
                SetCurrentStatus();
                return;
            }
            this.geoOperationMode = 2;
            SetButtonsDefault();
            创建要素.Checked = true;
            InitializeEditingShape();
            SetCurrentStatus();
        }

        /// <summary>
        /// 点击编辑要素按钮后触发的事件：修改状态变量
        /// </summary>
        private void 编辑要素_Click(object sender, EventArgs e)
        {

            // 连点两次编辑节点，退出编辑节点状态
            if(geoOperationMode == 3)
            {
                geoOperationMode = 0;
                编辑要素.Checked = false;
                箭头.Enabled = true;
                箭头.Checked = true;

                editingGeometry = null;
                draggingPoint = null;
                isDragging = false;
                myMapControl.RedrawMap();
                DrawEditingShapes(myMapControl.GetDrawingTool());
                SetCurrentStatus();
                return;
            }

            if (!editLayer.Visible)
            {
                editLayer.Visible = true;
                int index = myMapControl.Layers.IndexOf(editLayer);
                if (index != -1) this.checkedListBox1.SetItemChecked(index, true);
            }
            if (editLayer.SelectedFeatures.Count!=1)
            {
                MessageBox.Show("请选择且仅选择一个要素！");
                return;
            }



            // 复制
            if (editingGeometry == null)
            {
                if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    MyMapObjects.moMultiPolygon sOriMultiPolygon = (MyMapObjects.moMultiPolygon)editLayer.SelectedFeatures.GetItem(0).Geometry;
                    MyMapObjects.moMultiPolygon sDesMultiPolygon = sOriMultiPolygon.Clone();
                    editingGeometry = sDesMultiPolygon;
                }
                else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    MyMapObjects.moMultiPolyline sOriMultiPolyline = (MyMapObjects.moMultiPolyline)editLayer.SelectedFeatures.GetItem(0).Geometry;
                    MyMapObjects.moMultiPolyline sDesMultiPolyline = sOriMultiPolyline.Clone();
                    editingGeometry = sDesMultiPolyline;
                }

                else if (editLayer.ShapeType == moGeometryTypeConstant.Point)
                {
                    MyMapObjects.moPoint sOriPoint = (MyMapObjects.moPoint)editLayer.SelectedFeatures.GetItem(0).Geometry;
                    MyMapObjects.moPoint sDesPoint = sOriPoint.Clone();
                    editingGeometry = sDesPoint;
                }
            }


            // 设置操作类型并重绘跟踪
            this.geoOperationMode = 3;
            SetButtonsDefault();
            箭头.Enabled = false;
            编辑要素.Checked = true;
            myMapControl.RedrawTrackingShapes();
            SetCurrentStatus();
        }

        #endregion

        #region 数据输入/输出

        /// <summary>
        /// 添加图层文件菜单栏点击后触发的事件：读取lay文件并显示
        /// </summary>
        private void 添加图层文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FetchLayerMetaData fetchLayerMeta = new FetchLayerMetaData();
            string selectLayer = "";
            if (fetchLayerMeta.ShowDialog() == DialogResult.OK)
            {
                selectLayer = fetchLayerMeta.SelectLayer;
            }

            try
            {
                MyMapObjects.moMapLayer sLayer = DataBaseTools.LoadMapLayer(selectLayer);
                myMapControl.Layers.Add(sLayer);
                if (myMapControl.Layers.Count == 1)
                {
                    myMapControl.FullExtent();
                }
                else
                {
                    myMapControl.RedrawMap();
                }

                //修改checklistbox
                AddLayerInCheckList(System.IO.Path.GetFileNameWithoutExtension(selectLayer));
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
                return;
            }
        }

        /// <summary>
        /// 保存图层文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存图层文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < myMapControl.Layers.Count; ++i)
            {
                DataBaseTools.WriteLayerToFile(myMapControl.Layers.GetItem(i));
                //DataIOTools.WriteLayerToFile(myMapControl.Layers.GetItem(i), myMapControl.Layers.GetItem(i).Path);
            }
        }

        /// <summary>
        /// 读取工程文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 读取工程文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog sFileDialog = new OpenFileDialog();
            string sFileName = "";
            if (sFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                sFileName = sFileDialog.FileName;
                projPath = sFileName;
                sFileDialog.Dispose();
            }
            else
            {
                sFileDialog.Dispose();
                return;
            }

            try
            {
                FileStream sFileStream = new FileStream(sFileName, FileMode.Open);
                BinaryReader sr = new BinaryReader(sFileStream);
                MyMapObjects.moLayers sLayers = DataBaseTools.LoadMapProj(sr, sFileName);
                for(int i = 0; i < sLayers.Count; ++i)
                {
                    myMapControl.Layers.Add(sLayers.GetItem(i));
                }

                if (myMapControl.Layers.Count == 1)
                {
                    myMapControl.FullExtent();
                }
                else
                {
                    myMapControl.RedrawMap();
                }

                for (int i = 0; i < sLayers.Count; ++i)
                {

                    //修改checklistbox
                    AddLayerInCheckList(System.IO.Path.GetFileNameWithoutExtension(sLayers.GetItem(i).Path));

                }
                sr.Dispose();
                sFileStream.Dispose();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
                return;
            }

            myMapControl.FullExtent();
            myMapControl.RedrawMap();

        }

        /// <summary>
        /// 另存工程文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 另存工程文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog layFileDialog = new SaveFileDialog();
            layFileDialog.Title = "另存为";
            layFileDialog.Filter = ".FSPRJ文件(*.fsprj)|*.fsprj";
            layFileDialog.RestoreDirectory = true;
            if (DialogResult.OK == layFileDialog.ShowDialog())
            {
                DataBaseTools.WriteLayersToProj(myMapControl.Layers, layFileDialog.FileName);
                MessageBox.Show("成功写入文件");
                //DataIOTools.WriteLayerToFile(myMapControl.Layers.GetItem(selectedLayerIndex),
                //    layFileDialog.FileName.Split('.')[0] + ".lay");
            }
        }

        /// <summary>
        /// 保存工程文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存工程文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(projPath != "")
            {
                DataBaseTools.WriteLayersToProj(myMapControl.Layers, projPath);
                MessageBox.Show("成功写入文件");
            }
        }

        /// <summary>
        /// 打印地图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打印地图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfg = new SaveFileDialog();
            sfg.Title = "导出图片";
            sfg.Filter = "JPG文件(*.jpg)|*.jpg|BMP文件(*.bmp)|*.bmp|PNG文件(*.png)|*.png|所有文件(*.*)|*.*";

            sfg.RestoreDirectory = true;
            if (sfg.ShowDialog() == DialogResult.OK)
            {
                Image image = myMapControl.FSPicture();
                string fileType = sfg.FileName.Substring(sfg.FileName.LastIndexOf(".") + 1);
                switch (fileType)
                {

                }
                image.Save(sfg.FileName, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        /// <summary>
        /// 图层另存为
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 另存为ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog layFileDialog = new SaveFileDialog();
            layFileDialog.Title = "另存为";
            layFileDialog.Filter = "LAY文件(*.lay)|*.lay";
            layFileDialog.RestoreDirectory = true;
            if (DialogResult.OK == layFileDialog.ShowDialog())
            {
                DataIOTools.WriteLayerToFile(myMapControl.Layers.GetItem(selectedLayerIndex),
                    layFileDialog.FileName);
                MessageBox.Show("成功写入文件");
                //DataIOTools.WriteLayerToFile(myMapControl.Layers.GetItem(selectedLayerIndex),
                //    layFileDialog.FileName.Split('.')[0] + ".lay");
            }
        }

        #endregion

        #region 图层相关操作
        // 添加新的图层文件，更改列表显示 checkedListBox1
        private void AddLayerInCheckList(String layerName)
        {
            checkedListBox1.Items.Add(layerName);
            checkedListBox1.SetItemCheckState(checkedListBox1.Items.Count - 1, CheckState.Checked);
            //绘制
        }

        private void checkedListBox1_MouseDown(object sender, MouseEventArgs e)
        {
            int sSelectedLayerIndex = checkedListBox1.IndexFromPoint(e.Location);
            checkedListBox1.SelectedIndex = sSelectedLayerIndex;
            if (e.Button == MouseButtons.Right)
            {

                if (sSelectedLayerIndex != -1)
                {
                    selectedLayerIndex = sSelectedLayerIndex;
                    checkedListBox1.SelectedIndex = sSelectedLayerIndex;
                    for (int i = 0; i < contextMenuStrip.Items.Count; ++i)
                    {
                        contextMenuStrip.Items[i].Enabled = true;
                    }
                    contextMenuStrip.Show(MousePosition);
                }
                else
                {
                    selectedLayerIndex = -1;
                    for (int i = 0; i < contextMenuStrip.Items.Count; ++i)
                    {
                        contextMenuStrip.Items[i].Enabled = false;
                    }
                    contextMenuStrip.Show(MousePosition);
                }
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (sSelectedLayerIndex != -1)
                {
                    selectedLayerIndex = sSelectedLayerIndex;
                    checkedListBox1.SelectedIndex = sSelectedLayerIndex;
                    for (int i = 0; i < contextMenuStrip.Items.Count; ++i)
                    {
                        contextMenuStrip.Items[i].Enabled = true;
                    }

                }
                else
                {
                    selectedLayerIndex = -1;
                    for (int i = 0; i < contextMenuStrip.Items.Count; ++i)
                    {
                        contextMenuStrip.Items[i].Enabled = false;
                    }
                }
            }
        }

        private void RemoveLayerInCheckList()
        {
            checkedListBox1.Items.RemoveAt(selectedLayerIndex);
        }
        private void 移除图层ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (myMapControl.Layers.GetItem(selectedLayerIndex).IsDirty)
            {
                if (MessageBox.Show("确定移除图层吗？系统将不会保留您此次的修改，\n若要保留修改，请右键图层单击“另存为图层”，或者点击菜单“文件”-“保存图层文件”", "移除图层", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    if (startEditing == true && myMapControl.Layers.GetItem(selectedLayerIndex) == editLayer)
                    {
                        startEditing = false;
                        SetButtonsDefault();
                        InitializeEditingShape();
                        InitializeSketchingShape();
                        if (geoOperationMode == 1 || geoOperationMode == 2 || geoOperationMode == 3)
                        {
                            箭头.Checked = true;
                            geoOperationMode = 0;
                        }
                    }
                }
                else return;
            }

            if (this.attrTBL != null && 
                !this.attrTBL.IsDisposed && 
                this.attrTBL.Exists(myMapControl.Layers.GetItem(selectedLayerIndex)))
                attrTBL.RemoveTable(myMapControl.Layers.GetItem(selectedLayerIndex));

            myMapControl.Layers.RemoveAt(selectedLayerIndex);

            RemoveLayerInCheckList();
            //重绘
            myMapControl.RedrawMap();

        }
        private void 上移一层_Click(object sender, EventArgs e)
        {
            if (selectedLayerIndex == -1) return;
            if (selectedLayerIndex == 0)  //已位于底层，无操作
            {

            }
            else
            {
                myMapControl.Layers.MoveTo(selectedLayerIndex, selectedLayerIndex - 1);
                var item = checkedListBox1.Items[selectedLayerIndex];
                checkedListBox1.Items.RemoveAt(selectedLayerIndex);
                checkedListBox1.Items.Insert(selectedLayerIndex - 1, item);
                checkedListBox1.SetItemChecked(selectedLayerIndex - 1, true);
                myMapControl.RedrawMap();
            }
        }

        private void 下移一层_Click(object sender, EventArgs e)
        {
            if (selectedLayerIndex == -1) return;
            if (selectedLayerIndex == checkedListBox1.Items.Count - 1)  //已位于底层，无操作
            {

            }
            else
            {
                myMapControl.Layers.MoveTo(selectedLayerIndex, selectedLayerIndex + 1);
                var item = checkedListBox1.Items[selectedLayerIndex];
                checkedListBox1.Items.RemoveAt(selectedLayerIndex);
                checkedListBox1.Items.Insert(selectedLayerIndex + 1, item);
                checkedListBox1.SetItemChecked(selectedLayerIndex + 1, true);
                myMapControl.RedrawMap();
            }
        }

        private void 置顶_Click(object sender, EventArgs e)
        {
            if (selectedLayerIndex == -1) return;

            if (selectedLayerIndex == 0)  //已位于底层，无操作
            {

            }
            else
            {
                myMapControl.Layers.MoveTo(selectedLayerIndex, 0);
                var item = checkedListBox1.Items[selectedLayerIndex];
                checkedListBox1.Items.RemoveAt(selectedLayerIndex);
                checkedListBox1.Items.Insert(0, item);
                checkedListBox1.SetItemChecked(0, true);
                myMapControl.RedrawMap();
            }
        }

        private void 置底_Click(object sender, EventArgs e)
        {
            if (selectedLayerIndex == -1) return;

            if (selectedLayerIndex == checkedListBox1.Items.Count - 1)  //已位于底层，无操作
            {

            }
            else
            {
                Int32 count = checkedListBox1.Items.Count;
                myMapControl.Layers.MoveTo(selectedLayerIndex, count - 1);
                var item = checkedListBox1.Items[selectedLayerIndex];
                checkedListBox1.Items.RemoveAt(selectedLayerIndex);
                checkedListBox1.Items.Insert(count - 1, item);
                checkedListBox1.SetItemChecked(count - 1, true);
                myMapControl.RedrawMap();
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            int idx = e.Index;//idx就是当前的选中项序号
            CheckState checkState = e.NewValue; // 获取当前选中项选中状态
            if (checkState == CheckState.Checked)
            {
                myMapControl.Layers.Visible(idx);
            }
            else
            {
                myMapControl.Layers.UnVisible(idx);
            }
            //重绘
            myMapControl.RedrawMap();
        }
        #endregion

        #region 图层渲染/注记配置

        private void 符号化ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (layerRender == null || layerRender.IsDisposed)
            {
                layerRender = new LayerRender();
                layerRender.frmContainer = this;
            }
            layerRender._Layer = myMapControl.Layers.GetItem(selectedLayerIndex);
            layerRender.Show(this);
        }

        #region  符号化的相关补充

        #endregion


        private void 标注要素ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fontRender == null || fontRender.IsDisposed)
            {
                fontRender = new LabelRender();
                fontRender.frmContainer = this;
            }
            fontRender._Layer = myMapControl.Layers.GetItem(selectedLayerIndex);
            fontRender.Show(this);
        }

        #endregion

        #region 属性操作

        /// <summary>
        /// 打开属性表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 打开属性表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (attrTBL == null || attrTBL.IsDisposed)
            {
                attrTBL = new AttributesTableForm();
                attrTBL._RefreshTrackingShape += RedrawTrackingShape;
            }

            if (!attrTBL.Visible)
                attrTBL.Show(this);
            if (attrTBL.Exists(this.myMapControl.Layers.GetItem(selectedLayerIndex)))
            {
                attrTBL.SetShowLayer(this.myMapControl.Layers.GetItem(selectedLayerIndex));
            }
            else
            {
                attrTBL.AddLayerTable(this.myMapControl.Layers.GetItem(selectedLayerIndex), this.startEditing);
            }
        }

        /// <summary>
        /// 按属性选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 按属性选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.sbaForm == null || this.sbaForm.IsDisposed)
            {
                this.sbaForm = new SelectByAttri(this.myMapControl.Layers.GetItem(selectedLayerIndex));
                this.sbaForm.ApplySearch += SearchByAttribute;
                this.sbaForm.Show(this);
            }
        }

        /// <summary>
        /// 处理选择事件的函数
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="SQL"></param>
        private void SearchByAttribute(MyMapObjects.moMapLayer layer, string SQL)
        {
            DataTable dt = GenerateDataTable(layer);
            try
            {
                DataRow[] res = dt.Select(SQL);
                MyMapObjects.moFeatures selectedFeatures = new moFeatures();
                for (int i = 0; i < res.Length; ++i)
                {
                    selectedFeatures.Add(layer.Features.GetItem(dt.Rows.IndexOf(res[i])));
                }
                layer.ExecuteSelect(selectedFeatures, 0);
                this.myMapControl.RedrawTrackingShapes();
                if (attrTBL != null && !attrTBL.IsDisposed && attrTBL.Exists(layer))
                {
                    attrTBL.RefreshSelected();
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }

        /// <summary>
        /// 属性表选择带来的重绘事件的处理函数
        /// </summary>
        private void RedrawTrackingShape()
        {
            this.myMapControl.RedrawTrackingShapes();
        }

        #endregion

        #region 图形操作

        /// <summary>
        /// 在myMapControl控件上鼠标滚轮滚动后触发的事件：以当前鼠标位置为中心进行缩放
        /// </summary>
        private void myMapControl_MouseWheel(object sender, MouseEventArgs e)
        {
            // 获取当前鼠标位置在地图上的坐标，作为缩放中心
            moPoint center = this.myMapControl.ToMapPoint(e.X, e.Y);

            // 如果鼠标滚轮向上滚动，则地图放大
            if (e.Delta > 0)
            {
                this.myMapControl.ZoomByCenter(center, 3);
            }
            // 如果鼠标滚轮向下滚动，则地图缩小
            else
            {
                this.myMapControl.ZoomByCenter(center, 0.5);
            }
        }

        /// <summary>
        /// 缩放到全图按钮点击后的事件
        /// </summary>
        private void 缩放到全图_Click(object sender, EventArgs e)
        {
            // 将控件内的地图缩放到全图
            this.myMapControl.FullExtent();
        }

        /// <summary>
        /// 鼠标在myMapControl控件上按下后触发的事件
        /// </summary>
        private void myMapControl_MouseDown(object sender, MouseEventArgs e)
        {
            if(this.geoOperationMode == 0 || this.geoOperationMode == 5)
            {
                OnSelecting_MouseDown(e);
            }
            else if (this.geoOperationMode == 1)
            {
                OnMoving_MouseDown(e);
            }
            else if(this.geoOperationMode == 2)
            {
                OnSketching_MouseDown(e);
            }
            else if (this.geoOperationMode == 3)
            {
                OnEditing_MouseDown(e);
            }
            else if(this.geoOperationMode == 4)
            {
                OnPaning_MouseDown(e);
            }
        }

        /// <summary>
        /// 选择状态下鼠标按下
        /// </summary>
        /// <param name="e"></param>
        private void OnSelecting_MouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            isInSelecting = true;
            selectBoxStart = e.Location;
        }

        /// <summary>
        /// 描绘图形状态下鼠标落下
        /// </summary>
        /// <param name="e"></param>
        private void OnSketching_MouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;

            // 新建要素时右键菜单
            if (editLayer.ShapeType == moGeometryTypeConstant.Point) return;
            else
            {
                MapContextMenu.Items[0].Enabled = true;
                MapContextMenu.Items[1].Enabled = true;
                MapContextMenu.Items[2].Enabled = false;
                MapContextMenu.Items[3].Enabled = false;
                MapContextMenu.Items[4].Enabled = false;
                MapContextMenu.Items[5].Enabled = false;
                MapContextMenu.Show(MousePosition);
            }
        }

        /// <summary>
        /// 漫游，拖动地图状态下鼠标落下
        /// </summary>
        /// <param name="e"></param>
        private void OnPaning_MouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            this.isInPaning = true;
            panMousePositionX = e.X;
            panMousePositionY = e.Y;
        }

        /// <summary>
        /// 编辑节点状态下鼠标按下
        /// </summary>
        /// <param name="e"></param>
        private void OnEditing_MouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right) return;
            // 右键菜单
            if (e.Button == MouseButtons.Right)
            {
                OnEditing_MouseClick(e);
            }

            // 左键编辑节点
            else if (e.Button == MouseButtons.Left)
            {
                MyMapObjects.moPoint sCurPoint = myMapControl.ToMapPoint(e.X, e.Y);
                this.editingStart = e.Location;
                //this.mousePositionX = e.Location.X;
                //this.mousePositionY = e.Location.Y;
                double sTolerance = myMapControl.ToMapDistance(this.editingVertexSymbol.Size) * 2;
                if (editLayer.ShapeType == moGeometryTypeConstant.Point)
                {
                    if (moMapTools.IsPointOnPoint(sCurPoint, (moPoint)editingGeometry, sTolerance))
                    {
                        this.draggingPoint = (moPoint)editingGeometry;
                        this.isDragging = true;
                    }

                }
                else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    for (int i = 0; i < ((moMultiPolyline)this.editingGeometry).Parts.Count; ++i)
                    {
                        moPoints sPoints = ((moMultiPolyline)this.editingGeometry).Parts.GetItem(i);
                        for (int j = 0; j < sPoints.Count; ++j)
                        {
                            if (moMapTools.IsPointOnPoint(sCurPoint, sPoints.GetItem(j), sTolerance))
                            {
                                this.draggingPoint = sPoints.GetItem(j);
                                this.isDragging = true;
                            }
                        }
                    }
                }
                else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    for (int i = 0; i < ((moMultiPolygon)this.editingGeometry).Parts.Count; ++i)
                    {
                        moPoints sPoints = ((moMultiPolygon)this.editingGeometry).Parts.GetItem(i);
                        for (int j = 0; j < sPoints.Count; ++j)
                        {
                            if (moMapTools.IsPointOnPoint(sCurPoint, sPoints.GetItem(j), sTolerance))
                            {
                                this.draggingPoint = sPoints.GetItem(j);
                                this.isDragging = true;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 移动要素时鼠标落下
        /// </summary>
        /// <param name="e"></param>
        private void OnMoving_MouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left && e.Button!= MouseButtons.Right) return;

            if (e.Button == MouseButtons.Right)
            {
                MapContextMenu.Items[0].Enabled = false;
                MapContextMenu.Items[1].Enabled = false;
                MapContextMenu.Items[2].Enabled = false;
                MapContextMenu.Items[3].Enabled = true;
                MapContextMenu.Items[4].Enabled = false;
                MapContextMenu.Items[5].Enabled = false;
                MapContextMenu.Show(MousePosition);
            }

            else if (e.Button == MouseButtons.Left)
            {
                MyMapObjects.moMapLayer sLayer = this.editLayer;
                if (sLayer == null) return;

                // 是否有选中的要素
                Int32 sSelFeatureCount = sLayer.SelectedFeatures.Count;
                if (sSelFeatureCount == 0) return;

                // 复制图形
                this.movingStart = e.Location;
                movingGeometries.Clear();
                for (Int32 i = 0; i < sSelFeatureCount; ++i)
                {
                    if (sLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                    {
                        MyMapObjects.moMultiPolygon sOriPolygon = (MyMapObjects.moMultiPolygon)sLayer.SelectedFeatures.GetItem(i).Geometry;
                        MyMapObjects.moMultiPolygon sDesPolygon = sOriPolygon.Clone();
                        movingGeometries.Add(sDesPolygon);
                    }
                    else if (sLayer.ShapeType == moGeometryTypeConstant.Point)
                    {
                        MyMapObjects.moPoint sOriPoint = (MyMapObjects.moPoint)sLayer.SelectedFeatures.GetItem(i).Geometry;
                        MyMapObjects.moPoint sDesPoint = sOriPoint.Clone();
                        movingGeometries.Add(sDesPoint);
                    }
                    else if (sLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                    {
                        MyMapObjects.moMultiPolyline sOriLine = (MyMapObjects.moMultiPolyline)sLayer.SelectedFeatures.GetItem(i).Geometry;
                        MyMapObjects.moMultiPolyline sDesLine = sOriLine.Clone();
                        movingGeometries.Add(sDesLine);
                    }
                }

                // 设置变量
                movingPoint = e.Location;
                isInMovingShapes = true;
            }

        }

        /// <summary>
        /// 鼠标在myMapControl控件上弹起后触发的事件
        /// </summary>
        private void myMapControl_MouseUp(object sender, MouseEventArgs e)
        {
            if(this.geoOperationMode == 0 )
            {
                OnSelecting_MouseUp(e);
            }
            else if (this.geoOperationMode == 1)
            {
                OnMoving_MouseUp(e);
            }
            else if (this.geoOperationMode == 2)
            {

            }
            else if(this.geoOperationMode == 3)
            {
                OnEditing_MouseUp(e);
            }
            else if(this.geoOperationMode == 4)
            {
                OnPaning_MouseUp(e);
            }
            else if (geoOperationMode == 5)
            {
                OnSelecting_MouseUp(e);

                int layerNum = myMapControl.Layers.Count;
                for (int i = 0; i < layerNum; ++i)
                {
                    MyMapObjects.moMapLayer layer = myMapControl.Layers.GetItem(i);
                    if (layer.SelectedFeatures.Count == 1)
                    {
                        if (selectedAttri.IsDisposed)
                        {
                            selectedAttri = new SelectedAttri();
                            selectedAttri.UpdateContent();
                        }
                        selectedAttri.Visible = false;
                        selectedAttri.Show(this);
                    }
                }
            }
        }

        /// <summary>
        /// 漫游状态下鼠标松开
        /// </summary>
        /// <param name="e"></param>
        private void OnPaning_MouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            isInPaning = false;
            double sDeltaX = myMapControl.ToMapDistance(e.Location.X - panMousePositionX);
            double sDeltaY = myMapControl.ToMapDistance(panMousePositionY - e.Location.Y);
            myMapControl.PanDelta(sDeltaX, sDeltaY);
        }

        /// <summary>
        /// 选择状态下鼠标弹起
        /// </summary>
        /// <param name="e"></param>
        private void OnSelecting_MouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            isInSelecting = false;
            if (this.myMapControl.Layers.Count != 0)
            {
                Point selectBoxEnd = e.Location;

                if(Math.Abs(selectBoxStart.X - selectBoxEnd.X) < 1e-5 && 
                    Math.Abs(selectBoxStart.Y - selectBoxEnd.Y) < 1e-5)
                {
                    selectBoxStart.X -= 3;
                    selectBoxEnd.X += 3;
                    selectBoxStart.Y -= 3;
                    selectBoxEnd.Y += 3;
                }

                // 将起点和终点转为地图坐标
                moPoint startPoint = this.myMapControl.ToMapPoint(selectBoxStart.X, selectBoxStart.Y);
                moPoint endPoint = this.myMapControl.ToMapPoint(selectBoxEnd.X, selectBoxEnd.Y);

                // 求解minX, minY, maxX, maxY
                double minX, minY, maxX, maxY;
                if (startPoint.X > endPoint.X)
                {
                    minX = endPoint.X;
                    maxX = startPoint.X;
                }
                else
                {
                    minX = startPoint.X;
                    maxX = endPoint.X;
                }

                if (startPoint.Y > endPoint.Y)
                {
                    minY = endPoint.Y;
                    maxY = startPoint.Y;
                }
                else
                {
                    minY = startPoint.Y;
                    maxY = endPoint.Y;
                }

                // 生成搜索矩形
                moRectangle selectingBox = new moRectangle(minX, maxX, minY, maxY);

                // 执行搜索
                this.myMapControl.SelectByBox(selectingBox, 0.01, 0);
                if (attrTBL != null && !attrTBL.IsDisposed)
                    this.attrTBL.RefreshSelected();

                // 重绘图片
                this.myMapControl.RedrawMap();

                // 更新选中要素的属性表
                selectedAttri.UpdateContent();
                selectedAttri.Refresh();
            }
        }

        /// <summary>
        /// 编辑节点转台下鼠标松开
        /// </summary>
        /// <param name="e"></param>
        private void OnEditing_MouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            myMapControl.RedrawMap();
            DrawEditingShapes(myMapControl.GetDrawingTool());
            this.isDragging = false;
            this.draggingPoint = null;
        }

        /// <summary>
        /// 移动状态鼠标松开
        /// </summary>
        /// <param name="e"></param>
        private void OnMoving_MouseUp(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            if (isInMovingShapes == false) return;
            isInMovingShapes = false;

            // 做相应数据修改，最后统一保存
            for(int i = 0;i<editLayer.SelectedFeatures.Count;++i)
            {
                if(editLayer.ShapeType == moGeometryTypeConstant.Point)
                {
                    MyMapObjects.moPoint sPoint = (MyMapObjects.moPoint)editLayer.SelectedFeatures.GetItem(i).Geometry;
                    sPoint.X = sPoint.X + myMapControl.ToMapDistance(e.Location.X - this.movingStart.X);
                    sPoint.Y = sPoint.Y - myMapControl.ToMapDistance(e.Location.Y - this.movingStart.Y); ;
                }

                else if(editLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)editLayer.SelectedFeatures.GetItem(i).Geometry;
                    Int32 sPartCount = sMultiPolygon.Parts.Count;
                    for (Int32 j = 0; j <= sPartCount - 1; j++)
                    {
                        MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(j);
                        Int32 sPointCount = sPoints.Count;
                        for (Int32 k = 0; k <= sPointCount - 1; k++)
                        {
                            MyMapObjects.moPoint sPoint = sPoints.GetItem(k);
                            sPoint.X = sPoint.X + myMapControl.ToMapDistance(e.Location.X - this.movingStart.X);
                            sPoint.Y = sPoint.Y - myMapControl.ToMapDistance(e.Location.Y - this.movingStart.Y);
                        }
                    }
                    sMultiPolygon.UpdateExtent();
                }
                else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)editLayer.SelectedFeatures.GetItem(i).Geometry;
                    Int32 sPartCount = sMultiPolyline.Parts.Count;
                    for (Int32 j = 0; j <= sPartCount - 1; j++)
                    {
                        MyMapObjects.moPoints sPoints = sMultiPolyline.Parts.GetItem(j);
                        Int32 sPointCount = sPoints.Count;
                        for (Int32 k = 0; k <= sPointCount - 1; k++)
                        {
                            MyMapObjects.moPoint sPoint = sPoints.GetItem(k);
                            sPoint.X = sPoint.X + myMapControl.ToMapDistance(e.Location.X - this.movingStart.X);
                            sPoint.Y = sPoint.Y - myMapControl.ToMapDistance(e.Location.Y - this.movingStart.Y);
                        }
                    }
                    sMultiPolyline.UpdateExtent();
                }
            }


            myMapControl.RedrawMap();

            // 清除移动图形列表
            movingGeometries.Clear();
        }

        /// <summary>
        /// 鼠标在myMapControl控件上移动时触发的事件
        /// </summary>
        private void myMapControl_MouseMove(object sender, MouseEventArgs e)
        {
            ShowCoordinates(e.Location);
            if (this.geoOperationMode == 0)
            {
                OnSelecting_MouseMove(e);
            }
            else if (this.geoOperationMode == 1)
            {
                OnMoving_MouseMove(e);
            }
            else if(this.geoOperationMode == 2)
            {
                OnSketching_MouseMove(e);
            }
            else if (this.geoOperationMode == 3)
            {
                OnEditing_MouseMove(e);
            }
            else if (this.geoOperationMode == 4)
            {
                OnPaning_MouseMove(e);
            }
        }

        /// <summary>
        /// 选择状态下鼠标移动
        /// </summary>
        /// <param name="e"></param>
        private void OnSelecting_MouseMove(MouseEventArgs e)
        {
            if (!isInSelecting) return;
            // 将起点和终点转为地图坐标
            moPoint startPoint = this.myMapControl.ToMapPoint(selectBoxStart.X, selectBoxStart.Y);
            moPoint endPoint = this.myMapControl.ToMapPoint(e.Location.X, e.Location.Y);

            // 求解minX, minY, maxX, maxY
            double minX, minY, maxX, maxY;
            if (startPoint.X > endPoint.X)
            {
                minX = endPoint.X;
                maxX = startPoint.X;
            }
            else
            {
                minX = startPoint.X;
                maxX = endPoint.X;
            }

            if (startPoint.Y > endPoint.Y)
            {
                minY = endPoint.Y;
                maxY = startPoint.Y;
            }
            else
            {
                minY = startPoint.Y;
                maxY = endPoint.Y;
            }

            // 生成搜索矩形
            moRectangle selectingBox = new moRectangle(minX, maxX, minY, maxY);

            // 生成绘图工具
            moUserDrawingTool drawingTool = myMapControl.GetDrawingTool();

            // 生成绘图样式
            moSimpleFillSymbol mSelectingBoxSymbol = new moSimpleFillSymbol();
            mSelectingBoxSymbol.Color = Color.Transparent;
            mSelectingBoxSymbol.Outline.Color = Color.Black;
            mSelectingBoxSymbol.Outline.Size = 0.3;
            myMapControl.Refresh();
            drawingTool.DrawRectangle(selectingBox, mSelectingBoxSymbol);

            
        }

        /// <summary>
        /// 漫游状态下鼠标移动
        /// </summary>
        /// <param name="e"></param>
        private void OnPaning_MouseMove(MouseEventArgs e)
        {
            if (!isInPaning) return;
            myMapControl.PanMapImageTo(e.Location.X - panMousePositionX,e.Location.Y - panMousePositionY);
        }

        /// <summary>
        /// 编辑节点状态下鼠标移动 
        /// </summary>
        /// <param name="e"></param>
        private void OnEditing_MouseMove(MouseEventArgs e)
        {
            if (this.isDragging == true)
            {
                double sDeltaX = myMapControl.ToMapDistance(e.Location.X - editingStart.X);
                double sDeltaY = -myMapControl.ToMapDistance(e.Location.Y - editingStart.Y);

                this.draggingPoint.X = this.draggingPoint.X + sDeltaX;
                this.draggingPoint.Y = this.draggingPoint.Y + sDeltaY;

                myMapControl.Refresh();
                DrawEditingShapes(myMapControl.GetDrawingTool());

                editingStart = e.Location;
            }
        }

        /// <summary>
        /// 描绘状态下鼠标移动
        /// </summary>
        /// <param name="e"></param>
        private void OnSketching_MouseMove(MouseEventArgs e)
        {
            if (editLayer.ShapeType == moGeometryTypeConstant.Point) return;
            MyMapObjects.moPoint sCurPoint = myMapControl.ToMapPoint(e.Location.X, e.Location.Y);

            MyMapObjects.moPoints sLastPart = sketchingShape.Last();
            Int32 sPointCount = sLastPart.Count;
            if (sPointCount == 0)
            {
                // 无顶点，不进行操作
            }
            else if (sPointCount == 1)
            {
                myMapControl.Refresh();
                // 绘制1条橡皮筋
                MyMapObjects.moPoint sFirstPoint = sLastPart.GetItem(0);
                MyMapObjects.moUserDrawingTool sDrawingTool = myMapControl.GetDrawingTool();
                sDrawingTool.DrawLine(sFirstPoint, sCurPoint, elasticSymbol);
            }
            else
            {
                myMapControl.Refresh();
                //两个以上顶点，多边形绘制两条，复合线绘制一条
                if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    MyMapObjects.moPoint sFirstPoint = sLastPart.GetItem(0);
                    MyMapObjects.moPoint sLastPoitn = sLastPart.GetItem(sPointCount - 1);
                    MyMapObjects.moUserDrawingTool sDrawingTool = myMapControl.GetDrawingTool();
                    sDrawingTool.DrawLine(sFirstPoint, sCurPoint, elasticSymbol);
                    sDrawingTool.DrawLine(sLastPoitn, sCurPoint, elasticSymbol);
                }
                else if(editLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    MyMapObjects.moPoint sLastPoitn = sLastPart.GetItem(sPointCount - 1);
                    MyMapObjects.moUserDrawingTool sDrawingTool = myMapControl.GetDrawingTool();
                    sDrawingTool.DrawLine(sLastPoitn, sCurPoint, elasticSymbol);
                }
            }
        }

        /// <summary>
        /// 移动状态下鼠标移动
        /// </summary>
        /// <param name="e"></param>
        private void OnMoving_MouseMove(MouseEventArgs e)
        {
            if (isInMovingShapes == false) return;

            // 修改移动图形的坐标
            double sDeltaX = myMapControl.ToMapDistance(e.Location.X - movingPoint.X);
            double sDeltaY = -myMapControl.ToMapDistance(e.Location.Y - movingPoint.Y);
            ModifyMovingGeometries(sDeltaX, sDeltaY);

            // 刷新地图并绘制移动图形
            myMapControl.Refresh();
            DrawMovingShapes();

            // 重新设置鼠标位置
            movingPoint = e.Location;
        }

        /// <summary>
        /// 在myMapControl控件上鼠标点击后触发的事件
        /// </summary>
        private void myMapControl_MouseClick(object sender, MouseEventArgs e)
        {
            if (geoOperationMode == 0)
            {

            }
            else if(geoOperationMode == 1)
            {

            }
            else if(geoOperationMode == 2)
            {
                OnSketching_MouseClick(e);
            }
            else if (geoOperationMode == 3)
            {
                OnEditing_MouseClick(e);
            }
            else if (geoOperationMode == 4)
            {

            }
        }


        /// <summary>
        /// 编辑状态下鼠标单击
        /// </summary>
        /// <param name="e"></param>
        private void OnEditing_MouseClick(MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                // 判断是否点中结点
                bool sSelectOnePoint = false;
                MyMapObjects.moPoint sOnSelectPoint = null;
                MyMapObjects.moPoint sCurPoint = myMapControl.ToMapPoint(e.X, e.Y);
                this.editingStart = e.Location;
                
                double sTolerance = myMapControl.ToMapDistance(this.editingVertexSymbol.Size) * 2;
                if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                {
                    for (int i = 0; i < ((moMultiPolyline)this.editingGeometry).Parts.Count; ++i)
                    {
                        moPoints sPoints = ((moMultiPolyline)this.editingGeometry).Parts.GetItem(i);
                        if (sPoints.Count == 2) continue;
                        for (int j = 0; j < sPoints.Count; ++j)
                        {
                            if (moMapTools.IsPointOnPoint(sCurPoint, sPoints.GetItem(j), sTolerance))
                            {
                                sOnSelectPoint = sPoints.GetItem(j);
                                sSelectOnePoint = true;
                                this.partIndex = i;
                                this.pointIndex = j;
                                break;
                            }
                        }
                        if (sSelectOnePoint) break;
                    }
                }
                else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                {
                    for (int i = 0; i < ((moMultiPolygon)this.editingGeometry).Parts.Count; ++i)
                    {
                        moPoints sPoints = ((moMultiPolygon)this.editingGeometry).Parts.GetItem(i);
                        if (sPoints.Count == 3) continue;
                        for (int j = 0; j < sPoints.Count; ++j)
                        {
                            if (moMapTools.IsPointOnPoint(sCurPoint, sPoints.GetItem(j), sTolerance))
                            {
                                sOnSelectPoint = sPoints.GetItem(j);
                                sSelectOnePoint = true;
                                this.partIndex = i;
                                this.pointIndex = j;
                                break;
                            }
                        }
                        if (sSelectOnePoint) break;
                    }
                }



                // 判断是否点到了边上
                bool sClickOnEdge = false;
                if (sSelectOnePoint == false)
                {
                    if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
                    {
                        for (int i = 0; i < ((moMultiPolyline)this.editingGeometry).Parts.Count; ++i)
                        {
                            moPoints sPoints = ((moMultiPolyline)this.editingGeometry).Parts.GetItem(i);
                            for (int j = 0; j < sPoints.Count-1; ++j)
                            {
                                MyMapObjects.moPoint[] sPointsArray = { sPoints.GetItem(j), sPoints.GetItem(j + 1) };
                                MyMapObjects.moPoints sPointsObj = new moPoints(sPointsArray);
                                sPointsObj.UpdateExtent();
                                if (moMapTools.IsPointOnPolyline(sCurPoint, sPointsObj, sTolerance))
                                {
                                    sClickOnEdge = true;
                                    this.partIndex = i;
                                    this.pointIndex = j;
                                    addVertexClickPosition = e.Location;
                                    break;
                                }
                            }
                            if (sClickOnEdge) break;
                        }
                    }
                    else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
                    {
                        for (int i = 0; i < ((moMultiPolygon)this.editingGeometry).Parts.Count; ++i)
                        {
                            moPoints sPoints = ((moMultiPolygon)this.editingGeometry).Parts.GetItem(i);
                            for (int j = 0; j < sPoints.Count-1; ++j)
                            {
                                MyMapObjects.moPoint[] sPointsArray = { sPoints.GetItem(j), sPoints.GetItem(j + 1) };
                                MyMapObjects.moPoints sPointsObj = new moPoints(sPointsArray);
                                sPointsObj.UpdateExtent();
                                if (moMapTools.IsPointOnPolyline(sCurPoint, sPointsObj, sTolerance))
                                {
                                    sClickOnEdge = true;
                                    this.partIndex = i;
                                    this.pointIndex = j;
                                    addVertexClickPosition = e.Location;
                                    break;
                                }
                            }
                            MyMapObjects.moPoint[] sPointsArray_ = { sPoints.GetItem(sPoints.Count - 1), sPoints.GetItem(0) };
                            MyMapObjects.moPoints sPointsObj_ = new moPoints(sPointsArray_);
                            sPointsObj_.UpdateExtent();
                            if (moMapTools.IsPointOnPolyline(sCurPoint, sPointsObj_, sTolerance))
                            {
                                sClickOnEdge = true;
                                this.partIndex = i;
                                this.pointIndex = sPoints.Count - 1;
                                addVertexClickPosition = e.Location;
                                break;
                            }
                            if (sClickOnEdge) break;
                        }
                    }
                }


                MapContextMenu.Items[0].Enabled = false;
                MapContextMenu.Items[1].Enabled = false;
                MapContextMenu.Items[2].Enabled = true;
                MapContextMenu.Items[3].Enabled = false;
                MapContextMenu.Items[4].Enabled = false;
                MapContextMenu.Items[5].Enabled = false;
                if (sSelectOnePoint)
                {
                    MapContextMenu.Items[4].Enabled = true;
                }
                else if (sClickOnEdge)
                {
                    MapContextMenu.Items[5].Enabled = true;
                }
                MapContextMenu.Show(MousePosition);
            }

        }

        /// <summary>
        /// 描绘状态下鼠标点击
        /// </summary>
        /// <param name="e"></param>
        private void OnSketching_MouseClick(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            // 将屏幕坐标转化为地图坐标
            MyMapObjects.moPoint sPoint = myMapControl.ToMapPoint(e.Location.X, e.Location.Y);
            if (editLayer.ShapeType == moGeometryTypeConstant.Point)
            {
                AddPointToEditLayer(sPoint);
            }
            else
            {
                sketchingShape.Last().Add(sPoint);
            }
            // 地图控件重绘跟踪图层
            myMapControl.RedrawTrackingShapes();
        }

        /// <summary>
        /// 鼠标进入myMapControl控件后触发的事件
        /// </summary>
        private void myMapControl_MouseEnter(object sender, EventArgs e)
        {
            // 根据状态变量修改指针样式
            if (geoOperationMode == 0)
            {
                this.Cursor = Cursors.Arrow;
            }
            else if (geoOperationMode == 1)
            {
                this.Cursor = Cursors.SizeAll;
            }
            else if (geoOperationMode == 2)
            {
                this.Cursor = Cursors.Cross;
            }
            else if(geoOperationMode == 3)
            {
                this.Cursor = Cursors.Arrow;
            }
            else if (geoOperationMode == 4)
            {
                this.Cursor = Cursors.Hand;
            }
        }

        /// <summary>
        /// 鼠标移出myMapControl控件后触发的事件
        /// </summary>
        private void myMapControl_MouseLeave(object sender, EventArgs e)
        {
            // 将鼠标指针样式修改为默认
            this.Cursor = Cursors.Default;
        }

        /// <summary>
        /// 点击查看属性表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 查看属性表_Click(object sender, EventArgs e)
        {
            if (selectedAttri.IsDisposed)
            {
                selectedAttri = new SelectedAttri();
                selectedAttri.UpdateContent();
            }
            selectedAttri.Visible = false;
            selectedAttri.Show(this);
        }

        /// <summary>
        /// 图形编辑器下的移动要素按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 移动要素_Click(object sender, EventArgs e)
        {
            if (geoOperationMode == 1)
            {
                geoOperationMode = 0;
                移动要素.Checked = false;
                箭头.Checked = true;
                SetCurrentStatus();
                return;
            }
            if (!editLayer.Visible)
            {
                editLayer.Visible = true;
                int index = myMapControl.Layers.IndexOf(editLayer);
                if (index != -1) this.checkedListBox1.SetItemChecked(index, true);
            }
            this.geoOperationMode = 1;
            SetButtonsDefault();
            InitializeEditingShape();
            移动要素.Checked = true;
            SetCurrentStatus();
        }

        /// <summary>
        /// 创建要素时结束部分按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 结束部分ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
            {
                // 判断是否可以结束，即是否最少三个点
                if (sketchingShape.Last().Count < 3) return;

                // 往描绘图形中增加一个多点对象
                MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
                sketchingShape.Add(sPoints);
                myMapControl.RedrawTrackingShapes();
            }
            else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
            {
                // 判断是否可以结束，即是否最少三个点
                if (sketchingShape.Last().Count < 2) return;

                // 往描绘图形中增加一个多点对象
                MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
                sketchingShape.Add(sPoints);
                myMapControl.RedrawTrackingShapes();
            }
        }

        /// <summary>
        /// 创建要素时结束描绘按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 结束描绘ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
            {
                if (sketchingShape.Last().Count >= 1 && sketchingShape.Last().Count < 3) return;

                // 如果最后一个部件的点数为0，则删除最后一个部件
                if (sketchingShape.Last().Count == 0)
                {
                    sketchingShape.Remove(sketchingShape.Last());
                }

                // 如果用户确实输入了，则加入多边形图层中
                if (sketchingShape.Count > 0)
                {
                    // 查找多边形图层
                    MyMapObjects.moMapLayer sLayer = editLayer;
                    if (sLayer != null)
                    {
                        // 定义复合多边形
                        MyMapObjects.moMultiPolygon sMultiPolygon = new MyMapObjects.moMultiPolygon();
                        sMultiPolygon.Parts.AddRange(sketchingShape.ToArray());
                        sMultiPolygon.UpdateExtent();

                        // 生成要素并加入图层
                        MyMapObjects.moFeature sFeature = sLayer.GetNewFeature();
                        sFeature.Geometry = sMultiPolygon;
                        sLayer.Features.Add(sFeature);
                    }
                }
            }

            else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
            {
                if (sketchingShape.Last().Count >= 1 && sketchingShape.Last().Count < 2) return;

                // 如果最后一个部件的点数为0，则删除最后一个部件
                if (sketchingShape.Last().Count == 0)
                {
                    sketchingShape.Remove(sketchingShape.Last());
                }

                // 如果用户确实输入了，则加入多边形图层中
                if (sketchingShape.Count > 0)
                {
                    // 查找多边形图层
                    MyMapObjects.moMapLayer sLayer = editLayer;
                    if (sLayer != null)
                    {
                        // 定义复合多边形
                        MyMapObjects.moMultiPolyline sMultiPolygon = new MyMapObjects.moMultiPolyline();
                        sMultiPolygon.Parts.AddRange(sketchingShape.ToArray());
                        sMultiPolygon.UpdateExtent();

                        // 生成要素并加入图层
                        MyMapObjects.moFeature sFeature = sLayer.GetNewFeature();
                        sFeature.Geometry = sMultiPolygon;
                        sLayer.Features.Add(sFeature);
                    }
                }
            }
            // 初始化描绘图形
            InitializeSketchingShape();
            myMapControl.RedrawMap();
        }

        /// <summary>
        /// 保存编辑节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 保存节点编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (editLayer.ShapeType == moGeometryTypeConstant.Point)
            {
                editLayer.SelectedFeatures.GetItem(0).Geometry = (moPoint)this.editingGeometry;
            }
            else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
            {
                editLayer.SelectedFeatures.GetItem(0).Geometry = (moMultiPolyline)this.editingGeometry;
            }
            else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
            {
                editLayer.SelectedFeatures.GetItem(0).Geometry = (moMultiPolygon)this.editingGeometry;
            }

            myMapControl.RedrawMap();
        }

        /// <summary>
        /// 删除要素
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 删除选中要素ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            moFeatures sSelectedFeatures = this.editLayer.SelectedFeatures;
            for (int i = 0; i < sSelectedFeatures.Count; ++i)
            {
                Int32 index = editLayer.Features.FindIndex(sSelectedFeatures.GetItem(i));
                if (index != -1)
                {
                    editLayer.Features.RemoveAt(index);
                }
            }
            editLayer.ClearSelection();
            myMapControl.RedrawMap();
            if (attrTBL != null && !attrTBL.IsDisposed) attrTBL.RefreshLayerTable(editLayer);
        }

        #endregion

        #region 退出系统
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region 关于
        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }
        #endregion

        #endregion

        #region 私有函数

        /// <summary>
        /// 初始化编辑图形 
        /// </summary>
        private void InitializeEditingShape()
        {
            editingGeometry = null;
            draggingPoint = null;
            isDragging = false;
            RedrawTrackingShape();
        }

        /// <summary>
        /// 根据图层生成其属性表
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        private DataTable GenerateDataTable(MyMapObjects.moMapLayer layer)
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

        /// <summary>
        /// 新增点要素加入正在编辑的图层中
        /// </summary>
        /// <param name="e"></param>
        private void AddPointToEditLayer(MyMapObjects.moPoint point)
        {
            MyMapObjects.moFeature feature = editLayer.GetNewFeature();
            feature.Geometry = point;
            editLayer.Features.Add(feature);
            myMapControl.RedrawMap();
        }

        /// <summary>
        /// 修改移动图形的坐标
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        private void ModifyMovingGeometries(double deltaX, double deltaY)
        {
            Int32 sCount = movingGeometries.Count;
            for (Int32 i = 0; i <= sCount - 1; i++)
            {
                if (movingGeometries[i].GetType() == typeof(MyMapObjects.moMultiPolygon))
                {
                    MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)movingGeometries[i];
                    Int32 sPartCount = sMultiPolygon.Parts.Count;
                    for (Int32 j = 0; j <= sPartCount - 1; j++)
                    {
                        MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(j);
                        Int32 sPointCount = sPoints.Count;
                        for (Int32 k = 0; k <= sPointCount - 1; k++)
                        {
                            MyMapObjects.moPoint sPoint = sPoints.GetItem(k);
                            sPoint.X = sPoint.X + deltaX;
                            sPoint.Y = sPoint.Y + deltaY;
                        }
                    }
                    sMultiPolygon.UpdateExtent();
                }

                else if (movingGeometries[i].GetType() == typeof(MyMapObjects.moPoint))
                {
                    MyMapObjects.moPoint sPoint = (MyMapObjects.moPoint)movingGeometries[i];
                    sPoint.X = sPoint.X + deltaX;
                    sPoint.Y = sPoint.Y + deltaY;
                }

                else if (movingGeometries[i].GetType() == typeof(MyMapObjects.moMultiPolyline))
                {
                    MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)movingGeometries[i];
                    Int32 sPartCount = sMultiPolyline.Parts.Count;
                    for (Int32 j = 0; j <= sPartCount - 1; j++)
                    {
                        MyMapObjects.moPoints sPoints = sMultiPolyline.Parts.GetItem(j);
                        Int32 sPointCount = sPoints.Count;
                        for (Int32 k = 0; k <= sPointCount - 1; k++)
                        {
                            MyMapObjects.moPoint sPoint = sPoints.GetItem(k);
                            sPoint.X = sPoint.X + deltaX;
                            sPoint.Y = sPoint.Y + deltaY;
                        }
                    }
                    sMultiPolyline.UpdateExtent();
                }
            }
        }

        /// <summary>
        /// 绘制移动图形
        /// </summary>
        private void DrawMovingShapes()
        {
            MyMapObjects.moUserDrawingTool sDrawingTool = myMapControl.GetDrawingTool();
            Int32 sCount = movingGeometries.Count;
            for (Int32 i = 0; i <= sCount - 1; i++)
            {
                if (movingGeometries[i].GetType() == typeof(MyMapObjects.moMultiPolygon))
                {
                    MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)movingGeometries[i];
                    sDrawingTool.DrawMultiPolygon(sMultiPolygon, movingPolygonSymbol);
                }
                else if (movingGeometries[i].GetType() == typeof(MyMapObjects.moPoint))
                {
                    MyMapObjects.moPoint sPoint = (MyMapObjects.moPoint)movingGeometries[i];
                    sDrawingTool.DrawPoint(sPoint, movingPointSymbol);
                }
                else if (movingGeometries[i].GetType() == typeof(MyMapObjects.moMultiPolyline))
                {
                    MyMapObjects.moMultiPolyline sPolyline = (MyMapObjects.moMultiPolyline)movingGeometries[i];
                    sDrawingTool.DrawMultiPolyline(sPolyline, movingPolylineSymbol);
                }
            }
        }

        /// <summary>
        /// 初始化正在描绘的图形
        /// </summary>
        private void InitializeSketchingShape()
        {
            sketchingShape = new List<MyMapObjects.moPoints>();
            MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
            sketchingShape.Add(sPoints);
        }

        /// <summary>
        /// 追踪图层完毕后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="drawingTool"></param>
        private void myMapControl_AfterTrackingLayerDraw(object sender, moUserDrawingTool drawingTool)
        {
            DrawSketchingShapes(drawingTool);   // 绘制描绘图形，只有在这里绘制才能保持持久性
            DrawEditingShapes(drawingTool);     // 绘制编辑图形
        }

        /// <summary>
        /// 绘制正在描绘的图形
        /// </summary>
        /// <param name="drawingTool"></param>
        private void DrawSketchingShapes(MyMapObjects.moUserDrawingTool drawingTool)
        {
            if (sketchingShape == null || editLayer == null)
                return;
            Int32 sPartCount = sketchingShape.Count;
            //绘制已经描绘完成的部分
            if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
            {
                for (Int32 i = 0; i <= sPartCount - 2; i++)
                {
                    drawingTool.DrawPolygon(sketchingShape[i], editingPolygonSymbol);
                }
            }
            else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
            {
                for (Int32 i = 0; i <= sPartCount - 2; i++)
                {
                    drawingTool.DrawPolyline(sketchingShape[i], editingLineSymbol);
                }
            }
            //正在描绘的部分（只有一个Part）
            MyMapObjects.moPoints sLastPart = sketchingShape.Last();
            if (sLastPart.Count >= 2)
                drawingTool.DrawPolyline(sLastPart, editingPolygonSymbol.Outline);
            //绘制所有顶点手柄
            for (Int32 i = 0; i <= sPartCount - 1; i++)
            {
                MyMapObjects.moPoints sPoints = sketchingShape[i];
                drawingTool.DrawPoints(sPoints, editingVertexSymbol);
            }
        }

        /// <summary>
        /// 绘制正在编辑的图形
        /// </summary>
        /// <param name="drawingTool"></param>
        private void DrawEditingShapes(MyMapObjects.moUserDrawingTool drawingTool)
        {
            if (editingGeometry == null)
                return;
            if (editingGeometry.GetType() == typeof(MyMapObjects.moMultiPolygon))
            {
                MyMapObjects.moMultiPolygon sMultiPolygon = (MyMapObjects.moMultiPolygon)editingGeometry;
                //绘制边界
                drawingTool.DrawMultiPolygon(sMultiPolygon, editingPolygonSymbol);
                //绘制顶点手柄
                Int32 sPartCount = sMultiPolygon.Parts.Count;
                for (Int32 i = 0; i <= sPartCount - 1; i++)
                {
                    MyMapObjects.moPoints sPoints = sMultiPolygon.Parts.GetItem(i);
                    drawingTool.DrawPoints(sPoints, editingVertexSymbol);
                }
            }
            else if (editingGeometry.GetType() == typeof(MyMapObjects.moMultiPolyline))
            {
                MyMapObjects.moMultiPolyline sMultiPolyline = (MyMapObjects.moMultiPolyline)editingGeometry;
                //绘制边界
                drawingTool.DrawMultiPolyline(sMultiPolyline, editingLineSymbol);
                //绘制顶点手柄
                Int32 sPartCount = sMultiPolyline.Parts.Count;
                for (Int32 i = 0; i <= sPartCount - 1; i++)
                {
                    MyMapObjects.moPoints sPoints = sMultiPolyline.Parts.GetItem(i);
                    drawingTool.DrawPoints(sPoints, editingVertexSymbol);
                }
            }

            else if (editingGeometry.GetType() == typeof(MyMapObjects.moPoint))
            {
                drawingTool.DrawPoint((moPoint)editingGeometry, editingVertexSymbol);
            }
        }

        /// <summary>
        /// 初始化图层编辑符号
        /// </summary>
        private void InitializeSymbols()
        {
            movingPolygonSymbol = new MyMapObjects.moSimpleFillSymbol();
            movingPolygonSymbol.Color = Color.Transparent;
            movingPolygonSymbol.Outline.Color = Color.Black;
            editingPolygonSymbol = new MyMapObjects.moSimpleFillSymbol();
            editingPolygonSymbol.Color = Color.Transparent;
            editingPolygonSymbol.Outline.Color = Color.DarkGreen;
            editingPolygonSymbol.Outline.Size = 0.53;
            editingVertexSymbol = new MyMapObjects.moSimpleMarkerSymbol();
            editingVertexSymbol.Color = Color.DarkGreen;
            editingVertexSymbol.Style = MyMapObjects.moSimpleMarkerSymbolStyleConstant.SolidSquare;
            editingVertexSymbol.Size = 2;
            elasticSymbol = new MyMapObjects.moSimpleLineSymbol();
            elasticSymbol.Color = Color.DarkGreen;
            elasticSymbol.Size = 0.52;
            elasticSymbol.Style = MyMapObjects.moSimpleLineSymbolStyleConstant.Dash;
        }

        #endregion

        private void 编辑要素_CheckedChanged(object sender, EventArgs e)
        {
            if (编辑要素.Checked == false)
            {
                if(geoOperationMode != 4)
                {
                    InitializeEditingShape();
                    myMapControl.RedrawTrackingShapes();
                }
            }
        }
        private void SetButtonsDefault()
        {
            箭头.Enabled = true;
            箭头.Checked = false;
            拖动.Checked = false;
            编辑要素.Checked = false;
            移动要素.Checked = false;
            创建要素.Checked = false;
            Identify.Checked = false;
        }

        private void myMapControl_MapScaleChanged(object sender)
        {
            ShowMapScale();
        }
        private void ShowMapScale()
        {
            tssMapScale.Text = "1:  " + myMapControl.MapScale.ToString("0.00");
        }

        private void ShowCoordinates(PointF point)
        {
            MyMapObjects.moPoint sPoint = myMapControl.ToMapPoint(point.X, point.Y);
            if (showLngLat == false)
            {
                double sX = Math.Round(sPoint.X, 2);
                double sY = Math.Round(sPoint.Y, 2);
                tssCoordinate.Text = "X:" + sX.ToString() + ",Y:" + sY.ToString();
            }
            else
            {
                MyMapObjects.moPoint sLngLat = myMapControl.ProjectionCS.TransferToLngLat(sPoint);
                double sX = Math.Round(sLngLat.X, 4);
                double sY = Math.Round(sLngLat.Y, 4);
                tssCoordinate.Text = "X:" + sX.ToString() + ",Y:" + sY.ToString();
            }
        }

        private void SetCurrentStatus()
        {
            if (this.geoOperationMode == 0) this.tssGeoOperationStatus.Text = "当前状态：" + "选择要素";
            else if(this.geoOperationMode == 1) this.tssGeoOperationStatus.Text = "当前状态：" + "移动要素";
            else if (this.geoOperationMode == 2) this.tssGeoOperationStatus.Text = "当前状态：" + "创建要素";
            else if (this.geoOperationMode == 3) this.tssGeoOperationStatus.Text = "当前状态：" + "编辑结点";
            else if (this.geoOperationMode == 4) this.tssGeoOperationStatus.Text = "当前状态：" + "漫游";
            else if (this.geoOperationMode == 5) this.tssGeoOperationStatus.Text = "当前状态：" + "识别要素";
            if (this.startEditing) this.tssGeoOperationStatus.Text += "（编辑中）";
        }

        private void tssSwtichShowCor_Click(object sender, EventArgs e)
        {
            if (this.showLngLat == false)
            {
                this.showLngLat = true;
                tssSwtichShowCor.Text = "显示投影坐标";
            }
            else if (this.showLngLat == true)
            {
                this.showLngLat = false;
                tssSwtichShowCor.Text = "显示地理坐标";
            }
        }

        private void 新建图层文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewLayerFile sNewLayerFileForm = new NewLayerFile();
            sNewLayerFileForm.createNewLayer += CreateNewLayerFile;
            sNewLayerFileForm.ShowDialog();
        }

        /// <summary>
        /// 创建新图层处理函数
        /// </summary>
        /// <param name="path"></param>
        /// <param name="type"></param>
        private void CreateNewLayerFile(string layerName, MyMapObjects.moGeometryTypeConstant type)
        {
            // 新建属性
            MyMapObjects.moFields attributeFields = new MyMapObjects.moFields();//字段集合
            MyMapObjects.moField moField = new MyMapObjects.moField("名称", MyMapObjects.moValueTypeConstant.dText);
            attributeFields.Append(moField);

            string path = "";

            // 新建lay对象
            MyMapObjects.moMapLayer newLayer = new MyMapObjects.moMapLayer(layerName, type, attributeFields, path);

            // 写入文件
            DataIOTools.WriteLayerToFile(newLayer, layerName);

            // 添加到Layers中
            myMapControl.Layers.Add(newLayer);

            // 重绘
            if (myMapControl.Layers.Count == 1)
            {
                myMapControl.FullExtent();
            }
            else
            {
                myMapControl.RedrawMap();
            }

            //修改checklistbox
            AddLayerInCheckList(System.IO.Path.GetFileNameWithoutExtension(path));

        }

        private void Identify_Click(object sender, EventArgs e)
        {
            geoOperationMode = 5;
            SetButtonsDefault();
            Identify.Checked = true;
            SetCurrentStatus();
        }

        private void 删除结点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(editLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                ((MyMapObjects.moMultiPolyline)editingGeometry).Parts.GetItem(this.partIndex).RemoveAt(this.pointIndex);
                ((MyMapObjects.moMultiPolyline)editingGeometry).UpdateExtent();
            }
            else if (editLayer.ShapeType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                ((MyMapObjects.moMultiPolygon)editingGeometry).Parts.GetItem(this.partIndex).RemoveAt(this.pointIndex);
                ((MyMapObjects.moMultiPolygon)editingGeometry).UpdateExtent();
            }
            myMapControl.Refresh();
            myMapControl.RedrawMap();
        }

        private void 添加结点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolyline)
            {
                moPoint sStart = ((MyMapObjects.moMultiPolyline)editingGeometry).Parts.GetItem(this.partIndex).GetItem(this.pointIndex);
                moPoint sEnd = ((MyMapObjects.moMultiPolyline)editingGeometry).Parts.GetItem(this.partIndex).GetItem(this.pointIndex+1);
                MyMapObjects.moPoint[] sPointsArray = { sStart, sEnd };
                MyMapObjects.moPoints sPointsObj = new moPoints(sPointsArray);
                sPointsObj.UpdateExtent();
                moPoint sClickPoint = this.myMapControl.ToMapPoint(addVertexClickPosition.X, addVertexClickPosition.Y);
                moPoint newVertex = moMapTools.GetPedalPointOnLine(sClickPoint, sPointsObj);
                ((MyMapObjects.moMultiPolyline)editingGeometry).Parts.GetItem(this.partIndex).Insert(pointIndex+1, newVertex);
                ((MyMapObjects.moMultiPolyline)editingGeometry).UpdateExtent();
            }
            else if (editLayer.ShapeType == moGeometryTypeConstant.MultiPolygon)
            {
                Int32 sPointsCount = ((MyMapObjects.moMultiPolygon)editingGeometry).Parts.GetItem(this.partIndex).Count;
                moPoint sStart = ((MyMapObjects.moMultiPolygon)editingGeometry).Parts.GetItem(this.partIndex).GetItem(this.pointIndex);
                moPoint sEnd = ((MyMapObjects.moMultiPolygon)editingGeometry).Parts.GetItem(this.partIndex).GetItem((this.pointIndex+1)%sPointsCount);
                MyMapObjects.moPoint[] sPointsArray = { sStart, sEnd };
                MyMapObjects.moPoints sPointsObj = new moPoints(sPointsArray);
                sPointsObj.UpdateExtent();
                moPoint sClickPoint = this.myMapControl.ToMapPoint(addVertexClickPosition.X, addVertexClickPosition.Y);
                moPoint newVertex = moMapTools.GetPedalPointOnLine(sClickPoint, sPointsObj);
                ((MyMapObjects.moMultiPolygon)editingGeometry).Parts.GetItem(this.partIndex).Insert(pointIndex+1, newVertex);
                ((MyMapObjects.moMultiPolygon)editingGeometry).UpdateExtent();
            }
            myMapControl.Refresh();
            myMapControl.RedrawMap();
        }

        private void 帮助ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string helpFile = "help.chm";
            Help.ShowHelp(this, helpFile);
        }

        private void 打开图层文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog sFileDialog = new OpenFileDialog();
            string sFileName = "";
            if (sFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                sFileName = sFileDialog.FileName;
                sFileDialog.Dispose();
            }
            else
            {
                sFileDialog.Dispose();
                return;
            }

            try
            {
                FileStream sFileStream = new FileStream(sFileName, FileMode.Open);
                BinaryReader sr = new BinaryReader(sFileStream);
                MyMapObjects.moMapLayer sLayer = DataIOTools.LoadMapLayer(sr, sFileName);
                myMapControl.Layers.Add(sLayer);
                if (myMapControl.Layers.Count == 1)
                {
                    myMapControl.FullExtent();
                }
                else
                {
                    myMapControl.RedrawMap();
                }

                sr.Dispose();
                sFileStream.Dispose();

                //修改checklistbox
                AddLayerInCheckList(System.IO.Path.GetFileNameWithoutExtension(sFileName));
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
                return;
            }

        }
    }
}
