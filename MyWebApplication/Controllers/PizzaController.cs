#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebApplication.Data;
using MyWebApplication.Models;
using Newtonsoft.Json;
using System.Dynamic;

namespace MyWebApplication.Controllers
{
    public class PizzaController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public PizzaController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult PizzaIndex()
        {
            var Pizzas = _db.Pizzas.ToList();
            var MeatsVegetables = _db.MeatsVegetables.ToList();
            var Meats = _db.Meats.ToList();
            var Vegetables = _db.Vegetables.ToList();
            var Sauces = _db.Sauces.ToList();
            var Cheeses = _db.Cheeses.ToList();

            Pizzas.ForEach(pizzasItem =>
            {       
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

            if (User.Identity.IsAuthenticated)
            {
                var tempWholeOrderPrice = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).Sum(x => x.TempPrice);
                var tempOrder = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).ToList();
                var tempOrderCount = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).Count();
                ViewBag.tempWholeOrderPrice = tempWholeOrderPrice;
                ViewBag.order = tempOrder;
                ViewBag.orderCount = tempOrderCount;
            }
            else if (HttpContext.Session.GetString("SessionId") != null)
            {
                var tempWholeOrderPrice = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).Sum(x => x.TempPrice);
                var tempOrder = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).ToList();
                var tempOrderCount = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).Count();
                ViewBag.tempWholeOrderPrice = tempWholeOrderPrice;
                ViewBag.order = tempOrder;
                ViewBag.orderCount = tempOrderCount;
            }
            return View(Pizzas);        
        }

        [HttpGet]
        public IActionResult PizzaDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Pizzas = _db.Pizzas.FirstOrDefault(x => x.Id == id);
            var Ingredients = _db.MeatsVegetables.Where(x => x.PizzaId == id).ToList();

            if (Pizzas == null)
            {
                return NotFound();
            }

            var Size = _db.Sizes.Find(Pizzas.SizeId);
            var Dough = _db.Doughs.Find(Pizzas.DoughId);
            var Sauce = _db.Sauces.Find(Pizzas.SauceId);
            var Cheese = _db.Cheeses.Find(Pizzas.CheeseId);
            var Meat = _db.Meats.ToList();
            var Vegetable = _db.Vegetables.ToList();

            if (Pizzas.SizeId == Size.Id)
            {
                ViewBag.SizeName = Size.Name;
                Pizzas.WholePrice += Size.Price;
            }


            if (Pizzas.DoughId == Dough.Id)
            {
                ViewBag.DoughName = Dough.Name;
                Pizzas.WholePrice += Dough.Price;
            }


            if (Pizzas.SauceId == Sauce.Id)
            {
                ViewBag.SauceName = Sauce.Name;
                Pizzas.WholePrice += Sauce.Price;
            }


            if (Pizzas.CheeseId == Cheese.Id)
            {
                ViewBag.CheeseName = Cheese.Name;
                Pizzas.WholePrice += Cheese.Price;
            }

            Ingredients.ForEach(ingredients =>
            {
                Meat.ForEach(meat =>
                {
                    if (ingredients.MeatId == meat.Id)
                    {
                        Pizzas.MeatIngredients.Add(meat.Name);
                        Pizzas.WholePrice += meat.Price;
                    }
                });
            });

            Ingredients.ForEach(ingredients =>
            {
                Vegetable.ForEach(vegetable =>
                {
                    if (ingredients.VegetableId == vegetable.Id)
                    {
                        Pizzas.VegetableIngredients.Add(vegetable.Name);
                        Pizzas.WholePrice += vegetable.Price;
                    }
                });
            });


            if (User.Identity.IsAuthenticated)
            {
                var tempWholeOrderPrice = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).Sum(x => x.TempPrice);
                var tempOrder = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).ToList();
                var tempOrderCount = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).Count();
                ViewBag.tempWholeOrderPrice = tempWholeOrderPrice;
                ViewBag.order = tempOrder;
                ViewBag.orderCount = tempOrderCount;
                TempData["id"] = id;
            }
            else if (HttpContext.Session.GetString("SessionId") != null)
            {
                var tempWholeOrderPrice = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).Sum(x => x.TempPrice);
                var tempOrder = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).ToList();
                var tempOrderCount = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).Count();
                ViewBag.tempWholeOrderPrice = tempWholeOrderPrice;
                ViewBag.order = tempOrder;
                ViewBag.orderCount = tempOrderCount;
                TempData["id"] = id;
            }
            return View(Pizzas);
        }

        [HttpPost]
        public IActionResult PizzaDetails(int? id, TempOrderInBasket tmp)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Pizzas = _db.Pizzas.FirstOrDefault(x => x.Id == id);
            var Ingredients = _db.MeatsVegetables.Where(x => x.PizzaId == id).ToList();

            if (Pizzas == null)
            {
                return NotFound();
            }

            var Size = _db.Sizes.Find(Pizzas.SizeId);
            var Dough = _db.Doughs.Find(Pizzas.DoughId);
            var Sauce = _db.Sauces.Find(Pizzas.SauceId);
            var Cheese = _db.Cheeses.Find(Pizzas.CheeseId);
            var Meat = _db.Meats.ToList();
            var Vegetable = _db.Vegetables.ToList();

            if (Pizzas.SizeId == Size.Id)
            {
                tmp.TempOrderSize = Size.Name;
                tmp.TempOrderPrice += Size.Price;
            }


            if (Pizzas.DoughId == Dough.Id)
            {
                tmp.TempOrderDough = Dough.Name;
                tmp.TempOrderPrice += Dough.Price;
            }


            if (Pizzas.SauceId == Sauce.Id)
            {
                tmp.TempOrderSauce = Sauce.Name;
                tmp.TempOrderPrice += Sauce.Price;
            }


            if (Pizzas.CheeseId == Cheese.Id)
            {
                tmp.TempOrderCheese = Cheese.Name;
                tmp.TempOrderPrice += Cheese.Price;
            }

            Ingredients.ForEach(ingredients =>
            {
                Meat.ForEach(meat =>
                {
                    if (ingredients.MeatId == meat.Id)
                    {
                        tmp.TempOrderMeat += meat.Name + ", ";
                        tmp.TempOrderPrice += meat.Price;
                    }
                });
            });

            Ingredients.ForEach(ingredients =>
            {
                Vegetable.ForEach(vegetable =>
                {
                    if (ingredients.VegetableId == vegetable.Id)
                    {
                        tmp.TempOrderVegetable += vegetable.Name + ", ";
                        tmp.TempOrderPrice += vegetable.Price;
                    }
                });
            });
          
            tmp.TempOrderPizzaName = Pizzas.Name;

            if(tmp.TempOrderMeat != null)
            {
                ViewBag.TempOrderMeatSubstr = tmp.TempOrderMeat.Substring(0, tmp.TempOrderMeat.Length - 2);              
            }
            else
            {
                ViewBag.TempOrderMeatSubstr = null;
            }
            string tempOrderMeatSubstr = ViewBag.TempOrderMeatSubstr;


            if (tmp.TempOrderVegetable != null)
            {
                 ViewBag.TempOrderVegetableSubstr = tmp.TempOrderVegetable.Substring(0, tmp.TempOrderVegetable.Length - 2);
            }
            else
            {
                ViewBag.TempOrderVegetableSubstr = null;
            }
            string tempOrderVegetableSubstr = ViewBag.TempOrderVegetableSubstr;

            if (User.Identity.IsAuthenticated == true)
            {
                string userId = _userManager.GetUserId(HttpContext.User);

                var element = new TempOrderInBasket()
                {
                    TempPizzaName = tmp.TempOrderPizzaName,
                    TempSize = tmp.TempOrderSize,
                    TempDough = tmp.TempOrderDough,
                    TempSauce = tmp.TempOrderSauce,
                    TempCheese = tmp.TempOrderCheese,
                    TempMeat = tempOrderMeatSubstr,
                    TempVegetable = tempOrderVegetableSubstr,
                    TempPrice = tmp.TempOrderPrice,
                    TempSessionId = userId
                };

                if (ModelState.IsValid)
                {
                    _db.TempOrders.Add(element);
                    _db.SaveChanges();
                    return RedirectToAction("PizzaIndex");
                }
            }
            else if (HttpContext.Session.GetString("SessionId") != null)
            {
                string sessionId = HttpContext.Session.GetString("SessionId");
                TempData["SessionId"] = sessionId;

                var element = new TempOrderInBasket()
                {
                    TempPizzaName = tmp.TempOrderPizzaName,
                    TempSize = tmp.TempOrderSize,
                    TempDough = tmp.TempOrderDough,
                    TempSauce = tmp.TempOrderSauce,
                    TempCheese = tmp.TempOrderCheese,
                    TempMeat = tempOrderMeatSubstr,
                    TempVegetable = tempOrderVegetableSubstr,
                    TempPrice = tmp.TempOrderPrice,
                    TempSessionId = sessionId
                };

                if (ModelState.IsValid)
                {
                    _db.TempOrders.Add(element);
                    _db.SaveChanges();
                    return RedirectToAction("PizzaIndex");
                }
            }
            else
            {
                if(TempData["SessionId"] == null || !User.Identity.IsAuthenticated)
                {
                    return NotFound();
                }

                string sessionId = TempData["SessionId"].ToString();
                string userId = _userManager.GetUserId(HttpContext.User);

                var tempOrderSession = _db.TempOrders.ToList();
                tempOrderSession.ForEach(x =>
                {
                    if(x.TempSessionId == sessionId || x.TempSessionId == userId)
                    {
                        _db.TempOrders.Remove(x);
                        _db.SaveChanges();
                    }
                });

                return NotFound();
            }

            return View();
        }

        [HttpGet]
        public IActionResult PizzaEdit(int? id, Pizza p)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Pizza = _db.Pizzas.Find(id);
            var Ingredients = _db.MeatsVegetables.Where(x => x.PizzaId == id).ToList();
            var Meat = _db.Meats.ToList();
            var Vegetable = _db.Vegetables.ToList();
            var Size = _db.Sizes.Find(Pizza.SizeId);
            var Dough = _db.Doughs.Find(Pizza.DoughId);
            var Sauce = _db.Sauces.Find(Pizza.SauceId);
            var Cheese = _db.Cheeses.Find(Pizza.CheeseId);
            
            if(Pizza.SizeId == Size.Id)
            {
                p.SizeRadio = Size.Id.ToString();
                Pizza.SizeRadioPrice = Size.Price;
            }

            if (Pizza.DoughId == Dough.Id)
            {
                p.DoughRadio = Dough.Id.ToString();
                Pizza.DoughRadioPrice = Dough.Price;
            }

            if (Pizza.SauceId == Sauce.Id)
            {
                p.SauceRadio = Sauce.Id.ToString();
                Pizza.SauceRadioPrice = Sauce.Price;
            }

            if (Pizza.CheeseId == Cheese.Id)
            {
                p.CheeseRadio = Cheese.Id.ToString();
                Pizza.CheeseRadioPrice = Cheese.Price;
            }

            Ingredients.ForEach(i =>
            {
                Meat.ForEach(m =>
                {
                    if (i.MeatId == m.Id)
                    {
                        Pizza.MeatId.Add(m.Id);
                        Pizza.MeatCheckboxPrice += m.Price;
                    }
                });
            });

            Pizza.MeatId.ToList().ForEach(m =>
            {
                if (m == 1)
                {
                    p.MeatCheckbox1 = m.ToString();
                }
                else if (m == 2)
                {
                    p.MeatCheckbox2 = m.ToString();
                }
                else if (m == 3)
                {
                    p.MeatCheckbox3 = m.ToString();
                }
                else if (m == 4)
                {
                    p.MeatCheckbox4 = m.ToString();

                }
                else if (m == 5)
                {
                    p.MeatCheckbox5 = m.ToString();

                }
                else if (m == 6)
                {
                    p.MeatCheckbox6 = m.ToString();

                }
            });

            Ingredients.ForEach(i =>
            {
                Vegetable.ForEach(v =>
                {
                    if (i.VegetableId == v.Id)
                    {
                        Pizza.VegetableId.Add(v.Id);
                        Pizza.VegetableCheckboxPrice += v.Price;
                    }
                });
            });

            Pizza.VegetableId.ToList().ForEach(v =>
            {
                if (v == 1)
                {
                    p.MeatCheckbox1 = v.ToString();
                }
                else if (v == 2)
                {
                    p.MeatCheckbox2 = v.ToString();
                }
                else if (v == 3)
                {
                    p.MeatCheckbox3 = v.ToString();
                }
                else if (v == 4)
                {
                    p.MeatCheckbox4 = v.ToString();

                }
                else if (v == 5)
                {
                    p.MeatCheckbox5 = v.ToString();

                }
                else if (v == 6)
                {
                    p.MeatCheckbox6 = v.ToString();

                }
            });

            if (Pizza == null)
            {
                return NotFound();
            }


            if (User.Identity.IsAuthenticated)
            {
                var tempWholeOrderPrice = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).Sum(x => x.TempPrice);
                var tempOrder = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).ToList();
                var tempOrderCount = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).Count();
                ViewBag.tempWholeOrderPrice = tempWholeOrderPrice;
                ViewBag.order = tempOrder;
                ViewBag.orderCount = tempOrderCount;
            }
            else if (HttpContext.Session.GetString("SessionId") != null)
            {
                var tempWholeOrderPrice = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).Sum(x => x.TempPrice);
                var tempOrder = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).ToList();
                var tempOrderCount = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).Count();
                ViewBag.tempWholeOrderPrice = tempWholeOrderPrice;
                ViewBag.order = tempOrder;
                ViewBag.orderCount = tempOrderCount;
            }
            return View(Pizza);
        }

        [HttpPost]
        public IActionResult PizzaEdit(int id, Pizza pizza, TempOrderInBasket tmp)
        {
            var Pizzas = _db.Pizzas.FirstOrDefault(x => x.Id == id);

            if (Pizzas.Id != id)
            {
                return NotFound();
            }

            var Ingredients = _db.MeatsVegetables.Where(x => x.PizzaId == id).ToList();

            var Size = _db.Sizes.ToList();
            var Dough = _db.Doughs.ToList();
            var Sauce = _db.Sauces.ToList();
            var Cheese = _db.Cheeses.ToList();
            var Meat = _db.Meats.ToList();
            var Vegetable = _db.Vegetables.ToList();

            Size.ForEach(s =>
            {
                if (s.Id == Convert.ToInt32(pizza.SizeRadio))
                {
                    tmp.TempOrderSize = s.Name;
                    tmp.TempOrderPrice += s.Price;
                }
            });

            Dough.ForEach(d =>
            {
                if (d.Id == Convert.ToInt32(pizza.DoughRadio))
                {
                    tmp.TempOrderDough = d.Name;
                    tmp.TempOrderPrice += d.Price;
                }
            });

            Sauce.ForEach(s =>
            {
                if (s.Id == Convert.ToInt32(pizza.SauceRadio))
                {
                    tmp.TempOrderSauce = s.Name;
                    tmp.TempOrderPrice += s.Price;
                }
            });

            Cheese.ForEach(c =>
            {
                if (c.Id == Convert.ToInt32(pizza.CheeseRadio))
                {
                    tmp.TempOrderCheese = c.Name;
                    tmp.TempOrderPrice += c.Price;
                }
            });

            
            if (pizza.MeatCheckbox1 != "false")
            {
                Meat.ForEach(m =>
                {
                    if (m.Id == Convert.ToInt32(pizza.MeatCheckbox1))
                    {
                        tmp.TempOrderMeat += m.Name + ", ";
                        tmp.TempOrderPrice += m.Price;
                    }                 
                });
            }

            if (pizza.MeatCheckbox2 != "false")
            {
                Meat.ForEach(m =>
                {
                    if (m.Id == Convert.ToInt32(pizza.MeatCheckbox2))
                    {
                        tmp.TempOrderMeat += m.Name + ", ";
                        tmp.TempOrderPrice += m.Price;
                    }
                });
            }

            if (pizza.MeatCheckbox3 != "false")
            {
                Meat.ForEach(m =>
                {
                    if (m.Id == Convert.ToInt32(pizza.MeatCheckbox3))
                    {
                        tmp.TempOrderMeat += m.Name + ", ";
                        tmp.TempOrderPrice += m.Price;
                    }
                });
            }

            if (pizza.MeatCheckbox4 != "false")
            {
                Meat.ForEach(m =>
                {
                    if (m.Id == Convert.ToInt32(pizza.MeatCheckbox4))
                    {
                        tmp.TempOrderMeat += m.Name + ", ";
                        tmp.TempOrderPrice += m.Price;
                    }
                });
            }

            if (pizza.MeatCheckbox5 != "false")
            {
                Meat.ForEach(m =>
                {
                    if (m.Id == Convert.ToInt32(pizza.MeatCheckbox5))
                    {
                        tmp.TempOrderMeat += m.Name + ", ";
                        tmp.TempOrderPrice += m.Price;
                    }
                });
            }

            if (pizza.MeatCheckbox6 != "false")
            {
                Meat.ForEach(m =>
                {
                    if (m.Id == Convert.ToInt32(pizza.MeatCheckbox6))
                    {
                        tmp.TempOrderMeat += m.Name + ", ";
                        tmp.TempOrderPrice += m.Price;
                    }
                });
            }

            if (pizza.VegetableCheckbox1 != "false")
            {
                Vegetable.ForEach(v =>
                {
                    if (v.Id == Convert.ToInt32(pizza.VegetableCheckbox1))
                    {
                        tmp.TempOrderVegetable += v.Name + ", ";
                        tmp.TempOrderPrice += v.Price;
                    }
                });
            }

            if (pizza.VegetableCheckbox2 != "false")
            {
                Vegetable.ForEach(v =>
                {
                    if (v.Id == Convert.ToInt32(pizza.VegetableCheckbox2))
                    {
                        tmp.TempOrderVegetable += v.Name + ", ";
                        tmp.TempOrderPrice += v.Price;
                    }
                });
            }


            if (pizza.VegetableCheckbox3 != "false")
            {
                Vegetable.ForEach(v =>
                {
                    if (v.Id == Convert.ToInt32(pizza.VegetableCheckbox3))
                    {
                        tmp.TempOrderVegetable += v.Name + ", ";
                        tmp.TempOrderPrice += v.Price;
                    }
                });
            }

            if (pizza.VegetableCheckbox4 != "false")
            {
                Vegetable.ForEach(v =>
                {
                    if (v.Id == Convert.ToInt32(pizza.VegetableCheckbox4))
                    {
                        tmp.TempOrderVegetable += v.Name + ", ";
                        tmp.TempOrderPrice += v.Price;
                    }
                });
            }

            if (pizza.VegetableCheckbox5 != "false")
            {
                Vegetable.ForEach(v =>
                {
                    if (v.Id == Convert.ToInt32(pizza.VegetableCheckbox5))
                    {
                        tmp.TempOrderVegetable += v.Name + ", ";
                        tmp.TempOrderPrice += v.Price;
                    }
                });
            }

            if (pizza.VegetableCheckbox6 != "false")
            {
                Vegetable.ForEach(v =>
                {
                    if (v.Id == Convert.ToInt32(pizza.VegetableCheckbox6))
                    {
                        tmp.TempOrderVegetable += v.Name + ", ";
                        tmp.TempOrderPrice += v.Price;
                    }
                });
            }

            if (pizza.VegetableCheckbox7 != "false")
            {
                Vegetable.ForEach(v =>
                {
                    if (v.Id == Convert.ToInt32(pizza.VegetableCheckbox7))
                    {
                        tmp.TempOrderVegetable += v.Name + ", ";
                        tmp.TempOrderPrice += v.Price;
                    }
                });
            }

            if (pizza.VegetableCheckbox8 != "false")
            {
                Vegetable.ForEach(v =>
                {
                    if (v.Id == Convert.ToInt32(pizza.VegetableCheckbox8))
                    {
                        tmp.TempOrderVegetable += v.Name + ", ";
                        tmp.TempOrderPrice += v.Price;
                    }
                });
            }

            tmp.TempOrderPizzaName = Pizzas.Name;

            if (tmp.TempOrderMeat != null)
            {
                ViewBag.TempOrderMeatSubstr = tmp.TempOrderMeat.Substring(0, tmp.TempOrderMeat.Length - 2);
            }
            else
            {
                ViewBag.TempOrderMeatSubstr = null;
            }
            string tempOrderMeatSubstr = ViewBag.TempOrderMeatSubstr;


            if (tmp.TempOrderVegetable != null)
            {
                ViewBag.TempOrderVegetableSubstr = tmp.TempOrderVegetable.Substring(0, tmp.TempOrderVegetable.Length - 2);
            }
            else
            {
                ViewBag.TempOrderVegetableSubstr = null;
            }
            string tempOrderVegetableSubstr = ViewBag.TempOrderVegetableSubstr;

            if (User.Identity.IsAuthenticated)
            {
                string userId = _userManager.GetUserId(HttpContext.User);

                var element = new TempOrderInBasket()
                {
                    TempPizzaName = tmp.TempOrderPizzaName + " - Edytowano",
                    TempSize = tmp.TempOrderSize,
                    TempDough = tmp.TempOrderDough,
                    TempSauce = tmp.TempOrderSauce,
                    TempCheese = tmp.TempOrderCheese,
                    TempMeat = tempOrderMeatSubstr,
                    TempVegetable = tempOrderVegetableSubstr,
                    TempPrice = tmp.TempOrderPrice,
                    TempSessionId = userId
                };

                if (ModelState.IsValid)
                {
                    _db.TempOrders.Add(element);
                    _db.SaveChanges();
                    return RedirectToAction("PizzaIndex");
                }
            }
            else if (HttpContext.Session.GetString("SessionId") != null)
            {
                string sessionId = HttpContext.Session.GetString("SessionId");
                TempData["SessionId"] = sessionId;

                var element = new TempOrderInBasket()
                {
                    TempPizzaName = tmp.TempOrderPizzaName + " - Edytowano",
                    TempSize = tmp.TempOrderSize,
                    TempDough = tmp.TempOrderDough,
                    TempSauce = tmp.TempOrderSauce,
                    TempCheese = tmp.TempOrderCheese,
                    TempMeat = tempOrderMeatSubstr,
                    TempVegetable = tempOrderVegetableSubstr,
                    TempPrice = tmp.TempOrderPrice,
                    TempSessionId = sessionId
                };

                if (ModelState.IsValid)
                {
                    _db.TempOrders.Add(element);
                    _db.SaveChanges();
                    return RedirectToAction("PizzaIndex");
                }
            }
            else
            {
                if (TempData["SessionId"] == null)
                {
                    return NotFound();
                }

                string sessionId = TempData["SessionId"].ToString();
                string userId = _userManager.GetUserId(HttpContext.User);
                var tempOrderSession = _db.TempOrders.ToList();
                tempOrderSession.ForEach(x =>
                {
                    if (x.TempSessionId == sessionId || x.TempSessionId == userId)
                    {
                        _db.TempOrders.Remove(x);
                        _db.SaveChanges();
                    }
                });

                return NotFound();
            }


            return View();
        }

        [HttpDelete]
        public void Delete(int id)
        {
            if (HttpContext.Session.GetString("SessionId") != null || User.Identity.IsAuthenticated)
            {
                string sessionId = HttpContext.Session.GetString("SessionId");
                string userId = _userManager.GetUserId(HttpContext.User);
                var TempOrder = _db.TempOrders.ToList();

                TempOrder.ForEach(x =>
                {
                    if (x.TempSessionId == sessionId || x.TempSessionId == userId)
                    {
                        if (x.Id == id)
                        {
                            _db.TempOrders.Remove(x);
                            _db.SaveChanges();
                        }
                    }
                });
            }
        }
    }
}
