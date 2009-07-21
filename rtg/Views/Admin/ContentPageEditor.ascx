<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.Page>" %>

<% rtg.Models.PageObject po = Model.PageObjects.FirstOrDefault();  %>
<form action="<%= Url.RouteUrl("AdminPages", new { action="UpdatePageContent", id=Model.PageID})%>" class="droppable">
<%= Html.Hidden("PageId", Model.PageID) %>
<textarea id="HtmlContent-<%= Model.PageID %>" name="HtmlContent-<%= Model.PageID %>" class="xinha-editor" rows="25" cols="80">
<%= po.HtmlContent %>
</textarea>
<input type="submit" value="Save" name="HtmlContent-<%= Model.PageID %>" class="PageContentSave"/> <a href="#" class="PageContentCloseTab">Close</a>
</form>