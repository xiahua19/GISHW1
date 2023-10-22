# 建立数据库连接 
    连接参数：
        Host, Port
        Password
        Database name
    接口：
        DataBaseTools.GetConnectionToDB（两个版本）✅

# 图层相关内容
    1. 修改图层数据格式：文件-->数据库 ✅
       | Attributes | Geometry | 
       为减少修改量，建议把Symbol数据存储到工程文件中
    2. 写入图层文件/新建图层文件
       - NewLayerFile <-- MainForm.CreateNewLayerFile(layerPath, layerType) to (layerName, layerType)：删除界面中的选择layer路径的下拉框，修改为填写layer名字的文本框 ✅
       - DataIOTools.WriteLayerToFile(layer, layerPath) to (layer) ✅
    3. 添加图层文件
        添加图层文件ToolStripMenuItem_Click：函数逻辑需要改为从数据库中查询可用表
        DataIOTools.LoadMapLayer(binaryReader, layerPath) to (layerName)：根据DataIOTools.WriteLayerToFile中数据在表格中的存储逻辑，解析得到layer对应的数据 ✅
    
    4. 保存图层文件
        DataIOTools.WriteLayerToFile (同上2) ✅

# 工程文件相关内容

    1. 修改工程文件格式
        原来的工程文件存储列表信息是列表的路径，现在需要修改为在数据库中对应的表名。如果Symbol仍然在工程文件中存储的话，其余部分不必修改。✅

        以下两个接口需要按照修改后的工程文件格式进行对应的修改。

    2. 读取工程文件
        DataIOTools.LoadMapProj ✅

    3. 保存工程文件
        DataIOTools.WriteLayersToProj ✅
    
    4. 另存工程文件
        DataIOTools.WriteLayersToProj ✅

# Unit Test in `DataBaseTools.cs`
```C#
// pass
internal static NpgsqlConnection GetConnectionToDB()
```

```C#
// pass
internal static Tuple<List<string>, List<string>> GetLayerNamesTypes()
```

```C#
internal static MyMapObjects.moMapLayer LoadMapLayer(string layerName)
```

```C#
// partly pass, need to think how to write geom
internal static void WriteLayerToFile(MyMapObjects.moMapLayer layer)
```

```C#
internal static MyMapObjects.moLayers LoadMapProj(BinaryReader sr, string path)
```

```C#
internal static void WriteLayersToProj(MyMapObjects.moLayers layers, string projName)
```
