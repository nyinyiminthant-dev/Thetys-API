using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs;

public class UserResponseDTO
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public decimal Balance { get; set; }
    
}
