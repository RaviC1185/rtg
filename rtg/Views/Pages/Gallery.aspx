<%@ Import Namespace="rtg.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/FrontEnd.Master" Inherits="System.Web.Mvc.ViewPage<rtg.Models.Gallery>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="FrontEndTitleContent" runat="server">
<%= Model.Title %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FrontEndMainContent" runat="server">
<h3><%= Model.Title %></h3>

<% foreach (GalleryImage gi in Model.GalleryImages.OrderBy(g => g.Position))
   { %>
  <a href="<%= gi.Src%>" class="lightbox"><img id="<%= gi.Position %>" src="/Plugins/image_resize/Image.aspx?imgsrc=<%= Url.Encode(gi.Src)%>&width=100&height=100" /></a>
<% } %>
</asp:Content>


