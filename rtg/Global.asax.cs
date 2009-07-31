using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace rtg
{
  // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
  // visit http://go.microsoft.com/?LinkId=9394801

  public class MvcApplication : System.Web.HttpApplication
  {
    public static void RegisterRoutes(RouteCollection routes)
    {
      routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

      routes.IgnoreRoute("Settings.mvc/{*pathInfo}");
      routes.IgnoreRoute("Settings/{*pathInfo}");
      routes.IgnoreRoute("FileManager.mvc/{*pathInfo}");
      routes.IgnoreRoute("FileManager/{*pathInfo}");


      //routes are doubled up so that I can produce clean urls when using url.route...
      //but can accept .mvc for iis6 ... yuk I know.
      routes.MapRoute(
      "Pages",
      "Pages/{id}",
      new { controller = "Pages", action = "Index", id = "" }
      );

      routes.MapRoute(
      "PagesMVC",
      "Pages.mvc/{id}",
      new { controller = "Pages", action = "Index", id = "" }
      );

      routes.MapRoute(
      "Gallery",
      "Gallery/{id}",
      new { controller = "Pages", action = "Gallery", id = "" }
      );

      routes.MapRoute(
      "GalleryMVC",
      "Gallery.mvc/{id}",
      new { controller = "Pages", action = "Gallery", id = "" }
      );

      routes.MapRoute(
      "AdminPages",
      "Admin/Pages/{action}/{id}",
      new { controller = "Admin", action = "Index", id = "" }
      );

      routes.MapRoute(
      "AdminPagesMVC",
      "Admin.mvc/Pages/{action}/{id}",
      new { controller = "Admin", action = "Index", id = "" }
      );

      routes.MapRoute(
      "Admin",
      "Admin/{controller}/{action}/{id}",
      new { controller = "Admin", action = "Index", id = "" }
      );

      routes.MapRoute(
      "AdminMVC",
      "Admin.mvc/{controller}/{action}/{id}",
      new { controller = "Admin", action = "Index", id = "" }
      );

      routes.MapRoute(
      "DefaultMVC",                                              // Route name
      "{controller}.mvc/{action}/{id}",                           // URL with parameters
      new { controller = "Pages", action = "Index", id = "Home" }  // Parameter defaults
      );

      routes.MapRoute(
          "Default",                                              // Route name
          "{controller}/{action}/{id}",                           // URL with parameters
          new { controller = "Pages", action = "Index", id = "Home" }  // Parameter defaults
      );

    }

    protected void Application_Start()
    {
      RegisterRoutes(RouteTable.Routes);
    }
  }
}