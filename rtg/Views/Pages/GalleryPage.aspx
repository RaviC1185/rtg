<%@ Import Namespace="rtg.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/FrontEnd.Master" Inherits="System.Web.Mvc.ViewPage<rtg.Models.Page>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="FrontEndTitleContent" runat="server">
<%= Model.MenuTitle %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="FrontEndMainContent" runat="server">
 <% foreach (Gallery g in Model.Galleries.Where(g => g.GalleryCategoryID == null || g.GalleryCategoryID == 0).OrderBy(g=>g.Title))
   { %>
    <% Html.RenderPartial("GalleryPreview", g); %>
<% } %>

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
</asp:Content>
