using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET.Interfaces;
using MVC_ASP.NET.Models;
using MVC_ASP.NET.ViewModel;

namespace MVC_ASP.NET.Controllers
{
    public class ClubController : Controller
    {
		private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;

        public ClubController(IClubRepository clubRepository, IPhotoService photoService)
        {
			_clubRepository = clubRepository;
            _photoService = photoService;
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if(ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    Address = new Address
                    {
                        Street = clubVM.Address.Street,
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                    }
                };
                _clubRepository.Add(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View(clubVM);
        }


        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsynch(id);
            if (club == null) return View("Error");
            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", clubVM);
            }

            var userClub = await _clubRepository.GetByIdAsynchNoTracking(id);

			if (userClub == null)	return View(clubVM);
			else
			{
				try
				{
					await _photoService.DeletePhotoAsync(userClub.Image);
				}
				catch (Exception)
				{
					ModelState.AddModelError("", "Could not delete photo");
					return View(clubVM);
				}
				var photoResult = await _photoService.AddPhotoAsync(clubVM.Image);

				var club = new Club
				{
					Id = id,
					Title = clubVM.Title,
					Description = clubVM.Description,
					Image = photoResult.Url.ToString(),
					AddressId = clubVM.AddressId,
					Address = clubVM.Address,
				};
				_clubRepository.Update(club);
				return RedirectToAction("Index");
			}
		}

        public async Task<IActionResult> Delete(int id)
        { 
            var club = await _clubRepository.GetByIdAsynch(id);
            if (club == null) return View("Error");
            return View(club);
        }


        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var club = await _clubRepository.GetByIdAsynch(id);
			if (club == null) return View("Error");

            _clubRepository.Delete(club);
            return RedirectToAction("Index");
		}

    }
}
