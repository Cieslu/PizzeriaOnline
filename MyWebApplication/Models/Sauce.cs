#nullable disable
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication.Models
{
    public class Sauce
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public ICollection<Pizza> Pizzas { get; set; }
    }
}
