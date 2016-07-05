﻿$(document).ready(function () {

    //var currentUrl = window.location.href;
    //var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
    //var userFunctions = user.Function;

    //var authorized = false;
    //$.each(userFunctions, function (key, userfunction) {
    //    var link = settingsManager.websiteURL.trimRight('/') + userfunction.PageLink;
    //    if (currentUrl == link) {
    //        authorized = true;
    //    }
    //});

    //if (!authorized)
    //    window.location.href = '../System/UnAuthorized';
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function addCriteria() {
    try {

        var formValid = $('#demo-form').parsley().validate();
        if (formValid) {
            $('#addBtn').html('Add...');
            $("#addBtn").attr("disabled", "disabled");

            var criteriaName = $('#criteriaName').val().toUpperCase();
            var criteriaDecription = $('#criteriaDecription').val();

            var data = { Name: criteriaName, Description: criteriaDecription };
            $.ajax({
                url: settingsManager.websiteURL + 'api/CatalogueCriteriaAPI/SaveCriteria',
                type: 'POST',
                data: data,
                processData: true,
                async: true,
                cache: false,
                success: function (response) {
                    displayMessage("success", response);
                    $('#criteriaName').val('');
                    $('#criteriaDecription').val('');
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