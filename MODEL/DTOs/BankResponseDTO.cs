using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL.ApplicationConfig;
using MODEL.Entities;

namespace MODEL.DTOs
{
    public class BankResponseDTO : Common
    {
        public User Data { get; set; }
    }

    public class BankListResponseDTO : Common
    {
        public List<User> Data { get; set; }
    }
}
