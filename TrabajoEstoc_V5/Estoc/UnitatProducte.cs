using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gabriel.Cat;
namespace Estoc
{
	public delegate void UnitatEventHandler(UnitatProducte unitat);
	public class UnitatProducte : ObjecteSql
	{
		Producte producte;
		DateTime dataCaducitat;
		int quantitat;
		DateTime dataQueEsVaObrir;
		Recepta recepta;
		public event UnitatEventHandler UnitatCaducada;
		public event UnitatEventHandler UnitatApuntDeCaducar;
		public enum Estat
		{
			Normal, ApuntDeCaducar, Caducat
		}
		#region Constructors
		public UnitatProducte(Producte producte)
		{
			if (producte == null)
				throw new Exception("La unitatProducte Ha de tenir un producte associat!! ");
			AltaCanvi("DataObertura");
			AltaCanvi("DataCaducitat");
			AltaCanvi("IdProducte");
			AltaCanvi("IdRecepta");
			AltaCanvi("Quantitat");
			this.producte = producte;
			dataCaducitat = new DateTime();
			dataQueEsVaObrir = new DateTime();
			recepta = new Recepta();
			CampPrimaryKey = "IdUnitat";
			PrimaryKey = producte.PrimaryKey + ";unitat";//algo para hacerlo unico...quizas se autoGenera y no hace falta...
			Taula = "UnitatsProductes";
			producte.AltaUnitat(this);

		}
		public UnitatProducte(Producte producte, decimal quantitat)
			: this(producte)
		{ this.quantitat = Convert.ToInt32(quantitat*100); }
		public UnitatProducte(Producte producte, decimal quantitat, DateTime dataCaducitat)
			: this(producte, quantitat)
		{ this.dataCaducitat = dataCaducitat; }
		public UnitatProducte(Producte producte, decimal quantitat, DateTime dataCaducitat, DateTime dataObertura)
			: this(producte, quantitat, dataCaducitat)
		{ this.dataQueEsVaObrir = dataObertura; }
		public UnitatProducte(Producte producte, decimal quantiat, Recepta recepta)
			: this(producte, quantiat)
		{ this.recepta = recepta; }
		public UnitatProducte(Producte producte, decimal quantiat, DateTime dataCaducitat, DateTime dataObertura, Recepta recepta)
			: this(producte, quantiat, dataCaducitat, dataObertura)
		{
			if (recepta != null)
				this.recepta = recepta;
			else
				this.recepta = new Recepta();
		}
		#endregion
		#region Propietats
		public bool Obert
		{
			get { return !dataQueEsVaObrir.Equals(new DateTime()); }
			set
			{
				if (value)
					DataQueEsVaObrir = DateTime.Now;
				else
					DataQueEsVaObrir = new DateTime();
			}
		}
		public decimal Quantitat
		{
			get { return Convert.ToDecimal(quantitat/100.0); }
			set
			{

				quantitat = Convert.ToInt32(value*100);
				if (quantitat > 0)
					CanviString("Quantitat", quantitat + "");
				else
					OnBaixa();


			}
		}
		public Recepta Recepta
		{
			get { return recepta; }
			set
			{
				recepta = value;
				if (recepta == null)
					recepta = new Recepta();
				CanviString("IdRecepta", recepta.PrimaryKey);

			}
		}
		public Producte Producte
		{
			get { return producte; }
			set
			{
				if (value == null)
					throw new Exception("La unitatProducte Ha de tenir un producte associat!! ");
				producte = value;
				CanviString("IdProducte", producte.PrimaryKey);
			}
		}
		public DateTime DataCaducitat
		{
			get { return dataCaducitat; }
			set
			{
				dataCaducitat = value;
				CanviData("DataCaducitat", dataCaducitat);
			}
		}
		public DateTime DataQueEsVaObrir
		{
			get { return dataQueEsVaObrir; }
			set
			{
				dataQueEsVaObrir = value;
				CanviData("DataObertura", dataQueEsVaObrir);
			}
		}
		public bool FetAmbReceptaOriginal
		{ get { return producte.ReceptaOriginal.PrimaryKey == Recepta.PrimaryKey; } }
		private string IdRecepta
		{
			get { return recepta.PrimaryKey; }
		}
		#endregion
		#region Metodes
		public override void OnAlta()
		{
			base.OnAlta();
			producte.AltaUnitat(this);
		}
		public override void OnBaixa()
		{
			base.OnBaixa();
			producte.BaixaUnitat(this);
		}
		public Estat ComprovaEstat()
		{


			bool estaBe = true;
			Estat estat = Estat.Normal;
			TimeSpan tempsEstaBe;
			if (Obert)
			{
				estaBe = DateTime.Now - DataQueEsVaObrir < producte.TempsUnCopObert;
				if (!estaBe)
				{
					if (UnitatCaducada != null)
						UnitatCaducada(this);
					estat = Estat.Caducat;
				}
			}
			else
			{
				tempsEstaBe = DataCaducitat - DateTime.Now;
				if (tempsEstaBe < new TimeSpan())
				{
					if (UnitatCaducada != null)
						UnitatCaducada(this);
					estaBe = false;
					estat = Estat.Caducat;
				}
				else if (tempsEstaBe < producte.TempsAvisPerCaducar)
				{
					if (UnitatApuntDeCaducar != null)
						UnitatApuntDeCaducar(this);
					estat = Estat.ApuntDeCaducar;
				}
			}

			return estat;
		}
        
        public override string StringInsertSql(TipusBaseDeDades tipusBD)
        {
            string sentencia = null;
            if (ComprovacioSiEsPot())
            {
                sentencia = "insert into " + Taula + "(IdProducte,IdRecepta,Quantitat,DataCaducitat,DataObertura) values(";
                sentencia += "'" + producte.PrimaryKey + "',";
                sentencia += "'" + IdRecepta + "',";
                sentencia += "'" + quantitat + "',";
                sentencia += ObjecteSql.DateTimeToStringSQL(tipusBD,DataCaducitat) + ",";
                sentencia += ObjecteSql.DateTimeToStringSQL(tipusBD, DataQueEsVaObrir) + ");";
            }
            return sentencia;
        }
		#endregion
		#region Overrides
		public override bool Equals(object obj)
		{
			UnitatProducte unitat = obj as UnitatProducte;
			bool iguals = unitat != null;
			if (iguals)
			{
				iguals = unitat.DataCaducitat.Equals(DataCaducitat) && unitat.DataQueEsVaObrir.Equals(DataQueEsVaObrir) && unitat.IdRecepta == IdRecepta && unitat.Obert.Equals(Obert) && unitat.Producte.PrimaryKey.Equals(Producte.PrimaryKey) && unitat.Quantitat == Quantitat && unitat.Recepta.Equals(Recepta) && unitat.Taula == Taula;
			}
			return iguals;
		}
		public override string ToString()
		{
			Gabriel.Cat.text toString = "";
			toString += "\nId unitat: " + PrimaryKey;
			toString += "\nId producte: " + Producte.PrimaryKey;
			toString += "\nId recepta: " + Recepta.PrimaryKey+" recepta";
			if (!FetAmbReceptaOriginal)
				toString += " " + " NO ";
			toString += " original";
			toString += "\nQuantitat: " + Quantitat;
			toString += "\nData Caducitat: " + DataCaducitat;
			toString += "\nData Que es va obrir: ";
			if (Obert)
				toString += DataQueEsVaObrir;
			return toString;
		}
		#endregion
		#region Metodes de clase
		public static UnitatProducte[] UnitatsDessades(Gabriel.Cat.BaseDeDades baseDeDades)
		{
			Recepta[] receptesDessades = Recepta.ReceptesDessades(baseDeDades);
			return UnitatsDessades(baseDeDades, receptesDessades, Producte.ProductesDessats(baseDeDades, receptesDessades));
		}
		public static UnitatProducte[] UnitatsDessades(Gabriel.Cat.BaseDeDades baseDeDades, Recepta[] receptes, Producte[] productes)
		{
			string[,]	taula = baseDeDades.ConsultaTableDirect("UnitatsProductes");
			List<UnitatProducte> unitatsProductes = new List<UnitatProducte>();
			SortedList<string, Recepta> indexRecepta = new SortedList<string, Recepta>();
			SortedList<string, Producte> indexProducte = new SortedList<string, Producte>();
			for (int i = 0; i < receptes.Length; i++)
				indexRecepta.Add(receptes[i].PrimaryKey, receptes[i]);

			for (int i = 0; i < productes.Length; i++)
				indexProducte.Add(productes[i].PrimaryKey, productes[i]);
			foreach(var recepta in indexRecepta)
				foreach(var ingredient in recepta.Value)
					try
			{
				if (indexProducte.ContainsKey(ingredient.IdProductePerPosar))
					ingredient.Producte = indexProducte[ingredient.IdProductePerPosar];
			}
			catch { }
			if (taula != null)
				for (int i = 1; i < taula.GetLength(1); i++)
			{//por mirar
				unitatsProductes.Add(new UnitatProducte(indexProducte[taula[1,i]], Convert.ToDecimal(taula[3,i])/100M, ObjecteSql.StringToDateTime(taula[4,i]), ObjecteSql.StringToDateTime(taula[5,i]), indexRecepta[taula[2,i]]));
				unitatsProductes[unitatsProductes.Count - 1].PrimaryKey = taula[0,i];
				unitatsProductes[unitatsProductes.Count - 1].DessaCanvis();
				
			}
			return unitatsProductes.ToArray<UnitatProducte>();
		}
		public static string CreateTable()
		{
			string create = "create table UnitatsProductes (";
			create += "IdUnitat MEDIUMINT primary key AUTO_INCREMENT ,";
			create += "IdProducte varchar(100) References Productes(IdProducte),";
			create += "IdRecepta varchar(100) References Receptes(IdRecepta),";
			create += "Quantitat integer,";
			create += "DataCaducitat varchar(20),";
			create += "DataObertura varchar(20));";
			return create;
		}
		public static string DropTable()
		{ return "drop table UnitatsProductes;"; }
		#endregion

    }
}
