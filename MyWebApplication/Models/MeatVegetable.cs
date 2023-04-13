#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApplication.Models
{
    public class MeatVegetable
    {
        public int Id { get; set; }
        public int PizzaId { get; set; }

        [Display(Name = "Mięso")]
        public int? MeatId { get; set; }

        [Display(Name = "Warzywa")]
        public int? VegetableId { get; set; }

        [NotMapped]
        [Display(Name = "Mięso")]
        public IList<int> Meats { get; set; } = new List<int>();

        [NotMapped]
        [Display(Name = "Warzywa")]
        public IList<int> Vegetables { get; set; } = new List<int>();

        public Meat Meat { get; set; }
        public Vegetable Vegetable { get; set; }
        public Pizza Pizza { get; set; }
    }
}
