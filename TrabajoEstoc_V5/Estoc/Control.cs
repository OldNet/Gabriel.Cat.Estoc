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
    public class Control : Gabriel.Cat.ControlObjectesSql
    {
        static readonly string[] creates = new string[] { Recepta.CreateTable(), Producte.CreateTable(), UnitatProducte.CreateTable(), Ingredient.CreateTable() };
        public Control() : this(new Gabriel.Cat.BaseDeDadesMySQL())
        {
        }
        public Control(Gabriel.Cat.BaseDeDades bd) : base(bd, creates)
        {
        }

        #region implemented abstract members of ControlObjectesSql

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
                for (int i = 0; i < receptes.Length; i++)
                    foreach (Ingredient ingredient in receptes[i])
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
