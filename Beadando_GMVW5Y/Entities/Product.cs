using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Beadando_GMVW5Y.Entities
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Manufacturer {get;set;}
        public int PurchasedAmount { get; set; }
        public int SaledAmount { get; set; }
        public int AvailableAmount { get; set; }
    }
}
