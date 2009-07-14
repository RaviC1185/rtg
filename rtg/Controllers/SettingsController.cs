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

      GenerateCSS();
    }

    private void GenerateCSS()
    {
      IDictionary<string, string> settings = GenerateSettingsDictionary();

      string menucontainer = "";
      string menu = "";
      string menu_ul_li = "";
      string menu_ul_li_a = "";
      string menu_ul_li_a_hover = "";
      string menu_ul_li_a_selected = "";

      string menu_ul_li_hover_ul = "";

      string submenu = "";
      string submenu_li = "";
      string submenu_li_a = "";
      string submenu_li_a_hover = "";
      string submenu_li_a_selected = "";

      string submenu_bg = "";
      string submenu_seperate = "";

      string content = "";

      //now put arrage settings into appropiate tags

      //#menucontainer
      if (settings["menu_position"] == "header" && settings["submenu_position"] != "below_header") 
        menucontainer = string.Format("position: relative; top: {0}px;", settings["menu_position_topmargin"]);

      //#menu
      if (settings["menu_orientation"] == "horizontal")
        menu = string.Format("width: 1000px; background-color: #{0};", settings["menu_background_extend"]);
      else
        menu = string.Format("width: 150px; background-color: #{0};", settings["menu_background_extend"]);

      //#menu ul li
      if(settings["menu_orientation"] == "horizontal")
        menu_ul_li = "float:left;";

      if (settings["menu_background_icon"] != "")
        menu_ul_li += string.Format("list-style-image: {0};padding: 0;", settings["menu_background_icon"]);
      else
        menu_ul_li += "list-style-type:none;";

      //#menu ul li a
      menu_ul_li_a          = string.Format("color: #{0}; background-color: #{1};", settings["menu_text_colour"], settings["menu_background_colour"]);
      menu_ul_li_a_hover    = string.Format("color: #{0}; background-color: #{1};", settings["menu_text_hover"], settings["menu_background_hover"]);
      menu_ul_li_a_selected = string.Format("color: #{0}; background-color: #{1};", settings["menu_text_selected"], settings["menu_background_selected"]);

      //#menu ul li:hover ul
      if (settings["submenu_position"] == "dropdown" || settings["submenu_position"] == "leftcolumn")
        menu_ul_li_hover_ul = "display:block;";

      //.submenu
      submenu = string.Format("background-color: #{0}", settings["submenu_background_extend"]);
 
      // .submenu li
      if(settings["submenu_position"] == "below_menu" || settings["submenu_position"] == "below_header")
        submenu_li = "float: left;";  
      else
         submenu_li = "float:none;"; 

      // .submenu li a
      submenu_li_a          = string.Format("color: #{0}; background-color: #{1};", settings["submenu_text_colour"], settings["submenu_background_colour"]);
      submenu_li_a_hover    = string.Format("color: #{0}; background-color: #{1};", settings["submenu_text_hover"], settings["submenu_background_hover"]);
      submenu_li_a_selected = string.Format("color: #{0}; background-color: #{1};", settings["menu_text_selected"], settings["submenu_background_selected"]);

      // #submenucontainerbelow, #submenucontainerseperate
      submenu_bg = string.Format("background_color: #{0}", settings["submenu_background_extend"]);

      if (settings["menu_orientation"] == "vertical" && settings["submenu_position"] == "below_header")
        submenu_seperate = "float:left; width:850px;";
      else if ( settings["submenu_position"] == "leftcolumn")
        submenu_seperate = "float:left;";
      
      //content
      if (settings["menu_orientation"] == "vertical" && settings["submenu_position"] == "below_header")
        content = "float:left; width:850px;";
      else if (settings["submenu_position"] == "leftcolumn")
        content = "float:left;";


      //grab the css template and write over placeholders

      System.IO.StreamReader fileContents = new System.IO.StreamReader("/Content/template.css");
      string css = fileContents.ReadToEnd();
      fileContents.Close();

      css = css.Replace("/*menucontainer*/", menucontainer);
      css = css.Replace("/*menu*/", menu);
      css = css.Replace("/*menu_ul_li*/", menu_ul_li);
      css = css.Replace("/*menu_ul_li_a*/", menu_ul_li_a);
      css = css.Replace("/*menu_ul_li_a_hover*/", menu_ul_li_a_hover);
      css = css.Replace("/*menu_ul_li_a_selected*/", menu_ul_li_a_selected);
      css = css.Replace("/*menu_ul_li_hover_ul*/", menu_ul_li_hover_ul);
      css = css.Replace("/*submenu*/", submenu);
      css = css.Replace("/*submenu_li*/", submenu_li);
      css = css.Replace("/*submenu_li_a*/", submenu_li_a);
      css = css.Replace("/*submenu_li_a_hover*/", submenu_li_a_hover);
      css = css.Replace("/*submenu_li_a_selected*/", submenu_li_a_selected);
      css = css.Replace("/*submenu_bg*/", submenu_bg);
      css = css.Replace("/*submenu_seperate*/", submenu_seperate);
      css = css.Replace("/*content*/", content);

      System.IO.StreamWriter newCssFile = new System.IO.StreamWriter("/Content/frontend.css");
      newCssFile.Write(css);
      newCssFile.Flush();

    }

    private IDictionary<string, string> GenerateSettingsDictionary()
    {
      IDictionary<string, string> settings = new Dictionary<string, string>();
      foreach (Setting s in db.Settings)
      {
        if (s.SettingKey2 == null)
        {
          settings.Add(s.SettingKey, s.Value); 
        }
        else if (s.Value == "true")
        {
          settings.Add(s.SettingKey, s.SettingKey2); 
        }
      }
      return settings;
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
