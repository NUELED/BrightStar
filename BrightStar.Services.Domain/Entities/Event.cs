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
    public class Event
    {
        [Key]
        public int EventId { get; set; }

        [Display(Name = "Max Participants")]
        public int MaxParticipants { get; set; }
        public string Title { get; set; }
        [ValidateNever]
        public string UserId { get; set; }
        [ValidateNever]
        //[NotMapped]
        public AppUser User { get; set; }

        public int CurrentParticipants { get; set; }

        public string EventCode { get; set; }
        public DateTime Date { get; set; }
 
        [Display(Name = "Start Time")]
        [DataType(DataType.Time)]
        public TimeSpan Time { get; set; }
        public string location { get; set; }
        public string Description { get; set; } 


    }
}
