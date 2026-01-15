using BookWorld.UI.Services.Intefaces;
using System.Security.Claims;

namespace BookWorld.UI.Services
{
    public class CurrentUserService:ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public int UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;

                if (user == null || !user.Identity!.IsAuthenticated)
                    throw new UnauthorizedAccessException("Kullanıcı giriş yapmamış");

                var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                    throw new Exception("JWT içinde UserId claim bulunamadı");

                return int.Parse(userIdClaim.Value);
            }
        }

      
    }
}
