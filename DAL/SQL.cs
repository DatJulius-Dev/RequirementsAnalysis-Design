using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Data.SqlClient;

namespace DAL
{
    public class SQL
    {
        private static SqlConnection conn;
        public static SqlConnection GetConnection()
        {
            if(conn == null)
            {
                string strcon = @"Data Source=DESKTOP-ITM1H24\KHANG;Initial Catalog=QuanLyCuaHang;Integrated Security=True";
                conn = new SqlConnection(strcon);
            }
            return conn;
        }
    }
}
