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
    public partial class IngredientGrafic : UserControl
    {
        bool esPotEditar;
        Ingredient ingredient;

     

        public IngredientGrafic()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            lblCmbReceptes.Modificat += ModificaReceptaEvent;
            lblCmbProductes.Modificat += ModificaProducteEvent;
            nudQuantitat.ValueChanged += ModificaQuantitat;

        }
        public void DessaCanvis()
        {
            if (ingredient != null)
                ingredient.DessaCanvis();
        }
        public void DonaDeBaixa()
        {
            if (ingredient != null)
                ingredient.OnBaixa();
        }
        private void ModificaQuantitat(object sender, EventArgs e)
        {
            if (ingredient != null)
                ingredient.Quantitat = nudQuantitat.Value;
        }

        private void ModificaProducteEvent(object sender, EventArgs e)
        {
            if (ingredient != null)
            {
                ingredient.Producte = (Producte)lblCmbProductes.ObjecteSeleccionat;
                if (ingredient.Producte.RutaImatge != "")
                    pictureBox1.Image = new Bitmap(ingredient.Producte.RutaImatge);
                else
                    pictureBox1.Image = Resource1.sinFoto;
            }
        }

        private void ModificaReceptaEvent(object sender, EventArgs e)
        {
            if (ingredient != null)
                ingredient.Recepta = (Recepta)lblCmbReceptes.ObjecteSeleccionat;
        }
        public bool Seleccionat
        {
            get { return BackColor.Equals(Color.Blue); }
            set { if (value)BackColor = Color.Blue; else BackColor = SystemColors.Control; }
        }
        public bool EsPotEditar
        {
            get { return esPotEditar; }
            set
            {
                esPotEditar = value;
                lblCmbProductes.EsPotModificar = esPotEditar;
                lblCmbReceptes.EsPotModificar = esPotEditar;
                nudQuantitat.Enabled = esPotEditar;
            }
        }
        public Ingredient Ingredient
        {
            get
            {

                if (ingredient != null)
                {
                    ingredient.Producte = (Producte)lblCmbProductes.ObjecteSeleccionat;
                    ingredient.Recepta = (Recepta)lblCmbReceptes.ObjecteSeleccionat;
                    ingredient.Quantitat = nudQuantitat.Value;
                }
                else
                    try { ingredient = new Ingredient((Producte)lblCmbProductes.ObjecteSeleccionat, (Recepta)lblCmbReceptes.ObjecteSeleccionat, nudQuantitat.Value); }
                    catch { }
                return ingredient;

            }
            set
            {
                ingredient = value;
                if (ingredient != null)
                {
                    lblCmbProductes.ObjecteSeleccionat = ingredient.Producte;
                    lblCmbReceptes.ObjecteSeleccionat = ingredient.Recepta;
                    nudQuantitat.Value = ingredient.Quantitat;
                    if (ingredient.Producte.RutaImatge != "")
                        pictureBox1.Image = new Bitmap(ingredient.Producte.RutaImatge);
                    else
                        pictureBox1.Image = Resource1.sinFoto;
                }
                else
                {
                    lblCmbProductes.ObjecteSeleccionat = null;
                    lblCmbReceptes.ObjecteSeleccionat = null;
                    nudQuantitat.Value = 0M;
                    pictureBox1.Image = Resource1.sinFoto;
                }
            }
        }
    }
}
