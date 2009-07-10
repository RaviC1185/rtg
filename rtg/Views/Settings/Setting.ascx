<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<rtg.Models.Setting>" %>
<%
  switch(Model.Type)
  {
    case "image":
      %>
        <input type="text" name="<%= Model.SettingKey %>" class="InputImage AutoSave" value="<%= Model.Value %>" /> <%= Model.Label %>
      <%
        break;
    case "colour":
      %>
        <input type="text" name="<%= Model.SettingKey %>" class="InputColour AutoSave" value="<%= Model.Value %>" /> <%= Model.Label %>
      <%
        break;     
    case "colour&image":
      %>
        <input type="text" name="<%= Model.SettingKey %>" class="InputColour InputImage AutoSave" value="<%= Model.Value %>" /> <%= Model.Label %>
      <%
        break;
    case "radioVert":
      %>
        <input type="radio" name="<%= Model.SettingKey %>" value="<%= Model.Label.Replace(" ", "") %>" <%= Model.Value == "true" ? "checked='checked'" : "" %> class="RadioAutoSave" /> <%= Model.Label %>
      <%
        break;      
    case "textarea":
      %>
        <textarea name="<%= Model.SettingKey %>" class="RadioAutoSave" ><%= Model.Value %></textarea>
      <%
        break;
    case "stylesheet":
      %>
        <input type="text" name="<%= Model.SettingKey %>" class="InputStylesheet AutoSave" value="<%= Model.Value %>" /> <%= Model.Label %>
      <%
        break;        
    default:
      %>
        <input type="text" name="<%= Model.SettingKey %>" value="<%= Model.Value %>" class="AutoSave" /> <%= Model.Label %>
      <%
        break;            
  } 
%><br />

