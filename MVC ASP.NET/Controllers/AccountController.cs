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


        public IActionResult Register()
        {
            var response = new RegisterViewModel();
            return View(response);
        }

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerVM)
		{
			if(!ModelState.IsValid) return View(registerVM);

			var isEmailUsed = await _userManager.FindByEmailAsync(registerVM.Email);
			if (isEmailUsed != null)
			{
				TempData["Error"] = "This email is already used!";
				return View(registerVM);
			}

			AppUser newUser = new AppUser
			{
				Email = registerVM.Email,
				UserName = registerVM.Email
			};

			var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);

            if (!newUserResponse.Succeeded)
            {
                TempData["Error"] = newUserResponse.ToString();
                return View(registerVM);
            }
            
			await _userManager.AddToRoleAsync(newUser, UserRoles.User);

            return RedirectToAction("Index", "Race");
        }

		[HttpPost]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Race");
		}
	}
}
