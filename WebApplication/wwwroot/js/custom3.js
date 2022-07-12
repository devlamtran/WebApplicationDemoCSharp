$(document).ready(function (e) {
	

	$('.paging-itemOrderAdmin').click(function (e) {
		e.preventDefault();
		const culture = $('#hidCulture').val();

		$(".pagingOrderAdmin").find(".paging-itemOrderAdmin.active").removeClass("active");
		$(this).addClass("active");

		var pageNumber = $(this).attr("data-page");

		var keyword = $(this).attr("data-key");

		$.ajax({
			type: 'GET',
			url: "/" + culture + '/CheckoutAdmin/PagingOrderAdminAjax',
			data: {
				"page": pageNumber,
				"languageId": culture,
				"keyword": keyword

			},
			success: function (result) {
				// alert(pageNumber);
				$("#body").empty();

				$('#body').append(result);
			}

		});
	});


})