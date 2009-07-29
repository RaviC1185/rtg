<%@ Import Namespace="rtg.Models" %>
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.Gallery>" %>

<a href="<%= Url.RouteUrl("Gallery", new {id= Model.GalleryID })%>" class="galleryPreview">
 <img src="<%= Model.GalleryImages.FirstOrDefault().Src %>" />
 <div>
  <h3><%= Model.Title %></h3>
  <div class="desc"><%= Model.Description %></div>
 </div>
</a>