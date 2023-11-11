function displayNotification(message, type) {
    localStorage.setItem('notificationMessage', message);
    localStorage.setItem('notificationType', type);
}

window.onload = function () {
    var storedMessage = localStorage.getItem('notificationMessage');
    var storedType = localStorage.getItem('notificationType');

    if (storedMessage && storedType) {
        var notification = document.getElementById('notification');
        var alertBox = notification.querySelector('.alert');

        alertBox.textContent = storedMessage;
        alertBox.className = 'alert alert-' + storedType;

        notification.style.display = 'block';

        setTimeout(function () {
            notification.style.display = 'none';
        }, 3000);

        localStorage.removeItem('notificationMessage');
        localStorage.removeItem('notificationType');
    }
}