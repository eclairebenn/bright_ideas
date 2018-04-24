using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace bright_ideas.Models
{
    public class Like : BaseEntity
    {
        public int LikeId {get;set;}
        public string UserId {get;set;}
        public User User {get;set;}
        public int IdeaId {get;set;}
        public Idea Idea {get;set;}
        
    }

    public class LikeComparer : IEqualityComparer<Like>
    {
        public bool Equals(Like x, Like y)
        {
            //Check whether the compared objects reference the same data.
            if (Object.ReferenceEquals(x, y)) return true;

            //Check whether any of the compared objects is null.
            if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                return false;

            return x.UserId == y.UserId;

        }

        public int GetHashCode(Like like)
        {
            //Check whether the object is null
            if (Object.ReferenceEquals(like, null)) return 0;

            //Get hash code for the Name field if it is not null.
            int hashLikeUserId = like.UserId == null ? 0 : like.UserId.GetHashCode();


            //Calculate the hash code for the like.
            return hashLikeUserId;
        }
    }
}