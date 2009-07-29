<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<rtg.Models.Page>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Pages
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div style="float:left; margin: 10px;">
<div>
  <a href="#" class="AddPage">Add page</a> <a href="#" class="DeletePage">Delete page</a>
</div>
<div class="jstree">
    <ul>
    <% foreach (rtg.Models.Page p in Model.Where(pg=>pg.ParentID == null).OrderBy(pg=>pg.MenuOrder))
       { %>
       <% Html.RenderPartial("PageNode", p); %>
    <% } %>
    </ul>
</div
<div id="PageSettings" style="clear:both;">
  Bottom panel
</div>
</div>
<div style="float:left;width:600px;">
		<!-- Tabs -->
		<div id="tabs">
			<ul>
			</ul>
		</div>
</div>
<div id="Gallery" style="float:left;width:300px;margin-left:20px;">
  <div id="FolderBrowser" style="height:300px; overflow:auto;">
  </div> 
  <div id="ImageGallery">
  </div>
</div>
<div style="clear:both;"></div>
</asp:Content>
