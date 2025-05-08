using Microsoft.AspNetCore.Mvc;
using Sedziowanie.Models;
using Sedziowanie.Services.Interfaces;
using System;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

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

            ViewBag.Sedziowie = _meczService.GetSedziowieByDate(dzisiaj);
            ViewBag.Rozgrywki = _meczService.GetRozgrywki();
            ViewBag.DataMeczu = dzisiaj.ToString("yyyy-MM-ddTHH:mm");

            return View(new Mecz { Data = dzisiaj });
        }



        [HttpPost]
        public IActionResult Add(string numerMeczu, DateTime data, int rozgrywkiId, string gospodarz, string gosc,
                          int? sedziaIId, int? sedziaIIId, int? sedziaSekretarzId)
        {
            
          

            if (rozgrywkiId == 0)
            {
                ModelState.AddModelError("rozgrywkiId", "Rozgrywki są wymagane.");
            }

       
            if (!ModelState.IsValid)
            {
                ViewBag.Sedziowie = _meczService.GetSedziowieByDate(data);
                ViewBag.Rozgrywki = _meczService.GetRozgrywki();
                ViewBag.DataMeczu = data.ToString("yyyy-MM-ddTHH:mm");
                return View();
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

           
            _meczService.AddMecz(numerMeczu, data, rozgrywkiId, gospodarz, gosc,
                    sedziaIId, sedziaIIId, sedziaSekretarzId);
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
