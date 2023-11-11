$(document).on("submit", "#deleteOrderDetailModal form", function (event) {
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