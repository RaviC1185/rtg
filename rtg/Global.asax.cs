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
      routes.IgnoreRoute("Settings/{*pathInfo}");

      routes.MapRoute(
      "Pages",
      "Pages/{id}",
      new { controller = "Pages", action = "Index", id = "" }
      );

      routes.MapRoute(
      "AdminPages",
      "Admin/Pages/{action}/{id}",
      new { controller = "Admin", action = "Index", id = "" }
      );

      routes.MapRoute(
      "Admin",
      "Admin/{controller}/{action}/{id}",
      new { controller = "Admin", action = "Index", id = "" }
      );



      routes.MapRoute(
          "Default",                                              // Route name
          "{controller}/{action}/{id}",                           // URL with parameters
          new { controller = "Home", action = "Index", id = "" }  // Parameter defaults
      );



    }

    protected void Application_Start()
    {
      RegisterRoutes(RouteTable.Routes);
    }
  }
}