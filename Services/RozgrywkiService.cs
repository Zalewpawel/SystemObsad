﻿using Microsoft.EntityFrameworkCore;
using Sedziowanie.Data;
using Sedziowanie.Models;
using Sedziowanie.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Sedziowanie.Services
{
    public class RozgrywkiService : IRozgrywkiService
    {
        private readonly DBObsadyContext _context;

        public RozgrywkiService(DBObsadyContext context)
        {
            _context = context;
        }

        public void AddRozgrywki(Rozgrywki rozgrywki)
        {
            _context.Rozgrywki.Add(rozgrywki);
            _context.SaveChanges();
        }

        public List<Rozgrywki> GetAllRozgrywki()
        {
            return _context.Rozgrywki.ToList();
        }

        public List<object> GetMeczeForRozgrywki(int rozgrywkiId)
        {
            return _context.Mecze
                .Include(m => m.SedziaI)
                .Include(m => m.SedziaII)
                .Include(m => m.SedziaSekretarz)
                .Where(m => m.RozgrywkiId == rozgrywkiId)
                .Select(m => new
                {
                    m.Id,
                    m.NumerMeczu,
                    m.Data,
                    m.Gospodarz,
                    m.Gosc,
                    m.SedziaI,
                    m.SedziaII,
                    m.SedziaSekretarz
                })
                .OrderBy(m=>m.Data)
                .ToList<object>();
        }

        public string GetRozgrywkaName(int rozgrywkiId)
        {
            return _context.Rozgrywki
                .Where(r => r.Id == rozgrywkiId)
                .Select(r => r.Nazwa)
                .FirstOrDefault();
        }

       
    }
}
