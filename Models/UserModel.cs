using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace dojo_activities.Models
{
    public class User : IdentityUser
    {
        public string FirstName {get;set;}
        public string LastName {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<Event> Events {get;set;}
        public List<Participant> Participating {get;set;}
        
        public User()
        {
            Events = new List<Event>();
            Participating = new List<Participant>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}