using Org.BouncyCastle.Pkcs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DAL
{
    public class ProductManagement
    {
        public static DataSet GetProduct()
        {
            DataSet ds = new DataSet();
            SqlConnection conn = SQL.GetConnection();
            conn.Open();
            SqlCommand cmd = new SqlCommand("GetProductDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(ds);
            conn.Close();
            return ds;
        }

        public static DataSet GetProductByID(int id)
        {
            string query = @"SELECT P.ProductName, P.Description, P.Price, SUM(PS.QuantityInStock) AS TotalQuantityInStock
                     FROM ProductStores PS
                     INNER JOIN Products P ON PS.ProductID = P.ProductID
                     WHERE PS.StoreID = (SELECT StoreID FROM Employees WHERE EmployeeID = @EmployeeID)
                     GROUP BY P.ProductName, P.Description, P.Price";

            SqlConnection connection = SQL.GetConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            adapter.SelectCommand.Parameters.AddWithValue("@EmployeeID", id);
            DataSet dataSet = new DataSet();
            connection.Open();
            adapter.Fill(dataSet);
            connection.Close();
            return dataSet;
        }

        public static string AddProduct(string name, string des, double price)
        {
            string query = "INSERT INTO Products (ProductName, Description, Price) VALUES (@ProductName, @Description, @Price);";
            SqlConnection connection = SQL.GetConnection();
            connection.Open();
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ProductName", name);
                command.Parameters.AddWithValue("@Description", des);
                command.Parameters.AddWithValue("@Price", price);

                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    connection.Close();
                    return "Thêm sản phẩm thành công";
                }
                else
                {
                    connection.Close();
                    return "Không thể thêm sản phẩm";
                }
            }
        }

        public static string EditProduct(int id, string name, string des, double price)
        {
            string query = "UPDATE Products SET Description = @Description, Price = @Price, ProductName = @ProductName WHERE ProductID = @ProductID";
            SqlConnection connection = SQL.GetConnection();
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", id);
            command.Parameters.AddWithValue("@ProductName", name);
            command.Parameters.AddWithValue("@Description", des);
            command.Parameters.AddWithValue("@Price", price);

            command.ExecuteNonQuery();
            command.Dispose();
            connection.Close();
            return "Thành công";
        }
    }
}
