<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>

<div id="SettingsAccordion">
	<h3><a href="#">Site Structure</a></h3>
	<div>
		<% for (int i = 1; i < 5; i++)
       { %>
       <div>
          <img src="/Images/SiteStyles/<%= i %>.jpg" />
       </div>
    <% } %>
    <div style="clear:both;"></div>
        
    <% for (int i = 5; i < 9; i++)
       { %>
       <div>
          <img src="/Images/SiteStyles/<%= i %>.jpg" />
       </div>
    <% } %>
    <div style="clear:both;"></div>
	</div>
	<h3><a href="#">Colors and Styles</a></h3>
	<div>
		<p>Sed non urna. Donec et ante. Phasellus eu ligula. Vestibulum sit amet purus. Vivamus hendrerit, dolor at aliquet laoreet, mauris turpis porttitor velit, faucibus interdum tellus libero ac justo. Vivamus non quam. In suscipit faucibus urna. </p>
	</div>
	<h3><a href="#">Section 3</a></h3>
	<div>
		<p>Nam enim risus, molestie et, porta ac, aliquam ac, risus. Quisque lobortis. Phasellus pellentesque purus in massa. Aenean in pede. Phasellus ac libero ac tellus pellentesque semper. Sed ac felis. Sed commodo, magna quis lacinia ornare, quam ante aliquam nisi, eu iaculis leo purus venenatis dui. </p>
		<ul>
			<li>List item</li>
			<li>List item</li>
			<li>List item</li>
			<li>List item</li>
			<li>List item</li>
			<li>List item</li>
			<li>List item</li>
		</ul>
	</div>
</div>
