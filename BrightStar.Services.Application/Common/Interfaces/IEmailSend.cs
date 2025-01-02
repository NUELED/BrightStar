using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BrightStar.Services.Application.Common.DTO;

namespace BrightStar.Services.Application.Common.Interfaces
{
    public interface IEmailSend
    {
        Task SendEmail2(EmailDto request);
    }
}
