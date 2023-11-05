using NetTopologySuite.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FSGIS.SubSystems
{
    internal static class UnitTest
    {
        // pass
        internal static void TestGetConnectionToDB()
        {
            DataBaseTools.GetConnectionToDB();
        }

        // pass
        internal static void TestGetLayerNameTypes()
        {
            var layerNameTypes = DataBaseTools.GetLayerNamesTypes();
            var layerNames = layerNameTypes.Item1;
            var layerTypes = layerNameTypes.Item2;

            MessageBox.Show(layerTypes.Count.ToString());
            MessageBox.Show(layerNames.Count.ToString());

            string info = "";
            for (int i = 0; i < layerNames.Count; i++)
            {
                info += layerNames[i] + " " + layerTypes[i] + " ";
            }

            MessageBox.Show(info);
        }

        // partly pass, need to think how to write geom in multilinestring, multipolygon.
        internal static void TestWriteLayerToFile(MyMapObjects.moMapLayer mapLayer) 
        {
            try
            {
                DataBaseTools.WriteLayerToFile(mapLayer);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        internal static MyMapObjects.moMapLayer TestLoadLayer(string layerName)
        {
            try
            {
               return DataBaseTools.LoadMapLayer(layerName);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }
    }
}
