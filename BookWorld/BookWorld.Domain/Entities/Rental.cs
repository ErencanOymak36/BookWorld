using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Domain.Entities
{
    public class Rental
    {
        public int Id { get; set; }

        // Foregein keys
        public int UserId { get; set; }
        public int BookId { get; set; }

        // Rental Info
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public int RentalPeriodDays { get; set; }   // Örn: 7 gün, 14 gün
        public decimal RentalPrice { get; set; }

        // Navigation
        public User User { get; set; }
        public Book Book { get; set; }

    }
}
