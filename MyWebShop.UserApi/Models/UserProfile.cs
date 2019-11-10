using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebShop.UserApi.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Passkey { get; set; }
        public DateTime CreatedOn { get; set; }
        public Nullable<DateTime> UpdatedOn { get; set; }
    }
}
