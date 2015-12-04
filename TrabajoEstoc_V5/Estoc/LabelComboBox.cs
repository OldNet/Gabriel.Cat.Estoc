using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Estoc
{
    internal partial class LabelComboBox : UserControl
    {
        private bool esPotModificar;
        public EventHandler Modificat;

        public LabelComboBox()
        {
            InitializeComponent();
            label1.Location = new Point(0, 0);
            comboBox1.Location = new Point(0, 0);
            EsPotModificar = false;
            comboBox1.SelectedValueChanged += ModificatEvent;
        }

        private void ModificatEvent(object sender, EventArgs e)
        {
            if (Modificat != null)
                Modificat(this, e);
        }
        public bool EsPotModificar
        {
            get { return esPotModificar; }
            set { esPotModificar = value;
            label1.Visible = !esPotModificar;
            comboBox1.Visible = esPotModificar;
            if (esPotModificar)
                comboBox1.SelectedText = label1.Text;
            else
                label1.Text = comboBox1.SelectedText;
                }
        }
        public object[] LlistaObjectes
        {
            get { return comboBox1.Items.OfType<object>().ToArray(); }
            set { comboBox1.Items.Clear(); comboBox1.Items.AddRange(value); }
        }
        public object ObjecteSeleccionat
        {
            get { return comboBox1.SelectedItem; }
            set { comboBox1.SelectedItem = value; }
        }
        private void LabelComboBox_Resize(object sender, EventArgs e)
        {
            if (!label1.Size.Equals(Size))
            {
                label1.Size = label1.PreferredSize;
                comboBox1.Size = label1.Size;
                Size = label1.Size;
            }
        }
    }
}
