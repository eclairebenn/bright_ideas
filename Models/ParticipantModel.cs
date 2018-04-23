using System;
using System.Collections.Generic;

namespace dojo_activities.Models
{
    public class Participant : BaseEntity
    {
        public int ParticipantId {get;set;}
        public string UserId { get; set; }
        public User User { get; set; }
        public int MeetId {get;set;}
        public Meet Meet {get;set;}


        public Participant()
        {}
    }
}