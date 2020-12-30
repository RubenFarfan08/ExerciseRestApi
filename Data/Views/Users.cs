using Exercise.Data.Model;
using System.Collections.Generic;
namespace Exercise.Data.Views
{
    public class Users: AppUser
    {
        public IEnumerable<string> Roles{get;set;}
    }
}