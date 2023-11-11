function updateUserModal(id, userName, email, role) {
    document.getElementById('updateUserIdInput').value = id;
    document.getElementById('newUserName').value = userName;
    document.getElementById('emailRead').value = email;

    var roleDropdown = document.getElementById('role');
    for (var i = 0; i < roleDropdown.options.length; i++) {
        if (roleDropdown.options[i].value === role) {
            roleDropdown.selectedIndex = i;
            break;
        }
    }
}

$(document).on("submit", "#updateUserModal form", function (event) {
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

$(document).on("submit", "#deleteUserModal form", function (event) {
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