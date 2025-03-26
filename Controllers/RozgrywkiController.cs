using Microsoft.AspNetCore.Mvc;
using Sedziowanie.Models;
using Sedziowanie.Services.Interfaces;

namespace Sedziowanie.Controllers
{
    public class RozgrywkiController : Controller
    {
        private readonly IRozgrywkiService _rozgrywkiService;

        public RozgrywkiController(IRozgrywkiService rozgrywkiService)
        {
            _rozgrywkiService = rozgrywkiService;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Rozgrywki rozgrywki)
        {
            if (!ModelState.IsValid)
            {
                return View(rozgrywki);
            }

            _rozgrywkiService.AddRozgrywki(rozgrywki);
            return RedirectToAction("ListaRozgrywek");
        }

        public IActionResult ListaRozgrywek()
        {
            var rozgrywki = _rozgrywkiService.GetAllRozgrywki();
            return View(rozgrywki);
        }

        [HttpGet]
        public IActionResult MeczeRozgrywek(int rozgrywkiId)
        {
            var mecze = _rozgrywkiService.GetMeczeForRozgrywki(rozgrywkiId);
            ViewBag.Rozgrywka = _rozgrywkiService.GetRozgrywkaName(rozgrywkiId);
            return View(mecze);
        }
    }
}
