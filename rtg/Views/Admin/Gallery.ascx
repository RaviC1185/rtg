<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.Gallery>" %>

<div id="Gallery<%= Model.GalleryID %>" class="Gallery DroppableGallery">
<a href="#" class="ToggleButton">toggle</a>
<form>
<%= Html.Hidden("GalleryID", Model.GalleryID) %>
<div class="GalleryTitle"><span><%= Model.Title %></span></div>

<div class="GalleryContent" style="display:none;">
<ul class="thumbs sortable">
<li style="list-style-type:none;"></li>
<% foreach (GalleryImage gi in Model.GalleryImages.OrderBy(i=>i.Position))
   { %>
   <% Html.RenderPartial("GalleryImage", gi); %>
<% } %>
<div style="clear:both;"></div>
</ul>
<div class="GalleryDescription"><span><%= Model.Description %></span></div>
</form>
  <div class="GalleryButtons" style="display:none;">
    <a href="#" class="CancelGallery ui-icon ui-icon-closethick" style="display:none;">Cancel</a>
    <a href="#" class="SaveGallery ui-icon ui-icon-check" style="display:none;">Save</a>
    <a href="#" class="DeleteGallery ui-icon ui-icon-trash" >Delete</a>
    <a href="#" class="EditGallery ui-icon ui-icon-wrench">Edit</a> 
  </div>
</div>
  <div style="clear:both;"></div>
</div>