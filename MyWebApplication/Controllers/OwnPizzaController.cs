#nullable disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyWebApplication.Data;
using MyWebApplication.Models;

namespace MyWebApplication.Controllers
{
    public class OwnPizzaController : Controller
    {
        private readonly ApplicationDbContext _db;
        private UserManager<IdentityUser> _userManager;

        public OwnPizzaController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult OwnPizzaIndex()
        {

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
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult OwnPizzaIndex(TempOrderInBasket tmp, Pizza pizza)
        {
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

                if (pizza.OwnPizzaName == null)
                {
                    var element = new TempOrderInBasket()
                    {
                        TempPizzaName = "Kreator pizzy",
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
                        return RedirectToAction("OwnPizzaIndex");
                    }
                }
                else
                {
                    var element = new TempOrderInBasket()
                    {
                        TempPizzaName = "Kreator pizzy - " + pizza.OwnPizzaName,
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
                        return RedirectToAction("OwnPizzaIndex");
                    }
                }
            }
            else if (HttpContext.Session.GetString("SessionId") != null)
            {
                string sessionId = HttpContext.Session.GetString("SessionId");
                TempData["SessionId"] = sessionId;

                if (pizza.OwnPizzaName == null)
                {
                    var element = new TempOrderInBasket()
                    {
                        TempPizzaName = "Kreator pizzy",
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
                        return RedirectToAction("OwnPizzaIndex");
                    }
                }
                else
                {
                    var element = new TempOrderInBasket()
                    {
                        TempPizzaName = "Kreator pizzy - " + pizza.OwnPizzaName,
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
                        return RedirectToAction("OwnPizzaIndex");
                    }
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

            return View();
        }

/*        [HttpPost]
        public IActionResult Delete(int id)
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

            return RedirectToAction("OwnPizzaIndex");
        }*/
    }
}
