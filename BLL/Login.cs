using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class Login
    {
        public static string Kiemtrathongtin(DTO.User user)
        {
            if (user.Username.Length == 0)
                return "Vui lòng nhập tài khoản";
            if (user.Password.Length == 0)
                return "Vui lòng nhập mật khẩu";
            int check = DAL.Login.Kiemtrathongtin(user);
            if (check == -1)
                return "Không tìm thấy tài khoản";
            if (check == 0)
                return "Đăng nhập admin";
            return "Chào mừng đăng nhập";
        }
        public static int GetUserID(DTO.User user)
        {
            return DAL.Login.GetUserID(user);
        }
    }
}
