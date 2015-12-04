/*
 * Creado por SharpDevelop.
 * Usuario: pc
 * Fecha: 22/01/2015
 * Hora: 20:42
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */
using System;

namespace Estoc
{
	/// <summary>
	/// Description of Control.
	/// </summary>
	public class Control:Gabriel.Cat.ControlObjectesSql
	{
		public Control():this(new Gabriel.Cat.BaseDeDadesMySQL())
		{
		}
		public Control(Gabriel.Cat.BaseDeDades bd):base(bd)
		{
		}

		#region implemented abstract members of ControlObjectesSql

		public override void Creates()
		 {
            try
            {
                if (BaseDeDades.ConsultaTableDirect("Receptes") == null)
                    throw new Exception("");

            }
            catch { BaseDeDades.ConsultaSQL(Recepta.CreateTable()); }
            try
            {
                if (BaseDeDades.ConsultaTableDirect("Productes") == null)
                    throw new Exception("");
            }
            catch { BaseDeDades.ConsultaSQL(Producte.CreateTable()); }


            try
            {
                if (BaseDeDades.ConsultaTableDirect("UnitatsProductes") == null)
                    throw new Exception("");

            }
            catch { BaseDeDades.ConsultaSQL(UnitatProducte.CreateTable()); }

            try
            {
                if (BaseDeDades.ConsultaTableDirect("Ingredients") == null)
                    throw new Exception("");

            }
            catch { BaseDeDades.ConsultaSQL(Ingredient.CreateTable()); }
        }
		public override void Drops()
		{
            try
            {
                BaseDeDades.ConsultaSQL(Ingredient.DropTable());
            }
            catch { }
            try
            {
                BaseDeDades.ConsultaSQL(UnitatProducte.DropTable());

            }
            catch { }
            try
            {

                BaseDeDades.ConsultaSQL(Producte.DropTable());
            }
            catch { }
            try
            {
                BaseDeDades.ConsultaSQL(Recepta.DropTable());

            }
            catch { }


        }
		public override dynamic Restaurar()
		 {
            Estoc estocDessat = new Estoc();
            try
            {
                BaseDeDades.Desconecta();
                BaseDeDades.Conecta();
                Recepta[] receptes = Recepta.ReceptesDessades(BaseDeDades);
                Producte[] productes = Producte.ProductesDessats(BaseDeDades, receptes);
                UnitatProducte[] unitats = UnitatProducte.UnitatsDessades(BaseDeDades, receptes, productes);
                estocDessat.Afegir(productes);
                //poso els objectes en el control!
                Afegir(receptes);
                foreach (Recepta recepta in receptes)
                	foreach(Ingredient ingredient in recepta)
                		Afegir(ingredient);
                Afegir(productes);
                Afegir(unitats);
            }
            catch { }
            return estocDessat;
        }

		#endregion
	}
}
