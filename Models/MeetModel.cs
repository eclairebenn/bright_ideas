using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace dojo_activities.Models
{
    public class Meet : BaseEntity
    {
        public int MeetId {get;set;}

        public string Title {get; set;}
        public string Description {get;set;}
        public DateTime Date {get;set;}
        public TimeSpan Duration {get;set;}
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Participant> Participants {get;set;}

        public Meet()
        {
            Participants = new List<Participant>();

        }
    }    
}