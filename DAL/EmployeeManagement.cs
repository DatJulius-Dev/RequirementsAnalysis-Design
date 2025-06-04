using Mysqlx.Datatypes;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class EmployeeManagement
    {
        public static DataSet GetEmployees()
        {
            DataSet ds = new DataSet();
            SqlConnection conn = SQL.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("GetEmployeeDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            cmd.Dispose();
            conn.Close();
            return ds;
        }
        public static void Reset(string username)
        {
            SqlConnection conn = SQL.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("update Accounts set password = 'chuoicuahang' where username = @username", conn);
            cmd.Parameters.AddWithValue("@username", username);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            conn.Close();
        }

        public static string AddEmployee(string first, string last, string pos, string store, double salary)
        {
            string query = "INSERT INTO Employees (FirstName, LastName, Position, StoreID, Salary, HireDate) " +
                           "VALUES (@FirstName, @LastName, @Position, @StoreID, @Salary, @HireDate)";

            SqlConnection connection = SQL.GetConnection();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();

                command.Parameters.AddWithValue("@FirstName", first);
                command.Parameters.AddWithValue("@LastName", last);
                command.Parameters.AddWithValue("@Position", pos);
                command.Parameters.AddWithValue("@StoreID", GetStoreIDByName(store, connection));
                command.Parameters.AddWithValue("@Salary", salary);
                command.Parameters.AddWithValue("@HireDate", DateTime.Today); // Ngày của hôm nay
                command.ExecuteNonQuery();
                connection.Close();
            }
            return "Thêm nhân viên thành công";

        }

        public static string EditEmployee(int id, string first, string last, string pos, string store, double salary)
        {
            string query = "UPDATE Employees " +
                           "SET FirstName = @FirstName, LastName = @LastName, Position = @Position, " +
                           "StoreID = @StoreID, Salary = @Salary " +
                           "WHERE EmployeeID = @EmployeeID";

            SqlConnection connection = SQL.GetConnection();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@EmployeeID", id);
                command.Parameters.AddWithValue("@FirstName", first);
                command.Parameters.AddWithValue("@LastName", last);
                command.Parameters.AddWithValue("@Position", pos);
                command.Parameters.AddWithValue("@StoreID", GetStoreIDByName(store, connection));
                command.Parameters.AddWithValue("@Salary", salary);

                int rowsAffected = command.ExecuteNonQuery();
                connection.Close();

                if (rowsAffected > 0)
                    return "Chỉnh sửa thông tin nhân viên thành công";
                else
                    return "Không tìm thấy nhân viên có ID " + id;
            }
        }

        public static void Change(int id, int bit)
        {
            string query = "UPDATE Accounts SET Available = @bit WHERE EmployeeID = @EmployeeID;";

            SqlConnection connection = SQL.GetConnection();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                command.Parameters.AddWithValue("@EmployeeID", id);
                command.Parameters.AddWithValue("@bit", bit);
                command.ExecuteNonQuery();
                connection.Close();
            }
        }


        // Phương thức để lấy StoreID dựa trên tên cửa hàng
        private static int GetStoreIDByName(string storeName, SqlConnection connection)
        {
            string query = "SELECT StoreID FROM Stores WHERE StoreName = @StoreName";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@StoreName", storeName);
                return (int)command.ExecuteScalar();
            }
        }
        public static List<string> GetStoreName()
        {
            List<string> storeNames = new List<string>();

            SqlConnection connection = SQL.GetConnection();

            string query = "SELECT StoreName FROM Stores";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string storeName = reader.GetString(0);
                        storeNames.Add(storeName);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            connection.Close();

            return storeNames;
        }

    }
}
