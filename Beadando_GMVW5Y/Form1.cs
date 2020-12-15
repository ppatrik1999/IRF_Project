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
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Office.Interop.Excel;
using Font = System.Drawing.Font;

namespace Beadando_GMVW5Y
{
    public partial class Form1 : Form
    {
        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlSheet;

        BindingList<Product> Store = new BindingList<Product>();
        List<Product> AvailableProducts = new List<Product>();
        List<Product> NotAvailableProducts = new List<Product>();
        public Form1()
        {
            InitializeComponent();
            Store = GetStore(@"C:\Users\Patrik\source\repos\IRF_Project\Beadando_GMVW5Y\CSVproduct\termék.csv");
            GetDgw();
            button1.Text = "Hiánycikkek megtekintése Excel-ben";
            button2.Text = "Készleten lévő termékek megjelenítése";
            button3.Text = "Termék kép nézegető";
            GetNot();
            Osszegkiir(GetBevetel());
            label1.Text = "Összes bevétel:";
        }
        private void GetRemove()
        {
            for (int i = 0; i < Store.Count; i++)
            {
                if (Store[i].Elérhető_db == 0)
                {
                    Store.RemoveAt(i);
                }
            }
            dataGridView1.Refresh();
        }

        public void GetDgw()
        {
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.DataSource = Store;
            dataGridView1.AutoSize = true;
            dataGridView1.BackgroundColor = Color.LightGray;
            dataGridView1.BorderStyle = BorderStyle.Fixed3D;

            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.Blue;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.TopLeft;
            dataGridView1.EnableHeadersVisualStyles = false;
        }

        public BindingList<Product> GetStore(string csvpath)
        {
            BindingList<Product> store = new BindingList<Product>();

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
                        Beszerzett_db = int.Parse(line[3]),
                        Eladott_db = int.Parse(line[4]),
                        Elérhető_db = int.Parse(line[5]),
                        Egységár = int.Parse(line[6])
                    });
                }
            }
            return store;
        }
        public void GetNot()
        {
            //NotAvailableProducts.Clear();

            foreach (var s in Store)
            {
                if (s.Elérhető_db == 0)
                {
                    NotAvailableProducts.Add(s);
                }
                else
                {
                    AvailableProducts.Add(s);
                }
            }
        }


        public void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application();
                xlSheet = xlWB.ActiveSheet;
                CreateTable();
                xlApp.Visible = true;
                xlApp.UserControl = true;
            }
            catch (Exception ex)
            {
                string errMsg = string.Format("Error: {0}\nLine: {1}", ex.Message, ex.Source);
                MessageBox.Show(errMsg, "Error");
                xlWB.Close(false, Type.Missing, Type.Missing);
                xlApp.Quit();
                xlWB = null;
                xlApp = null;
            }
        }
        public void CreateTable()
        {
            string[] headers = new string[]
            {
            "Terméknév",
            };
            object[,] values = new object[NotAvailableProducts.Count, headers.Length];
            int counter = 0;
            foreach (var s in NotAvailableProducts)
            {
                values[counter, 0] = s.Név;
                counter++;
            }

            xlSheet.get_Range(
            GetCell(2, 1),
            GetCell(1 + values.GetLength(0), values.GetLength(1))).Value2 = values;
            xlSheet.Cells[1, 1] = headers[0];

            Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 30;
            headerRange.Font.Color = Color.Black;
            headerRange.Interior.Color = Color.DarkOrange;
            headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);

            Excel.Range dataRange = xlSheet.get_Range(GetCell(2, 1), GetCell(1 + values.GetLength(0), values.GetLength(1)));
            dataRange.Font.Color = Color.Blue;
            dataRange.Font.Italic = true;
            dataRange.EntireColumn.AutoFit();
            dataRange.Interior.Color = Color.Orange;
        }
        public string GetCell(int x, int y)
        {
            string ExcelCoordinate = "";
            int dividend = y;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                ExcelCoordinate = Convert.ToChar(65 + modulo).ToString() + ExcelCoordinate;
                dividend = (int)((dividend - modulo) / 26);
            }
            ExcelCoordinate += x.ToString();

            return ExcelCoordinate;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GetRemove();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("A következő Excel-ben, azok a termékek láthatóak, melyekből berendelés szükséges, mert hiánycikk a vállalatnál.");
            CreateExcel();
            CreateTable();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2();
            f2.Show();
        }

        private int GetBevetel()
        {
            int osszeg = 0;
            for (int i = 0; i < AvailableProducts.Count; i++)
            {
                osszeg += Store[i].Egységár * Store[i].Eladott_db;
            }
            return osszeg;
        }

        public void Osszegkiir(int osszeg)
        {
            Bevetel b = new Bevetel(osszeg.ToString());
            panel1.Controls.Add(b);
        }
    }
    }

    
           
        
        
    


