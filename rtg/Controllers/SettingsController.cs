using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using rtg.Models;

namespace rtg.Controllers
{
  public class SettingsController : Controller
  {
    rtgDataContext db = new rtgDataContext();
    
    public ActionResult Index()
    {
      IEnumerable<Setting> settings = db.Settings;
      return View(settings);
    }

    public void Update()
    {
      foreach (Setting s in db.Settings)
      {
        if (Request.Form.AllKeys.Contains(s.SettingKey))
        {
          if (s.Type == "radioVert")
          {
            if (s.Label.Replace(" ", "") == Request.Form[s.SettingKey])
              s.Value = "true";
            else
              s.Value = "";
          }
          else if (s.Type != "stylesheet")
          {
            s.Value = Request.Form[s.SettingKey];
          }
        }
      }

      //now grab the stylesheets
      string[] stylesheets = Request.Form["style_sheet"].Split(',');
      IEnumerable<Setting> stylesheetSettigns = db.Settings.Where(st => st.SettingKey == "style_sheet");

      int i = 0;
      foreach (Setting s in stylesheetSettigns)
      {
        s.Value = stylesheets[i++];
      }
      
      //if stylesheets are added.
      if (stylesheets.Length > stylesheetSettigns.Count())
      {
        int index = stylesheets.Length - stylesheetSettigns.Count();
        //blah blah do this stuff later
      }

      db.SubmitChanges();
    }

    public ActionResult SelectStyle(int id)
    {
      Setting s = db.Settings.FirstOrDefault(st => st.SettingKey == "selected_style");
      string menu_orientation = "";
      string menu_position = "";
      int menu_position_topmargin = 0;
      string menu_extend = "";
      string submenu_postion = "";

      if (s != null)
      {
        s.Value = id.ToString();

        //clear advanced settings
        foreach(Setting cs in db.Settings.Where(st=>st.Tab == "Advanced"))
        {
          if (cs.SettingKey != "footer_text" || cs.SettingKey != "style_sheet")
          {
            cs.Value = "";
            db.SubmitChanges();
          }
        }
        
        switch (id)
        { 
          case 1:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Header";
            menu_position_topmargin = 0;
            menu_extend = "Full width/depth";
            submenu_postion = "Dropdown/out";
            break;
          case 2:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Header";
            menu_position_topmargin = 0;
            menu_extend = "Full width/depth";
            submenu_postion = "Below menu";
            break;
          case 3:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Header";
            menu_position_topmargin = 0;
            menu_extend = "Full width/depth";
            submenu_postion = "Below header";
            break;
          case 4:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Header";
            menu_position_topmargin = 0;
            menu_extend = "Amount needed";
            submenu_postion = "Left column";
            break;
          case 5:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Header";
            menu_position_topmargin = 20;
            menu_extend = "Full width/depth";
            submenu_postion = "Dropdown/out";
            break;
          case 6:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Header";
            menu_position_topmargin = 20;
            menu_extend = "Full width/depth";
            submenu_postion = "Below menu";
            break;
          case 7:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Header";
            menu_position_topmargin = 20;
            menu_extend = "Full width/depth";
            submenu_postion = "Below header";
            break;
          case 8:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Header";
            menu_position_topmargin = 20;
            menu_extend = "Amount needed";
            submenu_postion = "Left column";
            break;
          case 9:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Header";
            menu_position_topmargin = 20;
            menu_extend = "Full width/depth";
            submenu_postion = "Dropdown/out";
            break;
          case 10:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Header";
            menu_position_topmargin = 20;
            menu_extend = "Full width/depth";
            submenu_postion = "Below menu";
            break;
          case 11:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Header";
            menu_position_topmargin = 20;
            menu_extend = "Amount needed";
            submenu_postion = "Left column";
            break;
          case 12:
            menu_orientation = "Menu Vertical";
            menu_position = "In Header";
            menu_position_topmargin = 0;
            menu_extend = "Amount needed";
            submenu_postion = "Dropdown/out";
            break;
          case 13:
            menu_orientation = "Menu Vertical";
            menu_position = "In Header";
            menu_position_topmargin = 0;
            menu_extend = "Amount needed";
            submenu_postion = "Left column";
            break;
          case 14:
            menu_orientation = "Menu Vertical";
            menu_position = "In Header";
            menu_position_topmargin = 0;
            menu_extend = "Amount needed";
            submenu_postion = "Below header";
            break;
          case 15:
            menu_orientation = "Menu Vertical";
            menu_position = "In Content";
            menu_position_topmargin = 0;
            menu_extend = "Amount needed";
            submenu_postion = "Dropdown/out";
            break;
          case 16:
            menu_orientation = "Menu Vertical";
            menu_position = "In Content";
            menu_position_topmargin = 0;
            menu_extend = "Amount needed";
            submenu_postion = "Left column";
            break;
          case 17:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Content";
            menu_position_topmargin = 0;
            menu_extend = "Amount needed";
            submenu_postion = "Below header";
            break;
        }

        Setting s1 = db.Settings.FirstOrDefault(st=>st.Label == menu_orientation);
        Setting s2 = db.Settings.FirstOrDefault(st=>st.Label == menu_position);
        Setting s3 = db.Settings.FirstOrDefault(st => st.SettingKey == "menu_position_topmargin");
        Setting s4 = db.Settings.FirstOrDefault(st=>st.Label == menu_extend);
        Setting s5 = db.Settings.FirstOrDefault(st=>st.Label == submenu_postion);

        s1.Value = "true";
        s2.Value = "true";
        s3.Value = menu_position_topmargin.ToString();
        s4.Value = "true";
        s5.Value = "true";

        db.SubmitChanges();

        return View("SettingsTab", db.Settings.Where(st => st.Tab == "Advanced").OrderBy(st => st.ListOrder));
      }
      return null;
    }
  }
}
