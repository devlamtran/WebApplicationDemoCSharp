var CartController = function () {
    this.initialize = function () {
        loadData();
        registerEvents();
    }

    function loadData() {
        const culture = $('#hidCulture').val();
        
        $.ajax({
            type: "GET",
            url: "/" + culture + '/Cart/GetListItems',
            success: function (res) {
                if (res.length === 0) {
                    $('#tbl_cart').hide();
                }
                var html = '';
                var total = 0;

                $.each(res, function (i, item) {
                   
                    var amount = item.price * item.quantity;
                    html += "<tr>"
                        + " <td class=\"cart_product\" ><a href=\"\"><img width=\"80\" src=\"/" + item.image + "\" alt=\"\"></a></td>"
                        + " <td class=\"cart_description\"> <h4><a href=\"\">" + item.name + "</a></h4><p>Web ID: 1089772</p> </td>"
                        + " <td class=\"cart_price\"> <p>" + numberWithCommas(item.price) + "</p></td>"
                        + " <td class=\"cart_quantity\"><div class=\"cart_quantity_button\"><a class=\"cart_quantity_up\" data-id=\"" + item.productId + "\" href=\"\">" + "+" + "</a> <input class=\"cart_quantity_input\" type=\"text\" name=\"quantity\" value=\"" + item.quantity + "\" autocomplete=\"off\" size=\"2\"><a class=\"cart_quantity_down\" data-id=\"" + item.productId + "\" href=\"\"> - </a></div></td>"
                        + " <td class=\"cart_total\"><p class=\"cart_total_price\">" + numberWithCommas(amount) + "</p></td>"
                        + " <td class=\"cart_delete\"><a class=\"cart_quantity_delete btn-remove\" href=\"\"  data-id=\"" + item.productId + "\"><i class=\"fa fa-times\"></i></a></td>"
                         + "</tr>";
                    total += amount;
                });
                $('#cart_body').html(html);
                $('#lbl_number_of_items').text(res.length);
                $('#lbl_total').text(numberWithCommas(total));
            }
        });
    }

    function registerEvents() {
        $('body').on('click', '.cart_quantity_up', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('.cart_quantity_input').val()) + 1;
            updateCart(id, quantity);
        });

        $('body').on('click', '.cart_quantity_down', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            const quantity = parseInt($('.cart_quantity_input').val()) - 1;
            updateCart(id, quantity);
        });
        $('body').on('click', '.btn-remove', function (e) {
            e.preventDefault();
            const id = $(this).data('id');
            updateCart(id, 0);
        });
    }
    function updateCart(id, quantity) {
        const culture = $('#hidCulture').val();
        $.ajax({
            type: "POST",
            url: "/" + culture + '/Cart/UpdateCart',
            data: {
                id: id,
                quantity: quantity
            },
            success: function (res) {
                $('#lbl_number_items_header').text(res.length);
                loadData();
            },
            error: function (err) {
                console.log(err);
            }
        });
    }
    function numberWithCommas(x) {
        return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    }
}
