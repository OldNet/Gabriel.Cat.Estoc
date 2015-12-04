using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Estoc;
using System.Threading;
using Gabriel.Cat;
namespace TestConsola
{
	class Program
	{

		static void Main(string[] args)
		{
			BaseDeDadesMySQL bd = new BaseDeDadesMySQL();
			bd.Conecta();
			ControlObjectesSql control = new Control(bd);
			
			var estoc = control.Restaurar();
			Estoc.Estoc estocAct = new Estoc.Estoc();
			control.Reset();
			Producte producte = new Producte("ProvaIDProducte", "ProvaCategoria", "ProvaUnitat", new TimeSpan(4, 3, 2, 1), new TimeSpan(1, 0, 0, 0), new TimeSpan(1, 2, 3, 4), new Recepta(), 5, 100);
			UnitatProducte unitat = new UnitatProducte(producte);
			UnitatProducte unitat2 = new UnitatProducte(producte);
			unitat2.Recepta = new Recepta("Altre");
			control.Afegir((ObjecteSql)unitat2.Recepta);
			unitat2.Recepta.Afegir(new Ingredient(producte, 100M));
			control.Afegir((IEnumerable<Ingredient>)unitat2.Recepta);
			unitat2.Quantitat = 12.3M;
			unitat.Quantitat = 10;
			control.Afegir((ObjecteSql)producte.ReceptaOriginal);
			control.Afegir((ObjecteSql)producte);
			control.Afegir(unitat);
			control.Afegir(unitat2);
			control.ComprovaActualitzacions();
			estocAct.Afegir(producte);
			if (estocAct.Equals(estoc))
				Console.WriteLine("Funciona");//si esta bien!!
			else
				Console.WriteLine("No funciona");

			
			estoc = control.Restaurar();
			if (estocAct.Equals(estoc))
				Console.WriteLine("Funciona");//si esta bien!!
			else
				Console.WriteLine("No funciona");
			//Console.WriteLine(unitat2);
			//Console.WriteLine(unitat2.Recepta);
			//Console.WriteLine(producte);
			//Console.WriteLine(estoc);
			//Console.WriteLine(control);
			Console.ReadKey();
			
		}

	}
}
