using Beadando_GMVW5Y.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Beadando_GMVW5Y
{
    public partial class Form1 : Form
    {
        List<Product> Store = new List<Product>();
        List<Product> AvailableProducts = new List<Product>();
        List<Product> NotAvailableProducts = new List<Product>();
        public Form1()
        {
            InitializeComponent();
            Store = GetStore("termék.csv");
            dataGridView1.DataSource = Store;
        }

        public List<Product> GetStore(string csvpath)
        {
            List<Product> store = new List<Product>();
           
            using (StreamReader sr = new StreamReader(csvpath, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine().Split(';');
                    store.Add(new Product()
                    {
                        ID = int.Parse(line[0]),
                        Név = line[1],
                        Gyártó = line[2],
                        Beszerzett_mennyiség = int.Parse(line[3]),
                        Eladott_mennyiség = int.Parse(line[4]),
                        Elérhető_mennyiség = int.Parse(line[5]),
                        Egységár = int.Parse(line[6])
                    });
                }
            }
            return store;
        }

        public void GetDelete()
        {
            AvailableProducts.Clear();
            NotAvailableProducts.Clear();
            
            foreach (var s in Store)
            {
                if (s.Elérhető_mennyiség == 0 )
                {
                    NotAvailableProducts.Add(s);                  
                }
                else
                {
                    AvailableProducts.Add(s);
                }
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            GetDelete();
        }
    }
}
