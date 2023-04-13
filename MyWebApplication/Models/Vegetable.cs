#nullable disable
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication.Models
{
    public class Vegetable
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }

        public ICollection<MeatVegetable> MeatsVegetables { get; set; }
    }
}
