﻿using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sedziowanie.Data;
using Sedziowanie.Models;
using Sedziowanie.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sedziowanie.Services
{
    public class NiedyspozycjaService : INiedyspozycjaService
    {
        private readonly DBObsadyContext _context;

        public NiedyspozycjaService(DBObsadyContext context)
        {
            _context = context;
        }

       
        public List<Niedyspozycja> GetAllNiedyspozycje()
        {
            return _context.Niedyspozycje
                .Include(n => n.Sedzia)
                .ToList();
        }

        
        public List<SelectListItem> GetSedziowieList()
        {
            return _context.Sedziowie
                .Select(s => new SelectListItem
                {
                    Text = s.Imie + " " + s.Nazwisko,
                    Value = s.Id.ToString()
                })
                .ToList();
        }

        
       
        public Sedzia GetSedziaByUserId(string userId)
        {
            var user = _context.Users.Include(u => u.Sedzia).FirstOrDefault(u => u.Id == userId);
            return user?.Sedzia;
        }



        public void AddNiedyspozycja(int sedziaId, DateTime poczatek, DateTime koniec)
        {
            if (poczatek >= koniec)
            {
                throw new ArgumentException("Data początku musi być wcześniejsza niż data końca.");
            }

            var sedzia = _context.Sedziowie.FirstOrDefault(s => s.Id == sedziaId);
            if (sedzia == null)
            {
                throw new ArgumentException("Wybrany sędzia nie istnieje.");
            }

            var niedyspozycja = new Niedyspozycja
            {
                SedziaId = sedzia.Id,
                Poczatek = poczatek,
                Koniec = koniec
            };

            _context.Niedyspozycje.Add(niedyspozycja);
            _context.SaveChanges();
        }

       
    }
}
