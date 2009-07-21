<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.Page>" %>

<ul class="submenu">
    <% foreach (rtg.Models.Page page in Model.Children.OrderBy(p => p.MenuOrder))
       { %>
       <% Html.RenderPartial("MenuSubItem", page); %>
    <% } %>
    <div style="clear:both;"></div>
</ul>