using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET.Data;
using MVC_ASP.NET.Models;

namespace MVC_ASP.NET.Controllers
{
    public class RaceController : Controller
    {
		private readonly ApplicationDBContext _context;

		public RaceController(ApplicationDBContext context) {
			_context = context;
		}
        public IActionResult Index()
        {
            var races = _context.Races.ToList();
            return View(races);
        }

        public IActionResult Detail(int id)
        {
            Race race = _context.Races.Include(a => a.Address).FirstOrDefault(r => r.Id == id);
            return View(race);
        }
    }
}
