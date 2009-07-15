<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% 
  rtgDataContext db = new rtgDataContext();
  //Setting menuset = db.Settings.FirstOrDefault(st => st.SettingKey == "menu_orientation" && st.Value == "true");
  //Setting submenuset = db.Settings.FirstOrDefault(st => st.SettingKey == "submenu_position" && st.Value == "true");
  string menuclass = "";


  //if (menuset.SettingKey2 == "horizontal" && submenuset.SettingKey2 == "below_menu")
  //  menuclass = "SubmenuBelow";
  //else if (menuset.SettingKey2 == "horizontal" && (submenuset.SettingKey2 == "below_header" || submenuset.SettingKey2 == "leftcolumn"))
  //  menuclass = "SubmenuSeperate";
  //else if (menuset.SettingKey2 == "vertical" && submenuset.SettingKey2 == "below_header")
  //  menuclass = "SubmenuSeperate";  
%>
<div id="menu" class="<%= menuclass %>"> <!--SubmenuBelow SubmenuSeperate-->
  <ul>
    <%foreach(rtg.Models.Page page in db.Pages.Where(p=>p.ParentID == null).OrderBy(p=>p.MenuOrder))
      {
        if (page.DisplayInMenu == true)
        { %>
          <% Html.RenderPartial("MenuItem", page); %>
    <%  }
      } %>
    <!--li>
      <a href="#">menu item</a>
      <ul class="submenu">
        <li><a href="#">one</a></li>
        <li><a href="#">two</a></li>
        <li><a href="#">three</a></li>
        <li><a href="#">four</a></li>
        <div style="clear:both;"></div>
      </ul>
    </li>
    <li>
      <a href="#" class="selected">menu item</a>
      <ul class="submenu">
        <li><a href="#">one b</a></li>
        <li><a href="#">two b</a></li>
        <li><a href="#">three b</a></li>
        <li><a href="#">four b</a></li>
      </ul>
    </li>
    <li><a href="#">menu item</a></li>
    <li><a href="#">menu item</a></li-->
  </ul>
  <div style="clear:both;"></div>
</div>
