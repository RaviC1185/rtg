$(document).ready(function(){
/************************************************************************************************************************************************************
************ Menu
************************************************************************************************************************************************************/

$('.SubmenuBelow > ul > li').hover(
  function(){
   var submenu = $(this).find('.submenu').clone();
    $('#submenucontainerbelow').html(submenu);
  }
);

$('.SubmenuSeperate > ul > li').hover(
  function(){
   var submenu = $(this).find('.submenu').clone();
    $('#submenucontainerseperate').html(submenu);
  }
);

/************************************************************************************************************************************************************
************ Gallery
************************************************************************************************************************************************************/
  $('.lightbox').lightBox({
     imageLoading: '/Plugins/jquery-lightbox-0.5/images/lightbox-btn-loading.gif',
	   imageBtnClose: '/Plugins/jquery-lightbox-0.5/images/lightbox-btn-close.gif',
	   imageBtnPrev: '/Plugins/jquery-lightbox-0.5/images/lightbox-btn-prev.gif',
	   imageBtnNext: '/Plugins/jquery-lightbox-0.5/images/lightbox-btn-next.gif',
  });
});