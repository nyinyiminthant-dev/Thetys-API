using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MODEL.DTOs;

public class AppSettings
{
    public string? ConnectionStrings { get; set; }
    public string? LocalTestUrl { get; set; }
}
