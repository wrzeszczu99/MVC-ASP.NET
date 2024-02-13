using MVC_ASP.NET.Models;

namespace MVC_ASP.NET.Interfaces
{
    public interface IUsersRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser> GetByIdAsync(string id);
        bool Add(AppUser user);
        bool Update(AppUser user);
        bool Delete(AppUser user);
        bool Save();
    }
}
