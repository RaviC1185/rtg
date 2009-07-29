using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using rtg.Models;
using System.IO;
using System.Text.RegularExpressions;

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

    [ValidateInput(false)]
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
            s.Value = ValidateInput(s.Type,Request.Form[s.SettingKey]);
          }
        }
      }

      //now grab the stylesheets
      if (Request.Form.AllKeys.Contains("style_sheet"))
      {
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
      }

      db.SubmitChanges();

      GenerateCSS();
    }

    private string ValidateInput(string type, string value)
    {
      switch (type)
      { 
        case "image":
          return FileTest(value);
          break;
        case "colour":
          return ColourTest(value);
          break;
        case "colour&image":
          if (FileTest(value) != "")
            return value;
          else
            return ColourTest(value);
          break;
        default:
            return value;
            break;
      }
    }

    private string FileTest(string value)
    {
      try
      {
        FileInfo fi = new FileInfo(rtg.Properties.Settings.Default.Root+value);
        if (fi.Exists)
          return value;
        else
          return "";
      }
      catch {
        return "";
      }
    }

    private string ColourTest(string value)
    {
      Regex colorPattern = new Regex("^([0-9a-f]{1,2}){3}$");
      if (colorPattern.IsMatch(value))
        return value;
      else
        return "";
    }

    private void GenerateCSS()
    {
      IDictionary<string, string> settings = GenerateSettingsDictionary();

      string body = "";
      string site_width = "";
      
      string border = "";
      string menucontainer = "";
      string menu = "";
      string menu_ul_li = "";
      string menu_ul_li_a = "";
      string menu_ul_li_a_hover = "";
      string menu_ul_li_a_selected = "";

      string menu_ul_li_ul = "";
      string menu_ul_li_hover_ul = "";

      string submenu = "";
      string submenu_li = "";
      string submenu_li_a = "";
      string submenu_li_a_hover = "";
      string submenu_li_a_selected = "";

      string submenu_bg = "";
      string submenu_seperate = "";

      string content = "";
      string content_a = "";
      string content_a_hover = "";
      string footerText = "";

      //now put arrage settings into appropiate tags

      //body
      body = string.Format("background-color:#{0};", settings["site_bg_color"]);
      if (settings["site_bg_image"].Length > 0)
      {
        body += string.Format("background-image:url({0}); background-repeat:{1};", settings["site_bg_image"], settings["site_bg_repeat"]);
      }
      
      //site_width
      site_width = string.Format("width:{0}px;", settings["site_width"]);

      //border eg border: solid 10px #cccccc;
      if(settings["site_border_thickness"].Length>0 && settings["site_border_colour"].Length > 0)
        border = string.Format("border: solid {0}px #{1};", settings["site_border_thickness"], settings["site_border_colour"]);

      //#menucontainer
      if (settings["menu_position"] == "header") 
        menucontainer = string.Format("position: absolute; top: {0}px;", settings["menu_position_topmargin"]);
      else if (settings["menu_orientation"] == "vertical" && settings["menu_position"] == "content")
        menucontainer = "float:left;";

      //#menu
      if (settings["menu_orientation"] == "horizontal")
        menu = string.Format("{0} background-color: #{1};", site_width ,settings["menu_background_extend"]);
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

      //menu ul li ul
      //if (settings["submenu_position"] == "dropdown" || settings["submenu_position"] == "leftcolumn")
      
      if (settings["menu_orientation"] == "horizontal" && settings["submenu_position"] == "dropdown")
        menu_ul_li_ul = "position:absolute; top: 30px; left:0;";
      else if (settings["menu_orientation"] == "vertical" && settings["submenu_position"] == "dropdown")
        menu_ul_li_ul = "position:absolute; top: 0px; left:150px;";
      //else if (settings["menu_orientation"] == "vertical" && settings["submenu_position"] == "leftcolumn")
        //menu_ul_li_ul
      
      
      //#menu ul li:hover ul
      if (settings["submenu_position"] == "dropdown" || (settings["submenu_position"] == "leftcolumn" && settings["menu_orientation"] == "vertical"))
        menu_ul_li_hover_ul = "display:block;";

      //.submenu
      submenu = string.Format("background-color: #{0};", settings["submenu_background_extend"]);

      if ((settings["submenu_position"] == "below_menu" || settings["submenu_position"] == "below_header") && settings["menu_extend"] == "full")
        submenu += site_width;
      else if (settings["submenu_position"] == "dropdown" || settings["submenu_position"] == "leftcolumn")
        submenu += "width: 150px;";
      else if( settings["submenu_position"] == "below_header" 
            && settings["menu_extend"] == "full" 
            && settings["menu_orientation"] == "vertical"
            && settings["menu_position"] == "content")
        submenu += string.Format("width: {0}px;", int.Parse(settings["site_width"]) - 150); //left column is 150 so to make it float correctly I need to make it fit


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
      submenu_bg = string.Format("background-color: #{0}", settings["submenu_background_extend"]);

      if (settings["menu_orientation"] == "vertical" && settings["submenu_position"] == "below_header")
        submenu_seperate = string.Format("float:left; width:{0}px;",int.Parse(settings["site_width"]) - 150);
      else if ( settings["submenu_position"] == "leftcolumn")
        submenu_seperate = "float:left;";
      
      //content      
      if ((settings["menu_orientation"] == "vertical" && settings["menu_position"] == "content") ||
        (settings["submenu_position"] == "leftcolumn"))
        content = string.Format("float:left; width:{0}px;", int.Parse(settings["site_width"]) - 150 - 20);
                                                                                         //     menu   padding
      content += string.Format("background-color:#{0};", settings["content_bg_colour"]);
      if (settings["content_bg_image"].Length > 0)
      {
        content += string.Format("background-image:url({0}); background-repeat:{1};", settings["content_bg_image"], settings["content_bg_repeat"]);
      }

      content += string.Format("color:#{0};", settings["content_text_colour"]);

      //content_a
      content_a = string.Format("color:#{0}", settings["content_a_colour"]);
      content_a_hover = string.Format("color:#{0}", settings["content_a_hover"]);

      //footer
      if (settings["home_page_footer"].Length > 0)
        footerText = "position:absolute;top:0;";

      //grab the css template and write over placeholders

      System.IO.StreamReader fileContents = new System.IO.StreamReader(rtg.Properties.Settings.Default.CssFolder+"template.css");
      string css = fileContents.ReadToEnd();
      fileContents.Close();

      css = css.Replace("/*body*/", body); 
      css = css.Replace("/*site_width*/", site_width); 
      css = css.Replace("/*border*/", border);

      css = css.Replace("/*menucontainer*/", menucontainer);
      css = css.Replace("/*menu*/", menu);
      css = css.Replace("/*menu_ul_li*/", menu_ul_li);
      css = css.Replace("/*menu_ul_li_a*/", menu_ul_li_a);
      css = css.Replace("/*menu_ul_li_a_hover*/", menu_ul_li_a_hover);
      css = css.Replace("/*menu_ul_li_a_selected*/", menu_ul_li_a_selected);
      css = css.Replace("/*menu_ul_li_ul*/", menu_ul_li_ul); 
      css = css.Replace("/*menu_ul_li_hover_ul*/", menu_ul_li_hover_ul); 
      css = css.Replace("/*submenu*/", submenu);
      css = css.Replace("/*submenu_li*/", submenu_li);
      css = css.Replace("/*submenu_li_a*/", submenu_li_a);
      css = css.Replace("/*submenu_li_a_hover*/", submenu_li_a_hover);
      css = css.Replace("/*submenu_li_a_selected*/", submenu_li_a_selected);
      css = css.Replace("/*submenu_bg*/", submenu_bg);
      css = css.Replace("/*submenu_seperate*/", submenu_seperate);
      css = css.Replace("/*content*/", content);
      css = css.Replace("/*content_a*/", content_a);
      css = css.Replace("/*content_a_hover*/", content_a_hover);
      css = css.Replace("/*footerText*/", footerText);

      System.IO.StreamWriter newCssFile = new System.IO.StreamWriter(rtg.Properties.Settings.Default.CssFolder+"frontend.css");
      newCssFile.Write(css);
      newCssFile.Flush();
      newCssFile.Close();
    }

    private IDictionary<string, string> GenerateSettingsDictionary()
    {
      IDictionary<string, string> settings = new Dictionary<string, string>();
      foreach (Setting s in db.Settings)
      {
        if (s.SettingKey2 == null && s.SettingKey != "style_sheet")
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
          if (cs.SettingKey != "footer_text" && cs.SettingKey != "style_sheet" && cs.SettingKey != "site_width")
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
            menu_position = "In Content";
            menu_position_topmargin = 0;
            menu_extend = "Full width/depth";
            submenu_postion = "Dropdown/out";
            break;
          case 10:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Content";
            menu_position_topmargin = 0;
            menu_extend = "Full width/depth";
            submenu_postion = "Below menu";
            break;
          case 11:
            menu_orientation = "Menu Horizontal";
            menu_position = "In Content";
            menu_position_topmargin = 0;
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
            menu_orientation = "Menu Vertical";
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

        GenerateCSS();

        return View("SettingsTab", db.Settings.Where(st => st.Tab == "Advanced").OrderBy(st => st.ListOrder));
      }
      return null;
    }
  }
}
