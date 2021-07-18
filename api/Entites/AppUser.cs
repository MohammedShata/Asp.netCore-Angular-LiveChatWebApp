using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace api.Entites
{
    public class AppUser :IdentityUser<int>
    {
       
        public string KnownAs {get;set;}
         public DateTime DateOfBirth {get;set;}
         public DateTime LastActive {get;set;}
         public string Gender {get;set;}
         public string Introduction {get;set;}
         public string LookingFor {get;set;}
         public string Interests {get;set;}
         public string City {get;set;}
         public string Country{get; set;}
         public DateTime Created{get; set;}
         public ICollection<Photo> Photos{get;set;}
         public ICollection<UserLike> LikedByUsers{get;set;}
 
         public ICollection<UserLike> LikedUsers{get;set;}
         public ICollection<Message> MessagesSent{get;set;}
          public ICollection<Message> MessagesRecevied{get;set;}
           public ICollection<AppUserRole> UserRoles{get;set;}
    }
}