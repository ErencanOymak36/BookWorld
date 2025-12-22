using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.DTOs
{
    public class CreateOrderDto
    {
        public int UserId { get; set; }
        public IEnumerable<CreateOrderItemDto> OrderItems { get; set; }
    }
}
