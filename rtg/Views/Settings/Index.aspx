<%@ Import Namespace="rtg.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Setting>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Settings
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div id="SettingsAccordion" style="float:left;width:1280px;">
	<h3><a href="#">Pick your website look</a></h3>
	<div>
		<% 
		   Setting selectedstyle = Model.FirstOrDefault(s=>s.SettingKey == "selected_style");
		   for (int i = 1; i < 5; i++)
       {
         int styleint = 0;
         bool val = int.TryParse(selectedstyle.Value, out styleint);
         %>
       <div id="<%= i %>" class="StyleSelector <%= styleint == i? "StyleSelected" : ""  %>">
          <a href="#">
            <img src="/Images/SiteStyles/<%= i %>.jpg" />
          </a>
       </div>
    <% } %>
	<div style="clear:both;"></div>
	<% for (int i = 5; i < 9; i++)
       {
         int styleint = 0;
         bool val = int.TryParse(selectedstyle.Value, out styleint); 
      %>
       <div id="<%= i %>" class="StyleSelector <%= styleint == i? "StyleSelected" : ""  %>">
          <a href="#">
            <img src="/Images/SiteStyles/<%= i %>.jpg" />
          </a>
       </div>
    <% } %>
	<div style="clear:both;"></div>
	<% for (int i = 9; i < 12; i++)
       {
         int styleint = 0;
         bool val = int.TryParse(selectedstyle.Value, out styleint);
      %>
       <div id="<%= i %>" class="StyleSelector <%= styleint == i? "StyleSelected" : ""  %>">
          <a href="#">
            <img src="/Images/SiteStyles/<%= i %>.jpg" />
          </a>
       </div>
    <% } %>
	<div style="clear:both;"></div>
	<% for (int i = 12; i < 15; i++)
       {
         int styleint = 0;
         bool val = int.TryParse(selectedstyle.Value, out styleint);
      %>
       <div id="<%= i %>" class="StyleSelector <%= styleint == i? "StyleSelected" : ""  %>">
          <a href="#">
            <img src="/Images/SiteStyles/<%= i %>.jpg" />
          </a>
       </div>
    <% } %>
	<div style="clear:both;"></div>
	<% for (int i = 15; i < 18; i++)
       {
         int styleint = 0;
         bool val = int.TryParse(selectedstyle.Value, out styleint); 
      %>
       <div id="<%= i %>" class="StyleSelector <%= styleint == i? "StyleSelected" : ""  %>">
          <a href="#">
            <img src="/Images/SiteStyles/<%= i %>.jpg" />
          </a>
       </div>
    <% } %>
	<div style="clear:both;"></div>		
	</div>
	<h3><a href="#">Colors and Styles</a></h3>
	<div>
	  <% Html.RenderPartial("SettingsTab", Model.Where(stg => stg.Tab == "ColourStyle").OrderBy(stg => stg.ListOrder)); %>				
	</div>
	<h3><a href="#">Advanced</a></h3>
	<div id="AdvancedTab">
	 <% Html.RenderPartial("SettingsTab", Model.Where(stg => stg.Tab == "Advanced").OrderBy(stg => stg.ListOrder)); %>						
	</div>
</div>
<div id="File Manager" style="float:left;width:300px;margin-left:20px;">
  <div id="FolderBrowser" style="height:500px; overflow:auto;">
  </div> 
  <div id="ImageGallery">
  </div>
</div>
<div style="clear:both;"
</asp:Content>
