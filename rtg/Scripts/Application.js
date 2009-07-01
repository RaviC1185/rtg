var nodeCreate = 0; 

function filePreview(file)
{
  $.post("/Page/GetFilePreview", {file: file}, function(data){
    $("#ImageGallery").html(data);
  });
}

$(document).ready(function(){
  $('#tabs').tabs({  
     tabTemplate: '<li><a href="#{href}"><span>#{label}</span></a> <a href="#{href}" class="CloseTab ui-icon ui-icon-circle-close" style="padding:0;cursor:pointer;">Close</a></li>'
  });
  
  $('#FolderBrowser').fileTree({
      root: '/WhiteTree/WhiteTree/upload/',
      script: '/jqueryFileTree/connectors/jqueryFileTree.aspx'
    }, function(file) { 
        filePreview(file);
  });
  
  $(".AddPage").click(function(){
    nodeCreate = 1;
    if($.tree_reference('jstree_1').selected)
        $.tree_focused().create({ attributes: { id : "newnode", rel:"invisible"}});
    else
      $.tree_reference("jstree_1").create({ attributes: { id : "newnode", rel:"invisible"}}, -1);
    return false;
    //$.post("/Page/AddPage");
  });
  
  $(".DeletePage").click(function(){
    if($.tree_reference('jstree_1').selected.attr("rel") != "locked")
    {
      var id = $.tree_reference('jstree_1').selected.attr("id");
      $.tree_focused().remove();
      $.post("/Page/Delete/"+id);
    }
  });
  
  $(".jstree").tree({
       ui      : {
      theme_name  : "mezza",
      context : {}},
    rules   : {
      multiple    : false,    // FALSE | CTRL | ON - multiple selection off/ with or without holding Ctrl
      metadata    : false,    // FALSE or STRING - attribute name (use metadata plugin)
      type_attr   : "rel",    // STRING attribute name (where is the type stored if no metadata)
      multitree   : false,    // BOOL - is drag n drop between trees allowed
      createat    : "bottom", // STRING (top or bottom) new nodes get inserted at top or bottom
      use_inline  : false,    // CHECK FOR INLINE RULES - REQUIRES METADATA
      clickable   : "all",    // which node types can the user select | default - all
      renameable  : ["page", "invisible"],    // which node types can the user select | default - all
      deletable   : ["page", "invisible"],    // which node types can the user delete | default - all
      creatable   : ["page", "invisible"],    // which node types can the user create in | default - all
      draggable   : ["page", "invisible"],   // which node types can the user move | default - none | "all"
      dragrules   : "all",    // what move operations between nodes are allowed | default - none | "all"
      drag_copy   : false,    // FALSE | CTRL | ON - drag to copy off/ with or without holding Ctrl
      droppable   : [],
      drag_button : "left"},
    callback    : {           
      onchange    : function(NODE,TREE_OBJ) {jstree_onchange(NODE)},
      ondblclk    : function(NODE, TREE_OBJ) {jstree_ddclick(NODE)},
      onselect    : function(NODE,LANG,TREE_OBJ,RB) {},                  // node selected
      onrename    : function(NODE,LANG,TREE_OBJ,RB) { jstree_rename(NODE)},              // node renamed ISNEW - TRUE|FALSE, current language
      onmove      : function(NODE,REF_NODE,TYPE,TREE_OBJ,RB) {jstree_savetree()}, // move completed (TYPE is BELOW|ABOVE|INSIDE)
      oncreate    : function(NODE,REF_NODE,TYPE,TREE_OBJ,RB) { }, // node created, parent node (TYPE is createat)
      ondelete    : function(NODE, TREE_OBJ,RB) { },                  // node deleted
      ondrop      : function(NODE,REF_NODE,TYPE,TREE_OBJ) {}
    },
    lang : {new_node: "New Page"}
  });
  $.tree_reference('jstree_1').open_all();
});

function init()
{
  $('.CloseTab').unbind('click');
  $('.CloseTab').click(function(){
    var tabid = $(this).attr('href');
    $('.ui-tabs-panel').each(function(index){
      var tab = $(this);
      if('#'+tab.attr('id') == tabid){
        $('#tabs').tabs('remove', index);
      }  
    });
    return false;
  });

  $('.ToggleButton').click(function(){
    $(this).toggleClass('ToggleButtonOpen');
    $(this).parent().find('.GalleryContent').slideToggle();
    $(this).parent().find('.GalleryButtons').toggle();
  });

  $(".directory").unbind('click');
  $('.directory').click(function(){
    var folder= $(this).children("a").attr("rel");
    alert("folder["+folder+"]");
  });

  $(".SettingsEdit").unbind('keydown');
  $(".SettingsEdit").keydown(function(){
    $("#SettingsSave").show();
  });

  $(".SettingsClick").unbind('click');
  $(".SettingsClick").click(function(){
    $("#SettingsSave").show();
  });

  $("#SettingsForm").unbind('submit');
  $("#SettingsForm").submit(function(){
    var action = $(this).attr("action");
    var form = $(this).serialize();
    //save the settings and get the node back and replace it.
    $.post(action, form, function(data){
      var arr 
      eval("arr="+data); 
      var node = $("#"+arr[0]);
      node.removeClass("page");
      node.removeClass("invisible");
      node.removeClass("locked");
      node.addClass(arr[2]);
      node.attr("rel", arr[2]);
      node.children("a:visible").text(arr[1]);
      $.tree_reference('jstree_1').refresh;
    });
    return false;
  });
  
  $(".PageContentSave").unbind('click');
  $(".PageContentSave").click(function(){
    alert("Save");
    var id = $(this).attr("name");
    var editor = xinha_editors[id];
    var content = editor.getEditorContent();
    alert(content);
    var action = $(this).parents("form").attr("action");
    $.post(action, {HtmlContent: content});
    return false;
  });
  
  $(".PageContentCloseTab").unbind('click');
  $(".PageContentCloseTab").click(function(){
      var selected = $('#tabs').tabs('option', 'selected');
      if(selected == -1)
        selected = 0;
      $('#tabs').tabs('remove', selected);
  });
  
  $(".AddGalleryCategory").unbind('click');
  $(".AddGalleryCategory").click(function(){
    var nullcategory = $("#Category0");  
    $.get("/page/AddGalleryCategory", function(data){
      nullcategory.before(data);
      init();
    });
  });
  
  $(".SaveGalleryCategory").click(function(){
    var category = $(this).parents(".GalleryCategory");
    var catname = $(this).prev().attr("value");
    category.find(".Title").html(catname);
    $(this).prev().remove();
    $(this).next().remove();
    $(this).remove();
    $.post("/page/SaveGalleryCategory", {CategoryTitle: catname}, function(catID){
      category.attr("id", "Category"+catID);
      category.find("input[name=GalleryCategoryID]").attr("value", catID);
      category.find(".dropzone").show();
    });
  });
  
  $(".CancelGalleryCategory").click(function(){
    $(this).parents(".GalleryCategory").remove();
  });
  
  $(".AddGallery").unbind('click');
  $(".AddGallery").click(function(){
    if($("#Gallery0").length > 0)
    {
      $("#Gallery0").remove();
    }else{
      var me = $(this)
      $.get("/page/AddGallery", function(data){
        $("#Category0").prepend(data);
        init();
        $("#Gallery0").find(".sortable").sortable('disable');
      });
    }
  });
  
  $(".NewGallery").unbind('click');
  $(".NewGallery").click(function(){
    var pageid = $(this).parents(".ui-tabs-panel").find("input[name=PageID]").attr("value");
    var gallery = $(this).parents("div.Gallery");
    var form = gallery.find("form").serialize();
    form = form+"&pageid="+pageid;
    $.post("/page/SaveGallery", form, function(data){
      gallery.find("input[name=GalleryID]").attr("value", data);
      gallery.attr("id", "Gallery"+data);
    });
    var title = gallery.find("div.GalleryTitle").find("span");
    title.html(title.next().attr("value"));
    title.next().remove();
    //title.toggle();
    var desc = gallery.find("div.GalleryDescription").find("span");
    desc.html(desc.next().attr("value"));
    desc.next().remove();
    //desc.toggle();
    
    gallery.find(".sortable").sortable('enable');
    gallery.find(".dropzone").show();
    
    $(this).toggle();
    $(this).toggleClass("NewGallery");
    $(this).toggleClass("SaveGallery")
    var cg = $(this).siblings("a.NewCancelGallery");
    cg.toggleClass("NewCancelGallery");
    cg.toggleClass("CancelGallery");
    cg.toggle();
    $(this).siblings("a.DeleteGallery").toggle();
    $(this).siblings("a.EditGallery").toggle();
    return false;
  });
  
  $(".NewCancelGallery").unbind('click');
  $(".NewCancelGallery").click(function(){
   $(this).parents("div.Gallery").remove();
    return false;
  });
  
  $(".EditGallery").unbind('click');
  $(".EditGallery").click(function(){
    var gallery = $(this).parents("div.Gallery");
    var title = gallery.find("div.GalleryTitle").find("span");
    title.toggle();
    title.after("<input type='text' name='Title' value='"+title.text()+"'/>");
    var desc = gallery.find("div.GalleryDescription").find("span");
    desc.toggle();
    desc.after("<textarea name='Description'>"+desc.text()+"</textarea>");
    $(this).toggle();
    $(this).siblings("a.DeleteGallery").toggle();
    $(this).siblings("a.SaveGallery").toggle();
    $(this).siblings("a.CancelGallery").toggle();
    return false;
  });
  
  $(".PreviewGalleryImage").unbind('click');
  $(".PreviewGalleryImage").click(function(){
		  var src = $(this).attr('href');
			var $modal = $('img[src$="'+src+'"]');
      var arr = src.split("/");
      var title = arr[arr.length-1];
			//if ($modal.length) {
			//	$modal.dialog('open')
			//} else {
				var img = $('<img alt="" style="display:none;" />').attr('src',src).appendTo('body');
				var width = img.width()+25;
				var height = img.height()+49;
				setTimeout(function() {
					img.dialog({
							title: title,
							width: width,
							height: height,
							modal: true
						});
				}, 1);
			//}
			return false;
  });
  
  $(".DeleteGallery").unbind('click');
  $(".DeleteGallery").click(function(){
    var gallery = $(this).parents("div.Gallery");
    var galleryID = gallery.find("input[name=GalleryID]").attr("value");
    $.post("/page/DeleteGallery/"+galleryID);
    gallery.remove();
    return false;
  });
  
  $(".SaveGallery").unbind('click');
  $(".SaveGallery").click(function(){
    var gallery = $(this).parents("div.Gallery");
    var form = gallery.find("form").serialize();
    $.post("/page/SaveGallery", form);
    var title = gallery.find("div.GalleryTitle").find("span");
    title.html(title.next().attr("value"));
    title.next().remove();
    title.toggle();
    var desc = gallery.find("div.GalleryDescription").find("span");
    desc.html(desc.next().attr("value"));
    desc.next().remove();
    desc.toggle();
    $(this).toggle();
    $(this).siblings("a.DeleteGallery").toggle();
    $(this).siblings("a.EditGallery").toggle();
    $(this).siblings("a.CancelGallery").toggle();
    return false;
  });
  
  $(".CancelGallery").unbind('click');
  $(".CancelGallery").click(function(){
    var gallery = $(this).parents("div.Gallery");
    var title = gallery.find("div.GalleryTitle").find("span");
    title.next().remove();
    title.toggle();
    var desc = gallery.find("div.GalleryDescription").find("span");
    desc.next().remove();
    desc.toggle();
    $(this).toggle();
    $(this).siblings("a.DeleteGallery").toggle();
    $(this).siblings("a.EditGallery").toggle();
    $(this).siblings("a.SaveGallery").toggle();
    return false;
  });
  
  $(".DeleteGalleryImage").click(function(){
    var id =  $(this).attr("href");
    $(this).parents(".GalleryImage").remove();
    $.post("/page/DeleteGalleryImage/"+id);
    return false;
  });
  
  $(".draggable").draggable({revert: true, scroll: false, helper: 'clone' });
  //$(".DraggableImage").draggable({revert: true, handle: 'img'});
  
  $(".droppable").droppable({
	  drop: function(ev, ui) {
		  var pageId = $(this).find("input[name=PageId]").attr("value");
		  //if()
			dropImage(pageId, ui.draggable);
		}
  });
  $(".DroppableGallery").droppable({
	  drop: function(ev, ui) {
		  var galleryId = $(this).find("input[name=GalleryID]").attr("value");
		  var item = ui.draggable;
		  if(item.hasClass('file')){
			  dropGalleryImage(galleryId, ui.draggable);
			}else if(item.hasClass('directory')){
			  dropGalleryFolder(galleryId, ui.draggable);
			}
		}
  });
  
  $(".sortable").sortable({
    items: 'li',
    connectWith: ".sortable",
    update: function(ev, ui){ SaveGalleryCollection($(this)); }}).disableSelection();
    
  $(".SortableGallery").sortable({
    handle: "div.GalleryTitle span",
    connectWith: ".SortableGallery",
    update: function(ev, ui){ SaveCategoryCollection($(this)); }});
  $(".GalleryTitle span").disableSelection();
}

function SaveGalleryCollection(gallery)
{
  gallery.find(".dropzone").remove();
  var imageids = gallery.sortable("toArray");
  var galleryID = gallery.parents("div.Gallery").find("#GalleryID").attr("value");
  $.post("/page/SaveGalleryCollection", {galleryID: galleryID, images: imageids});
}

function SaveCategoryCollection(category)
{
  category.find(".dropzone").remove();
  var galleryIDs = category.sortable("toArray");
  var categoryID = category.find("#GalleryCategoryID").attr("value");
  $.post("/page/SaveCategoryCollection", {categoryID: categoryID, galleries: galleryIDs});
}

function dropImage(pageId, $item)
{
  var editor = xinha_editors["HtmlContent-"+pageId];
  var src = $item.children("a").attr("rel");
  var fname = $item.children("a").text();
  var arr = src.split('/upload/');
  var src = "/upload/"+arr[1];
  if(fname.indexOf(".jpg") != -1 || fname.indexOf(".gif") != -1|| fname.indexOf(".png") != -1)
    editor.insertHTML("<img src='"+src+"' />");
  else
    editor.insertHTML("<a href='"+src+"'>"+fname+"</a>")
}

function dropGalleryImage(galleryID, $item)
{
  //alert("drop Image");
  var src = $item.children("a").attr("rel");
  //alert("src="+src);
  $.post("/Page/AddImageToGallery", {galleryID: galleryID, imageSrc: src}, function(data){
    $("#Gallery"+galleryID).find(".thumbs").children("div").before(data);
    init();
  });
}

function dropGalleryFolder(galleryID, $item)
{
  //alert("DropFolder");
  var folder = $item.children("a").attr("rel");
  $.post("/Page/AddFolderToGallery", {galleryID: galleryID, folder: folder}, function(data){
    $("#Gallery"+galleryID).find(".thumbs").html(data);
    $("#Gallery"+galleryID).find(".thumbs").prepend('<li style="list-style-type: none;"/>');
    $("#Gallery"+galleryID).find(".thumbs").append('<div style="clear: both;"/>');
    init();
  });
}

function jstree_rename(node)
{
  var parentid = 0;
  if($(node).parents("li:eq(0)").get(0))
  {
    parentid = $(node).parents("li:eq(0)").get(0).id
  }
  
  var title = $(node).children("a:visible").text();
  if(nodeCreate==1)
  {
    //Create
    $.post("/Page/AddPage", {title: title, parentid: parentid}, function(data){
      //replace node id
      $(node).attr("id", data);
    });
    nodeCreate = 0;
  }
  else
  {
    //rename
  }
}

function jstree_onchange(node)
{
  if(node.id)
  {
    $.get("/Page/GetPageSettings/"+node.id, function(data){
      $("#PageSettings").html(data);
      init();
    });  
  }
}

function jstree_ddclick(node)
{
    var title = $(node).children("a:visible").text();
    var divIdName = 'tabs-' + node.id;
    if($("#"+divIdName).length == 0) //if the tab doesn't already exist
    {
      var newdiv = document.createElement('div');
      newdiv.setAttribute('id', divIdName);            
      document.getElementById("tabs").appendChild(newdiv);
      $('#tabs').tabs( 'add' , '#' + divIdName , title );
      tabContent(node.id, divIdName);
      $('#tabs').tabs( 'select' , '#' + divIdName );
    }
}

function jstree_savetree()
{ 
  //alert("move");
  var jtree = $.tree_reference('jstree_1').getJSON();
  //alert("jtree={"+jtree+"}");
  $.post("/Page/SavePageTree", {pagetree: JSON.stringify(jtree)});
  //alert("sent");
}

        
var xinha_plugins = [];
var xinha_editors = [];

function xinha_init(editor_ids)
{
  if(!Xinha.loadPlugins(xinha_plugins, xinha_init)) return;
  var xinha_config = new Xinha.Config();
  xinha_editors = Xinha.makeEditors(editor_ids, xinha_config, xinha_plugins);
  Xinha.startEditors(xinha_editors);
}

function tabContent(pageId, tabId)
{
  $.get("/Page/GetPageEditorPanel/"+pageId, function(data){
      $("#"+tabId).html(data);
      init();
      var ids = new Array();
      $("#"+tabId).find(".xinha-editor").each(function(index){
      //$(".xinha-editor").each(function(index){
        ids[index] = $(this).attr("id");
      });
      xinha_init(ids);
  });
}
