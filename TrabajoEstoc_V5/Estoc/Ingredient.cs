using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.Cat;
namespace Estoc
{
	public class Ingredient : ObjecteSql
	{
		Recepta recepta;
		int quantitat;
		Producte producte;
		string idProductePerPosar;
		string idReceptaPerPosar;
		internal Ingredient(string idProducte, string idRecepta, decimal quantitat)
			: this(null, quantitat)
		{
			idProductePerPosar = idProducte;
			idReceptaPerPosar = idRecepta;
			PrimaryKey = idRecepta + ";" + idProducte;
		}
		public Ingredient(Producte producte, decimal quantitat)
		{
			CampPrimaryKey = "IdIngredient";
			AltaCanvi("IdProducte");
			AltaCanvi("IdRecepta");
			AltaCanvi("Quantitat");
			this.producte = producte;
			Taula = "Ingredients";
			this.producte = producte;
			this.quantitat = Convert.ToInt32(quantitat * 100);
			Recepta = null;
			DessaCanvis();

		}
		public Ingredient(Producte producte, Recepta recepta, decimal quantitat)
			: this(producte, quantitat)
		{
			Recepta = recepta;
			DessaCanvis();
		}
		internal string IdReceptaPerPosar {
			get {
				return idReceptaPerPosar;
			}
		}
		internal string IdProductePerPosar {
			get { return idProductePerPosar; }
		}
		public decimal Quantitat {
			get { return Convert.ToDecimal(quantitat / 100.0); }
			set {
				quantitat = Convert.ToInt32(value * 100);
				CanviNumero("Quantitat", quantitat + "");
			}
		}
		public Recepta Recepta {
			get { return recepta; }
			set {


				if (recepta != null) {
					recepta.Actualitzat -= new ObjecteSqlEventHandler(IdReceptaCanviatEvent);
					recepta.Treu(this);
					PrimaryKey = IdIntern + ";" + producte.PrimaryKey;

				}

				if (value != null) {

					value.Afegir(this);
					if (!Equals(recepta, value))
						recepta = value;
				} else
					recepta = null;
				if (recepta != null) {
					recepta.Actualitzat -= new ObjecteSqlEventHandler(IdReceptaCanviatEvent);
					PrimaryKey = recepta.PrimaryKey + ";" + producte.PrimaryKey;
					CanviString("IdRecepta", recepta.PrimaryKey);
					recepta.Actualitzat += new ObjecteSqlEventHandler(IdReceptaCanviatEvent);
					
				} else
					CanviString("IdRecepta", "");


			}
		}


		public Producte Producte {
			get { return producte; }
			set {
				if (value == null)
					throw new Exception("Ha de ser d'un producte l'ingredient");
				if (producte != null)
					producte.Actualitzat -= new ObjecteSqlEventHandler(IdProducteCanviatEvent);
				producte = value;
				producte.Actualitzat += new ObjecteSqlEventHandler(IdProducteCanviatEvent);
				if (Recepta == null)
					PrimaryKey = IdIntern + ";" + producte.PrimaryKey;
				else
					PrimaryKey = recepta.PrimaryKey + ";" + producte.PrimaryKey;
				CanviString("IdProducte", producte.PrimaryKey);
			}
		}
		private void IdReceptaCanviatEvent(ObjecteSql sqlObj)
		{
			if (PrimaryKey.Contains(';'))
				if (PrimaryKey.Split(';')[0] != sqlObj.PrimaryKey)
					PrimaryKey = recepta.PrimaryKey + ";" + producte.PrimaryKey;
		}
		private void IdProducteCanviatEvent(ObjecteSql sqlObj)
		{
			if (PrimaryKey.Contains(';'))
				if (PrimaryKey.Split(';')[1] != sqlObj.PrimaryKey) {
				if (Recepta != null)
					PrimaryKey = recepta.PrimaryKey + ";" + producte.PrimaryKey;
				else
					PrimaryKey = IdIntern + ";" + producte.PrimaryKey;
			}
		}
		protected override bool ComprovacioSiEsPot()
		{
			return base.ComprovacioSiEsPot() && Recepta != null && Producte != null;
		}
        public override string StringInsertSql(TipusBaseDeDades tipusBD)
        {	//en este caso el tipo no importa...
            string sentencia = null;
			if (ComprovacioSiEsPot()) {
				sentencia = "Insert into " + Taula + "(" + CampPrimaryKey + ",IdRecepta,IdProducte,Quantitat) values(";
				sentencia += "'" + PrimaryKey + "',";
				sentencia += "'" + recepta.PrimaryKey + "',";
				sentencia += "'" + producte.PrimaryKey + "',";
				sentencia += quantitat + ");";
			}
			return sentencia;
        }

		public override bool Equals(object obj)
		{
			Ingredient other = obj as Ingredient;
			bool iguals = other != null;
			if (iguals) {
				iguals = other.PrimaryKey == PrimaryKey && other.Producte.PrimaryKey.Equals(Producte.PrimaryKey) && other.Quantitat == Quantitat && other.Recepta.PrimaryKey == Recepta.PrimaryKey && other.Taula == Taula;
			}
			return iguals;
		}
		public override string ToString()
		{
			Gabriel.Cat.text toString = "";
			toString += "\nId ingredient: " + PrimaryKey;
			toString += "\nId recepta: ";
			if (Recepta == null)
				toString += IdReceptaPerPosar;
			else
				toString += Recepta.PrimaryKey;
			toString += "\nId producte: ";
			if (Producte == null)
				toString += IdProductePerPosar;
			else
				toString += Producte.PrimaryKey;
			toString += "\nQuantitat: " + Quantitat;
			return toString;

		}
		public static Ingredient[] IngredientsDessats(Gabriel.Cat.BaseDeDades baseDeDades)
		{
			List<Ingredient> ingredients = new List<Ingredient>();
			string[,] taula = baseDeDades.ConsultaTableDirect("Ingredients");
			Ingredient ingredientDessat;
			if (taula != null)
				for (int i = 1; i < taula.GetLength(1); i++) {
				ingredientDessat = new Ingredient(taula[2, i], taula[1, i], Convert.ToDecimal(taula[3, i]) / 100M);//por mirar
				if (ingredientDessat.PrimaryKey == "")
					throw new Exception("FATAL ERROR");
				ingredients.Add(ingredientDessat);
			}
			return ingredients.ToArray<Ingredient>();

		}
		public static string CreateTable()
		{
			return "create table Ingredients(IdIngredient varchar(201)primary key,IdRecepta varchar(100) References Receptes(IdRecepta),IdProducte varchar(100) References Productes(IdProducte),Quantitat integer);";
		}
		public static string DropTable()
		{
			return "drop table Ingredients;";
		}


    }
}
