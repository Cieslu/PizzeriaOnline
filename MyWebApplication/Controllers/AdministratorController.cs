using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApplication.Data;
using MyWebApplication.Models;
using Newtonsoft.Json;
using System.IO;

namespace MyWebApplication.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdministratorController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _environment;
        private readonly IEmailSender _emailSender;

        public AdministratorController(ApplicationDbContext db, IWebHostEnvironment environment, IEmailSender emailSender)
        {
            _db = db;
            _environment = environment;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult AdministratorCurrentIndex()
        {
            var Today = DateTime.Today;
            var CurrentOrders = _db.UserDatas.Where(x => x.DateOrder.Day == Today.Day && x.DateOrder.Month == Today.Month && x.DateOrder.Year == Today.Year).ToList();
            var status = _db.Status.ToList();


            CurrentOrders.ForEach(x =>
            {
                status.ForEach(s =>
                {
                    if (x.StatusId == s.Id)
                    {
                        x.StatusName.Add(s.Name);
                    }
                });
            });

            ViewBag.Status = new SelectList(_db.Status, "Id", "Name");
            return View(CurrentOrders);
        }

        [HttpGet]
        public IActionResult AdministratorCurrentDetails(Guid id)
        {
            var DetailsOrder = _db.UserDatas.Where(x => x.Id == id).ToList();
            var Details = _db.Orders.Where(x => x.UserDataId == id).ToList();
            var Quantity = _db.Orders.Where(x => x.UserDataId == id).Count();
            var Sum = _db.Orders.Where(x => x.UserDataId == id).Select(x => x.Price).Sum();

            ViewBag.OrderId = id;
            ViewBag.DetailsOrder = DetailsOrder;
            ViewBag.Quantity = Quantity;
            ViewBag.Sum = Sum;

            return View(Details);
        }

        [HttpGet]
        public IActionResult AdministratorArchivalIndex()
        {
            var Today = DateTime.Today;
            var ArchivalOrders = _db.UserDatas.Where(x => x.DateOrder.Day != Today.Day || x.DateOrder.Month != Today.Month || x.DateOrder.Year != Today.Year).ToList();
            var status = _db.Status.ToList();

            ArchivalOrders.ForEach(x =>
            {
                status.ForEach(s =>
                {
                    if (x.StatusId == s.Id)
                    {
                        x.StatusName.Add(s.Name);
                    }
                });
            });

            return View(ArchivalOrders);
        }

        [HttpGet]
        public IActionResult AdministratorArchivalDetails(Guid id)
        {
            var DetailsOrder = _db.UserDatas.Where(x => x.Id == id).ToList();
            var Details = _db.Orders.Where(x => x.UserDataId == id).ToList();
            var Quantity = _db.Orders.Where(x => x.UserDataId == id).Count();
            var Sum = _db.Orders.Where(x => x.UserDataId == id).Select(x => x.Price).Sum();

            ViewBag.DetailsOrder = DetailsOrder;
            ViewBag.Quantity = Quantity;
            ViewBag.Sum = Sum;

            return View(Details);
        }

        [HttpGet]
        public IActionResult StatusEdit(Guid id)
        {
            var Order = _db.UserDatas.Where(x => x.Id == id).FirstOrDefault();
            ViewBag.Status = new SelectList(_db.Status, "Id", "Name");

            return View(Order);
        }

        [HttpPost]
        public async Task<IActionResult> StatusEdit(Guid id, UserData userData)
        {
            ViewBag.Status = new SelectList(_db.Status, "Id", "Name");

            var Order = _db.UserDatas.Where(x => x.Id == id).FirstOrDefault();

            if (Order == null)
            {
                return NotFound();
            }

            Order.StatusId = userData.StatusId;
            _db.SaveChanges();

            var Status = _db.Status.ToList();

            var StatusName = "";

            Status.ForEach(x =>
            {
                if (x.Id == Order.StatusId)
                {
                    StatusName = x.Name;
                }
            });

            String body = "Zamówienie o numerze: <span style='font-weight: bold;'>" + Order.Id + "</span> </br> zmieniło status na: <span style='font-weight: bold;'>" + StatusName + "</span>";
            await _emailSender.SendEmailAsync("szymonciesla1406@gmail.com", "Pizzeria", body);
            return RedirectToAction(nameof(StatusEdit));
        }

        [HttpGet]
        public IActionResult AdministratorPizzaIndex()
        {
            var Pizzas = _db.Pizzas.ToList();
            var MeatsVegetables = _db.MeatsVegetables.ToList();
            var Meats = _db.Meats.ToList();
            var Vegetables = _db.Vegetables.ToList();
            var Sizes = _db.Sizes.ToList();
            var Doughs = _db.Doughs.ToList();
            var Sauces = _db.Sauces.ToList();
            var Cheeses = _db.Cheeses.ToList();

            Pizzas.ForEach(pizzasItem =>
            {
                Sizes.ForEach(sizesItem =>
                {
                    if (pizzasItem.SizeId == sizesItem.Id)
                    {
                        pizzasItem.AllIngredients.Add(sizesItem.Name + ", ");
                    }
                });

                Doughs.ForEach(doughsItem =>
                {
                    if (pizzasItem.DoughId == doughsItem.Id)
                    {
                        pizzasItem.AllIngredients.Add(doughsItem.Name + ", ");
                    }
                });

                Sauces.ForEach(saucesItem =>
                {
                    if (pizzasItem.SauceId == saucesItem.Id)
                    {
                        pizzasItem.AllIngredients.Add(saucesItem.Name + ", ");
                    }
                });

                Cheeses.ForEach(cheesesItem =>
                {
                    if (pizzasItem.CheeseId == cheesesItem.Id)
                    {
                        pizzasItem.AllIngredients.Add(cheesesItem.Name + ", ");
                    }
                });

                MeatsVegetables.ForEach(meatsVegetablesItem =>
                {
                    Meats.ForEach(meatsItem =>
                    {
                        if (pizzasItem.Id == meatsVegetablesItem.PizzaId & meatsVegetablesItem.MeatId == meatsItem.Id)
                        {
                            pizzasItem.AllIngredients.Add(meatsItem.Name + ", ");
                        }
                    });

                    Vegetables.ForEach(vegetablesItem =>
                    {
                        if (pizzasItem.Id == meatsVegetablesItem.PizzaId & meatsVegetablesItem.VegetableId == vegetablesItem.Id)
                        {
                            pizzasItem.AllIngredients.Add(vegetablesItem.Name + ", ");

                        }
                    });
                });
            });

            return View(Pizzas);
        }

        [HttpGet]
        public IActionResult AdministratorPizzaAdd()
        {
            ViewBag.Size = new SelectList(_db.Sizes, "Id", "Name");
            ViewBag.Dough = new SelectList(_db.Doughs, "Id", "Name");
            ViewBag.Sauce = new SelectList(_db.Sauces, "Id", "Name");
            ViewBag.Cheese = new SelectList(_db.Cheeses, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdministratorPizzaAdd(Pizza p)
        {
            if (ModelState.IsValid)
            {
                if (p.Image != null && p.Name != null)
                {
                    string path = Path.Combine(_environment.WebRootPath, "pizza_images", p.Name);
                    Directory.CreateDirectory(path);

                    var image = p.Image.FileName;
                    var fileName = Path.GetFileNameWithoutExtension(image);
                    var extension = Path.GetExtension(image);
                    if (extension == ".jpg" || extension == ".png")
                    {
                        fileName = p.Name + extension;
                        var filePath = Path.Combine(path, fileName); 
                        using var fs = new FileStream(filePath, FileMode.Create);
                        p.Image.CopyTo(fs);

                        var newPizza = new Pizza()
                        {
                            Name = p.Name,
                            SizeId = p.SizeId,
                            DoughId = p.DoughId,
                            SauceId = p.SauceId,
                            CheeseId = p.CheeseId,
                            ImagePath = fileName
                        };

                        return RedirectToAction(nameof(AdministratorPizzaAddMeatsAndVegetables), newPizza);
                    }
                    else
                    {
                        ViewData["Error"] = "Wymagany format to jpg lub png";
                    }
                }
                else if (p.Image == null && p.Name == null)
                {
                    ViewData["ErrorName"] = "Pole Nazwa jest wymagane";
                    ViewData["ErrorImage"] = "Pole Dodaj zdjęcie jest wymagane";
                }
                else if (p.Image != null && p.Name == null)
                {
                    ViewData["ErrorName"] = "Pole Nazwa jest wymagane";
                }
                else if (p.Image == null && p.Name != null)
                {
                    ViewData["ErrorImage"] = "Pole Dodaj zdjęcie jest wymagane";
                }
            }

            ViewBag.Size = new SelectList(_db.Sizes, "Id", "Name");
            ViewBag.Dough = new SelectList(_db.Doughs, "Id", "Name");
            ViewBag.Sauce = new SelectList(_db.Sauces, "Id", "Name");
            ViewBag.Cheese = new SelectList(_db.Cheeses, "Id", "Name");
            return View();
        }

        [HttpGet]
        public IActionResult AdministratorPizzaAddMeatsAndVegetables()
        {
            ViewBag.Meat = new MultiSelectList(_db.Meats, "Id", "Name");
            ViewBag.Vegetable = new MultiSelectList(_db.Vegetables, "Id", "Name");
            ViewBag.MeatsQuantity = _db.Meats.Count();
            ViewBag.VegetablesQuantity = _db.Vegetables.Count();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdministratorPizzaAddMeatsAndVegetables(MeatVegetable mv, Pizza p)
        {
            _db.Pizzas.Add(p);
            _db.SaveChanges();

            var PizzaId = _db.Pizzas.Select(x => x.Id).OrderBy(x => x).LastOrDefault();

            foreach (var m in mv.Meats)
            {
                var Ingredniets = new MeatVegetable()
                {
                    PizzaId = PizzaId,
                    MeatId = m
                };
                _db.Add(Ingredniets);
            }

            foreach (var v in mv.Vegetables)
            {
                var Ingredients = new MeatVegetable()
                {
                    PizzaId = PizzaId,
                    VegetableId = v
                };
                _db.Add(Ingredients);
            }
            _db.SaveChanges();

            ViewBag.Meat = new MultiSelectList(_db.Meats, "Id", "Name");
            ViewBag.Vegetable = new MultiSelectList(_db.Vegetables, "Id", "Name");
            ViewBag.MeatsQuantity = _db.Meats.Count();
            ViewBag.VegetablesQuantity = _db.Vegetables.Count();

            return RedirectToAction(nameof(AdministratorPizzaIndex));
        }

        [HttpGet]
        public IActionResult AdministratorPizzaEdit(int id)
        {
            var Pizza = _db.Pizzas.FirstOrDefault(p => p.Id == id);

            if (Pizza == null)
            {
                return NotFound();
            }
            

            ViewBag.Size = new SelectList(_db.Sizes, "Id", "Name");
            ViewBag.Dough = new SelectList(_db.Doughs, "Id", "Name");
            ViewBag.Sauce = new SelectList(_db.Sauces, "Id", "Name");
            ViewBag.Cheese = new SelectList(_db.Cheeses, "Id", "Name");
            return View(Pizza);
        }

        [HttpPost]
        public IActionResult AdministratorPizzaEdit(int id, Pizza p)
        {
            var Pizza = _db.Pizzas.Find(id);

            if (Pizza == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (p.Name != null)
                {
                    string path = Path.Combine(_environment.WebRootPath, "pizza_images", Pizza.Name);
                    string newPath = Path.Combine(_environment.WebRootPath, "pizza_images", p.Name);

                    if (Pizza.Name != p.Name || Pizza.Name == p.Name)
                    {
                        if (Pizza.Name != p.Name && p.Image == null)
                        {
                            Directory.Move(path, newPath);
                            Pizza.Name = p.Name;
                            _db.SaveChanges();
                            return RedirectToAction(nameof(AdministratorPizzaAddMeatsAndVegetablesEdit), new { id });

                        }
                        if (p.Image != null)
                        {
                            var image = p.Image.FileName;
                            var fileName = Path.GetFileNameWithoutExtension(image);
                            var extension = Path.GetExtension(image);
                            if (extension == ".jpg" || extension == ".png")
                            {
                                Directory.Delete(path, true);
                                Directory.CreateDirectory(newPath);
                                fileName = p.Name + extension;

                                if (Pizza.Name != p.Name)
                                {
                                    var filePath = Path.Combine(newPath, fileName);
                                    using var fs = new FileStream(filePath, FileMode.Create);
                                    p.Image.CopyTo(fs);
                                }
                                else
                                {
                                    var filePath = Path.Combine(path, fileName);
                                    using var fs = new FileStream(filePath, FileMode.Create);
                                    p.Image.CopyTo(fs);
                                }

                                Pizza.Name = p.Name;
                                Pizza.SizeId = p.SizeId;
                                Pizza.DoughId = p.DoughId;
                                Pizza.SauceId = p.SauceId;
                                Pizza.CheeseId = p.CheeseId;
                                Pizza.ImagePath = fileName;
                                _db.SaveChanges();
                                return RedirectToAction(nameof(AdministratorPizzaAddMeatsAndVegetablesEdit), new { id });
                            }
                            else
                            {
                                ViewData["Error"] = "Wymagany format to jpg lub png";

                            }
                        }
                        else
                        {
                            Pizza.Name = p.Name;
                            Pizza.SizeId = p.SizeId;
                            Pizza.DoughId = p.DoughId;
                            Pizza.SauceId = p.SauceId;
                            Pizza.CheeseId = p.CheeseId;
                            _db.SaveChanges();
                            return RedirectToAction(nameof(AdministratorPizzaAddMeatsAndVegetablesEdit), new { id });

                        }

                        ViewBag.Size = new SelectList(_db.Sizes, "Id", "Name");
                        ViewBag.Dough = new SelectList(_db.Doughs, "Id", "Name");
                        ViewBag.Sauce = new SelectList(_db.Sauces, "Id", "Name");
                        ViewBag.Cheese = new SelectList(_db.Cheeses, "Id", "Name");
                        return View();
                    }
                }
                else if (p.Image == null)
                {
                    ViewData["ErrorName"] = "Pole Nazwa jest wymagane";
                }
            }

            ViewBag.Size = new SelectList(_db.Sizes, "Id", "Name");
            ViewBag.Dough = new SelectList(_db.Doughs, "Id", "Name");
            ViewBag.Sauce = new SelectList(_db.Sauces, "Id", "Name");
            ViewBag.Cheese = new SelectList(_db.Cheeses, "Id", "Name");

            return View();
        }

        [HttpGet]
        public IActionResult AdministratorPizzaAddMeatsAndVegetablesEdit(int id, MeatVegetable mv)
        {
            var Meat = _db.MeatsVegetables.Where(x => x.PizzaId == id && x.MeatId != null).Select(x => x.MeatId).ToList();

            var Vegetable = _db.MeatsVegetables.Where(x => x.PizzaId == id && x.VegetableId != null).Select(x => x.VegetableId).ToList();

            Meat.ForEach(x =>
            {
                mv.Meats.Add(Convert.ToInt32(x));
            });

            ViewBag.xd = Meat;

            Vegetable.ForEach(x =>
            {
                mv.Vegetables.Add(Convert.ToInt32(x));
            });

            ViewBag.Meat = new MultiSelectList(_db.Meats, "Id", "Name");
            ViewBag.Vegetable = new MultiSelectList(_db.Vegetables, "Id", "Name");
            ViewBag.MeatsQuantity = _db.Meats.Count();
            ViewBag.VegetablesQuantity = _db.Vegetables.Count();
            return View(mv);
        }

        [HttpPost]
        public IActionResult AdministratorPizzaAddMeatsAndVegetablesEdit(int id, MeatVegetable mv, Pizza p)
        {
            var MeatVegetable = _db.MeatsVegetables.Where(x => x.PizzaId == id).ToList();

            MeatVegetable.ForEach(mv =>
            {
                if (mv.PizzaId == id)
                {
                    _db.MeatsVegetables.Remove(mv);
                }
            });

            foreach (var m in mv.Meats)
            {
                var Ingredniets = new MeatVegetable()
                {
                    PizzaId = id,
                    MeatId = m
                };
                _db.Add(Ingredniets);
            }

            foreach (var v in mv.Vegetables)
            {
                var Ingredients = new MeatVegetable()
                {
                    PizzaId = id,
                    VegetableId = v
                };
                _db.Add(Ingredients);
            }
            _db.SaveChanges();

            ViewBag.Meat = new MultiSelectList(_db.Meats, "Id", "Name");
            ViewBag.Vegetable = new MultiSelectList(_db.Vegetables, "Id", "Name");
            ViewBag.MeatsQuantity = _db.Meats.Count();
            ViewBag.VegetablesQuantity = _db.Vegetables.Count();

            return RedirectToAction(nameof(AdministratorPizzaIndex));
        }

        public IActionResult AdministratorPizzaDelete(int id)
        {
            var Pizza = _db.Pizzas.Find(id);

            if (Pizza != null)
            {
                string path = Path.Combine(_environment.WebRootPath, "pizza_images", Pizza.Name);
                Directory.Delete(path, true);
                _db.Pizzas.Remove(Pizza);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(AdministratorPizzaIndex));
        }

        public IActionResult AdministratorChartIndex()
        {
            List<int> MonthsInt = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            List<string> MonthsString = new List<string> { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };


            List<int> OrderQuantity = new List<int>();

            MonthsInt.ForEach(x =>
            {
                OrderQuantity.Add(_db.UserDatas.Where(y => y.DateOrder.Month == x).Count());
            });

            ViewBag.Order = _db.UserDatas.Count();
            ViewBag.OrderQuantity = OrderQuantity;
            ViewBag.MonthsString = MonthsString;

            return View();
        }

        [HttpGet]
        public IActionResult AdministratorChartDetails(string id)
        {
            List<string> PizzaLabel = new List<string> { "Kreator pizzy", "Pizze edytowane", "Pizze gotowe" };
            List<string> Pizza = new List<string> { "Kreator pizzy", "Edytowano", "Inne" };

            List<int> PizzaQuantity = new List<int>();

            List<int> MonthsInt = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
            List<string> MonthsString = new List<string> { "Styczeń", "Luty", "Marzec", "Kwiecień", "Maj", "Czerwiec", "Lipiec", "Sierpień", "Wrzesień", "Październik", "Listopad", "Grudzień" };

            int MonthInt = 0;

            for (int i = 0; i < MonthsString.Count; i++)
            {
                if (MonthsString[i] == id)
                {
                    MonthInt = i + 1;
                }
            }

            Pizza.ForEach(x =>
            {
                if (x == "Inne")
                {
                    PizzaQuantity.Add(_db.Orders.Where(o => !o.PizzaName.Contains("Kreator pizzy") && !o.PizzaName.Contains("Edytowano") && o.UserData.DateOrder.Month == MonthInt).Count());
                }

                PizzaQuantity.Add(_db.Orders.Where(o => o.PizzaName.Contains(x) && o.UserData.DateOrder.Month == MonthInt).Count());
            });

            ViewBag.Quantity = _db.Orders.Where(x => x.UserData.DateOrder.Month == MonthInt).Count();
            ViewBag.Month = id;
            ViewBag.PizzaLabel = PizzaLabel;
            ViewBag.PizzaQuantity = PizzaQuantity;

            return View();
        }

        public IActionResult AdministratorOpinions()
        {
            var Opinions = _db.Opinions.ToList();

            return View(Opinions);
        }

        public IActionResult AdministratorOpinionsDelete(int id)
        {
            var Opinion = _db.Opinions.Find(id);

            if (Opinion != null)
            {
                _db.Opinions.Remove(Opinion);
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(AdministratorOpinions));
        }
    }
}
