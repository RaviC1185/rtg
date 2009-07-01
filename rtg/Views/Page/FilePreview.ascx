<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.IO.FileInfo>" %>

<% 
   string webref = ViewData["webref"].ToString();

   if(Model.Extension == ".jpg" || Model.Extension == ".gif" || Model.Extension == ".png")
     { 
     %>
     <div>
     <img src="<%= webref %>" /><br />
    <%= Model.Name %><br />
    </div>
<% } %>