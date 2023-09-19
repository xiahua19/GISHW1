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
    internal delegate void CreateNewLayerHandle(string newLayerPath, MyMapObjects.moGeometryTypeConstant newLayerType);
    public partial class NewLayerFile : Form
    {
        internal event CreateNewLayerHandle createNewLayer; 
        private string newLayerPath="";
        private MyMapObjects.moGeometryTypeConstant newLayerType=MyMapObjects.moGeometryTypeConstant.NotDefined;

        public NewLayerFile()
        {
            InitializeComponent();
        }


        private void openFile_Click(object sender, EventArgs e)
        {

            SaveFileDialog sFileDialog = new SaveFileDialog();
            sFileDialog.Title = "新建一个图层文件";
            sFileDialog.Filter = "LAY文件(*.lay)|*.lay";
            sFileDialog.RestoreDirectory = true;
            if (sFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                newLayerPath = sFileDialog.FileName;
                this.layerPath.Text = newLayerPath;
                sFileDialog.Dispose();
            }
            else
            {
                sFileDialog.Dispose();
                return;
            }


        }

        private void YES_Click(object sender, EventArgs e)
        {
            newLayerType = ParseTypeString(this.layerType.Text);
            if (newLayerType == MyMapObjects.moGeometryTypeConstant.NotDefined)
            {
                MessageBox.Show("类型不得为空", "参数提示", MessageBoxButtons.OK);
                return;
            }
            if (newLayerPath.Length == 0)
            {
                MessageBox.Show("路径不得为空", "参数提示", MessageBoxButtons.OK);
                return;
            }

            createNewLayer(newLayerPath, newLayerType);
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
