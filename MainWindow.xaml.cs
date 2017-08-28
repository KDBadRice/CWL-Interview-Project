using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;


namespace ACME
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            InitializeComponent();
        }



        private void butInsert_Click(object sender, RoutedEventArgs e)
        {

             
               //modified from @"https://www.codeproject.com/tips/636719/import-ms-excel-data-to-sql-server-table-using-csh"
               //declare variables - edit these based on your particular situation
               string ssqltable = "tdatamigrationtable";
    
               string myexceldataquery = "select * from [Employees]";
               string excelfilepath = @"C:\Users\kdong\Desktop\Assignment";
                   try
                         {
                              //create our connection strings
                        string sexcelconnectionstring = @"provider=microsoft.jet.oledb.4.0;data source=" + excelfilepath + ";extended properties=" + "\"excel 8.0;hdr=yes;\"";
        
                        string ssqlconnectionstring = "server=mydatabaseservername;userid=ACME;password=P3nd1ng*;database=databasename;connection reset=false";
                        //series of commands to bulk copy data from the excel file into our sql table
                        oledbconnection oledbconn = new oledbconnection(sexcelconnectionstring);
                        oledbcommand oledbcmd = new oledbcommand(myexceldataquery, oledbconn);
                        oledbconn.open();
                        oledbdatareader dr = oledbcmd.executereader();
                        sqlbulkcopy bulkcopy = new sqlbulkcopy(ssqlconnectionstring);
                        bulkcopy.destinationtablename = ssqltable;
                        while (dr.read())
                        {
                            bulkcopy.writetoserver(dr);
                        }
     
                        oledbconn.close();
                        }
                     catch (Exception)
                        {
                            //handle exception
                        }
                      }
        

        

        private void butPrintReport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
