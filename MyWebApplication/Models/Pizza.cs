#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyWebApplication.Models
{
    public class Pizza
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [NotMapped]
        public double WholePrice { get; set; }
        [NotMapped]
        public List<string> AllIngredients { get; set; } = new List<string>();
        [NotMapped]
        public List<string> MeatIngredients { get; set; } = new List<string>();
        [NotMapped]
        public List<string> VegetableIngredients { get; set; } = new List<string>();

        [Display(Name = "Rozmiar")]
        public int SizeId { get; set; }

        [Display(Name = "Ciasto")]
        public int DoughId { get; set; }

        [Display(Name = "Sos")]
        public int SauceId { get; set; }

        [Display(Name = "Ser")]
        public int CheeseId { get; set; }

        [NotMapped]
        public ICollection<int> MeatId { get; set; } = new List<int>();
        [NotMapped]
        public ICollection<int> VegetableId { get; set; } = new List<int>();

        [NotMapped]
        public string SizeRadio { get; set; }
        [NotMapped]
        public string DoughRadio { get; set; }
        [NotMapped]
        public string SauceRadio { get; set; }
        [NotMapped]
        public string CheeseRadio { get; set; }

        [NotMapped]
        public string MeatCheckbox1 { get; set; }
        [NotMapped]
        public string MeatCheckbox2 { get; set; }
        [NotMapped]
        public string MeatCheckbox3 { get; set; }
        [NotMapped]
        public string MeatCheckbox4 { get; set; }
        [NotMapped]
        public string MeatCheckbox5 { get; set; }
        [NotMapped]
        public string MeatCheckbox6 { get; set; }

        [NotMapped]
        public string VegetableCheckbox1 { get; set; }
        [NotMapped]
        public string VegetableCheckbox2 { get; set; }
        [NotMapped]
        public string VegetableCheckbox3 { get; set; }
        [NotMapped]
        public string VegetableCheckbox4 { get; set; }
        [NotMapped]
        public string VegetableCheckbox5 { get; set; }
        [NotMapped]
        public string VegetableCheckbox6 { get; set; }
        [NotMapped]
        public string VegetableCheckbox7 { get; set; }
        [NotMapped]
        public string VegetableCheckbox8 { get; set; }

        [NotMapped]
        public double SizeRadioPrice { get; set; }
        [NotMapped]
        public double DoughRadioPrice { get; set; }
        [NotMapped]
        public double SauceRadioPrice { get; set; }
        [NotMapped]
        public double CheeseRadioPrice { get; set; }
        [NotMapped]
        public double MeatCheckboxPrice { get; set; }
        [NotMapped]
        public double VegetableCheckboxPrice { get; set; }

        [NotMapped]
        [Display(Name = "Nazwa pizzy")]
        [StringLength(20)]
        public string OwnPizzaName { get; set; }

        [NotMapped]
        [Display(Name = "Dodaj zdjęcie")]    
        public IFormFile Image { get; set; }

        public string ImagePath { get; set; } 

        public Size Size { get; set; }
        public Dough Dough { get; set; }
        public Sauce Sauce { get; set; }
        public Cheese Cheese { get; set; }
        public ICollection<MeatVegetable> MeatsVegetables { get; set; }
    }
}
