using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.DTOs
{
    public class UpdateBookDto:CreateBookDto
    {
        public int Id { get; set; }
    }
}
