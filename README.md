# 建立数据库连接√
    连接参数：
        Host, Port
        Password
        Database name
    接口：
        DataBaseTools.GetConnectionToDB

# 图层相关内容

    1. 修改图层数据格式：文件-->数据库
       | Attributes | Geometry | Symbol |
    2. 新建图层文件
        NewLayerFile <-- MainForm.CreateNewLayerFile(layerPath, layerType) to (layerName, layerType)
        
        DataIOTools.WriteLayerToFile(layer, layerPath) to (layer, layerName)
    
    3. 添加图层文件
        添加图层文件ToolStripMenuItem_Click
        DataIOTools.LoadMapLayer(binaryReader, layerPath) to (npgsqlconnection, layerName)
    
    4. 保存图层文件
        DataIOTools.WriteLayerToFile

# 工程文件相关内容

    1. 修改工程文件格式
        数据库连接信息
        图层列表信息

    2. 读取工程文件
        DataIOTools.LoadMapProj

    3. 保存工程文件

    4. 另存工程文件
        DataIOTools.WriteLayersToProj

主要修改在DataIOTools下面的公共接口以及其调用的各个私有函数。
