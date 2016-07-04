$(document).ready(function () {

    if (window.sessionStorage.getItem("loggedInUser") === null) {
        window.location = '../';
        alert("Your session has expired. Kindly login again.");
    } else {

        var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
        $('#user').html(user.Username);
        $('#userrole').html(user.Role);
    }
});

function unlockscreen() {
    try {
        var formValid = $('#demo-form').parsley().validate();
        if (formValid) {

            var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));

            var username = user.Username;
            var password = $('#password').val();

            $("#addBtn").attr("disabled", "disabled");

            var data = { Username: username, Password: password };
            $.ajax({
                url: settingsManager.websiteURL + 'api/UserAPI/AuthenticateUser',
                type: 'POST',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (data) {
                    //Remove local storages if they exist before adding new ones
                    if (window.sessionStorage.getItem("loggedInUser") != null)
                        window.sessionStorage.removeItem("loggedInUser");

                    //Add new local storages
                    var user = JSON.stringify(data);

                    window.sessionStorage.setItem("loggedInUser", user);

                    window.location = ("../Home/Dashboard");

                },
                error: function (xhr) {
                    if (xhr.status == 404)
                        displayMessage("error", 'Error experienced: Incorrect System Application Url.', "User Login");
                    else
                        displayMessage("error", 'Error experienced: ' + xhr.responseText);
                    console.log(xhr);
                    $("#addBtn").removeAttr("disabled");
                }
            });
        } else {
            displayMessage("warning", "Fill the required values");
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
        console.log(err);
        $("#addBtn").removeAttr("disabled");
    }
}