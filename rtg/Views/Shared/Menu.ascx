<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% 
  rtgDataContext db = new rtgDataContext();
  Setting menuposition = db.Settings.FirstOrDefault(st => st.SettingKey == "menu_position" && st.Value.ToString() == "true");
  Setting menuset = db.Settings.FirstOrDefault(st => st.SettingKey == "menu_orientation" && st.Value.ToString() == "true");
  Setting submenuset = db.Settings.FirstOrDefault(st => st.SettingKey == "submenu_position" && st.Value.ToString() == "true");
  
  string menuclass = "";

  rtg.Models.Page currentPage = null; 

  if (menuset.SettingKey2 == "horizontal" && submenuset.SettingKey2 == "below_menu")
    menuclass = "SubmenuBelow";
  else if (menuset.SettingKey2 == "horizontal" && submenuset.SettingKey2 == "below_header")
    menuclass = "SubmenuSeperate";
  else if (menuset.SettingKey2 == "vertical" && submenuset.SettingKey2 == "below_header")
    menuclass = "SubmenuSeperate";
%>
<div id="menucontainer">
<div id="menu" class="<%= menuclass %>"> <!--SubmenuBelow SubmenuSeperate-->
  <ul>
    <%foreach(rtg.Models.Page page in db.Pages.Where(p=>p.ParentID == null).OrderBy(p=>p.MenuOrder))
      { %>
          <% Html.RenderPartial("MenuItem", page); %>
    <%  if(Request.Path.EndsWith(page.Permalink))
          currentPage = page;
      } %>
  </ul>
  <div style="clear:both;"></div>
</div>   
  <div id="submenucontainerbelow">
  </div>
</div>
<div id="submenucontainerseperate">
<% 
  if ((menuset.SettingKey2 == "horizontal" && submenuset.SettingKey2 == "leftcolumn" && !Request.Path.StartsWith("/Gallery/")))
  { %>
    <% Html.RenderPartial("MenuSubmenu", currentPage); %>
 <% } %>
</div>