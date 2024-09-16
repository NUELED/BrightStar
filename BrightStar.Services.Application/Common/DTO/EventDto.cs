using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.Common.DTO
{
    public class EventDto
    {
      //  [JsonIgnore]
        public int EventId { get; set; }

        [Display(Name = "Max Participants")]
        public int MaxParticipants { get; set; }

        public string Title { get; set; }

        public DateTime Date { get; set; }

        public string EventCode { get; set; }

        [Display(Name = "Start Time")]
        public string Time { get; set; }  // Using string for "HH:mm" format

        public string Location { get; set; }

        public string Description { get; set; }
    }

}
