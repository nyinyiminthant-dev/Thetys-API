using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs
{
    public class ResponseUserLoginDTO
    {
        public Guid UserId { get; set; }

        public string? AccountNumber { get; set; }

        public bool AccountStatus { get; set; }
        public string? Token { get; set; }
        public bool PasswordStatus { get; set; }
    }
}
