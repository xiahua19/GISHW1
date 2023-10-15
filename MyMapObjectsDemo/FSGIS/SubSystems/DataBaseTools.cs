using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace FSGIS.SubSystems
{
    internal static class DataBaseTools
    {
        #region 程序集方法
        /// <summary>
        /// 获取数据库连接对象 (函数内部设置默认的连接参数).
        /// </summary>
        /// <returns>NpgsqlConnection (调用者手动开启关闭)</returns>
        internal static NpgsqlConnection GetConnectionToDB()
        {
            string host = "localhost";
            string port = "5432";
            string database = "postgis_34_sample";
            string username = "postgres";
            string password = "1783";

            string connectionSQL = "Host="+host+
                                ";Port="+port+
                                ";Database="+database+
                                ";Username="+username+
                                ";Password="+password+";";
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionSQL);
            return npgsqlConnection;
        }

        /// <summary>
        /// 获取数据库连接对象 (调用者传入数据库连接参数)
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <param name="database"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>NpgsqlConnection (调用者手动开启关闭)</returns>
        internal static NpgsqlConnection GetConnectionToDB(string host, string port, string database, 
                    string username, string password)
        {
            string connectionSQL = "Host=" + host +
                    ";Port=" + port +
                    ";Database=" + database +
                    ";Username=" + username +
                    ";Password=" + password + ";";
            NpgsqlConnection npgsqlConnection = new NpgsqlConnection(connectionSQL);
            return npgsqlConnection;
        }

        /// <summary>
        /// 读取图层文件
        /// </summary>
        /// <param name="sr">二进制读取对象</param>
        /// <param name="path">图层文件名称（对应表名）</param>
        /// <returns></returns>
        internal static MyMapObjects.moMapLayer LoadMapLayer(NpgsqlConnection npgsqlConnection , string layerName)
        {

            string name = sr.ReadString();
            MyMapObjects.moGeometryTypeConstant sGeometryType = (MyMapObjects.moGeometryTypeConstant)sr.ReadInt32();
            MyMapObjects.moFields sFields = LoadFields(sr);
            MyMapObjects.moFeatures sFeatures = LoadFeatures(sGeometryType, sFields, sr);
            MyMapObjects.moMapLayer sMapLayer = new MyMapObjects.moMapLayer(name, sGeometryType, sFields, path);
            sMapLayer.Features = sFeatures;
            return sMapLayer;
        }
        

        #endregion

        #region 私有函数

        #endregion
    }
}
