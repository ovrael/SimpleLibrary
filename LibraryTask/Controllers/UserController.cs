using LibraryTask.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DatabaseAccess.Access;
using DatabaseAccess.Database;
using LibraryTask.Tools;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace LibraryTask.Controllers
{
	public class UserController : Controller
	{
		private LibraryContext libraryContext;

		public UserController(LibraryContext libraryCtx)
		{
			libraryContext = libraryCtx;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Registration()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Registration(RegisterViewModel model)
		{
			string email = model.Email.Trim();
			string login = model.Login.Trim();
			User tmpUser = libraryContext.Users.Where(u => u.Email.Equals(login) || u.Login.Equals(login)).FirstOrDefault();

			if (tmpUser != null)
			{
				ViewBag.UserExists = "User with given email or login already exists.";
				return View(model);
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			string salt = Security.GenerateSalt(70);
			string pwdHashed = Security.HashPassword(model.Password, salt, 10101, 70);
			User newUser = new User
			{
				Name = model.Name,
				LastName = model.LastName,
				Login = login,
				Email = email,
				Salt = salt,
				Password = pwdHashed
			};

			libraryContext.Users.Add(newUser);
			libraryContext.SaveChanges();

			return RedirectToAction("SuccessRegistration");
		}

		[HttpGet]
		public IActionResult SuccessRegistration()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		private AuthenticationProperties CreateProperties(bool rememberMe)
		{
			return new AuthenticationProperties
			{
				IsPersistent = rememberMe,
				ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
			};
		}

		private ClaimsPrincipal CreatePrincipal(string userLogin)
		{
			var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, userLogin)
				};

			var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			return new ClaimsPrincipal(identity);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> LoginAsync(LoginViewModel model)
		{
			string email = model.Email.Trim();
			User tmpUser = await libraryContext.Users.SingleAsync(u => u.Email == email);

			if (tmpUser == null)
			{
				ViewBag.NoUser = "There is no user with given email.";
				return View(model);
			}

			if (!ModelState.IsValid)
			{
				return View(model);
			}

			string pwdHashed = Security.HashPassword(model.Password, tmpUser.Salt, 10101, 70);

			if (pwdHashed.Equals(tmpUser.Password))
			{
				// Zaloguj
				var principal = CreatePrincipal(tmpUser.Login);
				var properties = CreateProperties(model.RememberMe);
				HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, properties).Wait();

				return RedirectToAction("Index", "Home");
			}
			else
			{
				ViewBag.WrongPassword = "Password is wrong.";
				return View(model);
			}
		}

		[HttpGet]
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
	}
}
