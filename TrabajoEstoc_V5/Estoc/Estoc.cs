using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Gabriel.Cat;
namespace Estoc
{
	public class Estoc : IEnumerable<Producte>
	{
		SortedList<string, Producte> productes;
		Semaphore semaforAltaBaixa;
		System.Timers.Timer tmrControlEstat;
		public Estoc()
		{
			productes = new SortedList<string, Producte>();
			semaforAltaBaixa = new Semaphore(1, 1);
			tmrControlEstat = new System.Timers.Timer();
			tmrControlEstat.Interval = (int)TempsEnMiliSegons.hora * 6;//cada sis hores mira l'estoc.
			tmrControlEstat.Elapsed += new System.Timers.ElapsedEventHandler(ComprovaProductesEvent);

		}
		public double TempsComprovacioEstat
		{
			get { return tmrControlEstat.Interval; }
			set { tmrControlEstat.Interval = value; }
		}
		public Producte this[string id]
		{
			get
			{
				Producte producte = null;
				if (productes.ContainsKey(id))
					producte = productes[id];
				return producte;
			}
		}
		public void Afegir(IEnumerable<Producte> productes)
		{
			foreach (Producte producte in productes)
				Afegir(producte);
		}
		public void Afegir(Producte producte)
		{
			if (producte != null)
				if (!productes.ContainsKey(producte.PrimaryKey))
			{
				semaforAltaBaixa.WaitOne();
				producte.Alta -= new ObjecteSqlEventHandler(AltaProducteEvent);
				producte.Baixa += new ObjecteSqlEventHandler(BaixaProduceteEvent);
				productes.Add(producte.PrimaryKey, producte);
				semaforAltaBaixa.Release();
			}
		}

		private void BaixaProduceteEvent(ObjecteSql sqlObj)
		{
			Treu(sqlObj as Producte);
		}

		private void AltaProducteEvent(ObjecteSql sqlObj)
		{
			Afegir(sqlObj as Producte);
		}
		public void Treu(IEnumerable<Producte> productes)
		{
			foreach (Producte producte in productes)
				Treu(producte);
		}
		public void Treu(IEnumerable<string> idProductes)
		{
			foreach (string idProducte in idProductes)
				Treu(idProducte);
		}
		public void Treu(Producte producte)
		{
			if (producte != null)
				if (productes.ContainsKey(producte.PrimaryKey))
			{
				semaforAltaBaixa.WaitOne();
				producte.Alta += new ObjecteSqlEventHandler(AltaProducteEvent);
				producte.Baixa -= new ObjecteSqlEventHandler(BaixaProduceteEvent);
				productes.Remove(producte.PrimaryKey);
				semaforAltaBaixa.Release();
			}
		}
		public void Treu(string idProducte)
		{
			if (idProducte != null)
				if (productes.ContainsKey(idProducte))
			{
				semaforAltaBaixa.WaitOne();
				productes[idProducte].Alta += new ObjecteSqlEventHandler(AltaProducteEvent);
				productes[idProducte].Baixa -= new ObjecteSqlEventHandler(BaixaProduceteEvent);
				productes.Remove(idProducte);
				semaforAltaBaixa.Release();

			}
		}
		private void ComprovaProductesEvent(object sender, System.Timers.ElapsedEventArgs e)
		{
			foreach (Producte producte in this)
				producte.ComprovaEstatProductes();
		}


		public IEnumerator<Producte> GetEnumerator()
		{
			List<Producte> productesL = new List<Producte>();
			semaforAltaBaixa.WaitOne();
			foreach (var producte in productes)
				productesL.Add(producte.Value);
			semaforAltaBaixa.Release();
			return productesL.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		public override bool Equals(object obj)
		{
			Estoc other = obj as Estoc;
			bool iguals = other != null;
			IEnumerator<Producte> productes,productesOther;
			bool potPrimer=true, potSegon=false;
			if (iguals)
			{
				productes = GetEnumerator();
				productesOther = other.GetEnumerator();
				while (iguals&&potPrimer)
				{
					potPrimer = productes.MoveNext();
					potSegon = productesOther.MoveNext();
					if (potSegon.Equals(potPrimer))
					{
						if (potSegon)
							iguals = productesOther.Current.Equals(productes.Current);
					}
					else
						iguals = false;

				}
				if (iguals)
					iguals = productes.MoveNext().Equals(productesOther.MoveNext());

			}
			return iguals;
		}
		public override string ToString()
		{
			Gabriel.Cat.text toString = "Productes en estoc\n";
			foreach (Producte producte in this)
			{
				toString +="\n"+ producte;
				toString += "\nEs poden fer: " + producte.EsPodenFer(this);
				toString += "\nCom a maxim pot haver: " + producte.MaximQuePotHaver(this);
			}
			return toString;

		}
	}
}
