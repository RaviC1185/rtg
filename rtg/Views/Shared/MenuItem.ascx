<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.Page>" %>

<%
  string cssclass= "";
  if(Request.RawUrl.EndsWith(Model.Permalink))
    cssclass = "selected";
%>

<% if (Model.DisplayInMenu == true)
   {  %>
<li>
  <a href="<%= Url.RouteUrl("Pages", new {id= Model.Permalink}) %>" class="<%= cssclass %>"><%= Model.MenuTitle %></a>
   <% Html.RenderPartial("MenuSubmenu", Model); %>
</li>
<% } %>