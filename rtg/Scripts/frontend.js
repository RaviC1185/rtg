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
});