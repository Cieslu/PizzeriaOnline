#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApplication.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public string PizzaName { get; set; }
        public string Size { get; set; }
        public string Dough { get; set; }
        public string Sauce { get; set; }
        public string Cheese { get; set; }
        public string Meat { get; set; }
        public string Vegetable { get; set; }
        public double Price { get; set; }

        [ForeignKey("UserData")]
        public Guid UserDataId { get; set; }
        public UserData UserData { get; set; }
    }
}
