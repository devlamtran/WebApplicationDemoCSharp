$(document).ready(function (e) {
	$('.buttonPrice').change(function (e) {
		e.preventDefault();
		$("input:radio", '#myFormPrice').attr("checked", false);
		$(this).prop('checked', true);
		const culture = $('#hidCulture').val();
		var priceRange = $(this).val();
		var brandRange = $('input[name=brandRange]:checked', '#myFormBrand').val();
		var categoryId = $('#categoryId').val();
		

		$.ajax({
			url: "/" + culture + '/Product/FilterPriceBrandProductAjax',
			type: "GET",
			data: {
				"categoryId": categoryId,
				"priceRange": priceRange,
				"brandRange": brandRange,
				"languageId": culture

			},
			success: function (result) {
				$("#body").empty();
				$("#body").append("<h2 class='title text-center'>Danh sách sản phẩm</h2>");
				$('#body').append(result);
				$.ajax({
					url: "/" + culture + '/Product/PagingPriceBrandProductAjax',
					type: "GET",
					data: {
						"categoryId": categoryId,
						"priceRange": priceRange,
						"brandRange": brandRange,
						"languageId": culture

					},
					success: function (dataPaging) {
						$(".pagingProduct").empty();
					
						$('.pagingProduct').append(dataPaging);

					}
				});
				

			}
		});


	});

	$('.buttonBrand').change(function (e) {
		e.preventDefault();
		$("input:radio", '#myFormBrand').attr("checked", false);
		$(this).prop('checked', true);
		const culture = $('#hidCulture').val();
		var brandRange = $(this).val();
		var priceRange = $('input[name=priceRange]:checked', '#myFormPrice').val();
		var categoryId = $('#categoryId').val();

		$.ajax({
			url: "/" + culture + '/Product/FilterPriceBrandProductAjax',
			type: "GET",
			data: {
				"categoryId": categoryId,
				"priceRange": priceRange,
				"brandRange": brandRange,
				"languageId": culture

			},
			success: function (result) {
				$("#body").empty();
				$("#body").append("<h2 class='title text-center'>Danh sách sản phẩm</h2>");
				$('#body').append(result);
				$.ajax({
					url: "/" + culture + '/Product/PagingPriceBrandProductAjax',
					type: "GET",
					data: {
						"categoryId": categoryId,
						"priceRange": priceRange,
						"brandRange": brandRange,
						"languageId": culture

					},
					success: function (dataPaging) {
						$(".pagingProduct").empty();

						$('.pagingProduct').append(dataPaging);

					}
				});
				
			}
		});


	});
	$('.paging-item').click(function (e) {
				e.preventDefault();
		        const culture = $('#hidCulture').val();
		        var priceRange = $('input[name=priceRange]:checked', '#myFormPrice').val();
		        var brandRange = $('input[name=brandRange]:checked', '#myFormBrand').val();

		        var keyword = $(this).attr("data-key");
		        var categoryId = $(this).attr("data-categoryId");
		        $(".pagingProduct").find(".paging-item.active").removeClass("active");
				$(this).addClass("active");
				
				var pageNumber = $(this).attr("data-page");
				
				$.ajax({
						type: 'GET',
					    url: "/" + culture + '/Product/PagingProductAjax',
					    data: {
						    "categoryId": categoryId,
							"page": pageNumber,
							"languageId": culture,
						    "keyword": keyword,
						    "priceRange": priceRange,
						    "brandRange": brandRange
					    },
						success: function (result) {
							// alert(pageNumber);
							$("#body").empty();
							$("#body").append("<h2 class='title text-center'>Danh sách sản phẩm</h2>");
							$('#body').append(result);
						}

			    });
	});

	$('.paging-itemBill').click(function (e) {
		e.preventDefault();
		const culture = $('#hidCulture').val();
	
		$(".pagingBill").find(".paging-itemBill.active").removeClass("active");
		$(this).addClass("active");

		var pageNumber = $(this).attr("data-page");
		var userName = $(this).attr("data-user");
		var keyword = $(this).attr("data-key");
		$.ajax({
			type: 'GET',
			url: "/" + culture + '/Checkout/PagingBillAjax',
			data: {
				
				"page": pageNumber,
				"languageId": culture,
				"userName": userName,
				"keyword": keyword
			},
			success: function (result) {
				// alert(pageNumber);
				$("#body").empty();
			
				$('#body').append(result);
			}

		});
	});

	$('.paging-itemProductAdmin').click(function (e) {
		e.preventDefault();
		const culture = $('#hidCulture').val();

		$(".pagingProductAdmin").find(".paging-itemProductAdmin.active").removeClass("active");
		$(this).addClass("active");

		var pageNumber = $(this).attr("data-page");
		
		var keyword = $(this).attr("data-key");
		var categoryId = $(this).attr("data-categoryId");
		$.ajax({
			type: 'GET',
			url: "/" + culture + '/ProductAdmin/PagingProductAdminAjax',
			data: {
				"page": pageNumber,
				"languageId": culture,
				"keyword": keyword,
				"categoryId": categoryId,
				
			},
			success: function (result) {
				// alert(pageNumber);
				$("#body").empty();

				$('#body').append(result);
			}

		});
	});

	$('.paging-itemUserAdmin').click(function (e) {
		e.preventDefault();
		const culture = $('#hidCulture').val();

		$(".pagingUserAdmin").find(".paging-itemUserAdmin.active").removeClass("active");
		$(this).addClass("active");

		var pageNumber = $(this).attr("data-page");

		var keyword = $(this).attr("data-key");
	
		$.ajax({
			type: 'GET',
			url: "/" + culture + '/UserAdmin/PagingUserAdminAjax',
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
	$('.paging-itemContactAdmin').click(function (e) {
		e.preventDefault();
		const culture = $('#hidCulture').val();

		$(".pagingContactAdmin").find(".paging-itemContactAdmin.active").removeClass("active");
		$(this).addClass("active");

		var pageNumber = $(this).attr("data-page");

		var keyword = $(this).attr("data-key");

		$.ajax({
			type: 'GET',
			url: "/" + culture + '/ContactAdmin/PagingContactAdminAjax',
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