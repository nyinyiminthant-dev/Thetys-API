using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Shared
{
    public static class CommonAuthentication
    {


        public static bool VerifyPassword(string inputPassword, string actualPassword)
        {
            return inputPassword == actualPassword;
        }
    }
}
