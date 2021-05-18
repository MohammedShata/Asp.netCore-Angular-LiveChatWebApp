using System;
using System.Collections.Generic;
using api.Entites;

namespace api.Dto
{
    public class MemberDto
    {
        
        public int id {get; set;}
         public int Age {get; set;}
        public string UserName {get;set;}

         public string KnownAs {get;set;}
        public byte[] PasswordHash {get;set;}
         public byte[] PasswordSalt {get;set;}
         public DateTime DateOfBirth {get;set;}
         public DateTime LastActive {get;set;}
         public string Gender {get;set;}
         public string Introduction {get;set;}
         public string LookingFor {get;set;}
         public string Interests {get;set;}
         public string City {get;set;}
         public string Country{get; set;}
         public string PhotoUrl {get; set;}
           public ICollection<PhotosDto> Photos{get;set;}
    }
}