<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div>
  
  <div id="FileUpload">You have a problem with your javascript</div>
	<div id="fileQueue"></div>
  <p></p>
</div>

<input type="button" class="FileManagerNewFolder" value="New Folder" />
<input type="button" class="FileManagerDeleteItem" value="Delete" />
<div id="FileManager">
</div> 
<div style="clear:both;"></div>

<div id="ConfirmDeleteDialog" title="Delete Items?">
	<p><span class="ui-icon ui-icon-alert" style="float:left; margin:0 7px 20px 0;"></span>Are you sure?</p>
</div>

</asp:Content>
