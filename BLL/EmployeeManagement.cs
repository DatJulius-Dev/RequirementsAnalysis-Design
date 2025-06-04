using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class EmployeeManagement
    {
        public static DataSet GetEmployees()
        {
            return DAL.EmployeeManagement.GetEmployees();
        }

        public static void Reset(string username)
        {
            DAL.EmployeeManagement.Reset(username);
        }

        public static string AddEmployee(string first, string last, string pos, string store, double salary)
        {
            if (first.Length == 0 || last.Length == 0 || pos.Length == 0 || store.Length == 0 || salary < 0)
            {
                return "Vui lòng nhập chính xác dữ liệu";
            }
            return DAL.EmployeeManagement.AddEmployee(first, last, pos, store, salary);
        }

        public static string EditEmployee(int id, string first, string last, string pos, string store, double salary)
        {
            if (first.Length == 0 || last.Length == 0 || pos.Length == 0 || store.Length == 0 || salary < 0)
            {
                return "Vui lòng nhập chính xác dữ liệu";
            }
            return DAL.EmployeeManagement.EditEmployee(id, first, last, pos, store, salary);
        }
        public static void Change(int id,int bit)
        {
            DAL.EmployeeManagement.Change(id, bit);
        }


        public static List<string> GetStoreName()
        {
            return DAL.EmployeeManagement.GetStoreName();
        }
    }
}
