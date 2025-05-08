using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using Sedziowanie.Data;
using Sedziowanie.Models;
using Sedziowanie.Services.Interfaces;

public class MeczService : IMeczService
{
    private readonly DBObsadyContext _context;
    private readonly IEmailService _emailService;

    public MeczService(DBObsadyContext context, IEmailService emailService)
    {
        _context = context;
        _emailService = emailService;
    }

    public void AddMecz(string numerMeczu, DateTime data, int rozgrywkiId, string gospodarz, string gosc,
                    int? sedziaIId, int? sedziaIIId, int? sedziaSekretarzId)
    {
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

        _context.Mecze.Add(mecz);
        _context.SaveChanges();

        List<string> sedziowieEmails = new List<string>();

        if (mecz.SedziaIId.HasValue)
            sedziowieEmails.Add(_context.Sedziowie.FirstOrDefault(s => s.Id == mecz.SedziaIId)?.Email);

        if (mecz.SedziaIIId.HasValue)
            sedziowieEmails.Add(_context.Sedziowie.FirstOrDefault(s => s.Id == mecz.SedziaIIId)?.Email);

        if (mecz.SedziaSekretarzId.HasValue)
            sedziowieEmails.Add(_context.Sedziowie.FirstOrDefault(s => s.Id == mecz.SedziaSekretarzId)?.Email);

        sedziowieEmails = sedziowieEmails.Where(email => !string.IsNullOrEmpty(email)).ToList();

        if (sedziowieEmails.Any())
        {
            Task.Run(() => SendMatchEmail(mecz, sedziowieEmails));
        }
    }

    private async Task SendMatchEmail(Mecz mecz, List<string> sedziowieEmails)
    {
        string subject = $"Nowy mecz: {mecz.NumerMeczu}";
        string body = $"Data: {mecz.Data}\nLokalizacja: {mecz.Gospodarz} vs {mecz.Gosc}\nRozgrywki ID: {mecz.RozgrywkiId}";

        foreach (var email in sedziowieEmails)
        {
            await _emailService.SendEmailAsync(email, subject, body);
        }
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
                .OrderBy(m => m.Data)
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

