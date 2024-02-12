using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET.Data;
using MVC_ASP.NET.Interfaces;
using MVC_ASP.NET.ViewModel;

namespace MVC_ASP.NET.Controllers
{
	public class DashboardController : Controller
	{
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
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
	}
}
