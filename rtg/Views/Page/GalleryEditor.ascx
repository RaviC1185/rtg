<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.Page>" %>
<%= Html.Hidden("PageID", Model.PageID) %>

<a href="#" class="AddGalleryCategory">Add Category</a> <a href="#" class="AddGallery">Add Gallery</a> 

<div id="Category0" class="GalleryCategory SortableGallery">
<%= Html.Hidden("GalleryCategoryID", 0) %>
<% foreach (Gallery g in Model.Galleries.Where(g => g.GalleryCategoryID == null || g.GalleryCategoryID == 0).OrderBy(g=>g.Title))
   { %>
    <% Html.RenderPartial("Gallery", g); %>
<% } %>
</div>

<% 
  rtgDataContext db = new rtgDataContext();
   
   foreach (GalleryCategory gc in db.GalleryCategories.OrderBy(g=>g.Title))
   {
     IEnumerable<Gallery> galleries = Model.Galleries.Where(g => g.GalleryCategoryID == gc.GalleryCategoryID);
     if ( galleries.Count() > 0)
     {%>
       <% Html.RenderPartial("GalleryCategory", galleries); %>
  <% }
   }  %>