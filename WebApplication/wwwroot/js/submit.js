$(document).ready(function (e) {

    $('input#submitButton').click(function () {
        const culture = $('#hidCulture').val();
        var userName = $('#userName').val();
        var fdata = new FormData();
        var form = $('#myForm')[0];
        var data = new FormData(form);
        

        $.ajax({
            url: "/" + culture + '/account/' + userName,
            type: 'POST',
            enctype: 'multipart/form-data',
            data: data,
            processData: false,
            contentType: false,
            success: function () {
               
            }
        });

        $("#image").show("fast", function () {
            // Which anchor is being used?
            const culture = $('#hidCulture').val();
            var userName = $('#userName').val();
            $.ajax({
                url: "/" + culture + '/User/GetImage',
                type: 'GET',
                data: userName,
                success: function (data) {
                    $(this).attr('src', "/"+data);
                }
            });
        });
    });
})