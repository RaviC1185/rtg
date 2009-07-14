using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using rtg.Models;

namespace rtg.Controllers
{
  [HandleError]
  public class HomeController : Controller
  {
    rtgDataContext db = new rtgDataContext();

    public ActionResult Index()
    {
      ViewData["Message"] = "Welcome to ASP.NET MVC!";

      return View();
    }

    public ActionResult About()
    {
      return View();
    }

    public ActionResult HomePage()
    {
      return View();
    }
  }
}
