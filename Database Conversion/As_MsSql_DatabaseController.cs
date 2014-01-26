using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Data.OleDb;
using System.Diagnostics;

namespace Database_Conversion
{
    class As_MsSql_DatabaseController : Controller
    {
        private DataTable data1, data2, data3, MondialCityData, NorthwindEmployeesData, MondialCountryData, MondialProvinceData,
        PubsAuthors_As_Data, Imported_MsSql_MondialCityData, Imported_MsSql_NorthwindEmployeesData, Imported_MsSql_MondialCountryData;
        private string EmployeesTableDataSelectSatement, Portfolio_TransactionDataSelectSatement;

        CultureInfo ukCulture = new CultureInfo("en-GB");
        byte[] DummyData = { 0, 0, 0, 0, 1, 1, 1, 1 };
        int count = 0;



        public void ConversionFn(string text1, string text2, string text3, string text4, string text5, string text6, string text7, string text8, string text9)
        {
            int i = 0;

            string Region, Country, City, Population;
            DataRow[] TempArray; OleDbDataAdapter TempAdapter = new OleDbDataAdapter();
            OleDbCommandBuilder builder = new OleDbCommandBuilder(TempAdapter);

            try
            {
                // I access a method from the inherited DataAccess object which opens the connections to the database.
                connType as1 = connType.As_MsSql;
                DataAccess.SetConnection(as1, text1, text2, text3, text4, text5, text6, text7, text8, text9);

                //This fn creates the datarow arrays from data tables and then returns a loopcount variable to use in the for loop.

                MondialCityData = DataAccess.DatabaseTables("Select * from [City] order by ID", DataAccess.database);
                MondialCountryData = DataAccess.DatabaseTables("Select * from [Country] ", DataAccess.database);
                MondialProvinceData = DataAccess.DatabaseTables("Select * from [Province] order by ID", DataAccess.database);
                NorthwindEmployeesData = DataAccess.DatabaseTables("Select * from [Employees] order by EmployeeID", DataAccess.database1);
                //PubsAuthors_As_Data = DataAccess.DatabaseTables("Select * from [authors] order by au_ID", DataAccess.database2);
                Imported_MsSql_MondialCityData = DataAccess.DatabaseTables("Select * from [City] order by ID", DataAccess.database2);

                MondialCity_As_DataArray = MondialCityData.Select();
                MondialCountry_As_DataArray = MondialCountryData.Select();
                MondialProvince_As_DataArray = MondialProvinceData.Select();
                //PubsauthorsDataArray = PubsAuthors_As_Data.Select();
                Imported_MsSql_MondialCityDataArray = Imported_MsSql_MondialCityData.Select();

                DataTable dt = new DataTable();
                dt.Columns.Add("City", typeof(string));
                dt.Columns.Add("Country", typeof(string));
                dt.Columns.Add("Population", typeof(string));

                /* This controller is a hypothetical migration of data from a Ms Access & MsSql database to a MsSql database.
                 * I am using a mondial access db. The source db is infact a imported mssql version of the mondial. The 
                 * destination db is again the northwind db. Although tools are available for this sort of thing, it is a
                 * good exercise none the less. In order to pefroma join between the two source databases, i have to use 
                 * anaymous types and links and then i put the result into a datatable which i then write to the northwind database.
                 */
                var results = from table1 in MondialCityData.AsEnumerable()
                              join table2 in Imported_MsSql_MondialCityData.AsEnumerable()
                              on table1["CityName"] equals table2["CityName"]
                              select new
                              {
                                  CityName = table1["CityName"],
                                  Country = table1["Country"],
                                  Population = table2["Population"]
                              };
                
                foreach (var item in results)
                {
                    dt.Rows.Add(item.CityName, item.Country, item.Population);
                }

                ResultsArray = dt.Select();
    
                for (i = 0; i < dt.Rows.Count; i++)
                {

                    City = Truncate(ResultsArray[i]["City"].ToString(), 15);

                    if (i > dt.Rows.Count - 1)
                    {
                        Population = "No Data";
                    }
                    else
                    {
                        Population = Truncate(ResultsArray[i]["Population"].ToString(), 15);
                    }



                    if (i > dt.Rows.Count - 1)
                    {
                        Country = "No Data";
                    }
                    else
                    {
                        Country = Truncate(ResultsArray[i]["Country"].ToString(), 15);
                    }
                    NorthwindEmployeesData.Rows.Add(0, "", "", "", "", DateTime.Now, DateTime.Now, "", City, Population, "", Country,
                                                    "", "", DummyData, "", 0, "");
                    TempArray = NorthwindEmployeesData.AsEnumerable().Reverse().Take(1).ToArray();
                    DataAccess.WritetoDatabase(TempArray, "Employees", DataAccess.connectionString1);
                }

                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(Convert.ToString(i));
                return;
            }

        }
    }
}
