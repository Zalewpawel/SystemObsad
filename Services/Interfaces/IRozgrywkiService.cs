using Sedziowanie.Models;
using System.Collections.Generic;

namespace Sedziowanie.Services.Interfaces
{
    public interface IRozgrywkiService
    {
        void AddRozgrywki(Rozgrywki rozgrywki);
        List<Rozgrywki> GetAllRozgrywki();
        List<object> GetMeczeForRozgrywki(int rozgrywkiId);
        string GetRozgrywkaName(int rozgrywkiId);
    }
}
