using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public decimal Price {  get; set; }
        public int Stock { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        //FOREGEİN KEYS
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        //bağlantı
        public Author Author { get; set; }
        public Category Category { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Rental> Rentals { get; set; }



    }
}
