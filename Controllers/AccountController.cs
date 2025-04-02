using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sedziowanie.Data;
using Sedziowanie.Models;
using Sedziowanie.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Sedziowanie.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly DBObsadyContext _context;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, DBObsadyContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                AvailableSedziowie = _context.Sedziowie.ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, EmailConfirmed = true };

                if (model.Role == "Sedzia" && model.SedziaId.HasValue)
                {
                    var sedzia = _context.Sedziowie.Find(model.SedziaId.Value);
                    if (sedzia != null)
                    {
                        user.Imie = sedzia.Imie;
                        user.Nazwisko = sedzia.Nazwisko;
                        user.Email = sedzia.Email;
                        user.SedziaId = sedzia.Id;
                    }
                }
                else
                {
                    user.Imie = model.Imie;
                    user.Nazwisko = model.Nazwisko;
                }

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction("ShowAll", "Sedzia");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            model.AvailableSedziowie = _context.Sedziowie.ToList();
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("ShowSedzia", "Sedzia");
                }

                ModelState.AddModelError("", "Nieprawidłowy login lub hasło.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("ShowBezDanych", "Sedzia");
        }

        [HttpGet]
        public IActionResult GetSedziaData(int sid)
        {
            var sedzia = _context.Sedziowie
                .Where(s => s.Id == sid)
                .Select(s => new { s.Imie, s.Nazwisko, s.Email })
                .FirstOrDefault();

            if (sedzia == null)
            {
                return NotFound();
            }

            return Json(sedzia);
        }

    }
}
