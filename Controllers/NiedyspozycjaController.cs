using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sedziowanie.Services.Interfaces;
using System;

namespace Sedziowanie.Controllers
{
    public class NiedyspozycjaController : Controller
    {
        private readonly INiedyspozycjaService _niedyspozycjaService;

        public NiedyspozycjaController(INiedyspozycjaService niedyspozycjaService)
        {
            _niedyspozycjaService = niedyspozycjaService;
        }

        public IActionResult Show()
        {
            var niedyspozycje = _niedyspozycjaService.GetAllNiedyspozycje();
            return View(niedyspozycje);
        }

        [HttpGet]
        [Authorize(Roles = "Sedzia")]
        public IActionResult Add()
        {
            ViewBag.Sedziowie = new SelectList(_niedyspozycjaService.GetSedziowieList(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        public IActionResult Add(int sedziaId, DateTime poczatek, DateTime koniec)
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
    }
}
