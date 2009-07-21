<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.Page>" %>

<%
  string cssclass= "";
  if(Request.RawUrl.Contains(Model.Permalink))
    cssclass = "selected";
%>

<% if (Model.DisplayInMenu == true)
   {  %>
  <li><a href="<%= Url.RouteUrl("Pages", new {id= Model.Permalink}) %>" class="<%= cssclass %>"><%= Model.MenuTitle%></a></li>
  <% foreach (rtg.Models.Page page in Model.Children.OrderBy(p => p.MenuOrder))
     { %>
   <% Html.RenderPartial("MenuSubItem", page); %>
  <% } %>
<% } %>
