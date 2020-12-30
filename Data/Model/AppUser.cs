using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
namespace Exercise.Data.Model
{
    public class AppUser : IdentityUser<Guid> { 
        public DateTime DateCreated { get; set; }
    }
}