function updateProductModal(productId, productName, productColor, productDescription, productUrl, productPrice, categoryId) {
    document.getElementById('productIdInput').value = productId;
    document.getElementById('newProductName').value = productName;
    document.getElementById('newProductColor').value = productColor;
    document.getElementById('newProductDescription').value = productDescription;
    document.getElementById('newProductUrl').value = productUrl;
    document.getElementById('newProductPrice').value = productPrice;
    document.getElementById('newProductCategoryId').value = categoryId;
}

$(document).on("submit", "#addProductModal form", function (event) {
    event.preventDefault();

    $.ajax({
        url: $(this).attr("action"),
        type: $(this).attr("method"),
        data: $(this).serialize(),
        success: function (response) {
            displayNotification(response, 'success');
            location.reload();
        },
        error: function (error) {
            displayNotification(error.responseText, 'danger');
            location.reload();
        }
    });
});

$(document).on("submit", "#updateProductModal form", function (event) {
    event.preventDefault();

    $.ajax({
        url: $(this).attr("action"),
        type: $(this).attr("method"),
        data: $(this).serialize(),
        success: function (response) {
            displayNotification(response, 'success');
            location.reload();
        },
        error: function (error) {
            displayNotification(error.responseText, 'danger');
            location.reload();
        }
    });
});

$(document).on("submit", "#deleteProductModal form", function (event) {
    event.preventDefault();

    $.ajax({
        url: $(this).attr("action"),
        type: $(this).attr("method"),
        data: $(this).serialize(),
        success: function (response) {
            displayNotification(response, 'success');
            location.reload();
        },
        error: function (error) {
            displayNotification(error.responseText, 'danger');
            location.reload();
        }
    });
});