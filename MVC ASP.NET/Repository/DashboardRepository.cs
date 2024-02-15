using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET.Data;
using MVC_ASP.NET.Interfaces;
using MVC_ASP.NET.Models;

namespace MVC_ASP.NET.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDBContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Club>> GetAllUserClubs()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = _context.Clubs.Where(c => c.AppUser.Id == currentUser);
            return userClubs.ToList();
        }

        public async Task<List<Race>> GetAllUserRaces()
        {
            var currentUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userRaces = _context.Races.Where(r => r.AppUser.Id == currentUser);
            return userRaces.ToList();
        }


        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<AppUser> GetUserByIdNoTracking(string id)
        {
            return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
        }
        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

    }
}
