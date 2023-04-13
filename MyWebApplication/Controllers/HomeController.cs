#nullable disable
using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Data;
using MyWebApplication.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace MyWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;

        public HomeController(ApplicationDbContext db, UserManager<IdentityUser> userManager, IEmailSender emailSender)
        {
            _db = db;
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult HomeIndex()
        {
            var sessionId = HttpContext.Session.Id;
            HttpContext.Session.SetString("SessionId", sessionId);

            var Opinions = _db.Opinions.ToList();
            var Pizzas = _db.Pizzas.ToList();
            var PizzasCount = _db.Pizzas.Count();

            if (User.Identity.IsAuthenticated)
            {
                var tempWholeOrderPrice = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).Sum(x => x.TempPrice);
                var tempOrder = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).ToList();
                var tempOrderCount = _db.TempOrders.Where(x => x.TempSessionId == this._userManager.GetUserId(HttpContext.User)).Count();
                ViewBag.tempWholeOrderPrice = tempWholeOrderPrice;
                ViewBag.order = tempOrder;
                ViewBag.orderCount = tempOrderCount;
                ViewBag.Pizza = Pizzas;
                ViewBag.PizzaCount = PizzasCount;
                ViewBag.Index = 0;

            }
            else if (HttpContext.Session.GetString("SessionId") != null)
            {
                var tempWholeOrderPrice = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).Sum(x => x.TempPrice);
                var tempOrder = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).ToList();
                var tempOrderCount = _db.TempOrders.Where(x => x.TempSessionId == this.HttpContext.Session.Id).Count();
                ViewBag.tempWholeOrderPrice = tempWholeOrderPrice;
                ViewBag.order = tempOrder;
                ViewBag.orderCount = tempOrderCount;
                ViewBag.Pizza = Pizzas;
                ViewBag.PizzaCount = PizzasCount;
                ViewBag.Index = 0;
            }

            return View(Opinions);
        }

        [Authorize]
        [HttpGet]
        public IActionResult HomeOpinion()
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

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult HomeOpinion(Opinion o)
        {
            if (User.Identity.IsAuthenticated)
            {
                var opinion = new Opinion()
                {
                    Name = o.Name,
                    Description = o.Description
                };

                if (ModelState.IsValid)
                {
                    _db.Opinions.Add(opinion);
                    _db.SaveChanges();
                    return RedirectToAction("HomeIndex");
                }
            }
            else if (HttpContext.Session.GetString("SessionId") != null)
            {
                var opinion = new Opinion()
                {
                    Name = o.Name,
                    Description = o.Description
                };

                if (ModelState.IsValid)
                {
                    _db.Opinions.Add(opinion);
                    _db.SaveChanges();
                    return RedirectToAction("HomeIndex");
                }
            }
            else
            {
                return NotFound();
            }

            return View();
        }
    }
}

