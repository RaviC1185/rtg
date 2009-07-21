<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.Page>" %>
<form id="SettingsForm" method="post" action="<%= Url.RouteUrl("AdminPages",new {id = Model.PageID, action="SavePageSettings"})%>"  >
  <%= Html.Hidden("PageId", Model.PageID)%>
<p>
  <label>Title</label><%= Html.TextBox("Title", Model.Title, new { @class = "SettingsEdit" })%>
</p>
<p>
  <label>Menu label</label><%= Html.TextBox("MenuTitle", Model.MenuTitle, new { @class = "SettingsEdit" })%>
</p>
<p>
  <label>Locked</label><%= Html.CheckBox("Locked", Model.Locked == 1, new { @class = "SettingsClick" })%>
</p>
<p>
  <label>Visable in menu</label><%= Html.CheckBox("DisplayInMenu", Model.DisplayInMenu == true, new { @class = "SettingsClick" })%>
</p>
<p>
  <label>Page Type</label>
  <% 
rtgDataContext db = new rtgDataContext();
SelectList sl = new SelectList(db.PageTemplates, "TemplateID", "Title", Model.PageTemplate); 
  %>
  <%= Html.DropDownList("TemplateID", sl, new { @class = "SettingsClick" })%>
</p>
<input type="submit" value="Save" id="SettingsSave" style="display:none;clear:both;"/>
</form>