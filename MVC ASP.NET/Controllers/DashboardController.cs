using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET.Data;
using MVC_ASP.NET.Interfaces;
using MVC_ASP.NET.Models;
using MVC_ASP.NET.ViewModel;

namespace MVC_ASP.NET.Controllers
{
	public class DashboardController : Controller
	{
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContext;

        public DashboardController(IDashboardRepository dashboardRepository, IPhotoService photoService, IHttpContextAccessor httpContext)
        {
            _dashboardRepository = dashboardRepository;
            _photoService = photoService;
            _httpContext = httpContext;
        }

        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editVM, ImageUploadResult photoResult)
        {
            user.Id = editVM.Id;
            user.Pace = editVM.Pace;
            user.Mileage = editVM.Mileage;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.City = editVM.City;
            user.State = editVM.State;
        }
        public async Task<IActionResult> Index()
		{
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var userRaces = await _dashboardRepository.GetAllUserRaces();

            DashboardViewModel dashboardVM = new DashboardViewModel()
            {
                Clubs = userClubs,
                Races = userRaces,
            };
			return View(dashboardVM);
		}

        public async Task<IActionResult> EditUserProfile()
        {
            string currentUserId = _httpContext.HttpContext.User.GetUserId();
            AppUser user = await _dashboardRepository.GetUserById(currentUserId);
            if (user == null) { return View("Error"); };
            EditUserDashboardViewModel editUserViewModel = new EditUserDashboardViewModel()
            {
                Id = currentUserId,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State,
            };
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editUserVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditUserProfile", editUserVM);
            }

            var user = await _dashboardRepository.GetUserByIdNoTracking(editUserVM.Id);

            if (user.ProfileImageUrl == null || user.ProfileImageUrl == "") 
            {
                var photoResult = await _photoService.AddPhotoAsync(editUserVM.Image);

                MapUserEdit(user, editUserVM, photoResult);

                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Could not delete photo");
                    return View(editUserVM);
                }

                var photoResult = await _photoService.AddPhotoAsync(editUserVM.Image);

                MapUserEdit(user, editUserVM, photoResult);

                _dashboardRepository.Update(user);
                return RedirectToAction("Index");
            }
        }
	}
}
