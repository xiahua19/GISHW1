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
    public delegate void SetEditLayerHandle(MyMapObjects.moMapLayer layer);
    public partial class SelectEditLayer : Form
    {
        internal event SetEditLayerHandle SetEditLayer;

        private MyMapObjects.moLayers _Layers;

        public SelectEditLayer(MyMapObjects.moLayers layers)
        {
            InitializeComponent();
            _Layers = layers;
            for(int i = 0;i<layers.Count;++i)
            {
                this.comboBox1.Items.Add(layers.GetItem(i).Name);
            }
        }

        private void YES_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex == -1)
                SetEditLayer(null);
            else
                SetEditLayer(this._Layers.GetItem(this.comboBox1.SelectedIndex));
            this.Dispose();
        }

        private void NO_Click(object sender, EventArgs e)
        {
            SetEditLayer(null);
            this.Dispose();
        }
    }
}
