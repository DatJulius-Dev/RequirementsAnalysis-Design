using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ChangePassword
    {
        public static string change(int id,string password,string pass1,string pass2)
        {
            if (password.Length == 0||pass1.Length == 0||pass2.Length == 0)
            {
                return "Vui lòng nhập đầy đủ thông tin";
            }
            if (password.Equals(pass1))
                return "Nhập mật khẩu mới khác mật khẩu cũ";
            if (!pass1.Equals(pass2))
                return "Nhập lại mật khẩu chưa khớp";
            return DAL.ChangePassword.change(id, password, pass1);
        }
    }
}
