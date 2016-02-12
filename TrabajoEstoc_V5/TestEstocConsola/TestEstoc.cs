using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Estoc;
using System.Collections.Generic;
using Gabriel.Cat;
namespace TestEstocConsola
{
    [TestClass]
    public class TestEstoc
    {
      
       static BaseDeDades bdProves = new BaseDeDadesMySQL();
       static Control control = new Control(bdProves);
        #region Producte
        [TestMethod]
        public void DonarDeAltaProducte()
        {

            Producte producte = new Producte("ProvaIDProducte234", "ProvaCategoria", "ProvaUnitat", new TimeSpan(4, 3, 2, 1), new TimeSpan(1, 0, 0, 0), new TimeSpan(1, 2, 3, 4), new Recepta(), 5, 100);
            control.Afegir((ObjecteSql)producte.ReceptaOriginal);//doy de alta la receta sinReceta
            control.Afegir((ObjecteSql)producte);
            Assert.AreEqual(true, bdProves.CompruebaSiFunciona(producte.StringConsultaSql()));//si esta bien!!


        }
        [TestMethod]
        public void DonarDeBaixaProducte()
        {

            Producte producte = new Producte("ProvaIDProducte2234", "ProvaCategoria", "ProvaUnitat", new TimeSpan(4, 3, 2, 1), new TimeSpan(1, 0, 0, 0), new TimeSpan(1, 2, 3, 4), new Recepta(), 5, 100);
            control.Afegir((ObjecteSql)producte.ReceptaOriginal);//doy de alta la receta sinReceta
            control.Afegir((ObjecteSql)producte);//lo doy de alta
            producte.OnBaixa();//lo doy de baja
            Assert.AreEqual(false, bdProves.CompruebaSiFunciona(producte.StringConsultaSql()));//si esta bien!!
        }
        [TestMethod]
        public void ActualitzarDadesProducte()
        {

            Producte producte = new Producte("ProvaIDProducte245", "ProvaCategoria", "ProvaUnitat", new TimeSpan(4, 3, 2, 1), new TimeSpan(1, 0, 0, 0), new TimeSpan(1, 2, 3, 4), new Recepta(), 5, 100);
            control.Afegir((ObjecteSql)producte.ReceptaOriginal);//doy de alta la receta sinReceta
            control.Afegir((ObjecteSql)producte);//doy de alta el producto
            producte.Unitat = "GramsProva";//lo modifico
            control.ComprovaActualitzacions(producte);//actualizo
            Producte producteDessat=null;
            SortedList<string,Producte> indexProductes=new SortedList<string,Producte>();
            foreach(Producte producteD in Producte.ProductesDessats(bdProves))
                indexProductes.Add(producteD.PrimaryKey,producteD);
            try
            {
                producteDessat = indexProductes[producte.PrimaryKey];
            }
            catch {producteDessat = new Producte("NOFUNCIONAAA"); }
            Assert.AreEqual(producte,producteDessat);//si esta bien!!
        }
        [TestMethod]
        public void ActualitzarIdProducte()
        {
            Producte producte = new Producte("ProvaIDProducte2123", "ProvaCategoria", "ProvaUnitat", new TimeSpan(4, 3, 2, 1), new TimeSpan(1, 0, 0, 0), new TimeSpan(1, 2, 3, 4), new Recepta(), 5, 100);
            control.Afegir((ObjecteSql)producte.ReceptaOriginal);
            control.Afegir((ObjecteSql)producte);
            producte.PrimaryKey = "ProvaIDActualitzada";
            control.ComprovaActualitzacions(producte);
            Producte producteDessat=null;
            SortedList<string, Producte> indexProductes = new SortedList<string, Producte>();
            foreach (Producte producteD in Producte.ProductesDessats(bdProves))
                indexProductes.Add(producteD.PrimaryKey, producteD);
            try
            {
                producteDessat = indexProductes[producte.PrimaryKey];
            }
            catch { producteDessat = new Producte("NOFUNCIONAAA"); }
            Assert.AreEqual(producte, producteDessat);//si esta bien!!
        }
        [TestMethod]
        public void FabricaProducteAmbReceptaOriginal()
        {
            Producte pa = new Producte("Pa");
            Producte formatge = new Producte("Formatge");
            Recepta receptaBocata = new Recepta("BocataFormatge");
            Producte bocataFormatge = new Producte("Bocata de Formatge");
            UnitatProducte unitatPa = new UnitatProducte(pa, 10);
            UnitatProducte unitatFormatge = new UnitatProducte(formatge, 15);
            UnitatProducte unitatBocata;
            Estoc.Estoc estoc = new Estoc.Estoc();
            bocataFormatge.ReceptaOriginal = receptaBocata;
            receptaBocata.Afegir(new Ingredient(pa, 1));
            receptaBocata.Afegir(new Ingredient(formatge, 4));
            estoc.Afegir(new Producte[] { pa, formatge, bocataFormatge });
            unitatBocata = Producte.Fabrica(bocataFormatge, estoc);
            Assert.AreNotEqual(null, unitatBocata);


        }
        [TestMethod]
        public void FabricaProducteAmbReceptaDiferent()
        {
            Producte pa = new Producte("Pa");
            Producte formatge = new Producte("Formatge");
            Recepta receptaBocata = new Recepta("BocataFormatge");
            Producte bocataFormatge = new Producte("Bocata de Formatge");
            UnitatProducte unitatPa = new UnitatProducte(pa, 10);
            UnitatProducte unitatFormatge = new UnitatProducte(formatge, 15);
            UnitatProducte unitatBocata;
            Estoc.Estoc estoc = new Estoc.Estoc();
            receptaBocata.Afegir(new Ingredient(pa, 1.5M));
            receptaBocata.Afegir(new Ingredient(formatge, 6));
            estoc.Afegir(new Producte[] { pa, formatge, bocataFormatge });
            unitatBocata = Producte.Fabrica(bocataFormatge, estoc,receptaBocata);
            Assert.AreNotEqual(null, unitatBocata);


        }
        [TestMethod]
        public void FabricaProducteAmbProductesEscollits()
        {
            Producte pa = new Producte("Pa");
            Producte formatge = new Producte("Formatge");
            Recepta receptaBocata = new Recepta("BocataFormatge");
            Producte bocataFormatge = new Producte("Bocata de Formatge");
            UnitatProducte unitatPa = new UnitatProducte(pa, 10);
            UnitatProducte unitatFormatge = new UnitatProducte(formatge, 15);
            UnitatProducte unitatBocata;
            Estoc.Estoc estoc = new Estoc.Estoc();
            receptaBocata.Afegir(new Ingredient(pa, 1.5M));
            receptaBocata.Afegir(new Ingredient(formatge, 6));
            estoc.Afegir(new Producte[] { pa, formatge, bocataFormatge });
            unitatBocata = Producte.Fabrica(bocataFormatge,receptaBocata,new Producte[]{pa,formatge});
            Assert.AreNotEqual(null, unitatBocata);


        }
        [TestMethod]
        public void FabricaElMaximDeProducteAmbProductesEscollits()
        {
            Producte pa = new Producte("Pa");
            Producte formatge = new Producte("Formatge");
            Recepta receptaBocata = new Recepta("BocataFormatge");
            Producte bocataFormatge = new Producte("Bocata de Formatge");
            UnitatProducte unitatPa = new UnitatProducte(pa, 10);
            UnitatProducte unitatFormatge = new UnitatProducte(formatge, 15);
            UnitatProducte[] unitatsBocata;
            Estoc.Estoc estoc = new Estoc.Estoc();
            receptaBocata.Afegir(new Ingredient(pa, 1.5M));
            receptaBocata.Afegir(new Ingredient(formatge, 6));
            estoc.Afegir(new Producte[] { pa, formatge, bocataFormatge });
            unitatsBocata = Producte.FabricaElMax(bocataFormatge, receptaBocata, new Producte[] { pa, formatge });
            Assert.AreNotEqual(new UnitatProducte[] { }, unitatsBocata);


        }
        [TestMethod]
        public void FabricaElMaximDeProducteAmbProductesDeEstoc()
        {
            Producte pa = new Producte("Pa");
            Producte formatge = new Producte("Formatge");
            Recepta receptaBocata = new Recepta("BocataFormatge");
            Producte bocataFormatge = new Producte("Bocata de Formatge");
            UnitatProducte unitatPa = new UnitatProducte(pa, 10);
            UnitatProducte unitatFormatge = new UnitatProducte(formatge, 15);
            UnitatProducte[] unitatsBocata;
            Estoc.Estoc estoc = new Estoc.Estoc();
            receptaBocata.Afegir(new Ingredient(pa, 1.5M));
            receptaBocata.Afegir(new Ingredient(formatge, 6));
            estoc.Afegir(new Producte[] { pa, formatge, bocataFormatge });
            unitatsBocata = Producte.FabricaElMax(bocataFormatge, estoc,receptaBocata);
            Assert.AreNotEqual(new UnitatProducte[]{}, unitatsBocata);


        }
        [TestMethod]
        public void ErrorFabricaProductesInsuficients()
        {
            Producte pa = new Producte("Pa");
            Producte formatge = new Producte("Formatge");
            Recepta receptaBocata = new Recepta("BocataFormatge");
            Producte bocataFormatge = new Producte("Bocata de Formatge");
            UnitatProducte unitatPa = new UnitatProducte(pa, 10);
            UnitatProducte unitatFormatge = new UnitatProducte(formatge, 15);
            UnitatProducte unitatBocata;
            Estoc.Estoc estoc = new Estoc.Estoc();
            receptaBocata.Afegir(new Ingredient(pa, 1.5M));
            receptaBocata.Afegir(new Ingredient(formatge, 50));
            estoc.Afegir(new Producte[] { pa, formatge, bocataFormatge });
            unitatBocata = Producte.Fabrica(bocataFormatge, receptaBocata, new Producte[] { pa, formatge });//por cambiar...se tiene que hacer...
            Assert.AreEqual(null, unitatBocata);


        }
        [TestMethod]
        public void ErrorFabricaProductesFaltants()
        {
            Producte pa = new Producte("Pa");
            Producte formatge = new Producte("Formatge");
            Recepta receptaBocata = new Recepta("BocataFormatge");
            Producte bocataFormatge = new Producte("Bocata de Formatge");
            Producte oli = new Producte("Oli");
            UnitatProducte unitatOli = new UnitatProducte(oli, 100);
            UnitatProducte unitatPa = new UnitatProducte(pa, 10);
            UnitatProducte unitatFormatge = new UnitatProducte(formatge, 15);
            UnitatProducte unitatBocata;
            Estoc.Estoc estoc = new Estoc.Estoc();
            receptaBocata.Afegir(new Ingredient(pa, 1.5M));
            receptaBocata.Afegir(new Ingredient(formatge, 50));
            receptaBocata.Afegir(new Ingredient(oli,10));
            estoc.Afegir(new Producte[] { pa, formatge, bocataFormatge,oli });
            unitatBocata = Producte.Fabrica(bocataFormatge, receptaBocata, new Producte[] { pa, formatge });//por cambiar...se tiene que hacer...
            Assert.AreEqual(null, unitatBocata);


        }
        #endregion

        #region UnitatProducte
        [TestMethod]
        public void DonarDeAltaUnitatProducte()
        {
            UnitatProducte unitat = new UnitatProducte(new Producte("ProvaUnitatProducte"));
            control.Afegir((ObjecteSql)unitat.Producte.ReceptaOriginal);
            control.Afegir((ObjecteSql)unitat.Producte);//doy de alta el producto
            control.Afegir(unitat);//doy de alta la unidad
            Assert.AreEqual(true, bdProves.CompruebaSiFunciona(unitat.StringConsultaSql()));
        }
        [TestMethod]
        public void DonarDeBaixaUnitatDonantDeBaixaElProducte()
        {
            UnitatProducte unitat = new UnitatProducte(new Producte("ProvaUnitatProducte2"));
            control.Afegir((ObjecteSql)unitat.Producte.ReceptaOriginal);
            control.Afegir((ObjecteSql)unitat.Producte);
            control.Afegir(unitat);
            unitat.Producte.OnBaixa();//tiene que dar de baja todas sus unidades 
            Assert.AreEqual(false, bdProves.CompruebaSiFunciona(unitat.StringConsultaSql()));
        }
        [TestMethod]
        public void DonarDeBaixaUnitatProducte()
        {
            UnitatProducte unitat = new UnitatProducte(new Producte("ProvaUnitatProducte2"));
            control.Afegir((ObjecteSql)unitat.Producte.ReceptaOriginal);
            control.Afegir((ObjecteSql)unitat.Producte);
            control.Afegir(unitat);
            unitat.OnBaixa();
            Assert.AreEqual(false, bdProves.CompruebaSiFunciona(unitat.StringConsultaSql()));
        }
        [TestMethod]
        public void ActualitzarDadesUnitatProducte()
        {
            UnitatProducte unitat = new UnitatProducte(new Producte("ProvaUnitatProducte2"));
            control.Afegir((ObjecteSql)unitat.Producte.ReceptaOriginal);
            control.Afegir((ObjecteSql)unitat.Producte);
            control.Afegir(unitat);
            unitat.Quantitat = 223;
            control.ComprovaActualitzacions(unitat);
            UnitatProducte unitatDessada = null;
            SortedList<string, UnitatProducte> indexProductes = new SortedList<string, UnitatProducte>();
            foreach (UnitatProducte producteD in UnitatProducte.UnitatsDessades(bdProves))
                indexProductes.Add(producteD.PrimaryKey, producteD);
            try
            {
                unitatDessada = indexProductes[unitat.PrimaryKey];
            }
            catch {unitatDessada=new UnitatProducte(new Producte("ProvaUnitatProducte2")); }
            Assert.AreEqual(unitat.Quantitat, unitatDessada.Quantitat);//si esta bien!!
        }
        #endregion

        #region Receptes
        [TestMethod]
        public void DonarDeAltaRecepta()
        {
            Recepta recepta = new Recepta("ProvaRecepta");
            control.Afegir((ObjecteSql)recepta);
            Assert.AreEqual(true, bdProves.CompruebaSiFunciona(recepta.StringConsultaSql()));
        }
        [TestMethod]
        public void DonarDeBaixaRecepta()
        {
            Recepta recepta = new Recepta("ProvaRecepta2");
            control.Afegir((ObjecteSql)recepta);
            recepta.OnBaixa();
            Assert.AreEqual(false, bdProves.CompruebaSiFunciona(recepta.StringConsultaSql()));
        }
        [TestMethod]
        public void ActualitzarRecepta()
        {
            Recepta recepta = new Recepta("ProvaRecepta324");
            control.Afegir((ObjecteSql)recepta);
            recepta.PrimaryKey = "ProvaReceptaACtualitzacio";
            control.ComprovaActualitzacions(recepta);
            Assert.AreEqual(true, bdProves.CompruebaSiFunciona(recepta.StringConsultaSql()));
        }
        #endregion

        #region Ingredient
        [TestMethod]
        public void DonarDeAltaIngredient()
        {
            Recepta recepta = new Recepta("ProvaRecepta98");
            Ingredient ingredient = new Ingredient(new Producte("ProvaProducte234"),recepta, 100);
            
            control.Afegir((ObjecteSql)recepta);
            control.Afegir((ObjecteSql)ingredient.Producte.ReceptaOriginal);
            control.Afegir((ObjecteSql)ingredient.Producte);
            control.Afegir((ObjecteSql)ingredient);
            Assert.AreEqual(true, bdProves.CompruebaSiFunciona(ingredient.StringConsultaSql()));
        }
        [TestMethod]
        public void DonarDeBaixaIngredient()
        {
            Recepta recepta = new Recepta("ProvaRecepta56");
            Ingredient ingredient = new Ingredient(new Producte("ProvaProducte25834"), recepta, 100);
            control.Afegir((ObjecteSql)recepta);
            control.Afegir((ObjecteSql)ingredient.Producte.ReceptaOriginal);
            control.Afegir((ObjecteSql)ingredient.Producte);
            control.Afegir((ObjecteSql)ingredient);
            ingredient.OnBaixa();
            Assert.AreEqual(false, bdProves.CompruebaSiFunciona(ingredient.StringConsultaSql()));
        }
        [TestMethod]
        public void DonarDeBaixaIngredientDonantDeBaixaRecepta()
        {
            Recepta recepta = new Recepta("ProvaRecepta423");
            Ingredient ingredient = new Ingredient(new Producte("ProvaProducte69234"), recepta, 100);
            control.Afegir((ObjecteSql)recepta);
            control.Afegir((ObjecteSql)ingredient.Producte.ReceptaOriginal);
            control.Afegir((ObjecteSql)ingredient.Producte);
            control.Afegir((ObjecteSql)ingredient);
            recepta.OnBaixa();
            Assert.AreEqual(false, bdProves.CompruebaSiFunciona(ingredient.StringConsultaSql()));
        }
        [TestMethod]
        public void ActualitzarDadesIngredient()
        {
            Recepta recepta = new Recepta("ProvaRecepta23e");
            Producte producte = new Producte("ProvaProducte2394");
            Ingredient ingredient = new Ingredient(producte, recepta, 100);
            control.Afegir((ObjecteSql)recepta);
            control.Afegir((ObjecteSql)ingredient.Producte.ReceptaOriginal);
            control.Afegir((ObjecteSql)ingredient.Producte);
            control.Afegir((ObjecteSql)ingredient);
            ingredient.Quantitat = 12344;
            control.ComprovaActualitzacions(ingredient);
            Ingredient ingredientDessat = null;
            SortedList<string, Ingredient> indexProductes = new SortedList<string, Ingredient>();
            foreach (Ingredient producteD in Ingredient.IngredientsDessats(bdProves))
                indexProductes.Add(producteD.PrimaryKey, producteD);
            try
            {
                ingredientDessat = indexProductes[ingredient.PrimaryKey];
            }
            catch {ingredientDessat= new Ingredient(producte, recepta, 100); }
            if(ingredientDessat!=null)
            Assert.AreEqual(ingredient.Quantitat, ingredientDessat.Quantitat);//si esta bien!!
            
        }
        [TestMethod]
        public void ActualitzarIdIngredient()
        {
            
            Recepta recepta = new Recepta("ProvaRecepta23e");
            string idAnt;
            Ingredient ingredient = new Ingredient(new Producte("ProvaProducte28934"), recepta, 100);
            idAnt = ingredient.PrimaryKey;
            control.Afegir((ObjecteSql)recepta);
            control.Afegir((ObjecteSql)ingredient.Producte.ReceptaOriginal);
            control.Afegir((ObjecteSql)ingredient.Producte);
            control.Afegir((ObjecteSql)ingredient);
            recepta.PrimaryKey = "NouIDRecepta";
            control.ComprovaActualitzacions(recepta);
            control.ComprovaActualitzacions(ingredient);
            Assert.AreNotEqual(idAnt, ingredient.PrimaryKey);
        }
        #endregion

        [TestMethod]
        public void ErrorAlActualitzarId()
        {
         
            Producte producte = new Producte("ProvaIDProducte29", "ProvaCategoria", "ProvaUnitat", new TimeSpan(4, 3, 2, 1), new TimeSpan(1, 0, 0, 0), new TimeSpan(1, 2, 3, 4), new Recepta(), 5, 100);
            Producte producte2 = new Producte("ProvaIDProducte239", "ProvaCategoria", "ProvaUnitat", new TimeSpan(4, 3, 2, 1), new TimeSpan(1, 0, 0, 0), new TimeSpan(1, 2, 3, 4), new Recepta(), 5, 100);
            control.Afegir((ObjecteSql)new Recepta());
            control.Afegir((ObjecteSql)producte);
            control.Afegir((ObjecteSql)producte2);
            producte2.PrimaryKey = producte.PrimaryKey;
            control.ComprovaActualitzacions(producte2);
            Assert.AreEqual("ProvaIDProducte239", producte2.PrimaryKey);
        }
    }
}
