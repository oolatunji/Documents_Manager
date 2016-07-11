function displayMessage(type, message) {
    noty({
        text: message,
        type: type,
        dismissQueue: true,
        theme: 'agileUI',
        layout: 'center'
    });
}