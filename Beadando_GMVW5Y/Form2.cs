using Beadando_GMVW5Y.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beadando_GMVW5Y
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
             int sorszám = 1;
            for (int sor = 0; sor < 5; sor++)
                for (int oszlop = 0; oszlop < 5; oszlop++)
                {
                    ProductPicture pp = new ProductPicture(sorszám.ToString()+".jpg", sor, oszlop);               
                    this.Controls.Add(pp);
                    sorszám++;
                }
        }
    }
}
