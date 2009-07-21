<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div id="Gallery0" class="Gallery DroppableGallery">
<form>
<%= Html.Hidden("GalleryID", 0) %>
<div class="GalleryTitle"><span></span><%= Html.TextBox("Title", "Title") %></div>

<ul class="thumbs sortable">
<li style="list-style-type:none;"></li>
  <div class="dropzone" style="display:none;">
    Drop photos here.
  </div>
<div style="clear:both;"></div>
</ul>
<div class="GalleryDescription"><span></span><%= Html.TextArea("Description", "Description") %></div>
</form>
  <div class="GalleryButtons">
    <a href="#" class="NewCancelGallery ui-icon ui-icon-closethick">Cancel</a>
    <a href="#" class="NewGallery ui-icon ui-icon-check">Save</a>
    <a href="#" class="DeleteGallery ui-icon ui-icon-trash" style="display:none;">Delete</a>
    <a href="#" class="EditGallery ui-icon ui-icon-wrench" style="display:none;">Edit</a> 
  </div>
  <div style="clear:both;"></div>
</div>
