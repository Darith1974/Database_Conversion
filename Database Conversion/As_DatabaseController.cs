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
    class As_DatabaseController: Controller
    {
        private DataTable data1, data2, data3, MondialCityData, NorthwindEmployeesData, MondialCountryData, MondialProvinceData;
        private string EmployeesTableDataSelectSatement, Portfolio_TransactionDataSelectSatement;
       
        CultureInfo ukCulture = new CultureInfo("en-GB");
        byte[] DummyData = {0,0,0,0,1,1,1,1};
        int count = 0;

        

        public void ConversionFn(string text1, string text2, string text3, string text4, string text5, string text6, string text7)
        {
            int i = 0;
            
            string Region, Country, City;
            DataRow[] TempArray;OleDbDataAdapter TempAdapter = new OleDbDataAdapter();
            OleDbCommandBuilder builder = new OleDbCommandBuilder(TempAdapter);
           
            try
            {
                // I access a method from the inherited DataAccess object which opens the connections to the database.
                connType as1 = connType.As;
                DataAccess.SetConnection(as1 ,text1, text2, text3, text4, text5, text6, text7, "", "");

                //This fn creates the datarow arrays from data tables and then returns a loopcount variable to use in the for loop.

                MondialCityData = DataAccess.DatabaseTables("Select * from [City] order by ID", DataAccess.database);
                MondialCountryData = DataAccess.DatabaseTables("Select * from [Country] ", DataAccess.database);
                MondialProvinceData = DataAccess.DatabaseTables("Select * from [Province] order by ID", DataAccess.database);
                NorthwindEmployeesData = DataAccess.DatabaseTables("Select * from [Employees] order by EmployeeID", DataAccess.database1);

                MondialCity_As_DataArray = MondialCityData.Select();
                MondialCountry_As_DataArray = MondialCountryData.Select();
                MondialProvince_As_DataArray = MondialProvinceData.Select();

                for (i = 0; i < MondialCityData.Rows.Count; i++)
                {

                    City = Truncate(MondialCity_As_DataArray[i]["CityName"].ToString(), 15);
                    
                    if (i > MondialProvinceData.Rows.Count -1)
                    {
                        Region = "No Data";
                    }
                    else
                    {
                        Region = Truncate(MondialProvince_As_DataArray[i]["ProvinceName"].ToString(),15);
                    }
                    
                    

                    if (i > MondialCountryData.Rows.Count-1)
                    {
                        Country = "No Data";
                    }
                    else
                    {
                        Country = Truncate(MondialCountry_As_DataArray[i]["CountryName"].ToString(),15);
                    }
                    NorthwindEmployeesData.Rows.Add(0,"","","","",DateTime.Now,DateTime.Now,"",City,Region,"",Country,
                                                    "","",DummyData,"",0,"");
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
