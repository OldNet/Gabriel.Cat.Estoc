using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Gabriel.Cat;
namespace Estoc
{
	public class Recepta : ObjecteSql, IEnumerable<Ingredient>
	{
		private SortedList<ulong, Ingredient> ingredients;
		SortedList<ulong, Ingredient> ingredientsBaixaRecepta;
		Semaphore semaforAltaBaixa;
		public Recepta(string idRecepta)
			: base("Receptes", idRecepta, "IdRecepta")
		{
			ingredients = new SortedList<ulong, Ingredient>();
			ingredientsBaixaRecepta = new SortedList<ulong, Ingredient>();
			semaforAltaBaixa = new Semaphore(1, 1);

		}
		public Recepta()
			: this("SenseRecepta")
		{
		}
		public decimal this[Ingredient ingredient] {
			get {
				try {
					return ingredients[ingredient.IdIntern].Quantitat;
				} catch {
					return -1M;
				}
			}
		}
		public void Afegir(IEnumerable<Ingredient> ingredients)
		{
			if (ingredients != null)
				foreach (Ingredient ingredient in ingredients)
					Afegir(ingredient);
		}
		public void Afegir(Ingredient ingredient)
		{
			if (ingredient != null)
			if (!ingredients.ContainsKey(ingredient.IdIntern)) {

				if (ingredient.Recepta == null || ingredient.Recepta.IdIntern != IdIntern) {
					ingredient.Baixa -= new ObjecteSqlEventHandler(BaixaEvent);
					semaforAltaBaixa.WaitOne();
					ingredients.Add(ingredient.IdIntern, ingredient);
					semaforAltaBaixa.Release();
					ingredient.Recepta = this;
					ingredient.Alta -= new ObjecteSqlEventHandler(AltaEvent);
					ingredient.Baixa += new ObjecteSqlEventHandler(BaixaEvent);
				}



			}
		}
		public override string PrimaryKey {
			get {
				return base.PrimaryKey;
			}
			set {
				base.PrimaryKey = value;
				OnActualitzat();
			}
		}
		private void AltaEvent(ObjecteSql sqlObj)
		{
			Afegir(sqlObj as Ingredient);
		}

		private void BaixaEvent(ObjecteSql sqlObj)
		{
			Treu(sqlObj as Ingredient);
		}
		public override void OnBaixa()
		{
			semaforAltaBaixa.WaitOne();
			ingredientsBaixaRecepta.Clear();
			semaforAltaBaixa.Release();
			foreach (Ingredient ingredient in this) {
				ingredient.OnBaixa();
				var ing = ingredient;
				ingredientsBaixaRecepta.Add(ing.IdIntern, ing);
			}
			base.OnBaixa();
			
		}
		public override void OnAlta()
		{
			base.OnAlta();
			foreach (var ingredient in ingredientsBaixaRecepta)
				ingredient.Value.OnAlta();
			semaforAltaBaixa.WaitOne();
			ingredientsBaixaRecepta.Clear();
			semaforAltaBaixa.Release();
		}
		public void Treu(IEnumerable<Ingredient> ingredients)
		{
			if (ingredients != null)
				foreach (Ingredient ingredient in ingredients)
					Treu(ingredient);
		}
		public void Treu(Ingredient ingredient)
		{
			if (ingredient != null)
			if (ingredients.ContainsKey(ingredient.IdIntern)) {
				if (ingredient.Recepta != null && ingredient.Recepta.IdIntern == IdIntern) {
					semaforAltaBaixa.WaitOne();
					ingredients.Remove(ingredient.IdIntern);
					semaforAltaBaixa.Release();
					ingredient.Recepta = null;
					ingredient.Alta += new ObjecteSqlEventHandler(AltaEvent);
					ingredient.Baixa -= new ObjecteSqlEventHandler(BaixaEvent);
				}


			}
		}
        public override string StringInsertSql(TipusBaseDeDades tipusBD)
        {
            string sentencia = null;
            if (ComprovacioSiEsPot())
                sentencia = "insert into " + Taula + "(" + CampPrimaryKey + ")values('" + PrimaryKey + "');";
            return sentencia;
        }
		public IEnumerator<Ingredient> GetEnumerator()
		{
			List<Ingredient> ingredientsL = new List<Ingredient>();
			semaforAltaBaixa.WaitOne();
			foreach (var ingredientKV in ingredients)
				ingredientsL.Add(ingredientKV.Value);
			semaforAltaBaixa.Release();
			return ingredientsL.GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		public override bool Equals(object obj)
		{
			Recepta recepta = obj as Recepta;
			bool iguals = recepta != null;
			bool potPrimer = true, potSegon = false;
			IEnumerator<Ingredient> ingredients = null, ingredientsAComparar = null;
			if (iguals) {
				iguals = PrimaryKey == recepta.PrimaryKey;
				if (iguals) {
					ingredients = this.GetEnumerator();
					ingredientsAComparar = recepta.GetEnumerator();
					while (iguals && potPrimer) {
						potPrimer = ingredients.MoveNext();
						potSegon = ingredientsAComparar.MoveNext();
						if (potSegon.Equals(potPrimer)) {
							if (potPrimer)
								iguals = ingredients.Current.Equals(ingredientsAComparar.Current);
						} else
							iguals = false;
					}
					if (iguals)
						iguals = ingredients.MoveNext().Equals(ingredientsAComparar.MoveNext());
				}
			}
			return iguals;
		}
		public override string ToString()
		{
			Gabriel.Cat.text toString = "";
			toString += "\nId recepta: " + PrimaryKey;
			toString += "\nIngredients:";
			if (ingredients.Count != 0) {
				foreach (Ingredient ingredient in this)
					toString += "\n" + ingredient;

			} else
				toString += "\nNo te ingredients";
			return toString;
		}
		#region Metodes de clase
		public static Recepta[] ReceptesDessades(Gabriel.Cat.BaseDeDades baseDeDades)
		{
			return ReceptesDessades(baseDeDades, Ingredient.IngredientsDessats(baseDeDades));
		}

		private static Recepta[] ReceptesDessades(Gabriel.Cat.BaseDeDades baseDeDades, Ingredient[] ingredients)
		{
			string[,] taula = baseDeDades.ConsultaTableDirect("Receptes");
			SortedList<string, List<Ingredient>> indexIngredients = new SortedList<string, List<Ingredient>>();
			List<Recepta> receptes = new List<Recepta>();
			for (int i = 0; i < ingredients.Length; i++) {
				if (!indexIngredients.ContainsKey(ingredients[i].IdReceptaPerPosar))
					indexIngredients.Add(ingredients[i].IdReceptaPerPosar, new List<Ingredient>());
				indexIngredients[ingredients[i].IdReceptaPerPosar].Add(ingredients[i]);
			}
			if (taula != null)
				for (int i = 1; i < taula.GetLength(1); i++) {
					receptes.Add(new Recepta(taula[0, i]));//por mirar
					try {
						receptes[receptes.Count - 1].Afegir(indexIngredients[receptes[receptes.Count - 1].PrimaryKey]);
						foreach (Ingredient ingredient in indexIngredients[receptes[receptes.Count - 1].PrimaryKey])
							ingredient.DessaCanvis();
					} catch {
					}
				}
			return receptes.ToArray<Recepta>();
		}
		public static string CreateTable()
		{
			return "create table Receptes(IdRecepta varchar(100)primary key)";

		}
		public static string DropTable()
		{
			return "drop table Receptes;";
		}
		public static Recepta DonamRecepta(string id, IEnumerable<Ingredient> ingredients)
		{
			Recepta recepta = new Recepta(id);
			recepta.Afegir(ingredients);
			return recepta;
		}
		#endregion


    }
}
