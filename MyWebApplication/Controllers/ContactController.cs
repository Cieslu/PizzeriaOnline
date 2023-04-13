#nullable disable
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyWebApplication.Data;
using MyWebApplication.Models;

namespace MyWebApplication.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public ContactController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult ContactIndex()
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

      /*  [HttpPost]
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

            return RedirectToAction("ContactIndex");
        }*/
    }
}
