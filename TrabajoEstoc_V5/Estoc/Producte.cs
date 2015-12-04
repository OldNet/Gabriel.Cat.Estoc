using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gabriel.Cat;
namespace Estoc
{
	public class Producte : ObjecteSql, IEnumerable<UnitatProducte>
	{
		Recepta receptaOri;
		Semaphore semaforAltaBaixa;
		string infoExtra;
		string rutaImatge;
		string categoria;
		string unitat;
		TimeSpan tempsUnCopFet;
		TimeSpan tempsUnCopObert;
		TimeSpan tempsAvisPerCaducar;
		int quantitatMinimaEstoc;
		int quantitatNormalEstoc;
		SortedList<ulong, UnitatProducte> unitats;
		SortedList<ulong, UnitatProducte> unitatsBaixaProducte;
		#region Constructors
		public Producte(string idProducte)
			: base("Productes", idProducte, "IdProducte")
		{
			semaforAltaBaixa = new Semaphore(1, 1);
			AltaCanvi("IdRecepta");
			AltaCanvi("InfoExtra");
			AltaCanvi("RutaImatge");
			AltaCanvi("Categoria");
			AltaCanvi("Unitat");
			AltaCanvi("TempsUnCopFet");
			AltaCanvi("TempsUnCopObert");
			AltaCanvi("TempsAvisPerCaducar");
			AltaCanvi("QuantitatMinimaEstoc");
			AltaCanvi("QuantitatNormalEstoc");
			receptaOri = new Recepta();
			unitats = new SortedList<ulong, UnitatProducte>();
			unitatsBaixaProducte = new SortedList<ulong, UnitatProducte>();
			tempsAvisPerCaducar = new TimeSpan(3, 0, 0, 0);
			tempsUnCopFet = new TimeSpan();
			tempsUnCopObert = new TimeSpan();
			unitat = "SenseUnitat";
			categoria = "SenseCategoria";
			quantitatMinimaEstoc = 0;
			quantitatNormalEstoc = 0;
			rutaImatge = "";
			infoExtra = "";


		}
		public Producte(string idProducte, string categoria, string unitat)
			: this(idProducte)
		{
			this.categoria = categoria;
			this.unitat = unitat;
		}
		public Producte(string idProducte, string categoria, string unitat, decimal quantitatMinimaEstoc, decimal quantitatNormalEstoc)
			: this(idProducte, categoria, unitat)
		{
			this.quantitatMinimaEstoc = Convert.ToInt32(quantitatMinimaEstoc * 100);
			this.quantitatNormalEstoc = Convert.ToInt32(quantitatNormalEstoc * 100);
		}
		public Producte(string idProducte, string categoria, string unitat, TimeSpan tempsAvisPerCaducar)
			: this(idProducte, categoria, unitat)
		{
			this.tempsAvisPerCaducar = tempsAvisPerCaducar;
		}
		public Producte(string idProducte, string categoria, string unitat, TimeSpan tempsAvisPerCaducar, TimeSpan tempsUnCopObert)
			: this(idProducte, categoria, unitat, tempsAvisPerCaducar)
		{
			this.tempsUnCopObert = tempsUnCopObert;
		}
		public Producte(string idProducte, string categoria, string unitat, TimeSpan tempsAvisPerCaducar, TimeSpan tempsUnCopObert, TimeSpan tempsUnCopFet, Recepta receptaOriginal)
			: this(idProducte, categoria, unitat, tempsAvisPerCaducar, tempsUnCopObert)
		{
			if (receptaOriginal != null)
				this.receptaOri = receptaOriginal;
			else
				this.receptaOri = new Recepta();
			this.tempsUnCopFet = tempsUnCopFet;
		}
		public Producte(string idProducte, string categoria, string unitat, TimeSpan tempsAvisPerCaducar, TimeSpan tempsUnCopObert, decimal quantitatMinimaEstoc, decimal quantitatNormalEstoc)
			: this(idProducte, categoria, unitat, tempsAvisPerCaducar, tempsUnCopObert)
		{
			this.quantitatMinimaEstoc = Convert.ToInt32(quantitatMinimaEstoc * 100);
			this.quantitatNormalEstoc = Convert.ToInt32(quantitatNormalEstoc * 100);
		}
		public Producte(string idProducte, string categoria, string unitat, TimeSpan tempsAvisPerCaducar, TimeSpan tempsUnCopObert, TimeSpan tempsUnCopFet, Recepta receptaOriginal, decimal quantitatMinimaEstoc, decimal quantitatNormalEstoc)
			: this(idProducte, categoria, unitat, tempsAvisPerCaducar, tempsUnCopObert, tempsUnCopFet, receptaOriginal)
		{
			this.quantitatMinimaEstoc = Convert.ToInt32(quantitatMinimaEstoc * 100);
			this.quantitatNormalEstoc = Convert.ToInt32(quantitatNormalEstoc * 100);
		}
		#endregion
		#region Propietats
		public Recepta ReceptaOriginal {
			get { return receptaOri; }
			set {
				receptaOri = value;

				if (receptaOri == null)
					receptaOri = new Recepta();
				CanviString("IdRecepta", receptaOri.PrimaryKey);

			}
		}
		public string InfoExtra {
			get { return infoExtra; }
			set {
				if (value == null)
					value = "";
				infoExtra = value;
				CanviString("InfoExtra", infoExtra);
			}
		}
		public string RutaImatge {
			get {
				string rutaFinal = "";
				Gabriel.Cat.text rt = rutaImatge;
				rt.Replace(';', '\\');

				if (rt != "")
					rutaFinal = Environment.CurrentDirectory + rt;
				if (!File.Exists(rutaFinal))
					rutaFinal = "";

				return rutaFinal;

			}
			set {
				if (value == null)
					value = "";
				rutaImatge = value;
				if (rutaImatge != "")
					RedueixIDessa(rutaImatge);
				CanviString("RutaImatge", rutaImatge);
			}
		}
		public string Categoria {
			get { return categoria; }
			set {
				if (value == null)
					value = "";
				categoria = value;
				CanviString("Categoria", categoria);
			}
		}
		public string Unitat {
			get { return unitat; }
			set {
				if (value == null)
					value = "";
				unitat = value;
				CanviString("Unitat", unitat);
			}
		}
		public TimeSpan TempsUnCopFet {
			get { return tempsUnCopFet; }
			set {
				if (value == null)
					value = new TimeSpan();
				tempsUnCopFet = value;
				CanviTimeSpan("TempsUnCopFet", tempsUnCopFet);
			}
		}
		public TimeSpan TempsUnCopObert {
			get { return tempsUnCopObert; }
			set {
				if (value == null)
					value = new TimeSpan();
				tempsUnCopObert = value;
				CanviTimeSpan("TempsUnCopObert", tempsUnCopObert);
			}
		}
		public TimeSpan TempsAvisPerCaducar {
			get { return tempsAvisPerCaducar; }
			set {
				if (value == null)
					value = new TimeSpan();
				tempsAvisPerCaducar = value;
				CanviTimeSpan("TempsAvisPerCaducar", tempsAvisPerCaducar);
			}
		}
		public decimal QuantitatMinimaEstoc {
			get { return (quantitatMinimaEstoc / 100.0M); }
			set {
				if (value < 0)
					value = 0;
				quantitatMinimaEstoc = Convert.ToInt32(value * 100);
				CanviNumero("QuantitatMinimaEstoc", quantitatMinimaEstoc + "");
			}
		}
		public decimal QuantitatNormalEstoc {
			get { return (quantitatNormalEstoc / 100.0M); }
			set {
				if (value < 0)
					value = 0;
				quantitatNormalEstoc = Convert.ToInt32(value * 100);
				CanviNumero("QuantitatNormalEstoc", quantitatNormalEstoc + "");
			}
		}
		public int UnitatsFetesAmbLaRecepteOriginal {
			get {
				int unitats = 0;
				foreach (UnitatProducte unitat in this)
					if (unitat.Recepta.Equals(ReceptaOriginal))
						unitats++;
				return unitats;
			}
		}
		public int UnitatsNoFetesAmbLaRecepteOriginal {
			get
			{ return unitats.Count - UnitatsFetesAmbLaRecepteOriginal; }
		}
		public decimal QuantitatReceptaOriginal {
			get {
				decimal total = 0;
				foreach (UnitatProducte unitat in this)
					if (unitat.Recepta.Equals(ReceptaOriginal))
						total += unitat.Quantitat;
				return total;
			}
		}
		public decimal QuantitatNoFetaAmbReceptaOriginal {
			get {
				decimal total = 0;
				foreach (UnitatProducte unitat in this)
					if (!unitat.Recepta.Equals(ReceptaOriginal))
						total += unitat.Quantitat;
				return total;
			}
		}
		public decimal QuantitatTotal {
			get { return QuantitatReceptaOriginal + QuantitatNoFetaAmbReceptaOriginal; }
		}
		#endregion
		#region Metodes
		internal void AltaUnitat(UnitatProducte unitat)
		{
			if (unitat != null) {
				if (!unitats.ContainsKey(unitat.IdIntern)) {
					semaforAltaBaixa.WaitOne();
					unitats.Add(unitat.IdIntern, unitat);
					semaforAltaBaixa.Release();
				}
			}
		}
		internal void BaixaUnitat(UnitatProducte unitat)
		{
			if (unitat != null) {
				if (unitats.ContainsKey(unitat.IdIntern)) {
					semaforAltaBaixa.WaitOne();
					unitats.Remove(unitat.IdIntern);
					semaforAltaBaixa.Release();
				}
			}
		}
		public override void OnAlta()
		{

			base.OnAlta();
			foreach (var unitat in unitatsBaixaProducte)
				unitat.Value.OnAlta();
			semaforAltaBaixa.WaitOne();
			unitatsBaixaProducte.Clear();
			semaforAltaBaixa.Release();
		}
		public override void OnBaixa()
		{
			semaforAltaBaixa.WaitOne();
			unitatsBaixaProducte.Clear();
			semaforAltaBaixa.Release();
			foreach (UnitatProducte unitat in this) {
				unitat.OnBaixa();
				unitatsBaixaProducte.Add(unitat.IdIntern, unitat);
			}
			base.OnBaixa();


		}
		public void DessaCanvisUnitatsProducte()
		{
			foreach (UnitatProducte unitat in this)
				unitat.DessaCanvis();
		}
		public void ComprovaEstatProductes()
		{
			foreach (UnitatProducte unitat in this)
				unitat.ComprovaEstat();
		}
		public UnitatProducte[] UnitatsApuntDeCaducar()
		{
			List<UnitatProducte> unitats = new List<UnitatProducte>();
			foreach (UnitatProducte unitat in this)
				if (unitat.ComprovaEstat().Equals(UnitatProducte.Estat.ApuntDeCaducar))
					unitats.Add(unitat);
			return unitats.ToArray<UnitatProducte>();
		}
		public UnitatProducte[] UnitatsCaducades()
		{
			List<UnitatProducte> unitats = new List<UnitatProducte>();
			foreach (UnitatProducte unitat in this)
				if (unitat.ComprovaEstat().Equals(UnitatProducte.Estat.Caducat))
					unitats.Add(unitat);
			return unitats.ToArray<UnitatProducte>();
		}
		public void DonaDeBaixaUnitatsCaducades()
		{
			foreach (UnitatProducte unitatCaducada in UnitatsCaducades())
				BaixaUnitat(unitatCaducada);
		}

        public override string StringInsertSql(TipusBaseDeDades tipusBD)
        {
            //en este caso no importa el tipo de BD
            string sentencia = null;
            if (ComprovacioSiEsPot())
            {
                sentencia = "insert into " + Taula + " (" + CampPrimaryKey + ",categoria,unitat,idRecepta,rutaImatge,infoExtra,quantitatMinimaEstoc,quantitatNormalEstoc,tempsAvisPerCaducar,tempsUnCopObert,tempsUnCopFet) values(";
                sentencia += "'" + PrimaryKey + "',";
                sentencia += "'" + categoria + "',";
                sentencia += "'" + unitat + "',";
                sentencia += "'" + receptaOri.PrimaryKey + "',";
                sentencia += "'" + rutaImatge + "',";
                sentencia += "'" + infoExtra + "',";
                sentencia += quantitatMinimaEstoc + ",";
                sentencia += quantitatNormalEstoc + ",";
                sentencia += ObjecteSql.TimeSpanToString(tempsAvisPerCaducar) + ",";
                sentencia += ObjecteSql.TimeSpanToString(tempsUnCopObert) + ",";
                sentencia += ObjecteSql.TimeSpanToString(tempsUnCopFet) + ");";

            }
            return sentencia;
        }
		public IEnumerator<UnitatProducte> GetEnumerator()
		{
			List<UnitatProducte> unitatsL = new List<UnitatProducte>();

			semaforAltaBaixa.WaitOne();
			foreach (var unitat in unitats)
				unitatsL.Add(unitat.Value);
			semaforAltaBaixa.Release();
			return unitatsL.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		private void RedueixIDessa(string rutaImatge)
		{
			if (File.Exists(rutaImatge)) {
				var img = new System.Drawing.Bitmap(rutaImatge);
				var imgR = gabriel.cat.Imagen.Escala(img, Convert.ToDecimal((img.Height % 100) / 100.0));
				if (!Directory.Exists(Environment.CurrentDirectory + "\\imatges"))
					Directory.CreateDirectory(Environment.CurrentDirectory + "\\imatges");
				rutaImatge = "\\imatges\\" + PrimaryKey + ".png";
				imgR.Save(Environment.CurrentDirectory + rutaImatge, System.Drawing.Imaging.ImageFormat.Png);
				Gabriel.Cat.text rt = rutaImatge;
				rt.Replace('\\', ';');
				this.rutaImatge = rt;
			} else
				rutaImatge = "";

		}
		public decimal EsPodenFer(Estoc estoc)
		{
			return EsPodenFer(estoc, ReceptaOriginal);
		}
		public decimal EsPodenFer(Estoc estoc, Recepta recepta)
		{
			decimal quantiatMin = Decimal.MaxValue;
			decimal quantiatAct = 0;
			if (ReceptaOriginal.PrimaryKey != new Recepta().PrimaryKey) {
				try {
					foreach (Ingredient ingredient in recepta) {
						quantiatAct = estoc[ingredient.Producte.PrimaryKey].QuantitatTotal / ingredient.Quantitat;
						if (quantiatAct < quantiatMin)
							quantiatMin = quantiatAct;
					}
				} catch {
					quantiatMin = 0;
				}
			} else
				quantiatMin = 0;
			return quantiatMin;
		}
		public decimal MaximQuePotHaver(Estoc estoc)
		{
			return MaximQuePotHaver(estoc, ReceptaOriginal);
		}

		public decimal MaximQuePotHaver(Estoc estoc, Recepta recepta)
		{
			return QuantitatTotal + EsPodenFer(estoc, recepta);
		}
		#endregion
		#region Metodes de Clase
		public static string CreateTable()
		{
			string create = "Create table Productes (";
			create += "idProducte varchar(100) primary key,";
			create += "categoria varchar(100),";
			create += "Unitat varchar(100),";
			create += "idRecepta varchar(100)  references Receptes(IdRecepta),";
			create += "RutaImatge varchar(200),";
			create += "InfoExtra varchar(200),";
			create += "quantitatMinimaEstoc integer,";
			create += "quantitatNormalEstoc integer,";
			create += "tempsAvisPerCaducar varchar(20),";
			create += "tempsUnCopObert varchar(20),";
			create += "tempsUnCopFet varchar(20));";

			return create;
		}
		public static Producte[] ProductesDessats(Gabriel.Cat.BaseDeDades baseDeDades)
		{
			return ProductesDessats(baseDeDades, Recepta.ReceptesDessades(baseDeDades));
		}
		public static Producte[] ProductesDessats(Gabriel.Cat.BaseDeDades baseDeDades, Recepta[] receptes)
		{
			List<Producte> productes = new List<Producte>();

			SortedList<string, Recepta> indexReceptes = new SortedList<string, Recepta>();
			for (int i = 0; i < receptes.Length; i++)
				indexReceptes.Add(receptes[i].PrimaryKey, receptes[i]);
			string[,] taula = baseDeDades.ConsultaTableDirect("Productes");
			if (taula != null)
				for (int i = 1; i < taula.GetLength(1); i++)
					try {//por mirar
				productes.Add(new Producte(taula[0, i], taula[1,i], taula[2, i], ObjecteSql.StringToTimeSpan(taula[8, i]), ObjecteSql.StringToTimeSpan(taula[9, i]), ObjecteSql.StringToTimeSpan(taula[10, i]), indexReceptes[taula[3, i]], Convert.ToDecimal(taula[6, i]) / 100M, Convert.ToDecimal(taula[7, i]) / 100M) {
				              	rutaImatge = taula[4, i],
				              	infoExtra = taula[5, i]
				              });
				foreach (Ingredient ingredient in indexReceptes[taula[3,i]])
					if (ingredient.IdProductePerPosar == productes[productes.Count - 1].PrimaryKey) {
					ingredient.Producte = productes[productes.Count - 1];
					ingredient.DessaCanvis();
				}
			} catch {
			}
			return productes.ToArray<Producte>();
		}
		public static string DropTable()
		{
			return "drop table Productes;";
		}
		public static UnitatProducte Fabrica(Producte producte, Estoc estoc)
		{
			return Fabrica(producte, estoc, producte.ReceptaOriginal);
		}
		public static UnitatProducte[] FabricaElMax(Producte producte, Estoc estoc)
		{
			List<UnitatProducte> unitatsFetes = new List<UnitatProducte>();
			UnitatProducte unitatFeta = Fabrica(producte, estoc, producte.ReceptaOriginal);
			while (unitatFeta != null) {
				unitatsFetes.Add(unitatFeta);
				unitatFeta = Fabrica(producte, estoc, producte.ReceptaOriginal);
			}
			return unitatsFetes.ToArray<UnitatProducte>();
		}
		public static UnitatProducte Fabrica(Producte producte, Estoc estoc, Recepta recepta)
		{
			List<Producte> productesRecepta = new List<Producte>();
			Producte producteR = null;
			try {
				if (recepta.PrimaryKey == new Recepta().PrimaryKey)
					throw new Exception();
				foreach (Ingredient ingredient in recepta) {
					producteR = estoc[ingredient.Producte.PrimaryKey];
					if (producteR == null)
						throw new Exception();
					productesRecepta.Add(producteR);
				}
				
			} catch {
				return null;
			}
			return Fabrica(producte, recepta, productesRecepta);
		}
		public static UnitatProducte[] FabricaElMax(Producte producte, Estoc estoc, Recepta recepta)
		{
			List<UnitatProducte> unitatsFetes = new List<UnitatProducte>();
			UnitatProducte unitatFeta = Fabrica(producte, estoc, recepta);
			while (unitatFeta != null) {
				unitatsFetes.Add(unitatFeta);
				unitatFeta = Fabrica(producte, estoc, recepta);
			}
			return unitatsFetes.ToArray<UnitatProducte>();
		}
		public static UnitatProducte Fabrica(Producte producte, Recepta recepta, IEnumerable<Producte> productesPerFerLo)
		{
			UnitatProducte unitatFeta = null;
			SortedList<string,Producte> productesIndex = new SortedList<string,Producte>();
			foreach (Producte productePerFer in productesPerFerLo)
				try {
				productesIndex.Add(productePerFer.PrimaryKey, productePerFer);
			} catch {
			}
			bool esPotFer = true;
			bool acabat = false;
			IEnumerator<Ingredient> ingredients = recepta.GetEnumerator();
			while (esPotFer && !acabat) {

				acabat = !ingredients.MoveNext();
				if (!acabat) {
					esPotFer = productesIndex.ContainsKey(ingredients.Current.Producte.PrimaryKey);
					if (esPotFer)
						esPotFer = (productesIndex[ingredients.Current.Producte.PrimaryKey].QuantitatTotal / ingredients.Current.Quantitat) >= 1;
				}
			}
			if (esPotFer) {
				foreach (Ingredient ingredient in recepta)
					productesIndex[ingredient.Producte.PrimaryKey].TreuQuantitat(ingredient.Quantitat);
				unitatFeta = new UnitatProducte(producte, 1M, new DateTime(), new DateTime(), recepta);
			}
			return unitatFeta;
		}
		public static UnitatProducte[] FabricaElMax(Producte producte, Recepta recepta, IEnumerable<Producte> productesPerFerLo)
		{
			List<UnitatProducte> unitatsFetes = new List<UnitatProducte>();
			UnitatProducte unitatFeta = Fabrica(producte, recepta, productesPerFerLo);
			while (unitatFeta != null) {
				unitatsFetes.Add(unitatFeta);
				unitatFeta = Fabrica(producte, recepta, productesPerFerLo);
			}
			return unitatsFetes.ToArray<UnitatProducte>();
		}

		//falta el metodo dado unos productos fabricar todo lo que se pueda con ellos
		public Decimal TreuQuantitat(Decimal quantitatATreure)
		{
			IEnumerator<UnitatProducte> unitats = this.GetEnumerator();
			while (quantitatATreure > 0 && unitats.MoveNext())
				quantitatATreure = TreuQuantitat(unitats.Current, quantitatATreure);
			return quantitatATreure;
		}
		public Decimal TreuQuantitat(string id, Decimal quantitatATreure)
		{
			return  TreuQuantitat(this[id], quantitatATreure);
		}
		public Decimal TreuQuantitat(UnitatProducte unitat, Decimal quantitatATreure)
		{
			if (unitat.Quantitat <= quantitatATreure) {
				quantitatATreure -= unitat.Quantitat;
				unitat.Quantitat = 0;
			} else {
				unitat.Quantitat -= quantitatATreure;
				quantitatATreure = 0;
			}
			return quantitatATreure;
		}
		public UnitatProducte this[string id] {
			get {
				UnitatProducte unitat = null;
				IEnumerator<UnitatProducte> unitats = this.GetEnumerator();
				while (unitat == null && unitats.MoveNext())
					if (unitats.Current.PrimaryKey == id)
						unitat = unitats.Current;
				return unitat;
			}
		}
		#endregion
		#region Overrides
		public override bool Equals(object obj)
		{
			Producte other = obj as Producte;
			bool iguals = other != null;
			bool pucPrimer = true;
			bool pucSegon = false;
			IEnumerator<UnitatProducte> unitats;
			IEnumerator<UnitatProducte> unitatsOther;
			if (iguals) {//per no fer una super linea la he dividit :)
				iguals = other.PrimaryKey == PrimaryKey && other.QuantitatMinimaEstoc == QuantitatMinimaEstoc && other.QuantitatNormalEstoc == QuantitatNormalEstoc;
				if (iguals) {
					iguals = other.ReceptaOriginal.Equals(ReceptaOriginal) && other.rutaImatge == rutaImatge && other.TempsAvisPerCaducar.Equals(TempsAvisPerCaducar) && other.TempsUnCopFet.Equals(TempsUnCopFet) && other.TempsUnCopObert.Equals(TempsUnCopObert);
					if (iguals) {
						iguals = other.Unitat == Unitat && other.Categoria == Categoria && other.InfoExtra == InfoExtra;
					}
					if (iguals) {
						unitats = this.GetEnumerator();
						unitatsOther = other.GetEnumerator();
						while (iguals && pucPrimer) {
							pucPrimer = unitats.MoveNext();
							pucSegon = unitatsOther.MoveNext();
							if (pucSegon.Equals(pucPrimer)) {
								if (pucPrimer)
									iguals = unitatsOther.Current.Equals(unitats.Current);
							} else
								iguals = false;

						}
						if (iguals)
							iguals = unitats.MoveNext().Equals(unitatsOther.MoveNext());
					}
				}
			}
			return iguals;
		}
		public override string ToString()
		{
			Gabriel.Cat.text toString = "";
			toString += "\nId producte: " + PrimaryKey;
			toString += "\nTaula SQL: " + Taula;
			toString += "\nCategoria: " + Categoria;
			toString += "\nUnitat: " + Unitat;
			toString += "\nRuta Imatge: " + RutaImatge;
			toString += "\nInformació EXTRA: " + InfoExtra;
			toString += "\nRecepta: " + ReceptaOriginal.PrimaryKey;
			toString += "\nQuantitat minima en estoc: " + QuantitatMinimaEstoc;
			toString += "\nQuantitat normal en estoc: " + QuantitatNormalEstoc;
			toString += "\nTemps avis per caducar: " + TempsAvisPerCaducar;
			toString += "\nTemps un cop Obert: " + TempsUnCopObert;
			toString += "\nTemps un cop Fet: " + TempsUnCopFet;
			toString += "\nUnitats fetes amb la recepta original: " + UnitatsFetesAmbLaRecepteOriginal;
			toString += "\nQuantitat: " + QuantitatReceptaOriginal;
			toString += "\nUnitats no fetes amb la recepta original: " + UnitatsNoFetesAmbLaRecepteOriginal;
			toString += "\nQuantitat: " + QuantitatNoFetaAmbReceptaOriginal;
			toString += "\nUnitats Total: " + unitats.Count;
			toString += "\nQuantitat total: " + QuantitatTotal;
			return toString;
		}
		#endregion





    }
}
