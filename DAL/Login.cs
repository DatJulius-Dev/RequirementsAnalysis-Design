using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class Login
    {
        public static int Kiemtrathongtin(DTO.User user)
        {
            int result = -1;
            SqlConnection conn = SQL.GetConnection();
            conn.Open();

            string query = "SELECT PositionID FROM Accounts WHERE username = @username AND password = @password and Available = 1";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.Parameters.AddWithValue("@username", user.Username);
            cmd.Parameters.AddWithValue("@password", user.Password);

            object positionID = cmd.ExecuteScalar();

            if (positionID != null)
            {
                result = Convert.ToInt32(positionID);
            }
            conn.Close();
            return result;

        }
        public static int GetUserID(DTO.User user)
        {
            int employeeID = -1; // Giá trị mặc định nếu không tìm thấy

            string query = "SELECT EmployeeID FROM Accounts WHERE Username = @Username";
            SqlConnection connection = SQL.GetConnection();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Username", user.Username);
            connection.Open();
            SqlDataReader reader = command.ExecuteReader();
            if (reader.HasRows)
            {
                reader.Read();
                if(reader.IsDBNull(0))
                {
                    employeeID = -1;
                }
                else
                {
                    employeeID = Convert.ToInt32(reader[0]);
                }
            }
            connection.Close();
            return employeeID;
        }
    }
}
