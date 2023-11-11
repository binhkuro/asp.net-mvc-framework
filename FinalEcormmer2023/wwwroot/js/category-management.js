function updateCategoryModal(categoryId, categoryName) {
    document.getElementById('categoryIdInput').value = categoryId;
    document.getElementById('updatedCategoryName').value = categoryName;
}

$(document).on("submit", "#addCategoryModal form", function (event) {
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

$(document).on("submit", "#updateCategoryModal form", function (event) {
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

$(document).on("submit", "#deleteCategoryModal form", function (event) {
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