using MVC_ASP.NET.Models;

namespace MVC_ASP.NET.Interfaces
{
	public interface IRaceRepository
	{
		Task<IEnumerable<Race>> GetAll();
		Task<Race> GetByIdAsynch(int id);
		Task<Race> GetByIdAsynchNoTracking(int id);
		Task<IEnumerable<Race>> GetRacesByCity(string city);
		bool Add(Race race);
		bool Update(Race race);
		bool Delete(Race race);
		bool Save();
	}
}
