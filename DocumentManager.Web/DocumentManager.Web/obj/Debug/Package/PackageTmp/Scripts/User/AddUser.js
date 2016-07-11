$(document).ready(function () {
    try {

        var currentUrl = window.location.href;
        var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
        var userFunctions = user.Function;

        var authorized = false;
        $.each(userFunctions, function (key, userfunction) {
            var link = settingsManager.websiteURL.trimRight('/') + userfunction.PageLink;
            if (currentUrl == link) {
                authorized = true;
            }
        });
        
        if (!authorized)
            window.location.href = '../System/UnAuthorized';
        else {
            $('#userRole').html('<option>Loading Roles...</option>');
            $('#userRole').prop('disabled', 'disabled');

            //Get Roles
            $.ajax({
                url: settingsManager.websiteURL + 'api/RoleAPI/RetrieveRoles',
                type: 'GET',
                async: true,
                cache: false,
                success: function (response) {
                    $('#userRole').html('');
                    $('#userRole').prop('disabled', false);
                    $('#userRole').append('<option value="">Select Role</option>');
                    var roles = response.data;
                    var html = '';
                    $.each(roles, function (key, value) {
                        $('#userRole').append('<option value="' + value.ID + '">' + value.Name + '</option>');
                    });
                },
                error: function (xhr) {
                    displayMessage("error", 'Error experienced: ' + xhr.responseText);
                }
            });
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function addUser() {
    try {

        var formValid = $('#demo-form').parsley().validate();
        if (formValid) {
            $('#addBtn').html('Add...');
            $("#addBtn").attr("disabled", "disabled");

            var lastname = $('#lastname').val();
            var othernames = $('#othernames').val();
            var gender = $('#gender').val();
            var phoneNumber = $('#phoneNumber').val();
            var email = $('#emailAddress').val();
            var username = $('#username').val();
            var userRole = $('#userRole').val();

            var data = { Lastname: lastname, Othernames: othernames, Gender: gender, PhoneNumber: phoneNumber, Email: email, Username: username, UserRole: userRole};
            $.ajax({
                url: settingsManager.websiteURL + 'api/UserAPI/SaveUser',
                type: 'POST',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (response) {
                    displayMessage("success", response);
                    $('#lastname').val('');
                    $('#othernames').val('');
                    $('#gender').val('');
                    $('#phoneNumber').val('');
                    $('#emailAddress').val('');
                    $('#username').val('');
                    $('#userRole').val('');

                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('Add');
                },
                error: function (xhr) {
                    displayMessage("error", 'Error experienced: ' + xhr.responseText);
                    $("#addBtn").removeAttr("disabled");
                    $('#addBtn').html('Add');
                }
            });
        } else {
            displayMessage("warning", "Fill the required values");
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('Add');
    }
}