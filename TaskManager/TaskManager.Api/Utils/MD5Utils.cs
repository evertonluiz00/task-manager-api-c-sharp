using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Api
{
    public class MD5Utils
    {
        public static string GerarHashMD5(string input)
        {
            MD5 md5Hash = MD5.Create();
            var stringBytes = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            StringBuilder sBuilder = new StringBuilder();

            foreach (var item in stringBytes)
            {
                sBuilder.Append(item.ToString("X2"));
            }

            return sBuilder.ToString();
        }
    }
}
