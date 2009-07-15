<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.Page>" %>
<li><a href=""><%= Model.MenuTitle %></a></li>
<% foreach (rtg.Models.Page page in Model.Children.OrderBy(p => p.MenuOrder))
   { %>
  <% Html.RenderPartial("MenuSubItem", page); %>
<% } %>
