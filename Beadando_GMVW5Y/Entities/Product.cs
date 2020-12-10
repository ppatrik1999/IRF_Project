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
        public string Név { get; set; }
        public string Gyártó {get;set;}
        public int Beszerzett_mennyiség { get; set; }
        public int Eladott_mennyiség { get; set; }
        public int Elérhető_mennyiség { get; set; }
        public int Egységár { get; set; }
    }
}
