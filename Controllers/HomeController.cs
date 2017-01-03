using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestMVC.Models;

namespace TestMVC.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult User()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPlayers(int? page, int? limit, string sortBy, string direction, string searchString = null)
        {
            int total;
            var records = new GridModel().GetPlayers(page, limit, sortBy, direction, searchString, out total);
            return Json(new { records, total }, JsonRequestBehavior.AllowGet);
        }

                [HttpPost]
        public JsonResult Save(UserView player)
        {
            new GridModel().Save(player);
            return Json(true);
        }

        [HttpPost]
        public JsonResult Remove(int id)
        {
            new GridModel().Remove(id);
            return Json(true);
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public sealed class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
            filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            filterContext.HttpContext.Response.Cache.SetNoStore();

            base.OnResultExecuting(filterContext);
        }
    }
}