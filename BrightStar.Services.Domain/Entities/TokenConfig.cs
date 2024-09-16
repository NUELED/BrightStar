using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Domain.Entities
{
    public class TokenConfig
    {
        public int Id { get; set; }
        public int Time { get; set; } // Represents token expiration time in minutes
    }
}
