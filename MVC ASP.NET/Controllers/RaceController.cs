using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET.Interfaces;
using MVC_ASP.NET.Models;
using MVC_ASP.NET.ViewModel;
using System.Security.Cryptography.X509Certificates;

namespace MVC_ASP.NET.Controllers
{
    public class RaceController : Controller
    {
		private readonly IRaceRepository _raceRepository;
        private readonly IPhotoService _photoService;

        public RaceController(IRaceRepository raceRepository, IPhotoService photoService) 
        {
			_raceRepository = raceRepository;
            _photoService = photoService;
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

        [HttpPost]
        public async Task<IActionResult> Create(CreateRaceViewModel raceVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
                {
                    Title = raceVM.Title,
                    Description = raceVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = raceVM.Address.Street,
                        City = raceVM.Address.City,
                        State = raceVM.Address.State,
                    }
                };
                _raceRepository.Add(race);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(raceVM);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var race = await _raceRepository.GetByIdAsynch(id);
            if (race == null) return View("Error");
            var raceVM = new EditRaceViewModel
            {
                Title = race.Title,
                Description = race.Description,
                AddressId = race.AddressId,
                Address = race.Address,
                URL = race.Image,
                RaceCategory = race.RaceCategory
            };

            return View(raceVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditRaceViewModel raceVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit race");
                return View("Edit", raceVM);
            }

            var userRace = await _raceRepository.GetByIdAsynchNoTracking(id);

            if (userRace == null) return View(raceVM);
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userRace.Image);
                }
                catch (Exception)
				{
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(raceVM);
                }

                var photoResult = await _photoService.AddPhotoAsync(raceVM.Image);

                var race = new Race
				{
					Id = id,
					Title = raceVM.Title,
					Description = raceVM.Description,
					Image = photoResult.Url.ToString(),
					AddressId = raceVM.AddressId,
					Address = raceVM.Address,
				};

                _raceRepository.Update(race);

                return RedirectToAction("Index");

			}
        }

        public async Task<IActionResult> Delete(int id)
        {
            var race = await _raceRepository.GetByIdAsynch(id);
            if (race == null) return View("Error");
            return View(race);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteRace(int id)
        {
			var race = await _raceRepository.GetByIdAsynch(id);
			if (race == null) return View("Error");

            _raceRepository.Delete(race);
            return RedirectToAction("Index");
		}
    }
}
