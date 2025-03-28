using Microsoft.AspNetCore.Mvc;
using Sedziowanie.Models;
using Sedziowanie.Services.Interfaces;

namespace Sedziowanie.Controllers
{
    public class SedziaController : Controller
    {
        private readonly ISedziaService _sedziaService;

        public SedziaController(ISedziaService sedziaService)
        {
            _sedziaService = sedziaService;
        }

        [HttpGet]
        public IActionResult ShowAll()
        {
            var sedziowie = _sedziaService.GetAllSedziowie();
            return View(sedziowie);
        }
        [HttpGet]
        public IActionResult ShowBezDanych()
        {
            var sedziowie = _sedziaService.GetAllSedziowie();
            return View(sedziowie);
        }

        [HttpGet]
        public IActionResult ShowSedzia()
        {
            var sedziowie = _sedziaService.GetAllSedziowie();
            return View(sedziowie);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Sedzia sedzia)
        {
            if (!ModelState.IsValid)
            {
                return View(sedzia);
            }

            _sedziaService.AddSedzia(sedzia);
            return RedirectToAction("ShowAll");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var sedzia = _sedziaService.GetSedziaById(id);
            if (sedzia == null)
            {
                return NotFound();
            }
            return View(sedzia);
        }

        [HttpPost]
        public IActionResult Edit(Sedzia sedzia)
        {
            if (!ModelState.IsValid)
            {
                return View(sedzia);
            }

            _sedziaService.UpdateSedzia(sedzia);
            return RedirectToAction("ShowAll");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var sedzia = _sedziaService.GetSedziaById(id);
            if (sedzia == null)
            {
                return NotFound();
            }
            return View(sedzia);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _sedziaService.DeleteSedzia(id);
            return RedirectToAction("ShowAll");
        }

        [HttpGet]
        public IActionResult MeczeSedziego(int sedziaId)
        {
            var mecze = _sedziaService.GetMeczeForSedzia(sedziaId);
            ViewBag.Sedzia = _sedziaService.GetSedziaName(sedziaId);
            return View(mecze);
        }
    }
}
