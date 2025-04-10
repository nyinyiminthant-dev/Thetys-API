using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs
{
    public class WithDrawRequestDTO
    {
        public Guid UserID { get; set; }
        public decimal Amount { get; set; }
    }
}
