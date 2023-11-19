using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIServiceConsumption.Models
{
    public class LibroCLS
    {
        public string Isbn { get; set; }
        public string Nombre { get; set; }
        public string Editorial { get; set; }
        public string Autor { get; set; }
    }
}