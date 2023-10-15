using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FSGIS.Forms
{
    internal delegate void CreateNewLayerHandle(string newLayerName, MyMapObjects.moGeometryTypeConstant newLayerType);
    public partial class NewLayerFile : Form
    {
        internal event CreateNewLayerHandle createNewLayer;
        private string newLayerName = "";
        private MyMapObjects.moGeometryTypeConstant newLayerType = MyMapObjects.moGeometryTypeConstant.NotDefined;

        public NewLayerFile()
        {
            InitializeComponent();
        }


        private void YES_Click(object sender, EventArgs e)
        {
            newLayerType = ParseTypeString(this.layerType.Text);
            newLayerName = this.layerName.Text;

            if (newLayerType == MyMapObjects.moGeometryTypeConstant.NotDefined)
            {
                MessageBox.Show("类型不得为空", "参数提示", MessageBoxButtons.OK);
                return;
            }
            if (newLayerName.Length == 0)
            {
                MessageBox.Show("名称不得为空", "参数提示", MessageBoxButtons.OK);
                return;
            }

            createNewLayer(newLayerName, newLayerType);
            this.Dispose();
        }

        private void NO_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private MyMapObjects.moGeometryTypeConstant ParseTypeString(string s)
        {
            if (s == "点") return MyMapObjects.moGeometryTypeConstant.Point;
            else if (s == "线") return MyMapObjects.moGeometryTypeConstant.MultiPolyline;
            else if (s == "面") return MyMapObjects.moGeometryTypeConstant.MultiPolygon;
            else return MyMapObjects.moGeometryTypeConstant.NotDefined;
        }
    }
}
