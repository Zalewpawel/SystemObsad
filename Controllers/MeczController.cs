using Microsoft.AspNetCore.Mvc;
using Sedziowanie.Models;
using Sedziowanie.Services.Interfaces;
using System;

namespace Sedziowanie.Controllers
{
    public class MeczController : Controller
    {
        private readonly IMeczService _meczService;

        public MeczController(IMeczService meczService)
        {
            _meczService = meczService;
        }

        [HttpGet]
        public IActionResult Add(DateTime? dataMeczu)
        {
            var dzisiaj = dataMeczu ?? DateTime.Now;
            Console.WriteLine($"📅 Pobieranie sędziów dla daty: {dzisiaj}");

            ViewBag.Sedziowie = _meczService.GetSedziowieByDate(dzisiaj);
            ViewBag.Rozgrywki = _meczService.GetRozgrywki();
            ViewBag.DataMeczu = dzisiaj.ToString("yyyy-MM-ddTHH:mm");

            return View();
        }

      
        [HttpPost]
        public IActionResult Add(string numerMeczu, DateTime data, int rozgrywkiId, string gospodarz, string gosc,
                                 int sedziaIId, int sedziaIIId, int sedziaSekretarzId)
        {
            if (data < DateTime.Now)
            {
                ModelState.AddModelError("", "Data meczu nie może być wcześniejsza niż aktualna data.");
            }

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Add");
            }

            var mecz = new Mecz
            {
                NumerMeczu = numerMeczu,
                Data = data,
                Gospodarz = gospodarz,
                Gosc = gosc,
                RozgrywkiId = rozgrywkiId,
                SedziaIId = sedziaIId,
                SedziaIIId = sedziaIIId,
                SedziaSekretarzId = sedziaSekretarzId
            };

            _meczService.AddMecz(mecz);

            return RedirectToAction("ListaMeczowAdmin");
        }

        public IActionResult ListaMeczowAdmin()
        {
            var mecze = _meczService.GetAllMecze();
            return View(mecze);
        }
        public IActionResult ListaMeczow()
        {
            var mecze = _meczService.GetAllMecze();
            return View(mecze);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var mecz = _meczService.GetMeczById(id);

            if (mecz == null)
            {
                return NotFound();
            }

            ViewBag.Sedziowie = _meczService.GetSedziowieByDate(mecz.Data);
            ViewBag.Rozgrywki = _meczService.GetRozgrywki();

            return View(mecz);
        }

        [HttpPost]
        public IActionResult Edit(Mecz mecz)
        {
            if (!ModelState.IsValid)
            {
                return View(mecz);
            }

            _meczService.UpdateMecz(mecz);
            return RedirectToAction("ListaMeczowAdmin");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var mecz = _meczService.GetMeczById(id);

            if (mecz == null)
            {
                return NotFound();
            }

            return View(mecz);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _meczService.DeleteMecz(id);
            return RedirectToAction("ListaMeczowAdmin");
        }
    }
}
