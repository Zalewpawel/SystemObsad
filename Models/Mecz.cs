using System.ComponentModel.DataAnnotations;


namespace Sedziowanie.Models
{
    public class Mecz
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(5)]
        public string NumerMeczu { get; set; }

        [Required(ErrorMessage = "Data meczu jest wymagana.")]
        public DateTime Data { get; set; }

        [Required(ErrorMessage = "Rozgrywki są wymagane.")]
        public int RozgrywkiId { get; set; }

        [MaxLength(100)]
        public string Gospodarz { get; set; }

        [MaxLength(100)]
        public string Gosc { get; set; }

        
        public int? SedziaIId { get; set; }
        public int? SedziaIIId { get; set; }
        public int? SedziaSekretarzId { get; set; }

        public virtual Rozgrywki Rozgrywki { get; set; }
        public virtual Sedzia SedziaI { get; set; }
        public virtual Sedzia SedziaII { get; set; }
        public virtual Sedzia SedziaSekretarz { get; set; }
    }

}
