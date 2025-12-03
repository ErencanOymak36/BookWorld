using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Domain.Entities
{
    public class Cart
    {
        public int Id { get; set; }

        //Foregin key
        public int UserId { get; set; }

        public User User { get; set; }
        public ICollection<CartItem> CartItems { get; set; }

    }
}
