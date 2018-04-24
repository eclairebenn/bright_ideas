using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace bright_ideas.Models
{
    public class User : IdentityUser
    {
        public string Name {get;set;}
        public DateTime CreatedAt {get;set;}
        public DateTime UpdatedAt {get;set;}
        public List<Idea> Ideas {get;set;}
        public List<Like> Likes {get;set;}

        
        public User()
        {
            Ideas = new List<Idea>();
            Likes = new List<Like>();
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}