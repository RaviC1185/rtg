<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<Setting>>" %>

<form action="<%= Url.RouteUrl(new {action = "Update" })%>" method="post">
<% 
  foreach (string s in Model.Select(stg => stg.Category).Distinct())
  { %>
       <div>
        <h3><%= s %></h3>
       
       <% foreach (Setting setting in Model.Where(stg => stg.Category == s).OrderBy(stg => stg.ListOrder))
          { %>
            <% Html.RenderPartial("Setting", setting); %>              
       <% } %>
       </div> 
<% } %>
</form>			