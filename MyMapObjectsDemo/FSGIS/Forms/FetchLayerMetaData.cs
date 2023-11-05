using FSGIS.SubSystems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FSGIS.Forms
{
    public partial class FetchLayerMetaData : Form
    {
        private string selectLayer = "";

        public string SelectLayer
        {
            get { return selectLayer; }
        }


        public FetchLayerMetaData()
        {
            InitializeComponent();
            FetchLayerMetaDataTools();
        }

        private void FetchLayerMetaDataTools()
        {
            var layerNameTypes = DataBaseTools.GetLayerNamesTypes();
            var layerNames = layerNameTypes.Item1;
            for (int i = 0; i < layerNames.Count; ++i)
            {
                layerList.Items.Add(layerNames[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            selectLayer = layerList.SelectedItem as string;
            this.DialogResult = DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }


    }
}
