function changePassword() {
    try {
        var formValid = $('#demo-form').parsley().validate();
        if (formValid) {
            var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));

            var username = user.Username;
            var new_password = $('#newPassword').val();
            var confirm_password = $('#confirmPassword').val();

            if (username === null || username == "") {
                window.location = "../";
                alert("Your session has expired. Kindly login again.")
            } else {

                $('#addBtn').html('Change...');
                $("#addBtn").attr("disabled", "disabled");

                var data = { Username: username, Password: new_password };
                $.ajax({
                    url: settingsManager.websiteURL + 'api/UserAPI/ChangePassword',
                    type: 'PUT',
                    data: data,
                    processData: true,
                    async: true,
                    cache: false,
                    success: function (data) {

                        $("#addBtn").removeAttr("disabled");
                        $('#addBtn').html('Change');

                        if (window.sessionStorage.getItem("loggedInUser") != null)
                            window.localStorage.removeItem("loggedInUser");

                        window.location.href = "../";

                        alert("Password was changed successfully. You will be redirected shorthly to login with your new password.");


                    },
                    error: function (xhr) {
                        displayMessage("error", "Error encountered: " + xhr.responseText);
                        $("#addBtn").removeAttr("disabled");
                        $('#addBtn').html('Change');
                    }
                });
            }
        } else {
            displayMessage("warning", "Fill the required values");
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
        console.log(err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('Change');
    }
}