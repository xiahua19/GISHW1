建立数据库连接
    Host, Port:
    Password:
    Database name:

工程文件相关内容
    1. 修改工程文件格式
        数据库连接信息
        图层列表信息
    2. 读取工程文件
        DataIOTools.LoadMapProj
    3. 保存工程文件
    4. 另存工程文件
        DataIOTools.WriteLayersToProj

图层相关内容
    1. 修改图层数据格式
        moMapLayer需要添加一个字段来记录对应数据库存储的表名
        由文件存储转为数据库表存储
    2. 新建图层文件
        MainForm.CreateNewLayerFile
        DataIOTools.WriteLayerToFile
    3. 添加图层文件
        添加图层文件ToolStripMenuItem_Click
        DataIOTools.LoadMapLayer
    4. 保存图层文件
        DataIOTools.WriteLayerToFile

主要修改在DataIOTools下面的公共接口以及其调用的各个私有函数。
