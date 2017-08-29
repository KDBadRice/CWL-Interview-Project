using System;
using System.Linq;
using System.Windows;
using System.Data;

using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;

using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Windows.Controls;

namespace CWL_Project_Entity
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 


    public partial class MainWindow : Window
    {

        CWL_Project_Entity.ACMEEntities db = new CWL_Project_Entity.ACMEEntities();
        /// private Button buttoninsert;
        ///private Button buttonprint;

        //public DbContext db = new DbContext(@"C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA");
        public MainWindow()
        {
            InitializeComponent();
        }
        private void insertbutton_click(object sender, RoutedEventArgs e)
        {
            Button insert_Button = (Button)sender;

            callmethod();
            MessageBox.Show("Inserted Employees");

        }
        private void callmethod()
        {
        var query = db.Employees1.Select(i => i).AsQueryable();
                try
                    {
                        ObjectContext dbCtx = ((IObjectContextAdapter)db).ObjectContext;
                        EntityToExcelSheet(@"C:\Users\Kevin\Documents\CWL Project\Employees.xls", "Employees", query, dbCtx);
                    }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message, 
                              "Error Creating Excel File", MessageBoxButton.OK);
                        }
                            }
        
        public void EntityToExcelSheet(string excelFilePath, string sheetName, IQueryable result, ObjectContext ctx)
            {//Query to grab excel data
               //             string query = "select * from Employees";
               // string excelFilePath = "";
                //string sheetName = "";
                //IQueryable result;
                //ObjectContext ctx;

                //Oledb set up
                Excel.Application appObj;
                Excel.Workbook workBookObj;
                Excel.Worksheet sheetObj;
                Excel.Range rangeCheck;
                            try
                            {
                                //Excel / application object reference
                                appObj = new Excel.Application();
                                // Setting properties 
                                appObj.Visible = true;
                                appObj.DisplayAlerts = false;

                                //New workbook
                                workBookObj = appObj.Workbooks.Add(Missing.Value);

                                //Get active 
                                sheetObj = (Excel.Worksheet)workBookObj.ActiveSheet;
                                sheetObj.Name = sheetName;

                                // Process the DataTable
                                // BE SURE TO CHANGE THIS LINE TO USE *YOUR* DATATABLE 
                                DataTable dt = EntityToDataTable(result, ctx);

                                int rowCount = 1;
                                foreach (DataRow dr in dt.Rows)
                                {
                                    rowCount += 1;
                                    for (int i = 1; i<dt.Columns.Count + 1; i++)
                                    {
                                        // Add the header the first time through 
                                        if (rowCount == 2)
                                            sheetObj.Cells[1, i] = dt.Columns[i - 1].ColumnName;
                                            sheetObj.Cells[rowCount, i] = dr[i - 1].ToString();
                                    }
                                }

                                // Resize the columns 
                                rangeCheck = sheetObj.Range[sheetObj.Cells[1, 1], sheetObj.Cells[rowCount, dt.Columns.Count]];
                                rangeCheck.Columns.AutoFit();

                                // Save the sheet and close 
                                sheetObj = null;
                                rangeCheck = null;
                                workBookObj.SaveAs(excelFilePath, Excel.XlFileFormat.xlWorkbookNormal, Missing.Value,
                                  Missing.Value, Missing.Value, Missing.Value,
                                  Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value,
                                  Missing.Value, Missing.Value, Missing.Value);
                                workBookObj.Close(Missing.Value, Missing.Value, Missing.Value);
                                workBookObj = null;
                                appObj.Quit();
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
    }


        
        public DataTable EntityToDataTable(IQueryable result, ObjectContext ctx)
        {
            try
            {
                EntityConnection conn = ctx.Connection as EntityConnection;
                using (SqlConnection SQLCon = new SqlConnection(conn.StoreConnection.ConnectionString))
                {
                    ObjectQuery query = result as ObjectQuery;
                    using (SqlCommand Cmd = new SqlCommand(query.ToTraceString(), SQLCon))
                    {
                        foreach (var param in query.Parameters)
                        {
                            Cmd.Parameters.AddWithValue(param.Name, param.Value);
                        }
                        using (SqlDataAdapter da = new SqlDataAdapter(Cmd))
                        {
                            using (DataTable dt = new DataTable())
                            {
                                da.Fill(dt);
                                return dt;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void printbutton_click(object sender, RoutedEventArgs e)
        {
            //Query to grab excel data
            //string query = "select * from Employees";
            //Oledb set up
            Button print_Button = (Button)sender;



            MessageBox.Show("Employees printed");
        }
    }
}