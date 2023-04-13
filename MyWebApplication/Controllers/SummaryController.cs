#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Data;
using MyWebApplication.Models;
using System.Net.Mail;

namespace MyWebApplication.Controllers
{
    public class SummaryController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IEmailSender _emailSender;
        private UserManager<IdentityUser> _userManager;

        public SummaryController(ApplicationDbContext db, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult SummaryIndex()
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
        public async Task<IActionResult> SummaryIndex(UserData userData)
        {
            if (User.Identity.IsAuthenticated)
            {
                string userId = _userManager.GetUserId(HttpContext.User);
                var tempOrder = _db.TempOrders.ToList();

                if (ModelState.IsValid)
                {
                    userData.UserId = userId;
                    _db.UserDatas.Add(userData);
                    _db.SaveChanges();

                    tempOrder.ForEach(x =>
                    {
                        if (x.TempSessionId == userId)
                        {
                            var FinallyOrder = new Order()
                            {
                                PizzaName = x.TempPizzaName,
                                Size = x.TempSize,
                                Dough = x.TempDough,
                                Sauce = x.TempSauce,
                                Cheese = x.TempCheese,
                                Meat = x.TempMeat,
                                Vegetable = x.TempVegetable,
                                Price = x.TempPrice,
                                UserDataId = userData.Id
                            };
                            _db.Orders.Add(FinallyOrder);
                            _db.SaveChanges();
                        }
                    });

                    tempOrder.ForEach(x =>
                    {
                        if (x.TempSessionId == userId)
                        {
                            _db.TempOrders.Remove(x);
                            _db.SaveChanges();
                        }
                    });

                    var Order = _db.UserDatas.Where(x => x.UserId.Contains(userId)).OrderBy(x => x.DateOrder).LastOrDefault();

                    var Status = _db.Status.ToList();

                    var StatusName = "";

                    Status.ForEach(x =>
                    {
                        if (x.Id == Order.StatusId)
                        {
                            StatusName = x.Name;
                        }
                    });

                    String body = "Dziękujemy za twoje zamówienie o numerze: <span style='font-weight: bold;'>" + Order.Id + "</span>. </br> Status zamówienia: <span style='font-weight: bold;'>" + StatusName + "</span>";
                    await _emailSender.SendEmailAsync("szymonciesla1406@gmail.com", "Pizzeria", body);
                    return RedirectToAction(nameof(StatusOrder));
                }
            }
            else if (HttpContext.Session.GetString("SessionId") != null)
            {
                string sessionId = HttpContext.Session.GetString("SessionId");
                TempData["SessionId"] = sessionId;

                var tempOrder1 = _db.TempOrders.ToList();

                if (ModelState.IsValid)
                {
                    userData.UserId = sessionId;
                    _db.UserDatas.Add(userData);
                    _db.SaveChanges();

                    tempOrder1.ForEach(x =>
                    {
                        if (x.TempSessionId == sessionId)
                        {
                            var FinallyOrder = new Order()
                            {
                                PizzaName = x.TempPizzaName,
                                Size = x.TempSize,
                                Dough = x.TempDough,
                                Sauce = x.TempSauce,
                                Cheese = x.TempCheese,
                                Meat = x.TempMeat,
                                Vegetable = x.TempVegetable,
                                Price = x.TempPrice,
                                UserDataId = userData.Id,
                            };
                            _db.Orders.Add(FinallyOrder);
                            _db.SaveChanges();
                        }
                    });

                    tempOrder1.ForEach(x =>
                    {
                        if (x.TempSessionId == sessionId)
                        {
                            _db.TempOrders.Remove(x);
                            _db.SaveChanges();
                        }
                    });

                    var Order = _db.UserDatas.Where(x => x.UserId.Contains(sessionId)).OrderBy(x => x.DateOrder).LastOrDefault();

                    var Status = _db.Status.ToList();

                    var StatusName = "";

                    Status.ForEach(x =>
                    {
                        if (x.Id == Order.StatusId)
                        {
                            StatusName = x.Name;
                        }
                    });

                    String body = "Dziękujemy za twoje zamówienie o numerze: <span style='font-weight: bold;'>" + Order.Id + "</span>. </br> Status zamówienia: <span style='font-weight: bold;'>" + StatusName + "</span>";
                    await _emailSender.SendEmailAsync("szymonciesla1406@gmail.com", "Pizzeria", body);
                    return RedirectToAction(nameof(StatusOrder));
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

        [HttpGet]
        public IActionResult StatusOrder()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                var Order = _db.UserDatas.Where(x => x.UserId.Contains(userId)).OrderBy(x => x.DateOrder).LastOrDefault();
                var Status = _db.Status.ToList();

                Status.ForEach(x =>
                {
                    if (x.Id == Order.StatusId)
                    {
                        ViewBag.StatusName = x.Name;
                    }
                }); 
                return View(Order);
            }
            else if(HttpContext.Session.GetString("SessionId") != null)
            {
                string sessionId = HttpContext.Session.GetString("SessionId");
                var Order = _db.UserDatas.Where(x => x.UserId.Contains(sessionId)).OrderBy(x => x.DateOrder).LastOrDefault();

                var Status = _db.Status.ToList();

                Status.ForEach(x =>
                {
                    if (x.Id == Order.StatusId)
                    {
                        ViewBag.StatusName = x.Name;
                    }
                });
                return View(Order);
            }

            return View();
        }
    }
}
