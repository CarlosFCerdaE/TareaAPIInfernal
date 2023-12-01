using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Model;

namespace DAL.Clases
{
    public class APISecurity
    {
        private static  APISeguridadEntities DB = new APISeguridadEntities();

        public static Nullable<bool> UsuarioValidar(string Usuario, string Clave)
        {
            var result = DB.UsuarioValidar(Usuario, Clave).FirstOrDefault();
            return result;
        }
    }
}