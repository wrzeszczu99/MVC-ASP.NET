using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET.Interfaces;
using MVC_ASP.NET.Models;
using MVC_ASP.NET.ViewModel;

namespace MVC_ASP.NET.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepository _repository;

        public UsersController(IUsersRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<AppUser> users = await _repository.GetAllUsersAsync();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (AppUser user in users)
            {
                UserViewModel userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Pace = user.Pace,
                    Mileage = user.Mileage,
                    ProfileImageUrl = user.ProfileImageUrl
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        public async Task<IActionResult> Detail(string id)
        {
            AppUser user = await _repository.GetByIdAsync(id);
            UserViewModel userViewModel = new UserViewModel()
            {
                Id = user.Id,
                UserName = user.UserName,
                Pace = user.Pace,
                Mileage = user.Mileage
            };
            return View(userViewModel);
        }
    }
}
