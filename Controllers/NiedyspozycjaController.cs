using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sedziowanie.Models;
using Sedziowanie.Services.Interfaces;
using System;

namespace Sedziowanie.Controllers
{
    public class NiedyspozycjaController : Controller
    {
        private readonly INiedyspozycjaService _niedyspozycjaService;
        private readonly UserManager<ApplicationUser> _userManager;

        public NiedyspozycjaController(INiedyspozycjaService niedyspozycjaService, UserManager<ApplicationUser> userManager)
        {
            _niedyspozycjaService = niedyspozycjaService;
            _userManager = userManager;
        }

        public IActionResult Show()
        {
            var niedyspozycje = _niedyspozycjaService.GetAllNiedyspozycje();
            return View(niedyspozycje);
        }

        [HttpGet]
        public async Task<IActionResult> AddForSedzia()
        {
            var user = await _userManager.GetUserAsync(User);
            var sedzia = _niedyspozycjaService.GetSedziaByUserId(user.Id);

         

            ViewBag.SedziaId = sedzia.Id;
            return View();
        }

        [HttpPost]
        public IActionResult AddForSedzia(int sedziaId, DateTime poczatek, DateTime koniec)
        {
            try
            {
                _niedyspozycjaService.AddNiedyspozycja(sedziaId, poczatek, koniec);
                return RedirectToAction("Show");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        
        [HttpGet]
        public IActionResult AddForAdmin()
        {
            ViewBag.Sedziowie = _niedyspozycjaService.GetSedziowieList();
            return View("AddForAdmin");
        }

        [HttpPost]
        public IActionResult AddForAdmin(int sedziaId, DateTime poczatek, DateTime koniec)
        {
            try
            {
                _niedyspozycjaService.AddNiedyspozycja(sedziaId, poczatek, koniec);
                return RedirectToAction("Show");
            }
            catch (ArgumentException ex)
            {
                ModelState.AddModelError("", ex.Message);
               // ViewBag.Sedziowie = new SelectList(_niedyspozycjaService.GetSedziowieList(), "Id", "FullName");
                return View();
            }
        }
    }

    }

