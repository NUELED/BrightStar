using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Domain.Entities
{
    public class AppSubscription
    {
        public int Id { get; set; }
        public string SubscriptionId { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public DateTime SubscribedOn { get; set; }
    }
}
