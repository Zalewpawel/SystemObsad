using Sedziowanie.Models;
using System;
using System.Collections.Generic;

namespace Sedziowanie.Services.Interfaces
{
    public interface INiedyspozycjaService
    {
        List<Niedyspozycja> GetAllNiedyspozycje();
        List<object> GetSedziowieList();
        Sedzia GetSedziaByUserId(string userId);

        void AddNiedyspozycja(int sedziaId, DateTime poczatek, DateTime koniec);
    }
}
