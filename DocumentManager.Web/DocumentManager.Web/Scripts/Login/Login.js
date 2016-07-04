function LoginIn() {

    try {
        var formValid = $('#demo-form').parsley().validate();
        if (formValid) {
            var username = $('#username').val();
            var password = $('#password').val();

            $('#addBtn').html('Login...');
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

                    window.location = ("Home/Dashboard");

                },
                error: function (xhr) {
                    if (xhr.status == 404)
                        displayMessage("error", 'Error experienced: Incorrect System Application Url.', "User Login");
                    else
                        displayMessage("error", 'Error experienced: ' + xhr.responseText);
                    console.log(xhr);
                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('Login');
                }
            });
        } else {
            displayMessage("warning", "Fill the required values");
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
        console.log(err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('Login');
    }
}

function retrievePassword() {

    try {
        var formValid = $('#demo-form1').parsley().validate();
        if (formValid) {
            var username = $('#theusername').val();

            $('#retrieveBtn').html('Retrieve...');
            $("#retrieveBtn").attr("disabled", "disabled");

            var data = { Username: username };
            $.ajax({
                url: settingsManager.websiteURL + 'api/UserAPI/ForgotPassword',
                type: 'PUT',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (data) {
                    displayMessage("success", "An email that contains a link to continue with your password reset has been sent to your email: " + data + ". If this email address is not correct, kindly contact your administrator to modify accordingly", "Password Management");
                    $('#theusername').val("");
                    $("#retrieveBtn").removeAttr("disabled");
                    $('#retrieveBtn').html('Retrieve');
                },
                error: function (xhr) {
                    displayMessage("error", "Error encountered: " + xhr.responseText);
                    $("#retrieveBtn").removeAttr("disabled");
                    $('#retrieveBtn').html('Retrieve');
                }
            });
        } else {
            displayMessage("warning", "Fill the required values");
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
        console.log(err);
        $("#retrieveBtn").removeAttr("disabled");
        $('#retrieveBtn').html('Retrieve');
    }
}
