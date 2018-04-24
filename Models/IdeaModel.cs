using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bright_ideas.Models
{
    public class Idea : BaseEntity
    {
        public int IdeaId {get;set;}
        
        [Required]
        [MinLength(10)]
        public string IdeaText {get;set;}
        public string UserId {get;set;}
        public User User {get;set;}
        public List<Like> Likes {get;set;}
        public Idea() 
        {
            Likes = new List<Like>();
        }
        
    }
}