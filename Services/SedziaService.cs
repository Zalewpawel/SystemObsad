using Microsoft.EntityFrameworkCore;
using Sedziowanie.Data;
using Sedziowanie.Models;
using Sedziowanie.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Sedziowanie.Services
{
    public class SedziaService : ISedziaService
    {
        private readonly DBObsadyContext _context;

        public SedziaService(DBObsadyContext context)
        {
            _context = context;
        }

        public List<Sedzia> GetAllSedziowie()
        {
            return _context.Sedziowie.ToList();
        }

        public Sedzia GetSedziaById(int id)
        {
            return _context.Sedziowie.FirstOrDefault(s => s.Id == id);
        }

        public void AddSedzia(Sedzia sedzia)
        {
            _context.Sedziowie.Add(sedzia);
            _context.SaveChanges();
        }

        public void UpdateSedzia(Sedzia sedzia)
        {
            _context.Sedziowie.Update(sedzia);
            _context.SaveChanges();
        }

        public void DeleteSedzia(int id)
        {
            var sedzia = _context.Sedziowie.FirstOrDefault(s => s.Id == id);
            if (sedzia != null)
            {
                _context.Sedziowie.Remove(sedzia);
                _context.SaveChanges();
            }
        }

        public List<object> GetMeczeForSedzia(int sedziaId)
        {
            return _context.Mecze
                .Include(m => m.Rozgrywki)
                .Include(m => m.SedziaI)
                .Include(m => m.SedziaII)
                .Include(m => m.SedziaSekretarz)
                .Where(m => m.SedziaIId == sedziaId || m.SedziaIIId == sedziaId || m.SedziaSekretarzId == sedziaId)
                .Select(m => new
                {
                    m.NumerMeczu,
                    m.Data,
                    m.Gospodarz,
                    m.Gosc,
                    Rozgrywki = m.Rozgrywki.Nazwa,
                    SedziaI = m.SedziaI.Imie + " " + m.SedziaI.Nazwisko,
                    SedziaII = m.SedziaII.Imie + " " + m.SedziaII.Nazwisko,
                    SedziaSekretarz = m.SedziaSekretarz.Imie + " " + m.SedziaSekretarz.Nazwisko
                })
                .ToList<object>();
        }

        public string GetSedziaName(int sedziaId)
        {
            return _context.Sedziowie
                .Where(s => s.Id == sedziaId)
                .Select(s => s.Imie + " " + s.Nazwisko)
                .FirstOrDefault();
        }
    }
}
