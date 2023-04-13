#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApplication.Models
{
    public class UserData
    {
        [Key]
        public Guid Id { get; set; }
        public string UserId { get; set; }

        [Display(Name = "Imię")]
        [StringLength(30)]
        [RegularExpression("^[A-ząćęłńóśźżĄĘŁŃÓŚŹŻ]{1,30}$", ErrorMessage = "Wymagane same litery")]
        [Required(ErrorMessage = "Pole {0} jest wymagane.")]
        public string Name { get; set; }

        [Display(Name = "Nazwisko")]
        [StringLength(30)]
        [RegularExpression("^[A-ząćęłńóśźżĄĘŁŃÓŚŹŻ]{1,30}$", ErrorMessage = "Wymagane same litery")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string Surname { get; set; }

        [Display(Name = "Miasto")]
        [StringLength(90)]
        [RegularExpression("^[A-ząćęłńóśźżĄĘŁŃÓŚŹŻ]{1,30}.?[A-ząćęłńóśźżĄĘŁŃÓŚŹŻ]{1,30}.?[A-ząćęłńóśźżĄĘŁŃÓŚŹŻ]{1,30}$", ErrorMessage = "Wymagane same litery, min. 3 znaki")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string City { get; set; }

        [Display(Name = "Ulica")]
        [StringLength(30)]
        [RegularExpression("^[A-ząćęłńóśźżĄĘŁŃÓŚŹŻ]{1,30}$", ErrorMessage = "Wymagane same litery")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string Street { get; set; }

        [Display(Name = "Numer domu")]
        [StringLength(6)]
        [RegularExpression("^[0-9]{1,6}$", ErrorMessage = "Wymagane same cyfry")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string HouseNumber { get; set; }

        [Display(Name = "Numer telefonu")]
        [RegularExpression("^[0-9]{3}-[0-9]{3}-[0-9]{3}$", ErrorMessage = "Prawidłowy format np. 123-456-789")]
        [Required(ErrorMessage = "Pole {0} jest wymagane")]
        public string PhoneNumber { get; set; }

        [NotMapped]
        public List<string> StatusName { get; set; } = new List<string>();
        public DateTime DateOrder { get; set; } = DateTime.Now;
        public int StatusId { get; set; } = 1;


        public ICollection<Order> Orders { get; set; }
        public Status Status { get; set; }

    }
}
