using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace FSGIS.SubSystems
{
    internal static class DataIOTools
    {
        #region 程序集方法

        /// <summary>
        /// 读取图层文件
        /// </summary>
        /// <param name="sr">二进制读取对象</param>
        /// <param name="path">图层文件所在路径</param>
        /// <returns></returns>
        internal static MyMapObjects.moMapLayer LoadMapLayer(BinaryReader sr, string path)
        {
            string name = sr.ReadString();
            MyMapObjects.moGeometryTypeConstant sGeometryType = (MyMapObjects.moGeometryTypeConstant)sr.ReadInt32();
            MyMapObjects.moFields sFields = LoadFields(sr);
            MyMapObjects.moFeatures sFeatures = LoadFeatures(sGeometryType, sFields, sr);
            MyMapObjects.moMapLayer sMapLayer = new MyMapObjects.moMapLayer(name, sGeometryType, sFields, path);
            sMapLayer.Features = sFeatures;
            return sMapLayer;
        }

        /// <summary>
        /// 将图层写入文件
        /// </summary>
        /// <param name="layer">待写入图层对象</param>
        /// <param name="layerName">图层文件路径</param>
        internal static void WriteLayerToFile(MyMapObjects.moMapLayer layer, string layerName)
        {
            // 根据文件路径创建文件
            FileStream fileStream = new FileStream(layerName, FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);

            // 写入数据
            try
            {
                // 写入图层名称
                string name = layer.Name;
                binaryWriter.Write(name);

                // 写入图层类型
                Int32 geometryType = (Int32)layer.ShapeType;
                binaryWriter.Write(geometryType);

                // 写入字段数量
                MyMapObjects.moFields fields = layer.AttributeFields;
                Int32 fieldsNum = fields.Count;
                binaryWriter.Write(fieldsNum);

                // 依次写入所有字段
                for(int i = 0; i < fieldsNum; ++i)
                {
                    MyMapObjects.moField attributeField = fields.GetItem(i);
                    string fieldName = attributeField.Name;
                    Int32 fieldType = (Int32)attributeField.ValueType;
                    Int32 place = 0;

                    binaryWriter.Write(fieldName);
                    binaryWriter.Write(fieldType);
                    binaryWriter.Write(place);
                }

                // 写入要素数量
                Int32 featuresNum = layer.Features.Count;
                binaryWriter.Write(featuresNum);

                // 依次写入所有要素
                for (int i = 0; i < featuresNum; ++i)
                {
                    if(geometryType == 0)// Point
                    {
                        // 该Point对象
                        MyMapObjects.moPoint point = (MyMapObjects.moPoint)layer.Features.GetItem(i).Geometry;
                        
                        // 待写入数据
                        Int32 pointNum = 1;
                        Double pointX = point.X;
                        Double pointY = point.Y;

                        // 将数据写入
                        binaryWriter.Write(pointNum);
                        binaryWriter.Write(pointX);
                        binaryWriter.Write(pointY);
                    }
                    else if(geometryType == 1)// MultiPolyline
                    {
                        // 该MultiPolyline对象
                        MyMapObjects.moMultiPolyline multiPolyline = (MyMapObjects.moMultiPolyline)layer.Features.GetItem(i).Geometry;
                        
                        // 获取并写入MultiPolyline对象中的折线的数量
                        Int32 polylinesNum = multiPolyline.Parts.Count;
                        binaryWriter.Write(polylinesNum);

                        // 遍历所有折线
                        for(int j = 0; j < polylinesNum; ++j)
                        {
                            // 获取当前折线对象
                            MyMapObjects.moParts parts = multiPolyline.Parts;

                            // 获取并写入当前折线中的点数目，每个parts只有一个moPoints
                            MyMapObjects.moPoints points = parts.GetItem(j);

                            // 获取当前折线中的点数目
                            Int32 pointsNum = points.Count;
                            binaryWriter.Write(pointsNum);

                            // 遍历所有点读取并写入坐标
                            for(int k = 0; k < pointsNum; ++k)
                            {
                                Double pointX = points.GetItem(k).X;
                                Double pointY = points.GetItem(k).Y;

                                binaryWriter.Write(pointX);
                                binaryWriter.Write(pointY);
                            }
                        }
                    }
                    else if(geometryType == 2)//MultiPolygon
                    {
                        // 该MultiPolygon对象
                        MyMapObjects.moMultiPolygon multiPolyline = (MyMapObjects.moMultiPolygon)layer.Features.GetItem(i).Geometry;

                        // 获取并写入MultiPolyline对象中的折线的数量
                        Int32 polylinesNum = multiPolyline.Parts.Count;
                        binaryWriter.Write(polylinesNum);

                        // 遍历所有折线
                        for (int j = 0; j < polylinesNum; ++j)
                        {
                            // 获取当前折线对象
                            MyMapObjects.moParts parts = multiPolyline.Parts;

                            // 获取并写入当前折线中的点数目，每个parts只有一个moPoints
                            MyMapObjects.moPoints points = parts.GetItem(j);

                            // 获取当前折线中的点数目
                            Int32 pointsNum = points.Count;
                            binaryWriter.Write(pointsNum);

                            // 遍历所有点读取并写入坐标
                            for (int k = 0; k < pointsNum; ++k)
                            {
                                Double pointX = points.GetItem(k).X;
                                Double pointY = points.GetItem(k).Y;

                                binaryWriter.Write(pointX);
                                binaryWriter.Write(pointY);
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("UnSupported Geometry Type!");
                    }

                    // 该对象的属性
                    MyMapObjects.moAttributes attributes = layer.Features.GetItem(i).Attributes;
                    for (int j = 0; j < fields.Count; ++j)
                    {
                        MyMapObjects.moField field = fields.GetItem(j);
                        MyMapObjects.moValueTypeConstant valueType = field.ValueType;
                        WriteValue(valueType, binaryWriter, attributes.GetItem(j));
                    }
                }

                binaryWriter.Dispose();
                fileStream.Dispose();
            }
            catch(IOException exception)
            {
                binaryWriter.Dispose();
                fileStream.Dispose();
                MessageBox.Show(exception.Message + "\nCannot write to file: " + layerName);
            }
        }

        /// <summary>
        /// 读取工程文件
        /// </summary>
        internal static MyMapObjects.moLayers LoadMapProj(BinaryReader sr, string path)
        {
            MyMapObjects.moLayers layers = new MyMapObjects.moLayers();

            // 图层文件数量
            Int32 layerCount = sr.ReadInt32();
            
            for(int i = 0; i < layerCount; ++i)
            {
                // 当前图层文件的路径
                string layerPath = sr.ReadString();
                
                // 根据当前文件的路径读取该文件
                FileStream sFileStream = new FileStream(layerPath, FileMode.Open);
                BinaryReader binaryReader = new BinaryReader(sFileStream);
                MyMapObjects.moMapLayer sLayer = DataIOTools.LoadMapLayer(binaryReader, layerPath);
                binaryReader.Dispose();
                sFileStream.Dispose();

                // 获取当前图层文件的样式类型
                Int32 symbolType = sr.ReadInt32();

                // 读取并设置图层文件具体样式内容
                if(symbolType == 1)
                {
                    MyMapObjects.moSimpleRenderer simpleRender = new MyMapObjects.moSimpleRenderer();
                    MyMapObjects.moSimpleMarkerSymbol simpleMarkerSymbol = ReadSimpleMarkerSymbol(sr);
                    simpleRender.Symbol = simpleMarkerSymbol;
                    sLayer.Renderer = simpleRender;
                }
                else if(symbolType == 2)
                {
                    MyMapObjects.moSimpleRenderer simpleRender = new MyMapObjects.moSimpleRenderer();
                    MyMapObjects.moSimpleLineSymbol simpleLineSymbol = ReadSimpleLineSymbol(sr);
                    simpleRender.Symbol = simpleLineSymbol;
                    sLayer.Renderer = simpleRender;
                }
                else if(symbolType == 3)
                {
                    MyMapObjects.moSimpleRenderer simpleRender = new MyMapObjects.moSimpleRenderer();
                    MyMapObjects.moSimpleFillSymbol simpleFillSymbol = ReadSimpleFillSymbol(sr);
                    simpleRender.Symbol = simpleFillSymbol;
                    sLayer.Renderer = simpleRender;
                }
                else if(symbolType == 4)
                {
                    MyMapObjects.moUniqueValueRenderer uniqueValueRenderer = new MyMapObjects.moUniqueValueRenderer();

                    string field = sr.ReadString();
                    uniqueValueRenderer.Field = field;

                    string headTitle = sr.ReadString();
                    uniqueValueRenderer.HeadTitle = headTitle;

                    bool showHead = sr.ReadBoolean();
                    uniqueValueRenderer.ShowHead = showHead;

                    List<string> values = new List<string>();// 唯一值列表
                    Int32 valuesCount = sr.ReadInt32();
                    for(int j = 0; j < valuesCount; ++j)
                    {
                        values.Add(sr.ReadString());
                    }
                    uniqueValueRenderer.Values = values;

                    List<MyMapObjects.moSymbol> symbols = new List<MyMapObjects.moSymbol>();
                    Int32 symbolsCount = sr.ReadInt32();
                    for(int j = 0; j < symbolsCount; ++j)
                    {
                        MyMapObjects.moSymbol symbol = ReadSimpleMarkerSymbol(sr);
                        symbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleMarkerSymbol;
                        symbols.Add(symbol);
                    }
                    uniqueValueRenderer.Symbols = symbols;

                    MyMapObjects.moSimpleMarkerSymbol defaultSymbol = ReadSimpleMarkerSymbol(sr);
                    defaultSymbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleMarkerSymbol;
                    uniqueValueRenderer.DefaultSymbol = defaultSymbol;

                    bool showDefaultSymbol = sr.ReadBoolean();
                    uniqueValueRenderer.ShowDefaultSymbol = showDefaultSymbol;

                    sLayer.Renderer = uniqueValueRenderer;

                }
                else if(symbolType == 5)
                {
                    MyMapObjects.moUniqueValueRenderer uniqueValueRenderer = new MyMapObjects.moUniqueValueRenderer();

                    string field = sr.ReadString();
                    uniqueValueRenderer.Field = field;

                    string headTitle = sr.ReadString();
                    uniqueValueRenderer.HeadTitle = headTitle;

                    bool showHead = sr.ReadBoolean();
                    uniqueValueRenderer.ShowHead = showHead;

                    List<string> values = new List<string>();// 唯一值列表
                    Int32 valuesCount = sr.ReadInt32();
                    for (int j = 0; j < valuesCount; ++j)
                    {
                        values.Add(sr.ReadString());
                    }
                    uniqueValueRenderer.Values = values;

                    List<MyMapObjects.moSymbol> symbols = new List<MyMapObjects.moSymbol>();
                    Int32 symbolsCount = sr.ReadInt32();
                    for (int j = 0; j < symbolsCount; ++j)
                    {
                        MyMapObjects.moSymbol symbol = ReadSimpleLineSymbol(sr);
                        symbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleLineSymbol;
                        symbols.Add(symbol);
                    }
                    uniqueValueRenderer.Symbols = symbols;

                    MyMapObjects.moSimpleLineSymbol defaultSymbol = ReadSimpleLineSymbol(sr);
                    defaultSymbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleLineSymbol;
                    uniqueValueRenderer.DefaultSymbol = defaultSymbol;

                    bool showDefaultSymbol = sr.ReadBoolean();
                    uniqueValueRenderer.ShowDefaultSymbol = showDefaultSymbol;

                    sLayer.Renderer = uniqueValueRenderer;

                }
                else if(symbolType == 6)
                {
                    MyMapObjects.moUniqueValueRenderer uniqueValueRenderer = new MyMapObjects.moUniqueValueRenderer();

                    string field = sr.ReadString();
                    uniqueValueRenderer.Field = field;

                    string headTitle = sr.ReadString();
                    uniqueValueRenderer.HeadTitle = headTitle;

                    bool showHead = sr.ReadBoolean();
                    uniqueValueRenderer.ShowHead = showHead;

                    List<string> values = new List<string>();// 唯一值列表
                    Int32 valuesCount = sr.ReadInt32();
                    for (int j = 0; j < valuesCount; ++j)
                    {
                        values.Add(sr.ReadString());
                    }
                    uniqueValueRenderer.Values = values;

                    List<MyMapObjects.moSymbol> symbols = new List<MyMapObjects.moSymbol>();
                    Int32 symbolsCount = sr.ReadInt32();
                    for (int j = 0; j < symbolsCount; ++j)
                    {
                        MyMapObjects.moSymbol symbol = ReadSimpleFillSymbol(sr);
                        symbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleFillSymbol;
                        symbols.Add(symbol);
                    }
                    uniqueValueRenderer.Symbols = symbols;

                    MyMapObjects.moSimpleFillSymbol defaultSymbol = ReadSimpleFillSymbol(sr);
                    defaultSymbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleFillSymbol;
                    uniqueValueRenderer.DefaultSymbol = defaultSymbol;

                    bool showDefaultSymbol = sr.ReadBoolean();
                    uniqueValueRenderer.ShowDefaultSymbol = showDefaultSymbol;

                    sLayer.Renderer = uniqueValueRenderer;

                }
                else if (symbolType == 7)
                {
                    MyMapObjects.moClassBreaksRenderer classBreaksRenderer = new MyMapObjects.moClassBreaksRenderer();

                    string field = sr.ReadString();
                    classBreaksRenderer.Field = field;

                    string headTitle = sr.ReadString();
                    classBreaksRenderer.HeadTitle = headTitle;

                    bool showHead = sr.ReadBoolean();
                    classBreaksRenderer.ShowHead = showHead;

                    List<double> values = new List<double>();// 唯一值列表
                    Int32 valuesCount = sr.ReadInt32();
                    for (int j = 0; j < valuesCount; ++j)
                    {
                        values.Add(sr.ReadDouble());
                    }
                    classBreaksRenderer.BreakValues = values;

                    List<MyMapObjects.moSymbol> symbols = new List<MyMapObjects.moSymbol>();
                    Int32 symbolsCount = sr.ReadInt32();
                    for (int j = 0; j < symbolsCount; ++j)
                    {
                        MyMapObjects.moSymbol symbol = ReadSimpleMarkerSymbol(sr);
                        symbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleMarkerSymbol;
                        symbols.Add(symbol);
                    }
                    classBreaksRenderer.Symbols = symbols;

                    MyMapObjects.moSimpleMarkerSymbol defaultSymbol = ReadSimpleMarkerSymbol(sr);
                    defaultSymbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleMarkerSymbol;
                    classBreaksRenderer.DefaultSymbol = defaultSymbol;

                    bool showDefaultSymbol = sr.ReadBoolean();
                    classBreaksRenderer.ShowDefaultSymbol = showDefaultSymbol;

                    sLayer.Renderer = classBreaksRenderer;

                }
                else if (symbolType == 8)
                {
                    MyMapObjects.moClassBreaksRenderer classBreaksRenderer = new MyMapObjects.moClassBreaksRenderer();

                    string field = sr.ReadString();
                    classBreaksRenderer.Field = field;

                    string headTitle = sr.ReadString();
                    classBreaksRenderer.HeadTitle = headTitle;

                    bool showHead = sr.ReadBoolean();
                    classBreaksRenderer.ShowHead = showHead;

                    List<double> values = new List<double>();// 唯一值列表
                    Int32 valuesCount = sr.ReadInt32();
                    for (int j = 0; j < valuesCount; ++j)
                    {
                        values.Add(sr.ReadDouble());
                    }
                    classBreaksRenderer.BreakValues = values;

                    List<MyMapObjects.moSymbol> symbols = new List<MyMapObjects.moSymbol>();
                    Int32 symbolsCount = sr.ReadInt32();
                    for (int j = 0; j < symbolsCount; ++j)
                    {
                        MyMapObjects.moSymbol symbol = ReadSimpleLineSymbol(sr);
                        symbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleLineSymbol;
                        symbols.Add(symbol);
                    }
                    classBreaksRenderer.Symbols = symbols;

                    MyMapObjects.moSimpleLineSymbol defaultSymbol = ReadSimpleLineSymbol(sr);
                    defaultSymbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleLineSymbol;
                    classBreaksRenderer.DefaultSymbol = defaultSymbol;

                    bool showDefaultSymbol = sr.ReadBoolean();
                    classBreaksRenderer.ShowDefaultSymbol = showDefaultSymbol;

                    sLayer.Renderer = classBreaksRenderer;

                }
                else if (symbolType == 9)
                {
                    MyMapObjects.moClassBreaksRenderer classBreaksRenderer = new MyMapObjects.moClassBreaksRenderer();

                    string field = sr.ReadString();
                    classBreaksRenderer.Field = field;

                    string headTitle = sr.ReadString();
                    classBreaksRenderer.HeadTitle = headTitle;

                    bool showHead = sr.ReadBoolean();
                    classBreaksRenderer.ShowHead = showHead;

                    List<double> values = new List<double>();// 唯一值列表
                    Int32 valuesCount = sr.ReadInt32();
                    for (int j = 0; j < valuesCount; ++j)
                    {
                        values.Add(sr.ReadDouble());
                    }
                    classBreaksRenderer.BreakValues = values;

                    List<MyMapObjects.moSymbol> symbols = new List<MyMapObjects.moSymbol>();
                    Int32 symbolsCount = sr.ReadInt32();
                    for (int j = 0; j < symbolsCount; ++j)
                    {
                        MyMapObjects.moSymbol symbol = ReadSimpleFillSymbol(sr);
                        symbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleFillSymbol;
                        symbols.Add(symbol);
                    }
                    classBreaksRenderer.Symbols = symbols;

                    MyMapObjects.moSimpleFillSymbol defaultSymbol = ReadSimpleFillSymbol(sr);
                    defaultSymbol.SymbolType = MyMapObjects.moSymbolTypeConstant.SimpleFillSymbol;
                    classBreaksRenderer.DefaultSymbol = defaultSymbol;

                    bool showDefaultSymbol = sr.ReadBoolean();
                    classBreaksRenderer.ShowDefaultSymbol = showDefaultSymbol;

                    sLayer.Renderer = classBreaksRenderer;
                }

                // 读取标注信息

                if (! sr.ReadBoolean())
                {
                    layers.Add(sLayer);
                    continue;
                }

                MyMapObjects.moLabelRenderer labelRenderer = new MyMapObjects.moLabelRenderer();
                MyMapObjects.moTextSymbol textSymbol = new MyMapObjects.moTextSymbol();

                bool labelFeatures = sr.ReadBoolean();
                labelRenderer.LabelFeatures = labelFeatures;

                if (labelFeatures)
                {
                    string labelField = sr.ReadString();
                    labelRenderer.Field = labelField;

                    Font font = new Font(sr.ReadString(), (float)sr.ReadDouble());       //字体
                    textSymbol.Font = font;

                    Byte fontColorA = sr.ReadByte();
                    Byte fontColorR = sr.ReadByte();
                    Byte fontColorG = sr.ReadByte();
                    Byte fontColorB = sr.ReadByte();
                    Color fontColor = Color.FromArgb(fontColorA, fontColorR, fontColorG, fontColorB);
                    textSymbol.FontColor = fontColor;

                    MyMapObjects.moTextSymbolAlignmentConstant alignment = (MyMapObjects.moTextSymbolAlignmentConstant)sr.ReadInt32();
                    textSymbol.Alignment = alignment;

                    double offsetX = sr.ReadDouble(), offsetY = sr.ReadDouble();  //X,Y方向偏移量，单位毫米，向右为正，向上为正
                    textSymbol.OffsetX = offsetX;
                    textSymbol.OffsetY = offsetY;

                    bool useMask = sr.ReadBoolean();
                    textSymbol.UseMask = useMask;

                    Byte maskColorA = sr.ReadByte();
                    Byte maskColorR = sr.ReadByte();
                    Byte maskColorG = sr.ReadByte();
                    Byte maskColorB = sr.ReadByte();
                    Color maskColor = Color.FromArgb(maskColorA, maskColorR, maskColorG, maskColorB); //描边的颜色
                    textSymbol.MaskColor = maskColor;

                    double maskWidth = sr.ReadDouble();    //描边宽度，单位毫米
                    textSymbol.MaskWidth = maskWidth;

                    labelRenderer.TextSymbol = textSymbol;
                }

                sLayer.LabelRenderer = labelRenderer;
                
                layers.Add(sLayer);
            }

            return layers;
        }

        /// <summary>
        /// 将所有图层文件对象写入工程文件
        /// </summary>
        /// <param name="layers">所有图层文件对象</param>
        /// <param name="projName">工程文件路径</param>
        internal static void WriteLayersToProj(MyMapObjects.moLayers layers, string projName)
        {
            // 根据文件路径创建文件
            FileStream fileStream = new FileStream(projName, FileMode.Create);
            BinaryWriter binaryWriter = new BinaryWriter(fileStream);

            // 写入图层文件数量
            binaryWriter.Write((Int32)layers.Count);

            // 遍历每一个图层文件，依次写入以下内容
            for (int i = 0; i < layers.Count; ++i)
            {
                try
                {
                    // 写入图层文件路径
                    string layerPath = layers.GetItem(i).Path;
                    binaryWriter.Write(layerPath);

                    // 获取图层文件的渲染方式和几何类型
                    MyMapObjects.moRendererTypeConstant renderType = layers.GetItem(i).Renderer.RendererType;
                    MyMapObjects.moGeometryTypeConstant geometryType = layers.GetItem(i).ShapeType;

                    // 根据图层文件的渲染方式和几何类型，判断写入图层文件的渲染方式
                    if(renderType == MyMapObjects.moRendererTypeConstant.Simple)
                    {
                        MyMapObjects.moSimpleRenderer simpleRender = (MyMapObjects.moSimpleRenderer)(layers.GetItem(i).Renderer);

                        if (geometryType == MyMapObjects.moGeometryTypeConstant.Point)
                        {
                            binaryWriter.Write((Int32)1);
                            MyMapObjects.moSimpleMarkerSymbol simpleMarkerSymbol = (MyMapObjects.moSimpleMarkerSymbol)simpleRender.Symbol;
                            WriteSimpleMarkerSymbol(binaryWriter, simpleMarkerSymbol);
                        }
                        else if(geometryType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
                        {
                            binaryWriter.Write((Int32)2);
                            MyMapObjects.moSimpleLineSymbol simpleLineSymbol = (MyMapObjects.moSimpleLineSymbol)simpleRender.Symbol;
                            WriteSimpleLineSymbol(binaryWriter, simpleLineSymbol);
                        }
                        else if(geometryType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
                        {
                            binaryWriter.Write((Int32)3);
                            MyMapObjects.moSimpleFillSymbol simpleFillSymbol = (MyMapObjects.moSimpleFillSymbol)simpleRender.Symbol;
                            WriteSimpleFillSymbol(binaryWriter, simpleFillSymbol);
                        }

                    }
                    else if(renderType == MyMapObjects.moRendererTypeConstant.UniqueValue)
                    {
                        MyMapObjects.moUniqueValueRenderer uniqueValueRenderer = (MyMapObjects.moUniqueValueRenderer)(layers.GetItem(i).Renderer);

                        if (geometryType == MyMapObjects.moGeometryTypeConstant.Point)
                        {
                            binaryWriter.Write((Int32)4);

                            string field = uniqueValueRenderer.Field;// 绑定字段
                            binaryWriter.Write(field);

                            string headTitle = uniqueValueRenderer.HeadTitle;// 在图层显示控件中的标题
                            binaryWriter.Write(headTitle);

                            bool showHead = uniqueValueRenderer.ShowHead;// 在图层显示控件中是否显示标题
                            binaryWriter.Write(showHead);

                            List<string> values = uniqueValueRenderer.Values;// 唯一值列表
                            binaryWriter.Write((Int32)values.Count);
                            foreach(string value in values)
                            {
                                binaryWriter.Write(value);
                            }

                            List<MyMapObjects.moSimpleMarkerSymbol> symbols = new List<MyMapObjects.moSimpleMarkerSymbol>();
                            for (int j = 0; j < uniqueValueRenderer.Symbols.Count; ++j) 
                            {
                                symbols.Add((MyMapObjects.moSimpleMarkerSymbol)uniqueValueRenderer.Symbols[j]);

                            }// 唯一值对应的符号列表


                            binaryWriter.Write((Int32)symbols.Count);
                            foreach (MyMapObjects.moSimpleMarkerSymbol symbol in symbols)
                            {
                                WriteSimpleMarkerSymbol(binaryWriter, symbol);
                            }

                            MyMapObjects.moSimpleMarkerSymbol defaultSymbol = (MyMapObjects.moSimpleMarkerSymbol)uniqueValueRenderer.DefaultSymbol;// 默认符号
                            WriteSimpleMarkerSymbol(binaryWriter, defaultSymbol);

                            bool showDefaultSymbol = uniqueValueRenderer.ShowDefaultSymbol;// 在图层显示控件中是否显示默认符号
                            binaryWriter.Write((Boolean)showDefaultSymbol);
                        }
                        else if (geometryType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
                        {
                            binaryWriter.Write((Int32)5);

                            string field = uniqueValueRenderer.Field;// 绑定字段
                            binaryWriter.Write(field);

                            string headTitle = uniqueValueRenderer.HeadTitle;// 在图层显示控件中的标题
                            binaryWriter.Write(headTitle);

                            bool showHead = uniqueValueRenderer.ShowHead;// 在图层显示控件中是否显示标题
                            binaryWriter.Write(showHead);

                            List<string> values = uniqueValueRenderer.Values;// 唯一值列表
                            binaryWriter.Write((Int32)values.Count);
                            foreach (string value in values)
                            {
                                binaryWriter.Write(value);
                            }

                            List<MyMapObjects.moSimpleLineSymbol> symbols = new List<MyMapObjects.moSimpleLineSymbol>();
                            for (int j = 0; j < uniqueValueRenderer.Symbols.Count; ++j)
                            {
                                symbols.Add((MyMapObjects.moSimpleLineSymbol)uniqueValueRenderer.Symbols[j]);
                            }// 唯一值对应的符号列表
                            binaryWriter.Write((Int32)symbols.Count);
                            foreach (MyMapObjects.moSimpleLineSymbol symbol in symbols)
                            {
                                WriteSimpleLineSymbol(binaryWriter, symbol);
                            }

                            MyMapObjects.moSimpleLineSymbol defaultSymbol = (MyMapObjects.moSimpleLineSymbol)uniqueValueRenderer.DefaultSymbol;// 默认符号
                            WriteSimpleLineSymbol(binaryWriter, defaultSymbol);

                            bool showDefaultSymbol = uniqueValueRenderer.ShowDefaultSymbol;// 在图层显示控件中是否显示默认符号
                            binaryWriter.Write((Boolean)showDefaultSymbol);
                        }
                        else if (geometryType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
                        {
                            binaryWriter.Write((Int32)6);

                            string field = uniqueValueRenderer.Field;// 绑定字段
                            binaryWriter.Write(field);

                            string headTitle = uniqueValueRenderer.HeadTitle;// 在图层显示控件中的标题
                            binaryWriter.Write(headTitle);

                            bool showHead = uniqueValueRenderer.ShowHead;// 在图层显示控件中是否显示标题
                            binaryWriter.Write(showHead);

                            List<string> values = uniqueValueRenderer.Values;// 唯一值列表
                            binaryWriter.Write((Int32)values.Count);
                            foreach (string value in values)
                            {
                                binaryWriter.Write(value);
                            }

                            List<MyMapObjects.moSimpleFillSymbol> symbols = new List<MyMapObjects.moSimpleFillSymbol>();
                            for (int j = 0; j < uniqueValueRenderer.Symbols.Count; ++j)
                            {
                                symbols.Add((MyMapObjects.moSimpleFillSymbol)uniqueValueRenderer.Symbols[j]);
                            }// 唯一值对应的符号列表
                            binaryWriter.Write((Int32)symbols.Count);
                            foreach (MyMapObjects.moSimpleFillSymbol symbol in symbols)
                            {
                                WriteSimpleFillSymbol(binaryWriter, symbol);
                            }

                            MyMapObjects.moSimpleFillSymbol defaultSymbol = (MyMapObjects.moSimpleFillSymbol)uniqueValueRenderer.DefaultSymbol;// 默认符号
                            WriteSimpleFillSymbol(binaryWriter, defaultSymbol);

                            bool showDefaultSymbol = uniqueValueRenderer.ShowDefaultSymbol;// 在图层显示控件中是否显示默认符号
                            binaryWriter.Write((Boolean)showDefaultSymbol);
                        }
                    }
                    else if(renderType == MyMapObjects.moRendererTypeConstant.ClassBreaks)
                    {
                        MyMapObjects.moClassBreaksRenderer classBreaksRenderer = (MyMapObjects.moClassBreaksRenderer)(layers.GetItem(i).Renderer);

                        if (geometryType == MyMapObjects.moGeometryTypeConstant.Point)
                        {
                            binaryWriter.Write((Int32)7);

                            string field = classBreaksRenderer.Field;// 绑定字段
                            binaryWriter.Write(field);

                            string headTitle = classBreaksRenderer.HeadTitle;// 在图层显示控件中的标题
                            binaryWriter.Write(headTitle);

                            bool showHead = classBreaksRenderer.ShowHead;// 在图层显示控件中是否显示标题
                            binaryWriter.Write(showHead);

                            List<double> values = classBreaksRenderer.BreakValues;// 唯一值列表
                            binaryWriter.Write((Int32)values.Count);
                            foreach (double value in values)
                            {
                                binaryWriter.Write(value);
                            }

                            List<MyMapObjects.moSimpleMarkerSymbol> symbols = new List<MyMapObjects.moSimpleMarkerSymbol>();
                            for (int j = 0; j < classBreaksRenderer.Symbols.Count; ++j)
                            {
                                symbols.Add((MyMapObjects.moSimpleMarkerSymbol)classBreaksRenderer.Symbols[j]);
                            }// 唯一值对应的符号列表
                            binaryWriter.Write((Int32)symbols.Count);
                            foreach (MyMapObjects.moSimpleMarkerSymbol symbol in symbols)
                            {
                                WriteSimpleMarkerSymbol(binaryWriter, symbol);
                            }

                            MyMapObjects.moSimpleMarkerSymbol defaultSymbol = (MyMapObjects.moSimpleMarkerSymbol)classBreaksRenderer.DefaultSymbol;// 默认符号
                            WriteSimpleMarkerSymbol(binaryWriter, defaultSymbol);

                            bool showDefaultSymbol = classBreaksRenderer.ShowDefaultSymbol;// 在图层显示控件中是否显示默认符号
                            binaryWriter.Write((Boolean)showDefaultSymbol);

                        }
                        else if (geometryType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
                        {
                            binaryWriter.Write((Int32)8);

                            string field = classBreaksRenderer.Field;// 绑定字段
                            binaryWriter.Write(field);

                            string headTitle = classBreaksRenderer.HeadTitle;// 在图层显示控件中的标题
                            binaryWriter.Write(headTitle);

                            bool showHead = classBreaksRenderer.ShowHead;// 在图层显示控件中是否显示标题
                            binaryWriter.Write(showHead);

                            List<double> values = classBreaksRenderer.BreakValues;// 唯一值列表
                            binaryWriter.Write((Int32)values.Count);
                            foreach (double value in values)
                            {
                                binaryWriter.Write(value);
                            }

                            List<MyMapObjects.moSimpleLineSymbol> symbols = new List<MyMapObjects.moSimpleLineSymbol>();
                            for (int j = 0; j < classBreaksRenderer.Symbols.Count; ++j)
                            {
                                symbols.Add((MyMapObjects.moSimpleLineSymbol)classBreaksRenderer.Symbols[j]);
                            }// 唯一值对应的符号列表
                            binaryWriter.Write((Int32)symbols.Count);
                            foreach (MyMapObjects.moSimpleLineSymbol symbol in symbols)
                            {
                                WriteSimpleLineSymbol(binaryWriter, symbol);
                            }

                            MyMapObjects.moSimpleLineSymbol defaultSymbol = (MyMapObjects.moSimpleLineSymbol)classBreaksRenderer.DefaultSymbol;// 默认符号
                            WriteSimpleLineSymbol(binaryWriter, defaultSymbol);

                            bool showDefaultSymbol = classBreaksRenderer.ShowDefaultSymbol;// 在图层显示控件中是否显示默认符号
                            binaryWriter.Write((Boolean)showDefaultSymbol);
                        }
                        else if (geometryType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
                        {
                            binaryWriter.Write((Int32)9);

                            string field = classBreaksRenderer.Field;// 绑定字段
                            binaryWriter.Write(field);

                            string headTitle = classBreaksRenderer.HeadTitle;// 在图层显示控件中的标题
                            binaryWriter.Write(headTitle);

                            bool showHead = classBreaksRenderer.ShowHead;// 在图层显示控件中是否显示标题
                            binaryWriter.Write(showHead);

                            List<double> values = classBreaksRenderer.BreakValues;// 唯一值列表
                            binaryWriter.Write((Int32)values.Count);
                            foreach (double value in values)
                            {
                                binaryWriter.Write(value);
                            }

                            List<MyMapObjects.moSimpleFillSymbol> symbols = new List<MyMapObjects.moSimpleFillSymbol>();
                            for (int j = 0; j < classBreaksRenderer.Symbols.Count; ++j)
                            {
                                symbols.Add((MyMapObjects.moSimpleFillSymbol)classBreaksRenderer.Symbols[j]);
                            }// 唯一值对应的符号列表
                            binaryWriter.Write((Int32)symbols.Count);
                            foreach (MyMapObjects.moSimpleFillSymbol symbol in symbols)
                            {
                                WriteSimpleFillSymbol(binaryWriter, symbol);
                            }

                            MyMapObjects.moSimpleFillSymbol defaultSymbol = (MyMapObjects.moSimpleFillSymbol)classBreaksRenderer.DefaultSymbol;// 默认符号
                            WriteSimpleFillSymbol(binaryWriter, defaultSymbol);

                            bool showDefaultSymbol = classBreaksRenderer.ShowDefaultSymbol;// 在图层显示控件中是否显示默认符号
                            binaryWriter.Write((Boolean)showDefaultSymbol);

                        }
                    }

                    // 写入LabelRender信息
                    MyMapObjects.moLabelRenderer labelRenderer = layers.GetItem(i).LabelRenderer;

                    if (labelRenderer == null)
                    {
                        binaryWriter.Write((Boolean)false);
                        continue;
                    }
                    else
                    {
                        binaryWriter.Write((Boolean)true);
                    }

                    bool labelFeatures = labelRenderer.LabelFeatures;       //是否标注
                    binaryWriter.Write((Boolean)labelFeatures);

                    string labelField = labelRenderer.Field;                 //注记字段
                    binaryWriter.Write(labelField);

                    if (labelFeatures)
                    {
                        MyMapObjects.moTextSymbol textSymbol = labelRenderer.TextSymbol;

                        Font font = textSymbol.Font;       //字体
                        binaryWriter.Write(font.Name);
                        binaryWriter.Write((double)font.Size);

                        Color fontColor = textSymbol.FontColor;
                        Byte fontColorA = fontColor.A;
                        Byte fontColorR = fontColor.R;
                        Byte fontColorG = fontColor.G;
                        Byte fontColorB = fontColor.B;
                        binaryWriter.Write(fontColorA);
                        binaryWriter.Write(fontColorR);
                        binaryWriter.Write(fontColorG);
                        binaryWriter.Write(fontColorB);

                        MyMapObjects.moTextSymbolAlignmentConstant alignment = textSymbol.Alignment;   //布局
                        binaryWriter.Write((Int32)alignment);

                        double offsetX = textSymbol.OffsetX, offsetY = textSymbol.OffsetY;  //X,Y方向偏移量，单位毫米，向右为正，向上为正
                        binaryWriter.Write(offsetX);
                        binaryWriter.Write(offsetY);

                        bool useMask = textSymbol.UseMask;      //是否使用描边
                        binaryWriter.Write((Boolean)useMask);

                        Color maskColor = textSymbol.MaskColor; //描边的颜色
                        Byte maskColorA = maskColor.A;
                        Byte maskColorR = maskColor.R;
                        Byte maskColorG = maskColor.G;
                        Byte maskColorB = maskColor.B;
                        binaryWriter.Write(maskColorA);
                        binaryWriter.Write(maskColorR);
                        binaryWriter.Write(maskColorG);
                        binaryWriter.Write(maskColorB);

                        double maskWidth = textSymbol.MaskWidth;    //描边宽度，单位毫米
                        binaryWriter.Write(maskWidth);

                    }

                }
                catch (IOException exception)
                {
                    binaryWriter.Dispose();
                    fileStream.Dispose();
                    MessageBox.Show(exception.Message + "\nCannot write to file: " + projName);
                }

            }

            binaryWriter.Dispose();
            fileStream.Dispose();
        }

        #endregion

        #region 私有函数

        // 从二进制文件中读取simpleMarkerSymbol对象的数据
        private static MyMapObjects.moSimpleMarkerSymbol ReadSimpleMarkerSymbol(BinaryReader sr)
        {
            MyMapObjects.moSimpleMarkerSymbol simpleMarkerSymbol = new MyMapObjects.moSimpleMarkerSymbol();

            string label = sr.ReadString();
            simpleMarkerSymbol.Label = label;

            bool visible = sr.ReadBoolean();
            simpleMarkerSymbol.Visible = visible;

            MyMapObjects.moSimpleMarkerSymbolStyleConstant style = (MyMapObjects.moSimpleMarkerSymbolStyleConstant)sr.ReadInt32();
            simpleMarkerSymbol.Style = style;

            byte colorA = sr.ReadByte();
            byte colorR = sr.ReadByte();
            byte colorG = sr.ReadByte();
            byte colorB = sr.ReadByte();
            Color color = Color.FromArgb(colorA, colorR, colorG, colorB);
            simpleMarkerSymbol.Color = color;

            double size = sr.ReadDouble();
            simpleMarkerSymbol.Size = size;

            return simpleMarkerSymbol;
        }

        // 从二进制文件中读取simpleLineSymbol对象的数据
        private static MyMapObjects.moSimpleLineSymbol ReadSimpleLineSymbol(BinaryReader sr)
        {
            MyMapObjects.moSimpleLineSymbol simpleLineSymbol = new MyMapObjects.moSimpleLineSymbol();

            string label = sr.ReadString();
            simpleLineSymbol.Label = label;

            bool visible = sr.ReadBoolean();
            simpleLineSymbol.Visible = visible;

            MyMapObjects.moSimpleLineSymbolStyleConstant style = (MyMapObjects.moSimpleLineSymbolStyleConstant)sr.ReadInt32();
            simpleLineSymbol.Style = style;

            byte colorA = sr.ReadByte();
            byte colorR = sr.ReadByte();
            byte colorG = sr.ReadByte();
            byte colorB = sr.ReadByte();
            Color color = Color.FromArgb(colorA, colorR, colorG, colorB);
            simpleLineSymbol.Color = color;

            double size = sr.ReadDouble();
            simpleLineSymbol.Size = size;

            return simpleLineSymbol;
        }

        // 从二进制文件中读取simpleFillSymbol对象的数据
        private static MyMapObjects.moSimpleFillSymbol ReadSimpleFillSymbol(BinaryReader sr)
        {
            MyMapObjects.moSimpleFillSymbol simpleFillSymbol = new MyMapObjects.moSimpleFillSymbol();
            MyMapObjects.moSimpleLineSymbol outline = new MyMapObjects.moSimpleLineSymbol();

            string label = sr.ReadString();
            simpleFillSymbol.Label = label;

            bool visible = sr.ReadBoolean();
            simpleFillSymbol.Visible = visible;

            Byte colorA = sr.ReadByte();
            Byte colorR = sr.ReadByte();
            Byte colorG = sr.ReadByte();
            Byte colorB = sr.ReadByte();
            Color color = Color.FromArgb(colorA, colorR, colorG, colorB);
            simpleFillSymbol.Color = color;

            string labelLine = sr.ReadString();
            outline.Label = labelLine;

            bool visibleLine = sr.ReadBoolean();
            outline.Visible = visibleLine;

            MyMapObjects.moSimpleLineSymbolStyleConstant styleLine = (MyMapObjects.moSimpleLineSymbolStyleConstant)sr.ReadInt32();
            outline.Style = styleLine;

            Byte colorALine = sr.ReadByte();
            Byte colorRLine = sr.ReadByte();
            Byte colorGLine = sr.ReadByte();
            Byte colorBLine = sr.ReadByte();
            Color colorLine = Color.FromArgb(colorALine, colorRLine, colorGLine, colorBLine);
            outline.Color = colorLine;

            double sizeLine = sr.ReadDouble();
            outline.Size = sizeLine;

            simpleFillSymbol.Outline = outline;

            return simpleFillSymbol;
        }

        // 将给定simpleMarkerSymbol对象的数据写入二进制文件中
        private static void WriteSimpleMarkerSymbol(BinaryWriter binaryWriter, MyMapObjects.moSimpleMarkerSymbol simpleMarkerSymbol)
        {
            string label = simpleMarkerSymbol.Label;     // 符号标签
            binaryWriter.Write(label);

            bool visible = simpleMarkerSymbol.Visible;   // 是否可见
            binaryWriter.Write((Boolean)visible);

            MyMapObjects.moSimpleMarkerSymbolStyleConstant style = simpleMarkerSymbol.Style;    // 形状
            binaryWriter.Write((Int32)style);

            Color color = simpleMarkerSymbol.Color; // 颜色
            byte colorA = color.A;
            byte colorR = color.R;
            byte colorG = color.G;
            byte colorB = color.B;
            binaryWriter.Write(colorA);
            binaryWriter.Write(colorR);
            binaryWriter.Write(colorG);
            binaryWriter.Write(colorB);

            double size = simpleMarkerSymbol.Size;       // 尺寸，默认为毫米
            binaryWriter.Write(size);

        }

        // 将给定的simpleLineSymbol对象的数据写入二进制文件中
        private static void WriteSimpleLineSymbol(BinaryWriter binaryWriter, MyMapObjects.moSimpleLineSymbol simpleLineSymbol)
        {
            string label = simpleLineSymbol.Label;     // 符号标签
            binaryWriter.Write(label);

            bool visible = simpleLineSymbol.Visible;   // 是否可见
            binaryWriter.Write((Boolean)visible);

            MyMapObjects.moSimpleLineSymbolStyleConstant style = simpleLineSymbol.Style;    // 形状
            binaryWriter.Write((Int32)style);

            Color color = simpleLineSymbol.Color; // 颜色
            byte colorA = color.A;
            byte colorR = color.R;
            byte colorG = color.G;
            byte colorB = color.B;
            binaryWriter.Write(colorA);
            binaryWriter.Write(colorR);
            binaryWriter.Write(colorG);
            binaryWriter.Write(colorB);

            double size = simpleLineSymbol.Size;       // 尺寸，默认为毫米
            binaryWriter.Write(size);

        }

        // 将给定的simpleFillSymbol对象的数据写入二进制文件中
        private static void WriteSimpleFillSymbol(BinaryWriter binaryWriter, MyMapObjects.moSimpleFillSymbol simpleFillSymbol)
        {
            string label = simpleFillSymbol.Label;
            binaryWriter.Write(label);

            bool visible = simpleFillSymbol.Visible;
            binaryWriter.Write((Boolean)visible);

            Color color = simpleFillSymbol.Color;
            Byte colorA = color.A;
            Byte colorR = color.R;
            Byte colorG = color.G;
            Byte colorB = color.B;
            binaryWriter.Write(colorA);
            binaryWriter.Write(colorR);
            binaryWriter.Write(colorG);
            binaryWriter.Write(colorB);

            MyMapObjects.moSimpleLineSymbol outline = simpleFillSymbol.Outline;
            string labelLine = outline.Label;     // 符号标签
            binaryWriter.Write(labelLine);

            bool visibleLine = outline.Visible;   // 是否可见
            binaryWriter.Write((Boolean)visibleLine);

            MyMapObjects.moSimpleLineSymbolStyleConstant styleLine = outline.Style;    // 形状
            binaryWriter.Write((Int32)styleLine);

            Color colorLine = outline.Color; // 颜色
            byte colorALine = colorLine.A;
            byte colorRLine = colorLine.R;
            byte colorGLine = colorLine.G;
            byte colorBLine = colorLine.B;
            binaryWriter.Write(colorALine);
            binaryWriter.Write(colorRLine);
            binaryWriter.Write(colorGLine);
            binaryWriter.Write(colorBLine);

            double sizeLine = outline.Size;       // 尺寸，默认为毫米
            binaryWriter.Write(sizeLine);
        }


        //读取字段集合
        private static MyMapObjects.moFields LoadFields(BinaryReader sr)
        {
            Int32 sFieldCount = sr.ReadInt32(); //字段数量
            MyMapObjects.moFields sFields = new MyMapObjects.moFields();
            for (Int32 i = 0; i <= sFieldCount - 1; i++)
            {
                string sName = sr.ReadString();
                MyMapObjects.moValueTypeConstant sValueType = (MyMapObjects.moValueTypeConstant)sr.ReadInt32();
                Int32 sTemp = sr.ReadInt32();   //不需要；
                MyMapObjects.moField sField = new MyMapObjects.moField(sName, sValueType);
                sFields.Append(sField);
            }
            return sFields;
        }

        //读取要素集合
        private static MyMapObjects.moFeatures LoadFeatures(MyMapObjects.moGeometryTypeConstant geometryType, MyMapObjects.moFields fields, BinaryReader sr)
        {
            MyMapObjects.moFeatures sFeatures = new MyMapObjects.moFeatures();
            Int32 sFeatureCount = sr.ReadInt32();
            for (int i = 0; i <= sFeatureCount - 1; i++)
            {
                MyMapObjects.moGeometry sGeometry = LoadGeometry(geometryType, sr);
                MyMapObjects.moAttributes sAttributes = LoadAttributes(fields, sr);
                MyMapObjects.moFeature sFeature = new MyMapObjects.moFeature(geometryType, sGeometry, sAttributes);
                sFeatures.Add(sFeature);
            }
            return sFeatures;
        }

        private static MyMapObjects.moGeometry LoadGeometry(MyMapObjects.moGeometryTypeConstant geometryType, BinaryReader sr)
        {
            if (geometryType == MyMapObjects.moGeometryTypeConstant.Point)
            {
                MyMapObjects.moPoint sPoint = LoadPoint(sr);
                return sPoint;
            }
            else if (geometryType == MyMapObjects.moGeometryTypeConstant.MultiPolyline)
            {
                MyMapObjects.moMultiPolyline sMultiPolyline = LoadMultiPolyline(sr);
                return sMultiPolyline;
            }
            else if (geometryType == MyMapObjects.moGeometryTypeConstant.MultiPolygon)
            {
                MyMapObjects.moMultiPolygon sMultiPolygon = LoadMultiPolygon(sr);
                return sMultiPolygon;
            }
            else
                return null;
        }

        //读取一个点
        private static MyMapObjects.moPoint LoadPoint(BinaryReader sr)
        {
            //原数据支持多点，按照多点读取，然后返回多点的第一个点
            Int32 sPointCount = sr.ReadInt32();
            MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
            for (Int32 i = 0; i <= sPointCount - 1; i++)
            {
                double sX = sr.ReadDouble();
                double sY = sr.ReadDouble();
                MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(sX, sY);
                sPoints.Add(sPoint);
            }
            return sPoints.GetItem(0);
        }

        //读取一个复合折线
        private static MyMapObjects.moMultiPolyline LoadMultiPolyline(BinaryReader sr)
        {
            MyMapObjects.moMultiPolyline sMultiPolyline = new MyMapObjects.moMultiPolyline();
            Int32 sPartCount = sr.ReadInt32();
            for (Int32 i = 0; i <= sPartCount - 1; i++)
            {
                MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
                Int32 sPointCount = sr.ReadInt32();
                for (Int32 j = 0; j <= sPointCount - 1; j++)
                {
                    double sX = sr.ReadDouble();
                    double sY = sr.ReadDouble();
                    MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(sX, sY);
                    sPoints.Add(sPoint);
                }
                sMultiPolyline.Parts.Add(sPoints);
            }
            sMultiPolyline.UpdateExtent();
            return sMultiPolyline;
        }

        //读取一个复合多边形
        private static MyMapObjects.moMultiPolygon LoadMultiPolygon(BinaryReader sr)
        {
            MyMapObjects.moMultiPolygon sMultiPolygon = new MyMapObjects.moMultiPolygon();
            Int32 sPartCount = sr.ReadInt32();
            for (Int32 i = 0; i <= sPartCount - 1; i++)
            {
                MyMapObjects.moPoints sPoints = new MyMapObjects.moPoints();
                Int32 sPointCount = sr.ReadInt32();
                for (Int32 j = 0; j <= sPointCount - 1; j++)
                {
                    double sX = sr.ReadDouble();
                    double sY = sr.ReadDouble();
                    MyMapObjects.moPoint sPoint = new MyMapObjects.moPoint(sX, sY);
                    sPoints.Add(sPoint);
                }
                sMultiPolygon.Parts.Add(sPoints);
            }
            sMultiPolygon.UpdateExtent();
            return sMultiPolygon;
        }

        private static MyMapObjects.moAttributes LoadAttributes(MyMapObjects.moFields fields, BinaryReader sr)
        {
            Int32 sFieldCount = fields.Count;
            MyMapObjects.moAttributes sAttributes = new MyMapObjects.moAttributes();
            for (Int32 i = 0; i <= sFieldCount - 1; i++)
            {
                MyMapObjects.moField sField = fields.GetItem(i);
                object sValue = LoadValue(sField.ValueType, sr);
                sAttributes.Append(sValue);
            }
            return sAttributes;
        }

        private static object LoadValue(MyMapObjects.moValueTypeConstant valueType, BinaryReader sr)
        {
            if (valueType == MyMapObjects.moValueTypeConstant.dInt16)
            {
                Int16 sValue = sr.ReadInt16();
                return sValue;
            }
            else if (valueType == MyMapObjects.moValueTypeConstant.dInt32)
            {
                Int32 sValue = sr.ReadInt32();
                return sValue;
            }
            else if (valueType == MyMapObjects.moValueTypeConstant.dInt64)
            {
                Int64 sValue = sr.ReadInt64();
                return sValue;
            }
            else if (valueType == MyMapObjects.moValueTypeConstant.dSingle)
            {
                float sValue = sr.ReadSingle();
                return sValue;
            }
            else if (valueType == MyMapObjects.moValueTypeConstant.dDouble)
            {
                double sValue = sr.ReadDouble();
                return sValue;
            }
            else if (valueType == MyMapObjects.moValueTypeConstant.dText)
            {
                string sValue = sr.ReadString();
                return sValue;
            }
            else
            {
                return null;
            }
        }

        private static void WriteValue(MyMapObjects.moValueTypeConstant valueType, BinaryWriter bw, object value)
        {
            if (valueType == MyMapObjects.moValueTypeConstant.dInt16)
            {
                Int16 sValue = (Int16) value;
                bw.Write(sValue);
            }
            else if (valueType == MyMapObjects.moValueTypeConstant.dInt32)
            {
                Int32 sValue = (Int32) value;
                bw.Write(sValue);
            }
            else if (valueType == MyMapObjects.moValueTypeConstant.dInt64)
            {
                Int64 sValue = (Int64) value;
                bw.Write(sValue);
            }
            else if (valueType == MyMapObjects.moValueTypeConstant.dSingle)
            {
                float sValue = (float) value;
                bw.Write(sValue);
            }
            else if (valueType == MyMapObjects.moValueTypeConstant.dDouble)
            {
                double sValue = (Double) value;
                bw.Write(sValue);
            }
            else if (valueType == MyMapObjects.moValueTypeConstant.dText)
            {
                string sValue = value.ToString();
                bw.Write(sValue);
            }
            else
            {
                return;
            }
        }

        #endregion
    }
}
