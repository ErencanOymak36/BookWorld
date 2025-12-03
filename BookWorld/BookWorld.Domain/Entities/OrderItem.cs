using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }

        // FKs
        public int OrderId { get; set; }
        public int BookId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigation
        public Order Order { get; set; }
        public Book Book { get; set; }

    }
}
