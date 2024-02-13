using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET.Data;
using MVC_ASP.NET.Interfaces;
using MVC_ASP.NET.Models;

namespace MVC_ASP.NET.Repository
{
	public class UsersRepository : IUsersRepository
	{
		private readonly ApplicationDBContext _context;

		public UsersRepository(ApplicationDBContext context)
		{
			_context = context;
		}
		public bool Add(AppUser user)
		{
			throw new NotImplementedException();
		}

		public bool Delete(AppUser user)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
		{
			return await _context.Users.ToListAsync();
		}

		public async Task<AppUser> GetByIdAsync(string id)
		{
			return await _context.Users.FindAsync(id);
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool Update(AppUser user)
		{
			_context.Users.Update(user);
			return Save();
		}
	}
}
