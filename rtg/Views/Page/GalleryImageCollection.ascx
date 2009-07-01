<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<rtg.Models.GalleryImage>>" %>

<% foreach(GalleryImage gi in Model) 
   { %>
      <% Html.RenderPartial("GalleryImage", gi); %>
<% } %>
