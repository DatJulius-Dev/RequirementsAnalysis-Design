using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
namespace BLL
{
    public class ProductManagement
    {
        public static DataSet GetProduct()
        {
            return DAL.ProductManagement.GetProduct();
        }
        public static DataSet GetProductByID(int id)
        {
            return DAL.ProductManagement.GetProductByID(id);
        }
        public static string AddProduct(string name, string des, double price)
        {
            if (name.Length == 0)
            {
                return "Không thể tạo";
            }
            return DAL.ProductManagement.AddProduct(name, des, price);
        }

        public static string EditProduct(int id, string name, string des, double price)
        {
            if (name.Length == 0||des.Length == 0)
            {
                return "Không thể sửa";
            }
            return DAL.ProductManagement.EditProduct(id,name, des, price);
        }
    }
}
