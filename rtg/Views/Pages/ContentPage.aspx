<%@ Import Namespace="rtg.Models" %>
<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/FrontEnd.Master" Inherits="System.Web.Mvc.ViewPage<rtg.Models.Page>" %>



<asp:Content ID="Content2" ContentPlaceHolderID="FrontEndTitleContent" runat="server">
<%= Model.MenuTitle %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="FrontEndMainContent" runat="server">
<% foreach (PageObject po in Model.PageObjects)
   {
     if (po.Type == "{html_content}")
     {
     %>
      <%= po.HtmlContent%>
<%   }
   } %>
</asp:Content>