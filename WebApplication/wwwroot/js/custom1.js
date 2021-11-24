
var CustomController = function () {
    this.initialize = function () {
        
        regsiterEvents();
        loadCart();
    }
    function loadCart() {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "GET",
            url: "/" + culture + '/Cart/GetListItems',
            success: function (res) {
                $('#lbl_number_items_header').text(res.length);
            }
        });
    }

    function regsiterEvents() {
        $('body').on('click', '.add-to-cart', function (e) {
            e.preventDefault();

            const id = $(this).data('id');
            const culture = $('#hidCulture').val();
            
            $.ajax({
                type: "POST",
                url: "/" + culture + '/Cart/AddToCart',
                data: {
                    id: id,
                    languageId: culture
                },
                success: function (res) {
                    $('#lbl_number_items_header').text(res.length);

                },
                error: function (err) {
                    console.log(err);
                }
            });
            

        });
    }
    function pagingEvents() {
        $('body').on('click', '.add-to-cart', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const culture = $('#hidCulture').val();
            $.ajax({
                type: "POST",
                url: "/" + culture + '/Cart/AddToCart',
                data: {
                    id: id,
                    languageId: culture
                },
                success: function (res) {
                    $('#lbl_number_items_header').text(res.length);

                },
                error: function (err) {
                    console.log(err);
                }
            });

        });
        function numberWithCommas(x) {
            return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
        }


    }
}
