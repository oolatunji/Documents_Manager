$(document).ready(function () {

    //var currentUrl = window.location.href;
    //var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
    //var userFunctions = user.Function;

    var authorized = false;
    //$.each(userFunctions, function (key, userfunction) {
    //    var link = settingsManager.websiteURL.trimRight('/') + userfunction.PageLink;
    //    if (currentUrl == link) {
    //        authorized = true;
    //    }
    //});
    authorized = true;
    if (!authorized) {
        window.location.href = '../System/UnAuthorized';
    } else {
        if (window.sessionStorage.getItem('location') === null) {
            window.location.href = '../PhysicalLocation/ViewLocation';
        } else {
            var data = JSON.parse(window.sessionStorage.getItem('location'));
            $('#locationName').val(data.Name);
            $('#location').val(data.Location);
            $('#locationDecription').val(data.Description);
        }
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function updateLocation() {
    try {
        var formValid = $('#demo-form').parsley().validate();
        if (formValid) {
            $('#updateBtn').html('Update...');
            $("#updateBtn").attr("disabled", "disabled");

            var locationName = $('#locationName').val().toUpperCase();
            var physicallocation = $('#location').val();
            var locationDecription = $('#locationDecription').val();

            var location = JSON.parse(window.sessionStorage.getItem('location'));
            var id = location.ID;

            var data = { Name: locationName, Description: locationDecription, Location: physicallocation, ID: id };
            $.ajax({
                url: settingsManager.websiteURL + 'api/PhysicalLocationAPI/UpdateLocation',
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
