using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class UserUse
    {
        private static int _id;
        public static void ChangeUser(int id)
        {
            _id = id;
        }
        public static int GetUser()
        {
            return _id;
        }
    }
}
