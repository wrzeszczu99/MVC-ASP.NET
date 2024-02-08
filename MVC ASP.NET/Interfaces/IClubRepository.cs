using MVC_ASP.NET.Models;

namespace MVC_ASP.NET.Interfaces
{
	public interface IClubRepository
	{
		Task<IEnumerable<Club>> GetAll();
		Task<Club> GetByIdAsynch(int id);
		Task<Club> GetByIdAsynchNoTracking(int id);
		Task<IEnumerable<Club>> GetClubsByCity(string city);
		bool Add(Club club);
		bool Update (Club club);
		bool Delete(Club club);
		bool Save();
	}
}
