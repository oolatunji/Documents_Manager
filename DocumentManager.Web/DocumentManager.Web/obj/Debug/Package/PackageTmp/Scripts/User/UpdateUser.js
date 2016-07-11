$(document).ready(function () {

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
    
    if (!authorized) {
        window.location.href = '../System/UnAuthorized';
    } else {
        if (window.sessionStorage.getItem('user') === null) {
            window.location.href = '../User/ViewUser';
        } else {
            var data = JSON.parse(window.sessionStorage.getItem('user'));
            $('#lastname').val(data.Lastname);
            $('#othernames').val(data.Othernames);
            $('#phoneNumber').val(data.PhoneNumber);
            $('#emailAddress').val(data.Email);
            $('#username').val(data.Username);

            var genderHtml = '';
            var gender = data.Gender;
            if (gender == "Male") {
                genderHtml += '<option selected="selected" value="Male">Male</option>';
                genderHtml += '<option value="Female">Female</option>';
            }
            else if (gender == "Female") {
                genderHtml += '<option value="Male">Male</option>';
                genderHtml += '<option selected="selected" value="Female">Female</option>';
            }
            $('#gender').append(genderHtml);

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
                    $.each(roles, function (key, value) {
                        if (data.Role.ID == value.ID)
                            $('#userRole').append('<option selected="selected" value="' + value.ID + '">' + value.Name + '</option>');
                        else
                            $('#userRole').append('<option value="' + value.ID + '">' + value.Name + '</option>');
                    });
                },
                error: function (xhr) {
                    displayMessage("error", 'Error experienced: ' + xhr.responseText);
                }
            });
        }
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function updateUser() {
    try {
        var formValid = $('#demo-form').parsley().validate();
        if (formValid) {
            $('#updateBtn').html('Update...');
            $("#updateBtn").attr("disabled", "disabled");

            var lastname = $('#lastname').val();
            var othernames = $('#othernames').val();
            var gender = $('#gender').val();
            var phoneNumber = $('#phoneNumber').val();
            var email = $('#emailAddress').val();
            var username = $('#username').val();
            var userRole = $('#userRole').val();

            var user = JSON.parse(window.sessionStorage.getItem('user'));
            var id = user.ID;

            var data = { Lastname: lastname, Othernames: othernames, Gender: gender, PhoneNumber: phoneNumber, Email: email, Username: username, UserRole: userRole, ID: id };

            $.ajax({
                url: settingsManager.websiteURL + 'api/UserAPI/UpdateUser',
                type: 'PUT',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (response) {
                    displayMessage("success", response);
                    $("#updateBtn").removeAttr("disabled");
                    $('#updateBtn').html('Update');
                },
                error: function (xhr) {
                    displayMessage("error", 'Error experienced: ' + xhr.responseText);
                    $("#updateBtn").removeAttr("disabled");
                    $('#updateBtn').html('Update');
                }
            });
        } else {
            displayMessage("warning", "Fill the required values");
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
        $("#updateBtn").removeAttr("disabled");
        $('#updateBtn').html('Update');
    }
}
