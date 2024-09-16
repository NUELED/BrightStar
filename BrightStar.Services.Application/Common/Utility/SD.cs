using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.Common.Utility
{
    public class SD
    {
     
        public static string EventAPIBase { get; set; }
        public const string RoleAdmin = "ADMIN";
        public const string RoleOrganizer = "EventOrganizer";
        public const string RoleParticipant = "Participant";
        public const string TokenCookie = "JWTToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }

    }
}
