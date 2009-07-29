using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using rtg.Models;

namespace rtg.Controllers
{
  public class PagesController : Controller
  {
    rtgDataContext db = new rtgDataContext();

    public ActionResult Index(string id)
    {
      Page p = db.Pages.FirstOrDefault(pg=>pg.Permalink == id);
      
      if(p != null)
        return View(p.PageTemplate.RenderFile, p);
      else
        return RedirectToAction("Error", new { id = "404" });
    }

    public ActionResult Gallery(int id)
    {
      Gallery g = db.Galleries.FirstOrDefault(gl => gl.GalleryID == id);

      if (g != null)
        return View("Gallery", g);
      else
        return RedirectToAction("Error", new { id = "404" });
    }

    public ActionResult Error(int? id)
    {
      return View(id);
    }

  }
}
