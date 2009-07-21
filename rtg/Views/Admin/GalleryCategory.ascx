<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<rtg.Models.Gallery>>" %>

<% if (Model != null) //Existing
   { %>
<div class="GalleryCategory SortableGallery">
<%= Html.Hidden("GalleryCategoryID", Model.First().GalleryCategory.GalleryCategoryID)%>
<div class="Title"><%= Model.First().GalleryCategory.Title%></div>

<% foreach (Gallery g in Model)
   { %>
       <% Html.RenderPartial("Gallery", g); %>
<% } %>

</div>
<% }
   else //new
   { %>
<div class="GalleryCategory SortableGallery">
  <%= Html.Hidden("GalleryCategoryID", 0)%>
  <div class="Title"></div>
  <%= Html.TextBox("Title", "Category Title", new { style="float:left;"})%>
    <a href="#" class="SaveGalleryCategory ui-icon ui-icon-check" style="float:left;">Save</a>
    <a href="#" class="CancelGalleryCategory ui-icon ui-icon-closethick" style="float:left;">Cancel</a>
  <div style="clear:both;"></div>
  <div class="dropzone" style="display:none;">
    Drop Galleries here.
  </div>
</div>
<% } %>
