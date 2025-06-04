using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace BLL
{
    public class StoreManagement
    {
        public static DataSet GetStore()
        {
            return DAL.StoreManagement.GetStore();
        }
        public static DataSet GetStoreByID(int id)
        {
            return DAL.StoreManagement.GetStoreByID(id);
        }
        public static string AddStore(string name,string add)
        {
            if (name.Length == 0 || add.Length == 0)
                return "Vui lòng nhập đầy đủ thông tin";
            return DAL.StoreManagement.AddStore(name,add);
        }
        public static string EditStore(int id,string name,string add)
        {
            if (name.Length == 0 || add.Length == 0)
                return "Vui lòng nhập đầy đủ thông tin";
            return DAL.StoreManagement.EditStore(id,name,add);
        }
        public static Dictionary<string, int> GetStoreInfo()
        {
            return DAL.StoreManagement.GetStoreInfo();
        }
    }
}
