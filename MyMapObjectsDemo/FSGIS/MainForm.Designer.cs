
namespace FSGIS
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.箭头 = new System.Windows.Forms.ToolStripButton();
            this.拖动 = new System.Windows.Forms.ToolStripButton();
            this.缩放到全图 = new System.Windows.Forms.ToolStripButton();
            this.查看属性表 = new System.Windows.Forms.ToolStripButton();
            this.Identify = new System.Windows.Forms.ToolStripButton();
            this.图形编辑器 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.创建要素 = new System.Windows.Forms.ToolStripButton();
            this.编辑要素 = new System.Windows.Forms.ToolStripButton();
            this.移动要素 = new System.Windows.Forms.ToolStripButton();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.打开属性表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.按属性选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移除图层ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.符号化ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.标注要素ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存为ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.上移一层 = new System.Windows.Forms.Button();
            this.下移一层 = new System.Windows.Forms.Button();
            this.置顶 = new System.Windows.Forms.Button();
            this.置底 = new System.Windows.Forms.Button();
            this.MapContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.结束部分ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.结束描绘ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存节点编辑ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除选中要素ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除结点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加结点ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.读取工程文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存工程文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.另存工程文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.新建图层文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加图层文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存图层文件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.打印地图ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.退出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssCoordinate = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssMapScale = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssGeoOperationStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.tssSwtichShowCor = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.myMapControl = new MyMapObjects.MyMapControl();
            this.从数据库添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.MapContextMenu.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.箭头,
            this.拖动,
            this.缩放到全图,
            this.查看属性表,
            this.Identify,
            this.图形编辑器,
            this.toolStripSeparator1,
            this.创建要素,
            this.编辑要素,
            this.移动要素});
            this.toolStrip.Location = new System.Drawing.Point(0, 30);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.toolStrip.Size = new System.Drawing.Size(1039, 31);
            this.toolStrip.TabIndex = 3;
            this.toolStrip.Text = "toolStrip1";
            // 
            // 箭头
            // 
            this.箭头.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.箭头.Image = ((System.Drawing.Image)(resources.GetObject("箭头.Image")));
            this.箭头.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.箭头.Name = "箭头";
            this.箭头.Size = new System.Drawing.Size(29, 28);
            this.箭头.Text = "箭头";
            this.箭头.Click += new System.EventHandler(this.箭头_Click);
            // 
            // 拖动
            // 
            this.拖动.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.拖动.Image = ((System.Drawing.Image)(resources.GetObject("拖动.Image")));
            this.拖动.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.拖动.Name = "拖动";
            this.拖动.Size = new System.Drawing.Size(29, 24);
            this.拖动.Text = "拖动";
            this.拖动.Click += new System.EventHandler(this.拖动_Click);
            // 
            // 缩放到全图
            // 
            this.缩放到全图.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.缩放到全图.Image = ((System.Drawing.Image)(resources.GetObject("缩放到全图.Image")));
            this.缩放到全图.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.缩放到全图.Name = "缩放到全图";
            this.缩放到全图.Size = new System.Drawing.Size(29, 24);
            this.缩放到全图.Text = "缩放到全图";
            this.缩放到全图.Click += new System.EventHandler(this.缩放到全图_Click);
            // 
            // 查看属性表
            // 
            this.查看属性表.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.查看属性表.Image = ((System.Drawing.Image)(resources.GetObject("查看属性表.Image")));
            this.查看属性表.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.查看属性表.Name = "查看属性表";
            this.查看属性表.Size = new System.Drawing.Size(29, 24);
            this.查看属性表.Text = "属性表";
            this.查看属性表.Click += new System.EventHandler(this.查看属性表_Click);
            // 
            // Identify
            // 
            this.Identify.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Identify.Image = ((System.Drawing.Image)(resources.GetObject("Identify.Image")));
            this.Identify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Identify.Name = "Identify";
            this.Identify.Size = new System.Drawing.Size(29, 24);
            this.Identify.Text = "识别";
            this.Identify.Click += new System.EventHandler(this.Identify_Click);
            // 
            // 图形编辑器
            // 
            this.图形编辑器.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.图形编辑器.Image = ((System.Drawing.Image)(resources.GetObject("图形编辑器.Image")));
            this.图形编辑器.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.图形编辑器.Name = "图形编辑器";
            this.图形编辑器.Size = new System.Drawing.Size(29, 24);
            this.图形编辑器.Text = "编辑";
            this.图形编辑器.Click += new System.EventHandler(this.图形编辑器_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // 创建要素
            // 
            this.创建要素.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.创建要素.Enabled = false;
            this.创建要素.Image = ((System.Drawing.Image)(resources.GetObject("创建要素.Image")));
            this.创建要素.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.创建要素.Name = "创建要素";
            this.创建要素.Size = new System.Drawing.Size(29, 24);
            this.创建要素.Text = "创建要素";
            this.创建要素.Click += new System.EventHandler(this.创建要素_Click);
            // 
            // 编辑要素
            // 
            this.编辑要素.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.编辑要素.Enabled = false;
            this.编辑要素.Image = ((System.Drawing.Image)(resources.GetObject("编辑要素.Image")));
            this.编辑要素.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.编辑要素.Name = "编辑要素";
            this.编辑要素.Size = new System.Drawing.Size(29, 24);
            this.编辑要素.Text = "编辑要素";
            this.编辑要素.CheckedChanged += new System.EventHandler(this.编辑要素_CheckedChanged);
            this.编辑要素.Click += new System.EventHandler(this.编辑要素_Click);
            // 
            // 移动要素
            // 
            this.移动要素.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.移动要素.Enabled = false;
            this.移动要素.Image = ((System.Drawing.Image)(resources.GetObject("移动要素.Image")));
            this.移动要素.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.移动要素.Name = "移动要素";
            this.移动要素.Size = new System.Drawing.Size(29, 24);
            this.移动要素.Text = "移动要素";
            this.移动要素.Click += new System.EventHandler(this.移动要素_Click);
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(0, 40);
            this.checkedListBox1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(184, 517);
            this.checkedListBox1.TabIndex = 4;
            this.checkedListBox1.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBox1_ItemCheck);
            this.checkedListBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkedListBox1_MouseDown);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.打开属性表ToolStripMenuItem,
            this.按属性选择ToolStripMenuItem,
            this.移除图层ToolStripMenuItem,
            this.符号化ToolStripMenuItem,
            this.标注要素ToolStripMenuItem,
            this.另存为ToolStripMenuItem});
            this.contextMenuStrip.Name = "contextMenuStrip1";
            this.contextMenuStrip.Size = new System.Drawing.Size(154, 148);
            // 
            // 打开属性表ToolStripMenuItem
            // 
            this.打开属性表ToolStripMenuItem.Name = "打开属性表ToolStripMenuItem";
            this.打开属性表ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.打开属性表ToolStripMenuItem.Text = "打开属性表";
            this.打开属性表ToolStripMenuItem.Click += new System.EventHandler(this.打开属性表ToolStripMenuItem_Click);
            // 
            // 按属性选择ToolStripMenuItem
            // 
            this.按属性选择ToolStripMenuItem.Name = "按属性选择ToolStripMenuItem";
            this.按属性选择ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.按属性选择ToolStripMenuItem.Text = "按属性选择";
            this.按属性选择ToolStripMenuItem.Click += new System.EventHandler(this.按属性选择ToolStripMenuItem_Click);
            // 
            // 移除图层ToolStripMenuItem
            // 
            this.移除图层ToolStripMenuItem.Name = "移除图层ToolStripMenuItem";
            this.移除图层ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.移除图层ToolStripMenuItem.Text = "移除图层";
            this.移除图层ToolStripMenuItem.Click += new System.EventHandler(this.移除图层ToolStripMenuItem_Click);
            // 
            // 符号化ToolStripMenuItem
            // 
            this.符号化ToolStripMenuItem.Name = "符号化ToolStripMenuItem";
            this.符号化ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.符号化ToolStripMenuItem.Text = "符号化";
            this.符号化ToolStripMenuItem.Click += new System.EventHandler(this.符号化ToolStripMenuItem_Click);
            // 
            // 标注要素ToolStripMenuItem
            // 
            this.标注要素ToolStripMenuItem.Name = "标注要素ToolStripMenuItem";
            this.标注要素ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.标注要素ToolStripMenuItem.Text = "标注要素";
            this.标注要素ToolStripMenuItem.Click += new System.EventHandler(this.标注要素ToolStripMenuItem_Click);
            // 
            // 另存为ToolStripMenuItem
            // 
            this.另存为ToolStripMenuItem.Name = "另存为ToolStripMenuItem";
            this.另存为ToolStripMenuItem.Size = new System.Drawing.Size(153, 24);
            this.另存为ToolStripMenuItem.Text = "另存为";
            this.另存为ToolStripMenuItem.Click += new System.EventHandler(this.另存为ToolStripMenuItem_Click);
            // 
            // 上移一层
            // 
            this.上移一层.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("上移一层.BackgroundImage")));
            this.上移一层.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.上移一层.Location = new System.Drawing.Point(3, 2);
            this.上移一层.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.上移一层.Name = "上移一层";
            this.上移一层.Size = new System.Drawing.Size(29, 30);
            this.上移一层.TabIndex = 5;
            this.上移一层.UseVisualStyleBackColor = true;
            this.上移一层.Click += new System.EventHandler(this.上移一层_Click);
            // 
            // 下移一层
            // 
            this.下移一层.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("下移一层.BackgroundImage")));
            this.下移一层.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.下移一层.Location = new System.Drawing.Point(55, 2);
            this.下移一层.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.下移一层.Name = "下移一层";
            this.下移一层.Size = new System.Drawing.Size(29, 30);
            this.下移一层.TabIndex = 6;
            this.下移一层.UseVisualStyleBackColor = true;
            this.下移一层.Click += new System.EventHandler(this.下移一层_Click);
            // 
            // 置顶
            // 
            this.置顶.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("置顶.BackgroundImage")));
            this.置顶.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.置顶.Location = new System.Drawing.Point(155, 2);
            this.置顶.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.置顶.Name = "置顶";
            this.置顶.Size = new System.Drawing.Size(29, 30);
            this.置顶.TabIndex = 7;
            this.置顶.UseVisualStyleBackColor = true;
            this.置顶.Click += new System.EventHandler(this.置顶_Click);
            // 
            // 置底
            // 
            this.置底.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("置底.BackgroundImage")));
            this.置底.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.置底.Location = new System.Drawing.Point(105, 2);
            this.置底.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.置底.Name = "置底";
            this.置底.Size = new System.Drawing.Size(29, 30);
            this.置底.TabIndex = 8;
            this.置底.UseVisualStyleBackColor = true;
            this.置底.Click += new System.EventHandler(this.置底_Click);
            // 
            // MapContextMenu
            // 
            this.MapContextMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MapContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.结束部分ToolStripMenuItem,
            this.结束描绘ToolStripMenuItem,
            this.保存节点编辑ToolStripMenuItem,
            this.删除选中要素ToolStripMenuItem,
            this.删除结点ToolStripMenuItem,
            this.添加结点ToolStripMenuItem});
            this.MapContextMenu.Name = "MapContextMenu";
            this.MapContextMenu.Size = new System.Drawing.Size(169, 148);
            // 
            // 结束部分ToolStripMenuItem
            // 
            this.结束部分ToolStripMenuItem.Name = "结束部分ToolStripMenuItem";
            this.结束部分ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.结束部分ToolStripMenuItem.Text = "结束部分";
            this.结束部分ToolStripMenuItem.Click += new System.EventHandler(this.结束部分ToolStripMenuItem_Click);
            // 
            // 结束描绘ToolStripMenuItem
            // 
            this.结束描绘ToolStripMenuItem.Name = "结束描绘ToolStripMenuItem";
            this.结束描绘ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.结束描绘ToolStripMenuItem.Text = "结束描绘";
            this.结束描绘ToolStripMenuItem.Click += new System.EventHandler(this.结束描绘ToolStripMenuItem_Click);
            // 
            // 保存节点编辑ToolStripMenuItem
            // 
            this.保存节点编辑ToolStripMenuItem.Name = "保存节点编辑ToolStripMenuItem";
            this.保存节点编辑ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.保存节点编辑ToolStripMenuItem.Text = "保存节点编辑";
            this.保存节点编辑ToolStripMenuItem.Click += new System.EventHandler(this.保存节点编辑ToolStripMenuItem_Click);
            // 
            // 删除选中要素ToolStripMenuItem
            // 
            this.删除选中要素ToolStripMenuItem.Name = "删除选中要素ToolStripMenuItem";
            this.删除选中要素ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.删除选中要素ToolStripMenuItem.Text = "删除选中要素";
            this.删除选中要素ToolStripMenuItem.Click += new System.EventHandler(this.删除选中要素ToolStripMenuItem_Click);
            // 
            // 删除结点ToolStripMenuItem
            // 
            this.删除结点ToolStripMenuItem.Name = "删除结点ToolStripMenuItem";
            this.删除结点ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.删除结点ToolStripMenuItem.Text = "删除结点";
            this.删除结点ToolStripMenuItem.Click += new System.EventHandler(this.删除结点ToolStripMenuItem_Click);
            // 
            // 添加结点ToolStripMenuItem
            // 
            this.添加结点ToolStripMenuItem.Name = "添加结点ToolStripMenuItem";
            this.添加结点ToolStripMenuItem.Size = new System.Drawing.Size(168, 24);
            this.添加结点ToolStripMenuItem.Text = "添加结点";
            this.添加结点ToolStripMenuItem.Click += new System.EventHandler(this.添加结点ToolStripMenuItem_Click);
            // 
            // 文件ToolStripMenuItem
            // 
            this.文件ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.读取工程文件ToolStripMenuItem,
            this.保存工程文件ToolStripMenuItem,
            this.另存工程文件ToolStripMenuItem,
            this.toolStripSeparator2,
            this.新建图层文件ToolStripMenuItem,
            this.添加图层文件ToolStripMenuItem,
            this.保存图层文件ToolStripMenuItem,
            this.toolStripSeparator3,
            this.打印地图ToolStripMenuItem,
            this.toolStripSeparator4,
            this.退出ToolStripMenuItem,
            this.从数据库添加ToolStripMenuItem});
            this.文件ToolStripMenuItem.Name = "文件ToolStripMenuItem";
            this.文件ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.文件ToolStripMenuItem.Text = "文件";
            // 
            // 读取工程文件ToolStripMenuItem
            // 
            this.读取工程文件ToolStripMenuItem.Name = "读取工程文件ToolStripMenuItem";
            this.读取工程文件ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.读取工程文件ToolStripMenuItem.Text = "读取工程文件";
            this.读取工程文件ToolStripMenuItem.Click += new System.EventHandler(this.读取工程文件ToolStripMenuItem_Click);
            // 
            // 保存工程文件ToolStripMenuItem
            // 
            this.保存工程文件ToolStripMenuItem.Name = "保存工程文件ToolStripMenuItem";
            this.保存工程文件ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.保存工程文件ToolStripMenuItem.Text = "保存工程文件";
            this.保存工程文件ToolStripMenuItem.Click += new System.EventHandler(this.保存工程文件ToolStripMenuItem_Click);
            // 
            // 另存工程文件ToolStripMenuItem
            // 
            this.另存工程文件ToolStripMenuItem.Name = "另存工程文件ToolStripMenuItem";
            this.另存工程文件ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.另存工程文件ToolStripMenuItem.Text = "另存工程文件";
            this.另存工程文件ToolStripMenuItem.Click += new System.EventHandler(this.另存工程文件ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(221, 6);
            // 
            // 新建图层文件ToolStripMenuItem
            // 
            this.新建图层文件ToolStripMenuItem.Name = "新建图层文件ToolStripMenuItem";
            this.新建图层文件ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.新建图层文件ToolStripMenuItem.Text = "新建图层文件";
            this.新建图层文件ToolStripMenuItem.Click += new System.EventHandler(this.新建图层文件ToolStripMenuItem_Click);
            // 
            // 添加图层文件ToolStripMenuItem
            // 
            this.添加图层文件ToolStripMenuItem.Name = "添加图层文件ToolStripMenuItem";
            this.添加图层文件ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.添加图层文件ToolStripMenuItem.Text = "添加图层文件";
            this.添加图层文件ToolStripMenuItem.Click += new System.EventHandler(this.添加图层文件ToolStripMenuItem_Click);
            // 
            // 保存图层文件ToolStripMenuItem
            // 
            this.保存图层文件ToolStripMenuItem.Name = "保存图层文件ToolStripMenuItem";
            this.保存图层文件ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.保存图层文件ToolStripMenuItem.Text = "保存图层文件";
            this.保存图层文件ToolStripMenuItem.Click += new System.EventHandler(this.保存图层文件ToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(221, 6);
            // 
            // 打印地图ToolStripMenuItem
            // 
            this.打印地图ToolStripMenuItem.Name = "打印地图ToolStripMenuItem";
            this.打印地图ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.打印地图ToolStripMenuItem.Text = "打印地图";
            this.打印地图ToolStripMenuItem.Click += new System.EventHandler(this.打印地图ToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(221, 6);
            // 
            // 退出ToolStripMenuItem
            // 
            this.退出ToolStripMenuItem.Name = "退出ToolStripMenuItem";
            this.退出ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.退出ToolStripMenuItem.Text = "退出";
            this.退出ToolStripMenuItem.Click += new System.EventHandler(this.退出ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.帮助ToolStripMenuItem1,
            this.关于ToolStripMenuItem});
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(53, 24);
            this.帮助ToolStripMenuItem.Text = "帮助";
            // 
            // 帮助ToolStripMenuItem1
            // 
            this.帮助ToolStripMenuItem1.Name = "帮助ToolStripMenuItem1";
            this.帮助ToolStripMenuItem1.Size = new System.Drawing.Size(122, 26);
            this.帮助ToolStripMenuItem1.Text = "帮助";
            this.帮助ToolStripMenuItem1.Click += new System.EventHandler(this.帮助ToolStripMenuItem1_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(122, 26);
            this.关于ToolStripMenuItem.Text = "关于";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.文件ToolStripMenuItem,
            this.帮助ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1039, 30);
            this.menuStrip.TabIndex = 2;
            this.menuStrip.Text = "menuStrip1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssCoordinate,
            this.tssMapScale,
            this.tssGeoOperationStatus,
            this.tssSwtichShowCor});
            this.statusStrip1.Location = new System.Drawing.Point(0, 624);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1039, 30);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssCoordinate
            // 
            this.tssCoordinate.AutoSize = false;
            this.tssCoordinate.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssCoordinate.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.tssCoordinate.Name = "tssCoordinate";
            this.tssCoordinate.Size = new System.Drawing.Size(200, 24);
            this.tssCoordinate.Text = "#";
            // 
            // tssMapScale
            // 
            this.tssMapScale.AutoSize = false;
            this.tssMapScale.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssMapScale.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.tssMapScale.Name = "tssMapScale";
            this.tssMapScale.Size = new System.Drawing.Size(200, 24);
            this.tssMapScale.Text = "#";
            // 
            // tssGeoOperationStatus
            // 
            this.tssGeoOperationStatus.AutoSize = false;
            this.tssGeoOperationStatus.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssGeoOperationStatus.BorderStyle = System.Windows.Forms.Border3DStyle.Sunken;
            this.tssGeoOperationStatus.Name = "tssGeoOperationStatus";
            this.tssGeoOperationStatus.Size = new System.Drawing.Size(200, 24);
            this.tssGeoOperationStatus.Text = "当前状态:";
            this.tssGeoOperationStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tssSwtichShowCor
            // 
            this.tssSwtichShowCor.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.tssSwtichShowCor.BorderStyle = System.Windows.Forms.Border3DStyle.Raised;
            this.tssSwtichShowCor.Name = "tssSwtichShowCor";
            this.tssSwtichShowCor.Size = new System.Drawing.Size(103, 24);
            this.tssSwtichShowCor.Text = "显示地理坐标";
            this.tssSwtichShowCor.Click += new System.EventHandler(this.tssSwtichShowCor_Click);
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(0, 618);
            this.splitter1.Margin = new System.Windows.Forms.Padding(4);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(1039, 6);
            this.splitter1.TabIndex = 11;
            this.splitter1.TabStop = false;
            // 
            // splitter2
            // 
            this.splitter2.Location = new System.Drawing.Point(184, 61);
            this.splitter2.Margin = new System.Windows.Forms.Padding(4);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(4, 557);
            this.splitter2.TabIndex = 12;
            this.splitter2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.checkedListBox1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 61);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(184, 557);
            this.panel1.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.上移一层);
            this.panel2.Controls.Add(this.下移一层);
            this.panel2.Controls.Add(this.置顶);
            this.panel2.Controls.Add(this.置底);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(184, 40);
            this.panel2.TabIndex = 5;
            // 
            // myMapControl
            // 
            this.myMapControl.AutoScroll = true;
            this.myMapControl.BackColor = System.Drawing.Color.White;
            this.myMapControl.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.myMapControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.myMapControl.FlashColor = System.Drawing.Color.Green;
            this.myMapControl.Location = new System.Drawing.Point(188, 61);
            this.myMapControl.Margin = new System.Windows.Forms.Padding(5, 8, 5, 8);
            this.myMapControl.Name = "myMapControl";
            this.myMapControl.SelectionColor = System.Drawing.Color.Cyan;
            this.myMapControl.Size = new System.Drawing.Size(851, 557);
            this.myMapControl.TabIndex = 9;
            this.myMapControl.MapScaleChanged += new MyMapObjects.MyMapControl.MapScaleChangedHandle(this.myMapControl_MapScaleChanged);
            this.myMapControl.AfterTrackingLayerDraw += new MyMapObjects.MyMapControl.AfterTrackingLayerDrawHandle(this.myMapControl_AfterTrackingLayerDraw);
            this.myMapControl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.myMapControl_MouseClick);
            this.myMapControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.myMapControl_MouseDown);
            this.myMapControl.MouseEnter += new System.EventHandler(this.myMapControl_MouseEnter);
            this.myMapControl.MouseLeave += new System.EventHandler(this.myMapControl_MouseLeave);
            this.myMapControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.myMapControl_MouseMove);
            this.myMapControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.myMapControl_MouseUp);
            // 
            // 从数据库添加ToolStripMenuItem
            // 
            this.从数据库添加ToolStripMenuItem.Name = "从数据库添加ToolStripMenuItem";
            this.从数据库添加ToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.从数据库添加ToolStripMenuItem.Text = "从数据库添加";
            this.从数据库添加ToolStripMenuItem.Click += new System.EventHandler(this.从数据库添加ToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1039, 654);
            this.Controls.Add(this.myMapControl);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "飞昇地理信息系统";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.MapContextMenu.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton 箭头;
        private System.Windows.Forms.ToolStripButton 拖动;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.Button 上移一层;
        private System.Windows.Forms.Button 下移一层;
        private System.Windows.Forms.Button 置顶;
        private System.Windows.Forms.Button 置底;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem 打开属性表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移除图层ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 符号化ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton 图形编辑器;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton 创建要素;
        private System.Windows.Forms.ToolStripButton 编辑要素;
        private System.Windows.Forms.ToolStripButton 缩放到全图;
        internal MyMapObjects.MyMapControl myMapControl;
        private System.Windows.Forms.ToolStripMenuItem 标注要素ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton 查看属性表;
        private System.Windows.Forms.ToolStripMenuItem 另存为ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 按属性选择ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton 移动要素;
        private System.Windows.Forms.ContextMenuStrip MapContextMenu;
        private System.Windows.Forms.ToolStripMenuItem 结束部分ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 结束描绘ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存节点编辑ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 读取工程文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 另存工程文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加图层文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打印地图ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退出ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 删除选中要素ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 保存图层文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem 保存工程文件ToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripStatusLabel tssCoordinate;
        private System.Windows.Forms.ToolStripStatusLabel tssMapScale;
        private System.Windows.Forms.ToolStripStatusLabel tssGeoOperationStatus;
        private System.Windows.Forms.ToolStripStatusLabel tssSwtichShowCor;
        private System.Windows.Forms.ToolStripMenuItem 新建图层文件ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton Identify;
        private System.Windows.Forms.ToolStripMenuItem 删除结点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加结点ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 从数据库添加ToolStripMenuItem;
    }
}

