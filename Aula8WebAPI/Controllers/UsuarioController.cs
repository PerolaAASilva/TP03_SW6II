using Aula8WebAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Aula8WebAPI.DAL.Seguranca;
using Aula8WebAPI.HttpClients;

namespace Aula8WebAPI.Controllers
{
    public class UsuarioController : Controller
    {

        private readonly AuthApiClient _auth;


        public UsuarioController(AuthApiClient auth)
        {
            _auth = auth;

        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            await HttpContext.SignOutAsync();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _auth.PostLoginAsync(model);
                if (result.Succeeded)
                {
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim (ClaimTypes.Name, model.Login),
                        new Claim ("Token", result.Token)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(String.Empty, "Erro na autenticação");
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]
		public IActionResult Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				//var user = new Usuario { UserName = model.Login };
				//var result = await _userManager.CreateAsync(user, model.Password);
				//if (result.Succeeded)
				//{

				//    await _signInManager.SignInAsync(user, isPersistent: false);
				//    return RedirectToAction("Index", "Home");
				//}
			}
			return View(model);
		}

		[HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }

    }
}