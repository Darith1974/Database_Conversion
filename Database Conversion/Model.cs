using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace Database_Conversion
{
    class Model
    {
        /* Here i created various OleDbConnection objects plus respective connection strings. SetConnection initialises two ole objects, using these
        * strings. This fn can use text strings passed from the calling function which in turn are passed from the user interface.
        */
        public OleDbConnection database { get; set; }

        public OleDbConnection database1 { get; set; }

        public OleDbConnection database2 { get; set; }

        public OleDbConnection database3 { get; set; }

        public OleDbConnection database4 { get; set; }

        public string connectionString { get; set; }

        public string connectionString1 { get; set; }

        public string connectionString2 { get; set; }

        public string connectionString3 { get; set; }

        public string connectionString4 { get; set; }

        

        public void SetConnection(Enum connType, string text1 ,string text2, string text3, string text4, string text5, string text6, string text7, string text8, string text9)
        {

            switch (connType.ToString())
            {
                case "As":
                    connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + text2 + ";Extended Properties=Excel 12.0";
                    connectionString = "Provider="+ text1 +";Data Source= " + text2;
                    
                    connectionString1 = ";User ID=" + text4 + ";Password=" + text5 + ";Initial Catalog=" + text6 + "; Data Source=" + text7;
                    
                    database = new OleDbConnection(connectionString);
                    database1 = new OleDbConnection("Provider= SQLOLEDB.1;" + connectionString1);
                    database.Open();
                    database1.Open();
                    break;
                    //MessageBox.Show("done");
                case "As_MsSql":
                    connectionString = "Provider=" + text1 + ";Data Source= " + text2;
                    
                    connectionString1 = ";User ID=" + text4 + ";Password=" + text5 + ";Initial Catalog=" + text6 + "; Data Source=" + text7;
                    
                    connectionString2 = ";User ID=" + text4 + ";Password=" + text5 + ";Initial Catalog=" + text8 + "; Data Source=" + text7;
                    
                    database = new OleDbConnection(connectionString);
                    database1 = new OleDbConnection("Provider= SQLOLEDB.1;" + connectionString1);
                    database2 = new OleDbConnection("Provider= SQLOLEDB.1;" + connectionString2);
                    database.Open();
                    database1.Open();
                    database2.Open();
                    break;

            }
            /*
            var filename = text1;
            var connString = string.Format(
                @"Provider=Microsoft.Jet.OleDb.4.0; Data Source={0};Extended Properties=""Text;HDR=YES;FMT=Delimited""",
                Path.GetDirectoryName(filename)
            );
            //connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + text1 + ";Extended Properties=\"text;HDR=Yes;FMT=Delimited\"";
            connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + text1 + ";Extended Properties=Excel 12.0";
            database = new OleDbConnection(connString);


            database.Open();
            connectionString1 = ";User ID=" + text4 + ";Password=" + text5 + ";Initial Catalog=" + text6 + "; Data Source=" + text7;

            database1 = new OleDbConnection("Provider= SQLOLEDB.1;" + connectionString1);

            database1.Open();
            */
        }

        // This fn is used to create a Datatable using a specific sql query string plus a specific ole object
        public DataTable DatabaseTables(string QueryString, OleDbConnection DataConnection)
        {

            OleDbCommand SQLQuery = new OleDbCommand();
            DataTable data = null;
            SQLQuery.Connection = null;
            OleDbDataAdapter dataAdapter = null;
            SQLQuery.CommandText = QueryString;
            SQLQuery.Connection = DataConnection;
            data = new DataTable();
            dataAdapter = new OleDbDataAdapter(SQLQuery);

            dataAdapter.Fill(data);
            return data;
        }
        // This fn is used to write data to a database using sqlBulkCopy.

        public void WritetoDatabase(DataRow[] Data, string table, string connection)
        {
            using (SqlBulkCopy DataToTable = new SqlBulkCopy(connection))
            {
                DataToTable.NotifyAfter = 1;
                DataToTable.DestinationTableName = "dbo." + table;
                DataToTable.WriteToServer(Data);
            }
        }

        private void OnSqlRowsCopied(object sender, SqlRowsCopiedEventArgs e)
        {
            //Console.WriteLine("Copied {0} so far...", e.RowsCopied);
            MessageBox.Show("Copied {0} so far..." + e.RowsCopied);
        }
    }
}
