using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET.Data;
using MVC_ASP.NET.Interfaces;
using MVC_ASP.NET.Models;

namespace MVC_ASP.NET.Repository
{
	public class RaceRepository : IRaceRepository
	{
		private readonly ApplicationDBContext _context;

		public RaceRepository(ApplicationDBContext context)
		{
			_context = context;
		}

		public bool Add(Race race)
		{
			_context.Add(race);
			return Save();
		}

		public bool Delete(Race race)
		{
			_context.Remove(race);
			return Save();
		}

		public async Task<IEnumerable<Race>> GetAll()
		{
			return await _context.Races.ToListAsync();
		}

		public async Task<Race> GetByIdAsynch(int id)
		{
			return await _context.Races.Include(a => a.Address).FirstOrDefaultAsync(r => r.Id == id);
		}	
		public async Task<Race> GetByIdAsynchNoTracking(int id)
		{
			return await _context.Races.Include(a => a.Address).AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
		}

		public async Task<IEnumerable<Race>> GetRacesByCity(string city)
		{
			return await _context.Races.Where(c => c.Address.City.Contains(city)).ToListAsync();
		}

		public bool Save()
		{
			var saved = _context.SaveChanges();
			return saved > 0 ? true : false;
		}

		public bool Update(Race race)
		{
			_context.Update(race);
			return Save();
		}
	}
}
