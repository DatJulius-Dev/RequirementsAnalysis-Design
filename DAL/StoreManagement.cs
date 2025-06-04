using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class StoreManagement
    {
        public static DataSet GetStore()
        {
            string query = "SELECT * FROM Stores";

            SqlConnection connection = SQL.GetConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataSet dataSet = new DataSet();

            connection.Open();
            adapter.Fill(dataSet, "Stores");
            connection.Close();

            return dataSet;
        }
        public static DataSet GetStoreByID(int id)
        {
            string query = @"SELECT P.ProductName, P.Description, P.Price, PS.QuantityInStock
                     FROM ProductStores PS
                     INNER JOIN Products P ON PS.ProductID = P.ProductID
                     WHERE PS.StoreID = @StoreID";

            SqlConnection connection = SQL.GetConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            adapter.SelectCommand.Parameters.AddWithValue("@StoreID", id);
            DataSet dataSet = new DataSet();
            connection.Open();
            adapter.Fill(dataSet);
            connection.Close();
            return dataSet;
        }

        public static string AddStore(string name, string add)
        {
            string query = @"INSERT INTO Stores (StoreName, Location) 
                     VALUES (@StoreName, @Location)";

            SqlConnection connection = SQL.GetConnection();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StoreName", name);
                command.Parameters.AddWithValue("@Location", add);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
            return "Store added successfully";
        }

        public static string EditStore(int id, string name, string add)
        {
            string query = @"UPDATE Stores 
                     SET StoreName = @StoreName, Location = @Location 
                     WHERE StoreID = @StoreID";

            SqlConnection connection = SQL.GetConnection();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StoreID", id);
                command.Parameters.AddWithValue("@StoreName", name);
                command.Parameters.AddWithValue("@Location", add);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                {
                    return "Store information updated successfully";
                }
                else
                {
                    return "Store not found";
                }
            }
        }
        public static Dictionary<string, int> GetStoreInfo()
        {
            Dictionary<string, int> storeInfo = new Dictionary<string, int>();

            SqlConnection connection = SQL.GetConnection();

            SqlCommand command = new SqlCommand("GetStoreInfo", connection);
            command.CommandType = CommandType.StoredProcedure;
            connection.Open();
            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    storeInfo["NumberOfProductTypes"] = Convert.ToInt32(reader["NumberOfProductTypes"]);

                    reader.NextResult();
                    reader.Read();
                    storeInfo["TotalStores"] = Convert.ToInt32(reader["TotalStores"]);

                    reader.NextResult();
                    reader.Read();
                    storeInfo["NumberOfEmployees"] = Convert.ToInt32(reader["NumberOfEmployees"]);

                    reader.NextResult();
                    reader.Read();
                    storeInfo["NumberOfPurchaseRequests"] = Convert.ToInt32(reader["NumberOfPurchaseRequests"]);
                }
                reader.Close();
            }
            connection.Close();
            return storeInfo;
        }

}
}
