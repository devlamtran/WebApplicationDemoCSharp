$(document).ready(function (e) {

    $('.topMenu').click(function () {

        //console.log("Clicked");
        $('.topMenu a.active').removeClass('active');
        $(this).addClass('active');
    });
     
})