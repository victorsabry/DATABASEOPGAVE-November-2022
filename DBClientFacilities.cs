using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace HotelDBConnection
{
    class DBClientFacilities
    {
        //Database connection string - replace it with the connnection string to your own database 
        string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=HotelDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private int GetMaxFacilityId(SqlConnection connection)
        {
            Console.WriteLine("Calling -> GetFacilityId");

            //This SQL command will fetch one row from the  table: The one with the max Facility_id
            string queryStringMaxFacilityId = "SELECT  MAX(Facility_id)  FROM Facilities";
            Console.WriteLine($"SQL applied: {queryStringMaxFacilityId}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringMaxFacilityId, connection);
            SqlDataReader reader = command.ExecuteReader();

            //Assume undefined value 0 for max Facility_id
            int MaxFacility_id = 0;

            //Is there any rows in the query
            if  (reader.Read())
            {
                //Yes, get max Facility_id
                MaxFacility_id = reader.GetInt32(0); //Reading int fro 1st column
            }

            //Close reader
            reader.Close();
            
            Console.WriteLine($"Max Facility#: {MaxFacility_id}");
            Console.WriteLine();

            //Return max Facility_no
            return MaxFacility_id;
        }
        
        private int DeleteFacility(SqlConnection connection, int facilityId)
        {
            Console.WriteLine("Calling -> DeleteFacility");

            //This SQL command will delete one row from the Facilities table: The one with primary key Facility_id
            string deleteCommandString = $"DELETE FROM Facilities WHERE Facility_id = {facilityId}";
            Console.WriteLine($"SQL applied: {deleteCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(deleteCommandString, connection);
            Console.WriteLine($"Deleting Facility #{facilityId}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int UpdateFacility(SqlConnection connection, Facility facility)
        {
            Console.WriteLine("Calling -> UpdateFacility");

            //This SQL command will update one row from the Facilities table: The one with primary key Facility_id
            string updateCommandString = $"UPDATE Facilities SET Name='{facility.Name}' WHERE Facility_id = {facility.Facility_id}";
            Console.WriteLine($"SQL applied: {updateCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(updateCommandString, connection);
            Console.WriteLine($"Updating Facility #{facility.Facility_id}");
            int numberOfRowsAffected = command.ExecuteNonQuery();

            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected
            return numberOfRowsAffected;
        }

        private int InsertFacility(SqlConnection connection, Facility facility)
        {
            Console.WriteLine("Calling -> InsertFacility");

            //This SQL command will insert one row into the Facilities table with primary key Facility_id
            string insertCommandString = $"INSERT INTO Facilities VALUES({facility.Facility_id}, '{facility.Name}')";
            Console.WriteLine($"SQL applied: {insertCommandString}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(insertCommandString, connection);
            
            Console.WriteLine($"Creating Facility #{facility.Facility_id}");
            int numberOfRowsAffected = command.ExecuteNonQuery();
            
            Console.WriteLine($"Number of rows affected: {numberOfRowsAffected}");
            Console.WriteLine();

            //Return number of rows affected 
            return numberOfRowsAffected;
        }

        private List<Facility> ListAllFacilities(SqlConnection connection)
        {
            Console.WriteLine("Calling -> ListAllFacilities");

            //This SQL command will fetch all rows and columns from the Facilities table
            string queryStringAllFacilities = "SELECT * FROM Facilities";
            Console.WriteLine($"SQL applied: {queryStringAllFacilities}");

            //Apply SQL command
            SqlCommand command = new SqlCommand(queryStringAllFacilities, connection);
            SqlDataReader reader = command.ExecuteReader();
            
            Console.WriteLine("Listing all facilities:");

            //NO rows in the query 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No facilities in database");
                reader.Close();

                //Return null for 'no facilities found'
                return null;
            }

            //Create list of facilities found
            List<Facility> facilities = new List<Facility>();
            while (reader.Read())
            {
                //If we reached here, there is still one facility to be put into the list 
                Facility nextFacility = new Facility()
                {
                    Facility_id = reader.GetInt32(0), //Reading int from 1st column
                    Name = reader.GetString(1),    //Reading string from 2nd column                   
                };

                //Add facility to list
                facilities.Add(nextFacility);

                Console.WriteLine(nextFacility);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return list of facilities
            return facilities;
        }

        private Facility GetFacility(SqlConnection connection, int facilityId)
        {
            Console.WriteLine("Calling -> GetFacility");

            //This SQL command will fetch the row with primary key Facility_id from the Facilities table
            string queryStringOneFacility = $"SELECT * FROM Facilities WHERE Facility_id = {facilityId}";
            Console.WriteLine($"SQL applied: {queryStringOneFacility}");

            //Prepare SQL command
            SqlCommand command = new SqlCommand(queryStringOneFacility, connection);
            SqlDataReader reader = command.ExecuteReader();

            Console.WriteLine($"Finding facility#: {facilityId}");

            //NO rows in the query? 
            if (!reader.HasRows)
            {
                //End here
                Console.WriteLine("No Facilities in database");
                reader.Close();

                //Return null for 'no facility found'
                return null;
            }

            //Fetch facility object from teh database
            Facility facility = null; 
            if (reader.Read())
            {
                facility = new Facility()
                {
                    Facility_id = reader.GetInt32(0), //Reading int fro 1st column
                    Name = reader.GetString(1),    //Reading string from 2nd column                
                };

                Console.WriteLine(facility);
            }

            //Close reader
            reader.Close();
            Console.WriteLine();

            //Return found facility
            return facility;
        }
        public void Start()
        {
            //Apply 'using' to connection (SqlConnection) in order to call Dispose (interface IDisposable) 
            //whenever the 'using' block exits
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                //Open connection
                connection.Open();
                
                //List all facilities in the database
                ListAllFacilities(connection);

                //Create a new facility with primary key equal to current max primary key plus 1
                Facility newFacility = new Facility()
                {
                    Facility_id = GetMaxFacilityId(connection) + 1,
                    Name = "New Facility",                   
                };

                //Insert the facility into the database
                InsertFacility(connection, newFacility);

                //List all facilities including the the newly inserted one
                ListAllFacilities(connection);

                //Get the newly inserted facility from the database in order to update it 
                Facility FacilityToBeUpdated = GetFacility(connection, newFacility.Facility_id);

                //Alter Name and Addess properties
                FacilityToBeUpdated.Name += "(updated)";

                //Update the facility in the database 
                UpdateFacility(connection, FacilityToBeUpdated);

                //List all facilities including the updated one
                ListAllFacilities(connection);

                //Get the updated facility in order to delete it
                Facility FacilityToBeDeleted = GetFacility(connection, FacilityToBeUpdated.Facility_id);

                //Delete the facility
                DeleteFacility(connection, FacilityToBeDeleted.Facility_id);

                //List all facilities - now without the deleted one
                ListAllFacilities(connection);
            }
        }
    }
}
