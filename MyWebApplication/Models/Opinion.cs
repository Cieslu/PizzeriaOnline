#nullable disable
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication.Models
{
    public class Opinion
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Imię")]
        [StringLength(50)]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        public string Name { get; set; }

        [Display(Name = "Opis")]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        public string Description { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
