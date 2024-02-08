using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ASP.NET.Data;
using MVC_ASP.NET.Models;
using MVC_ASP.NET.ViewModel;

namespace MVC_ASP.NET.Controllers
{
	public class AccountController : Controller
	{
		private readonly ApplicationDBContext _context;
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;

		public AccountController(ApplicationDBContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
		{
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Login()
		{
			var response = new LoginViewModel();
			return View(response);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel loginVM)
		{
			if (!ModelState.IsValid) return View(loginVM);

			var user = await _userManager.FindByEmailAsync(loginVM.Email);

			if (user != null)
			{
				var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
				if(passwordCheck)
				{
					var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
					if (result.Succeeded) return RedirectToAction("Index", "Race");
				}
				TempData["Error"] = "Wrong password";
				return View(loginVM);
			}
			TempData["Error"] = "No user with that email";
			return View(loginVM);


		}
	}
}
