function updateOrderModal(orderId, orderShipName, orderShipAddress, orderShipEmail, orderShipPhoneNumber, orderTotal) {
    document.getElementById('orderIdInput').value = orderId;
    document.getElementById('updatedOrderShipName').value = orderShipName;
    document.getElementById('updatedOrderShipAddress').value = orderShipAddress;
    document.getElementById('updatedOrderShipEmail').value = orderShipEmail;
    document.getElementById('updatedOrderShipPhoneNumber').value = orderShipPhoneNumber;
    document.getElementById('updatedOrderTotal').value = orderTotal;
}

$(document).on("submit", "#updateOrderModal form", function (event) {
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

$(document).on("submit", "#deleteOrderModal form", function (event) {
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