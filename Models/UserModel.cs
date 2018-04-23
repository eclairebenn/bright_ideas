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
        public List<Meet> Meets {get;set;}
        public List<Participant> Participating {get;set;}
        
        public User()
        {
            Meets = new List<Meet>();
            Participating = new List<Participant>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}