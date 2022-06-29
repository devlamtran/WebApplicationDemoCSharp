$(document).ready(function (e) {
	$('.buttonPrice').change(function (e) {
		e.preventDefault();
		
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

			}
		});


	});

	$('.buttonBrand').change(function (e) {
		e.preventDefault();

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

			}
		});


	});
   
})