using System.ComponentModel.DataAnnotations;

namespace Sedziowanie.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Imię")]
        public string Imie { get; set; }

        [Required]
        [Display(Name = "Nazwisko")]
        public string Nazwisko { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Hasło")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hasła nie są identyczne.")]
        [Display(Name = "Potwierdź hasło")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
