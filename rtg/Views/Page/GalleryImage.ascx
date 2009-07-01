<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.GalleryImage>" %>
<li id="GI<%= Model.GalleryImageID %>" class="GalleryImage DraggableImage" >
  <%= Html.Hidden("GalleryImageID", Model.GalleryImageID) %>
  <img src="<%= Model.Src %>" alt="<%= Model.Title %>" height="70" width="60"/>
  <div>
  <a href="<%= Model.Src %>" title="View larger image" class="PreviewGalleryImage ui-icon ui-icon-zoomin">View larger</a>
  <a href="<%= Model.GalleryImageID %>" title="Delete this image" class="DeleteGalleryImage ui-icon ui-icon-trash">Delete image</a>
  </div>
</li>