This is  a windows form which is a exercise in migrating data between databases of different types plus Excel using C#. It
is planned to  simulate different scenarios which are implemented in the various tabs shown. Only the 
"Source: ms Access" & "Source: Access & MsSql"is implemented at the moment. In each case the destination database is 
the Northwind MsSql Database. The first tab will implement a migration from a Ms Access and MsSql to MsSql. The 
others will implement various scenarios.


"Source: Ms Access". In this tab there are various text boxes to be filled in. The provider or driver to be used to read the Ms 
Access Database,  i recommend Ace. The details for the Northwind database stored locally. Finally there is a button which  will 
firstly request the location of the ms access database through a file dialog screen and proceed to migrate data from various 
tables in this case; the Mondial db to the Employees table in the Northwind db. A message box will apppear when done.
"Source: Access & MsSql": Similar to above. Only difference there is a extra MsSql source db  which is assumed to be
on the same server as the Northwind databse.



Technically speaking this a is a MVC structure. There is a model class , a base controller which is inherited by the respective
controller for each different migration. Lets take the sequence of events for the implementation of the migration initiated from
the "Source: Ms Access" tab. When the button is fired, control passes to its fn in the form.cs which declares an object of the
As_DatabaseController Class. Control is passed to the Conversionfn of this class. Here i create various datatables and hence 
datarow arrays. The for loop will scroll through each row of the mondial tables migrating elements of each of the tables to a 
new row in the Employees data table. How i do this is worth a brief mention. I add a row to a data table which emulates the destination
ttable Employees. I then pull the last and new row into an array and then write this to the databaase. Another way to implement this would
have been to use Sqlconnection as shown in the 'UpdateCell' fn of controller.cs.