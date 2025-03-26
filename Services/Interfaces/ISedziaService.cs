using Sedziowanie.Models;
using System.Collections.Generic;

namespace Sedziowanie.Services.Interfaces
{
    public interface ISedziaService
    {
        List<Sedzia> GetAllSedziowie();
        Sedzia GetSedziaById(int id);
        void AddSedzia(Sedzia sedzia);
        void UpdateSedzia(Sedzia sedzia);
        void DeleteSedzia(int id);
        List<object> GetMeczeForSedzia(int sedziaId);
        string GetSedziaName(int sedziaId);
    }
}
