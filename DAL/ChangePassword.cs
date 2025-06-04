using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ChangePassword
    {
        public static string change(int id, string password, string pass1)
        {
            SqlConnection connection = SQL.GetConnection();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            connection.Open();

            string checkCurrentPasswordQuery = "SELECT Password FROM Accounts WHERE EmployeeID = @EmployeeID";
            command.CommandText = checkCurrentPasswordQuery;
            command.Parameters.AddWithValue("@EmployeeID", id);
            string storedPassword = (string)command.ExecuteScalar();

            if (storedPassword != password)
            {
                return "Mật khẩu hiện tại không chính xác.";
            }

            string updatePasswordQuery = "UPDATE Accounts SET Password = @NewPassword WHERE EmployeeID = @EmployeeID";
            command.CommandText = updatePasswordQuery;
            command.Parameters.AddWithValue("@NewPassword", pass1);
            command.ExecuteNonQuery();
            connection.Close();

            return "Mật khẩu đã được thay đổi thành công.";

        }
    }

}
