
using System;
using System.Data.SqlClient;

namespace SurveyDesktopApp
{
    internal class DatabaseHelper
    {
        private static string connectionString = "Server=DESKTOP-OTAPUVT;Database=SurveyDB;Trusted_Connection=True;";

      
        public static void InitializeDatabase()
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string createTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SurveyResponses' AND xtype='U')

                CREATE TABLE SurveyResponses (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    FullName NVARCHAR(100) NOT NULL,
                    Email NVARCHAR(100) NOT NULL,
                    ContactNumber NVARCHAR(50),
                    Date NVARCHAR(50),
                    Age INT,
                    FavouriteFoods NVARCHAR(MAX),
                    EatOut INT,
                    Movies INT,
                    TV INT,
                    Radio INT
                );";

                using (var cmd = new SqlCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

     
        public static void InsertSurvey(string name, string email, string contact, string date, int age,
            string foods, int eatOut, int movies, int tv, int radio)
        {
            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string insertQuery = @"
                       INSERT INTO SurveyResponses 
                       (FullName, Email, ContactNumber, Date, Age, FavouriteFoods, EatOut, Movies, TV, Radio)
                       VALUES (@FullName, @Email, @ContactNumber, @Date, @Age, @FavouriteFoods, @EatOut, @Movies, @TV, @Radio);";

                using (var cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@ContactNumber", contact);
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@Age", age);
                    cmd.Parameters.AddWithValue("@FavouriteFoods", foods);
                    cmd.Parameters.AddWithValue("@EatOut", eatOut);
                    cmd.Parameters.AddWithValue("@Movies", movies);
                    cmd.Parameters.AddWithValue("@TV", tv);
                    cmd.Parameters.AddWithValue("@Radio", radio);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public static int GetSurveyCount()
        {
            int count = 0;
            string connectionString = "Server=DESKTOP-OTAPUVT;Database=SurveyDB;Trusted_Connection=True;";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM SurveyResponses";
                using (var cmd = new SqlCommand(query, conn))
                {
                    count = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            return count;
        }
        public static int GetMaxAge()
        {
            int maxAge = 0;
            string connectionString = "Server=DESKTOP-OTAPUVT;Database=SurveyDB;Trusted_Connection=True;";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MAX(Age) FROM SurveyResponses";

                using (var cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        maxAge = Convert.ToInt32(result);
                    }
                }
            }

            return maxAge;
        }
        public static int GetMinAge()
        {
            int minAge = 0;
            string connectionString = "Server=DESKTOP-OTAPUVT;Database=SurveyDB;Trusted_Connection=True;";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MIN(Age) FROM SurveyResponses";

                using (var cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        minAge = Convert.ToInt32(result);
                    }
                }
            }

            return minAge;
        }
        public static double GetAverageAge()
        {
            double avgAge = 0;
            string connectionString = "Server=DESKTOP-OTAPUVT;Database=SurveyDB;Trusted_Connection=True;";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT AVG(Age) FROM SurveyResponses";

                using (var cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        avgAge = Convert.ToDouble(result);
                    }
                }
            }

            return avgAge;
        }
        public static double GetFoodPercentage(string foodName)
        {
            int total = 0;
            int foodCount = 0;

            string connectionString = "Server=DESKTOP-OTAPUVT;Database=SurveyDB;Trusted_Connection=True;";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                using (var cmdTotal = new SqlCommand("SELECT COUNT(*) FROM SurveyResponses", conn))
                {
                    total = Convert.ToInt32(cmdTotal.ExecuteScalar());
                }

                using (var cmdFood = new SqlCommand("SELECT COUNT(*) FROM SurveyResponses WHERE FavouriteFoods LIKE @Food", conn))
                {
                    cmdFood.Parameters.AddWithValue("@Food", "%" + foodName + "%");
                    foodCount = Convert.ToInt32(cmdFood.ExecuteScalar());
                }
            }

            if (total == 0) return 0;
            return (foodCount / (double)total) * 100;
        }
        public static double GetAverageRating(string columnName)
        {
            double avg = 0;
            string connectionString = "Server=DESKTOP-OTAPUVT;Database=SurveyDB;Trusted_Connection=True;";

            using (var conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = $"SELECT AVG(CAST([{columnName}] AS FLOAT)) FROM SurveyResponses WHERE [{columnName}] IS NOT NULL";

                using (var cmd = new SqlCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        avg = Convert.ToDouble(result);
                    }
                }
            }

            return avg;
        }




    }
}
