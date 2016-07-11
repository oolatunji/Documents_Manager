var username = "";
$(document).ready(function () {
    try{
        var encncryptedUsername = getQueryStringValue("rq");
        if (encncryptedUsername != "") {
            var data = { Username: encncryptedUsername };
            $.ajax({
                url: settingsManager.websiteURL + 'api/UserAPI/ConfirmUsername',
                type: 'POST',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (data) {
                    username = data.Username;
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
        console.log(err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('Reset');
    }
});

function resetPassword() {
    try {

        var formValid = $('#demo-form').parsley().validate();
        if (formValid) {
            var new_password = $('#newPassword').val();
            var confirm_password = $('#confirmPassword').val();

            $('#addBtn').html('Reset...');
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
                    $('#addBtn').html('Reset');
                    $('#newPassword').val("");
                    $('#confirmPassword').val("");

                    var redirectToLogin = confirm("Password was changed successfully. Click OK to login with your new password.");
                    if (redirectToLogin) {
                        window.location = "../";
                    }

                },
                error: function (xhr) {
                    displayMessage("error", "Error encountered: " + xhr.responseText);
                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('Reset');
                }
            });
        } else {
            displayMessage("warning", "Fill the required values");
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
        console.log(err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('Reset');
    }
}

function getQueryStringValue(key) {
    return unescape(window.location.search.replace(new RegExp("^(?:.*[&\\?]" + escape(key).replace(/[\.\+\*]/g, "\\$&") + "(?:\\=([^&]*))?)?.*$", "i"), "$1"));
}