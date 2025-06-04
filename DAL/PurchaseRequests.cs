using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PurchaseRequests
    {
        public static DataSet GetRequest()
        {
            string query = "SELECT \r\n    PR.RequestID, \r\n    PR.RequestDate,pr.RequestType, \r\n    P.ProductName, \r\n    S.StoreName, \r\n    PR.QuantityRequested, \r\n    E.FirstName + ' ' + E.LastName AS EmployeeName, \r\n    PR.RequestStatus\r\nFROM \r\n    PurchaseRequests PR\r\nINNER JOIN \r\n    Products P ON PR.ProductID = P.ProductID\r\nINNER JOIN \r\n    Stores S ON PR.StoreID = S.StoreID\r\nINNER JOIN \r\n    Employees E ON PR.RequestedByEmployeeID = E.EmployeeID\r\nWHERE \r\n    PR.RequestStatus = N'Đã yêu cầu';";
            SqlConnection connection = SQL.GetConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataSet dataSet = new DataSet();


            connection.Open();
            adapter.Fill(dataSet);

            connection.Close();

            return dataSet;
        }
        public static DataSet GetRequestByID(int id)
        {
            string query = @"SELECT PR.RequestID, PR.RequestDate, PR.ProductID, PR.QuantityRequested, PR.StoreID, PR.RequestStatus, PR.RequestType
                     FROM PurchaseRequests PR
                     WHERE PR.RequestedByEmployeeID = @EmployeeID AND PR.RequestStatus = N'Đã yêu cầu'";

            SqlConnection connection = SQL.GetConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            adapter.SelectCommand.Parameters.AddWithValue("@EmployeeID", id);
            DataSet dataSet = new DataSet();

            connection.Open();
            adapter.Fill(dataSet);

            connection.Close();

            return dataSet;
        }

        public static DataSet GetAllRequest()
        {
            string query = "SELECT \r\n    PR.RequestID, \r\n    PR.RequestDate, \r\n    P.ProductName, \r\n    S.StoreName, \r\n    PR.QuantityRequested, \r\n    E.FirstName + ' ' + E.LastName AS EmployeeName, \r\n    PR.RequestStatus\r\nFROM \r\n    PurchaseRequests PR\r\nINNER JOIN \r\n    Products P ON PR.ProductID = P.ProductID\r\nINNER JOIN \r\n    Stores S ON PR.StoreID = S.StoreID\r\nINNER JOIN \r\n    Employees E ON PR.RequestedByEmployeeID = E.EmployeeID;";
            SqlConnection connection = SQL.GetConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            DataSet dataSet = new DataSet();


            connection.Open();
            adapter.Fill(dataSet);

            connection.Close();

            return dataSet;
        }
        public static void Delete(int id)
        {
            SqlConnection connection = SQL.GetConnection();
            string sql = "DELETE FROM PurchaseRequests WHERE RequestID = @RequestID";
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@RequestID", id);
                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static DataSet GetAllRequestByID(int id)
        {
            string query = @"SELECT PR.RequestID, PR.RequestDate, PR.ProductID, PR.QuantityRequested, PR.StoreID,  PR.RequestStatus, PR.RequestType
                     FROM PurchaseRequests PR
                     WHERE PR.RequestedByEmployeeID = @EmployeeID";

            SqlConnection connection = SQL.GetConnection();
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
            adapter.SelectCommand.Parameters.AddWithValue("@EmployeeID", id);
            DataSet dataSet = new DataSet();

            connection.Open();
            adapter.Fill(dataSet);

            connection.Close();

            return dataSet;
        }

        public static string Accept(int id)
        {
            SqlConnection connection = SQL.GetConnection();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;

            connection.Open();
            SqlTransaction transaction = connection.BeginTransaction();
            command.Transaction = transaction;

            // Lấy thông tin yêu cầu từ bảng PurchaseRequests
            string getRequestQuery = "SELECT ProductID, StoreID, QuantityRequested, RequestType FROM PurchaseRequests WHERE RequestID = @RequestID";
            command.CommandText = getRequestQuery;
            command.Parameters.AddWithValue("@RequestID", id);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                int productID = reader.GetInt32(0);
                int storeID = reader.GetInt32(1);
                int quantityRequested = reader.GetInt32(2);
                string requestType = reader.GetString(3);

                reader.Close();

                // Xử lý trường hợp nhập kho
                if (requestType == "Nhập kho")
                {
                    // Kiểm tra xem ProductStores đã có sản phẩm với StoreID tương ứng chưa
                    string checkExistQuery = "SELECT QuantityInStock FROM ProductStores WHERE ProductID = @ProductID AND StoreID = @StoreID";
                    command.CommandText = checkExistQuery;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProductID", productID);
                    command.Parameters.AddWithValue("@StoreID", storeID);
                    object quantityInStock = command.ExecuteScalar();

                    if (quantityInStock != null) // Sản phẩm đã tồn tại trong ProductStores
                    {
                        // Cộng thêm số lượng vào ProductStores
                        string updateQuantityQuery = "UPDATE ProductStores SET QuantityInStock = QuantityInStock + @QuantityRequested WHERE ProductID = @ProductID AND StoreID = @StoreID";
                        command.CommandText = updateQuantityQuery;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@QuantityRequested", quantityRequested);
                        command.ExecuteNonQuery();
                    }
                    else // Sản phẩm chưa tồn tại trong ProductStores
                    {
                        // Thêm mới sản phẩm vào ProductStores
                        string addProductQuery = "INSERT INTO ProductStores (ProductID, StoreID, QuantityInStock) VALUES (@ProductID, @StoreID, @QuantityRequested)";
                        command.CommandText = addProductQuery;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@ProductID", productID);
                        command.Parameters.AddWithValue("@StoreID", storeID);
                        command.Parameters.AddWithValue("@QuantityRequested", quantityRequested);
                        command.ExecuteNonQuery();
                    }
                }
                // Xử lý trường hợp xuất kho
                else if (requestType == "Xuất kho")
                {
                    // Kiểm tra xem số lượng trong ProductStores có đủ để xuất không
                    string checkQuantityQuery = "SELECT QuantityInStock FROM ProductStores WHERE ProductID = @ProductID AND StoreID = @StoreID";
                    command.CommandText = checkQuantityQuery;
                    command.Parameters.Clear();
                    command.Parameters.AddWithValue("@ProductID", productID);
                    command.Parameters.AddWithValue("@StoreID", storeID);
                    object quantityInStock = command.ExecuteScalar();

                    if (quantityInStock != null && (int)quantityInStock >= quantityRequested) // Đủ hàng trong kho để xuất
                    {
                        // Trừ đi số lượng xuất kho khỏi số lượng tồn kho
                        string updateQuantityQuery = "UPDATE ProductStores SET QuantityInStock = QuantityInStock - @QuantityRequested WHERE ProductID = @ProductID AND StoreID = @StoreID";
                        command.CommandText = updateQuantityQuery;
                        command.Parameters.Clear();
                        command.Parameters.AddWithValue("@ProductID", productID);
                        command.Parameters.AddWithValue("@StoreID", storeID);
                        command.Parameters.AddWithValue("@QuantityRequested", quantityRequested);
                        command.ExecuteNonQuery();
                    }
                    else // Không đủ hàng trong kho để xuất
                    {
                        return "Không thể thực hiện yêu cầu xuất kho. Số lượng hàng trong kho không đủ.";
                    }
                }
                // Cập nhật trạng thái của yêu cầu thành "Đã chấp nhận"
                string updateStatusQuery = "UPDATE PurchaseRequests SET RequestStatus = N'Đã chấp nhận' WHERE RequestID = @RequestID";
                command.CommandText = updateStatusQuery;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@RequestID", id);
                command.ExecuteNonQuery();
                transaction.Commit();
                connection.Close();
                return "Đã hoàn thành";
            }
            else
            {
                connection.Close();
                throw new Exception("Không tìm thấy yêu cầu có ID = " + id);
            }
        }

        public static void Reject(int id)
        {
            string query = "UPDATE PurchaseRequests SET RequestStatus = N'Đã hủy' WHERE RequestID = @RequestID";

            SqlConnection connection = SQL.GetConnection();

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@RequestID", id);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static string AddRequest(string requestType, int employeeID, int storeID, int productID, int quantityRequested)
        {
            SqlConnection connection = SQL.GetConnection();

            // Kiểm tra số lượng hàng tồn kho trước khi thực hiện yêu cầu
            if (requestType == "Xuất kho")
            {
                string checkQuery = "SELECT QuantityInStock FROM ProductStores WHERE ProductID = @ProductID AND StoreID = @StoreID";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                checkCommand.Parameters.AddWithValue("@ProductID", productID);
                checkCommand.Parameters.AddWithValue("@StoreID", storeID);

                connection.Open();
                int currentStock = Convert.ToInt32(checkCommand.ExecuteScalar());
                connection.Close();

                if (quantityRequested > currentStock)
                {
                    return "Không thể thêm yêu cầu. Số lượng yêu cầu vượt quá số lượng hàng tồn kho.";
                }
            }

            // Thêm yêu cầu vào bảng PurchaseRequests
            string query = @"INSERT INTO PurchaseRequests (RequestDate, ProductID, QuantityRequested, RequestedByEmployeeID, StoreID, RequestStatus, RequestType)
                 VALUES (GETDATE(), @ProductID, @QuantityRequested, @RequestedByEmployeeID, @StoreID, N'Đã yêu cầu', @RequestType)";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ProductID", productID);
            command.Parameters.AddWithValue("@QuantityRequested", quantityRequested);
            command.Parameters.AddWithValue("@RequestedByEmployeeID", employeeID);
            command.Parameters.AddWithValue("@StoreID", storeID);
            command.Parameters.AddWithValue("@RequestType", requestType);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            return "Yêu cầu đã được thêm thành công.";
        }
        public static string EditRequest(int requestID, string requestType, int employeeID, int storeID, int productID, int quantityRequested)
        {
            SqlConnection connection = SQL.GetConnection();
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            connection.Open();

            // Cập nhật thông tin của yêu cầu
            string updateQuery = @"UPDATE PurchaseRequests 
                               SET RequestType = @RequestType, 
                                   RequestedByEmployeeID = @EmployeeID, 
                                   StoreID = @StoreID, 
                                   ProductID = @ProductID, 
                                   QuantityRequested = @QuantityRequested
                               WHERE RequestID = @RequestID";
            command.CommandText = updateQuery;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@RequestID", requestID);
            command.Parameters.AddWithValue("@RequestType", requestType);
            command.Parameters.AddWithValue("@EmployeeID", employeeID);
            command.Parameters.AddWithValue("@StoreID", storeID);
            command.Parameters.AddWithValue("@ProductID", productID);
            command.Parameters.AddWithValue("@QuantityRequested", quantityRequested);
            command.ExecuteNonQuery();
            connection.Close();

            return "Yêu cầu đã được chỉnh sửa thành công.";
        }

    }
}
