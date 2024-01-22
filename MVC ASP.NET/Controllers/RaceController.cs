using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_ASP.NET.Data;
using MVC_ASP.NET.Interfaces;
using MVC_ASP.NET.Models;

namespace MVC_ASP.NET.Controllers
{
    public class RaceController : Controller
    {
		private readonly IRaceRepository _raceRepository;

		public RaceController(IRaceRepository raceRepository) {
			this._raceRepository = raceRepository;
		}
        public async Task<IActionResult> Index()
        {
            IEnumerable<Race> races = await _raceRepository.GetAll();
            return View(races);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Race race = await _raceRepository.GetByIdAsynch(id);
            return View(race);
        }

        public IActionResult Create()
        {
            return View();
        }

        public async Task<IActionResult> Create(Race race)
        {
            if(!ModelState.IsValid)
            {
                return View(race);
            }    
            _raceRepository.Add(race);
            return RedirectToAction("Index");
        }
    }

}
