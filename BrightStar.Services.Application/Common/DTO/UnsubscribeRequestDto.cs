using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.Common.DTO
{
    public class UnsubscribeRequestDto
    {
        public string PhoneNumber { get; set; }
        public string service_id { get; set; }
    }
}
