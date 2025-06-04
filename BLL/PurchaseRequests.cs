using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PurchaseRequests
    {
        public static DataSet GetRequest()
        {
            return DAL.PurchaseRequests.GetRequest();
        }
        public static DataSet GetRequestByID(int id)
        {
            return DAL.PurchaseRequests.GetRequestByID(id);
        }
        public static DataSet GetAllRequest()
        {
            return DAL.PurchaseRequests.GetAllRequest();
        }
        public static DataSet GetAllRequestByID(int id)
        {
            return DAL.PurchaseRequests.GetAllRequestByID(id);
        }
        public static string Accept(int id)
        {
            return DAL.PurchaseRequests.Accept(id);
        }
        public static void Reject(int id)
        {
            DAL.PurchaseRequests.Reject(id);
        }
        public static void Delete(int id)
        {
            DAL.PurchaseRequests.Delete(id);
        }
        public static string AddRequest(string RequestType, int iD, int storeID, int productID, int QuantityRequested)
        {
            if (QuantityRequested < 1)
            {
                return "Giá trị không hợp lệ";
            }
            return DAL.PurchaseRequests.AddRequest(RequestType, iD, storeID, productID, QuantityRequested);
        }
        public static string EditRequest(int requestID,string RequestType, int iD, int storeID, int productID, int QuantityRequested)
        {
            if (QuantityRequested < 1)
            {
                return "Giá trị không hợp lệ";
            }
            return DAL.PurchaseRequests.EditRequest(requestID,RequestType, iD, storeID, productID, QuantityRequested);
        }
    }
}
