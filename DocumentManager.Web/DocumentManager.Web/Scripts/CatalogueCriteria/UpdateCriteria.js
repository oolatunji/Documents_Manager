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
        if (window.sessionStorage.getItem('criteria') === null) {
            window.location.href = '../CatalogueCriteria/ViewCriteria';
        } else {
            var data = JSON.parse(window.sessionStorage.getItem('criteria'));
            $('#criteriaName').val(data.Name);
            $('#criteriaDecription').val(data.Description);
        }
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function updateCriteria() {
    try {
        var formValid = $('#demo-form').parsley().validate();
        if (formValid) {
            $('#updateBtn').html('Update...');
            $("#updateBtn").attr("disabled", "disabled");

            var criteriaName = $('#criteriaName').val().toUpperCase();
            var criteriaDecription = $('#criteriaDecription').val();

            var criteria = JSON.parse(window.sessionStorage.getItem('criteria'));
            var id = criteria.ID;

            var data = { Name: criteriaName, Description: criteriaDecription, ID: id };
            $.ajax({
                url: settingsManager.websiteURL + 'api/CatalogueCriteriaAPI/UpdateCriteria',
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
