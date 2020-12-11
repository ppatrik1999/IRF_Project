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

namespace Beadando_GMVW5Y
{
    public partial class Form1 : Form
    {
        Excel.Application xlApp;
        Excel.Workbook xlWB;
        Excel.Worksheet xlSheet;

        List<Product> Store = new List<Product>();
        List<Product> AvailableProducts = new List<Product>();
        List<Product> NotAvailableProducts = new List<Product>();
        public Form1()
        {
            InitializeComponent();
            Store = GetStore("termék.csv");
            dataGridView1.DataSource = Store;
            button1.Text = ("Hiánycikkek megtekintése Excel-ben");
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
                if (s.Elérhető_mennyiség == 0)
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
            MessageBox.Show("A következő Excel-ben, azok a termékek láthatóak, melyekből berendelés szükséges, mert hiánycikk a vállalatnál.");

            GetDelete();
            CreateExcel();
            CreateTable();
        }

        public void CreateExcel()
        {
            try
            {
                xlApp = new Excel.Application();
                xlWB = xlApp.Workbooks.Add(Missing.Value);
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
            /*  int x = 2; // azért 2 mert 1 a fejléc
              xlSheet.Cells[1, 1] = "Terméknév";
              for (int i = 0; i < NotAvailableProducts.Count; i++)
              {
                  xlSheet.Cells[x, 1] = NotAvailableProducts[i].Név;             
                  x++;                
              }
             */

            xlSheet.get_Range(
            GetCell(2, 1),
            GetCell(1 + values.GetLength(0), values.GetLength(1))).Value2 = values;
            xlSheet.Cells[1, 1] = headers[0];

            Excel.Range headerRange = xlSheet.get_Range(GetCell(1, 1), GetCell(1, headers.Length));
            Excel.Range dataRange = xlSheet.get_Range(GetCell(2, 1), GetCell(1 + values.GetLength(0), values.GetLength(1)));
            dataRange.Font.Color = Color.Blue;
            dataRange.Font.Italic = true;           
            dataRange.EntireColumn.AutoFit();
            dataRange.Interior.Color = Color.Orange;
            headerRange.Font.Bold = true;
            headerRange.VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            headerRange.EntireColumn.AutoFit();
            headerRange.RowHeight = 30;
            headerRange.Font.Color = Color.Black;
            headerRange.Interior.Color = Color.DarkOrange;
            headerRange.BorderAround2(Excel.XlLineStyle.xlContinuous, Excel.XlBorderWeight.xlThick);
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
    }
}
    
           
        
        
    


