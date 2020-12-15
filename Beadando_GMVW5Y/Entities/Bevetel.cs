using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beadando_GMVW5Y.Entities
{
   public class Bevetel : System.Windows.Forms.Label
    {
        public string text { get; set; }
        public Bevetel(string text)
        {
            Height = 30;
            Width = 50;

            this.Text = text;
            TextAlign = ContentAlignment.MiddleCenter;
        }
    }
}
