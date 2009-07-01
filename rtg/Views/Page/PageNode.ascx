<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.Page>" %>
<%  string rel = "";
    if (Model.Locked == 1)
      rel = "locked";
    else if (Model.DisplayInMenu == false)
      rel = "invisible";
    else
      rel = "page";
%>

<li id="<%= Model.PageID %>" rel="<%= rel %>"  class="<%= rel %>">
  <a href="#">
    <%= Model.Title %>
  </a>
<% if( Model.Children.Count > 0)
   { %>
  <ul>
    <% foreach (rtg.Models.Page p in Model.Children.OrderBy(pg => pg.MenuOrder))
       { %>
        <% Html.RenderPartial("~/Views/Page/PageNode.ascx", p); %>
    <% } %>
  </ul>
<% } %>
</li>
