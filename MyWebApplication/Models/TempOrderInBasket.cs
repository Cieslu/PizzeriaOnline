#nullable disable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApplication.Models
{
    public class TempOrderInBasket
    {
        [Key]
        public int Id { get; set; }
        public string TempSessionId { get; set; }
        public string TempPizzaName { get; set; }
        public string TempSize { get; set; }
        public string TempDough { get; set; }
        public string TempSauce { get; set; }
        public string TempCheese { get; set; }
        public string TempMeat { get; set; }
        public string TempVegetable { get; set; }
        public double TempPrice { get; set; }

        [NotMapped]
        public string TempOrderPizzaName { get; set; }
        [NotMapped]
        public string TempOrderSize { get; set; }
        [NotMapped]
        public string TempOrderDough { get; set; }
        [NotMapped]
        public string TempOrderSauce { get; set; }
        [NotMapped]
        public string TempOrderCheese { get; set; }
        [NotMapped]
        public string TempOrderMeat { get; set; }
        [NotMapped]
        public string TempOrderVegetable { get; set; }
        [NotMapped]
        public double TempOrderPrice { get; set; }
        [NotMapped]
        public double TempWholeOrderPrice { get; set; } 
    }
}
