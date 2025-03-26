using Microsoft.EntityFrameworkCore;
using Sedziowanie.Data;
using Sedziowanie.Models;
using Sedziowanie.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sedziowanie.Services
{
    public class MeczService : IMeczService
    {
        private readonly DBObsadyContext _context;

        public MeczService(DBObsadyContext context)
        {
            _context = context;
        }

        public List<object> GetSedziowieByDate(DateTime date)
        {
            return _context.Sedziowie
                .Select(s => new
                {
                    Id = s.Id,
                    FullName = $"{s.Imie} {s.Nazwisko}",
                    Klasa = s.Klasa ?? "Brak",
                    CzyNiedostepny = _context.Niedyspozycje
                        .Any(n => n.SedziaId == s.Id && date >= n.Poczatek && date <= n.Koniec)
                })
                .ToList<object>();
        }

        public List<object> GetRozgrywki()
        {
            return _context.Rozgrywki
                .Select(r => new
                {
                    Id = r.Id,
                    Nazwa = r.Nazwa
                })
                .ToList<object>();
        }

        public List<Mecz> GetAllMecze()
        {
            return _context.Mecze
                .Include(m => m.Rozgrywki)
                .Include(m => m.SedziaI)
                .Include(m => m.SedziaII)
                .Include(m => m.SedziaSekretarz)
                .ToList();
        }

        public Mecz GetMeczById(int id)
        {
            return _context.Mecze
                .Include(m => m.Rozgrywki)
                .Include(m => m.SedziaI)
                .Include(m => m.SedziaII)
                .Include(m => m.SedziaSekretarz)
                .FirstOrDefault(m => m.Id == id);
        }

        public void AddMecz(Mecz mecz)
        {
            _context.Mecze.Add(mecz);
            _context.SaveChanges();
        }

        public void UpdateMecz(Mecz mecz)
        {
            _context.Mecze.Update(mecz);
            _context.SaveChanges();
        }

        public void DeleteMecz(int id)
        {
            var mecz = _context.Mecze.FirstOrDefault(m => m.Id == id);
            if (mecz != null)
            {
                _context.Mecze.Remove(mecz);
                _context.SaveChanges();
            }
        }
    }
}
