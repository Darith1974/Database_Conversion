using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

namespace Database_Conversion
{
    class Controller
    {
        // I create a object of the Model class in order to access it's methods for retreiving and storing data.
        protected Model DataAccess = new Model();

        //These are various
        protected DataRow[] NewClientData, Client, NewCaseData, NewContactsData, SpecialCategoreyist, Case_Documents, Client_Documents,
                            Client_Other_Documents, Code_Link_btw_CasesAndContacts, Data, TypeData, specialCategoreyData, system_Users,
                            A_Client, A_Children, 
                            O_Cases, Contacts2012, Contacts2013, RelContacts2012, RelContacts2013, NewItemData,
                            CsvImportData, MondialCity_As_DataArray, MondialCountry_As_DataArray, MondialProvince_As_DataArray, 
                            NorthwindEmployeesDataArray, PubsauthorsDataArray, ResultsArray,
                            Imported_MsSql_MondialCityDataArray, Imported_MsSql_NorthwindEmployeesDataArray, Imported_MsSql_MondialCountryDataArray;

        //an enum that i use to distingish 
        protected enum connType { As, As_MsSql, MsSql, MsSql_MsSql, MsSql_Excel }

        protected void MaxMinfield(ref int min, ref int max, DataTable data1, string column)
        {
            DataRow[] DataArray = data1.Select();

            for (int i =0; i< data1.Rows.Count ; i++)
            {
                if(min > Convert.ToInt32(DataArray[i][column]))
                {
                    min = Convert.ToInt32(DataArray[i][column]);
                }
                if (max < Convert.ToInt32(DataArray[i][column]))
                {
                    max = Convert.ToInt32(DataArray[i][column]);
                }
            }
        }

        protected static string ReverseDate(string date)
        {
            string[] temp = date.Split('.');
            char[] array = date.ToCharArray();
            Array.Reverse(array);
            string date1 = temp[2] + "." + temp[1] + "." + temp[0];
            return date1;
        }

        //This fn specifies and creates if neccessary the destination file path whwn a file is sopied from the opsis server to our own.
        protected string dbgenopen(int number, string gendir, string genext)
        {
            string NewLocation = "";
            int a = (number / 500) * 500;

            if (!Directory.Exists("P" + ":\\practpro\\" + gendir + "\\" + number.ToString()))
            {
                Directory.CreateDirectory("P" + ":\\practpro\\" + gendir + "\\" + number.ToString());
            }

            return NewLocation;
        }

        protected static void UpdateCell(string ColumnName, string TableName, int NewValue, string connectionString)
        {
            try
            {
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand("UPDATE " + TableName + " SET " + ColumnName + " = '" + NewValue + "'", connection);
                command.Connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        //Returns User ID.
        protected int GetUserID(string Value, DataTable data5)
        {
            int defaultresult = 0;
            for (int count = 0; count < data5.Rows.Count; count++)
            {
                if (system_Users[count]["User_Name"].ToString() == Value)
                {
                    return Convert.ToInt32(system_Users[count]["User_ID"]);

                }
            }
            return defaultresult;
        }

        //Ret
        // Returns The Contact ID.
        protected int GetContactID(string test)
        {
            DataTable data4 = DataAccess.DatabaseTables("Select * from [Contacts]", DataAccess.database1);
            NewContactsData = data4.Select();
            int id = 0;

            for (int count = 0; count < data4.Rows.Count; count++)
            {
                if (NewContactsData[count]["Contact_CabinetNumber"].ToString() == test)
                {
                    return id = Convert.ToInt32(NewContactsData[count]["Contact_ID"]);
                }

            }

            return id;
        }

        //Returns the Client ID.
        protected int GetClientID(string test, DataTable data6)
        {
            int id = 0;
            Client = data6.Select();
            for (int count = 0; count < data6.Rows.Count; count++)
            {
                if (Client[count]["Contact_CabinetNumber"].ToString() == test)
                {
                    return id = Convert.ToInt32(Client[count]["Client_ID"]);
                }

            }
            return id;
        }

        //Returns the Case ID.
        protected int GetCaseID(string test, DataTable data4)
        {
            int id = 0;
            NewCaseData = data4.Select();
            for (int count = 0; count < data4.Rows.Count; count++)
            {
                if (NewCaseData[count]["code"].ToString() == test)
                {
                    return id = Convert.ToInt32(NewCaseData[count]["Case_ID"]);
                }

            }
            return id;
        }

        //Returns the Category ID
        protected int GetCategoryId(string type, DataTable data5)
        {
            SpecialCategoreyist = data5.Select();
            //OpsisContacts = data1.Select();
            int TypeId = 0;
            for (int count = 0; count < data5.Rows.Count; count++)
            {
                if (SpecialCategoreyist[count]["Item"].ToString() == type)
                {
                    TypeId = Convert.ToInt32(SpecialCategoreyist[count]["ID"]);
                    return TypeId;
                }
            }
            return TypeId = 6;
        }

        //Various DataRow arrays for use in Program.
        
        //Truncates a fn to a specific lenght.
        protected string Truncate(string value, int maxLength)
        {
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        //Verifies whether a string valus is a numeric.


        protected int IsNumber(string value)
        {
            int a = 0;

            if (Regex.IsMatch(value, @"^[a-zA-Z]*$") && Regex.IsMatch(value, @"^[0-9]*$"))
            {
                return a = 1;
            }
            else if (Regex.IsMatch(value, @"^[0-9]*$"))
            {
                return a = 2;
            }
            return a;
        }

        /* This fn will evaluate whether a datetime taken from the access database is valid for insertion.
         */
        protected bool IsValidSqlDateTimeNative(string someval)
        {
            bool valid = false;
            DateTime testDate = DateTime.MinValue;
            System.Data.SqlTypes.SqlDateTime sdt;
            if (DateTime.TryParse(someval, out testDate))
            {
                try
                {
                    // take advantage of the native conversion
                    sdt = new System.Data.SqlTypes.SqlDateTime(testDate);
                    valid = true;
                }
                catch (System.Data.SqlTypes.SqlTypeException ex)
                {

                    // no need to do anything, this is the expected out of range error
                }
            }

            return valid;
        }
    }
    
}
