using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET.Data;
using MVC_ASP.NET.Interfaces;
using MVC_ASP.NET.Models;

namespace MVC_ASP.NET.Repository
{
	public class ClubRepository : IClubRepository
	{
		private readonly ApplicationDBContext _context;

		public ClubRepository(ApplicationDBContext context)
        {
			_context = context;
		}
        public bool Add(Club club)
		{
			_context.Add(club);
			return Save();
		}

		public bool Delete(Club club)
		{
			_context.Remove(club);
			return Save();
		}

		public async Task<IEnumerable<Club>> GetAll()
		{
			return await _context.Clubs.ToListAsync();
		}

		public async Task<Club> GetByIdAsynch(int id)
		{
			return await _context.Clubs.Include(a => a.Address).FirstOrDefaultAsync(i => i.Id == id);
		}

		public async Task<Club> GetByIdAsynchNoTracking(int id)
		{
			return await _context.Clubs.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
		}

		public async Task<IEnumerable<Club>> GetClubsByCity(string city)
		{
			return await _context.Clubs.Where(c => c.Address.City.Contains(city)).ToListAsync();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool Update(Club club)
		{
			_context.Update(club);
			return Save();
		}
	}
}
