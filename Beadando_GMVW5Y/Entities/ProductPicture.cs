using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beadando_GMVW5Y.Entities
{
        public class ProductPicture : PictureBox
        {
            public ProductPicture(string kep, int sor, int oszlop)
            {
                this.Load(kep);
                this.Height = this.Image.Height;
                this.Width = this.Image.Width;
                this.Top = this.Image.Height * sor;
                this.Left = this.Image.Width * oszlop;
            }


        }
    }
