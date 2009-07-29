<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<rtg.Models.Gallery>>" %>
<div class="galleryCategory">
<h3><%= Model.FirstOrDefault().GalleryCategory.Title %></h3>
 <% foreach (Gallery g in Model.OrderBy(g=>g.Title))
   { %>
    <% Html.RenderPartial("GalleryPreview", g); %>
<% } %>
</div>