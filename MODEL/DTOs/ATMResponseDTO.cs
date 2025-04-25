using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL.ApplicationConfig;
using MODEL.Entities;

namespace MODEL.DTOs
{
    public class ATMResponseDTO : Common
    {
       public Transaction Data { get; set; }
    }

    public class ATMListResponseDTO : Common
    {
        public List<Transaction> Data { get; set; }
    }
}
