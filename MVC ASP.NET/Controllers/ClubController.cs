using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET.Data;
using MVC_ASP.NET.Interfaces;
using MVC_ASP.NET.Models;

namespace MVC_ASP.NET.Controllers
{
    public class ClubController : Controller
    {
		private readonly IClubRepository _clubRepository;

		public ClubController(IClubRepository clubRepository)
        {
			_clubRepository = clubRepository;
		}
        public async Task<IActionResult> Index()
        {
            IEnumerable<Club> clubs = await _clubRepository.GetAll();
            return View(clubs);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Club club = await _clubRepository.GetByIdAsynch(id);
            return View(club);
        }
    }
}
