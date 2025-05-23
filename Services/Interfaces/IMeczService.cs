﻿using Sedziowanie.Models;
using System;
using System.Collections.Generic;

namespace Sedziowanie.Services.Interfaces
{
    public interface IMeczService
    {
        List<object> GetSedziowieByDate(DateTime date);
        List<object> GetRozgrywki();
        List<Mecz> GetAllMecze();
        Mecz GetMeczById(int id);
        void AddMecz(string numerMeczu, DateTime data, int rozgrywkiId, string gospodarz, string gosc,
                    int? sedziaIId, int? sedziaIIId, int? sedziaSekretarzId);
        void UpdateMecz(Mecz mecz);
        void DeleteMecz(int id);
    }
}
