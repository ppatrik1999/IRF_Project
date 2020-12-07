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
                        Name = line[1],
                        Manufacturer = line[2],
                        PurchasedAmount = int.Parse(line[3]),
                        SaledAmount = int.Parse(line[4]),
                        AvailableAmount = int.Parse(line[5])
                    });
                }
            }
            return store;
        }
        
        
    }
}
