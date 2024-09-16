using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Domain.Entities
{
    public class EventSubscription
    {
        public int Id { get; set; }
        public string SubscriptionId { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        [ValidateNever]
        // [NotMapped]
        public AppUser User { get; set; }

        [Required]
        public string Username { get; set; }
        public DateTime SubscribedOn { get; set; }
     
        public int EventId { get; set; }
        [ValidateNever]
        public Event Event { get; set; }


    }
}
