using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }

        public ICollection<Order> Orders { get; set; }
        public ICollection<Rental> Rentals { get; set; }
        public Cart Cart { get; set; }

    }
}
