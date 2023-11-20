using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCRUDapi.Models
{
    public class CreditCardCLS
    {
        public int CreditCardID { get; set; }
        public string CardType { get; set; }
        public string CardNumber { get; set; }
        public int ExpMonth { get; set; }
        public int ExpYear { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}