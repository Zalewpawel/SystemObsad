using Microsoft.AspNetCore.Identity;

namespace Sedziowanie.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? Imie { get; set; }
        public string? Nazwisko { get; set; }
    }
}
