using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.DTOs
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
