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
            $('#catalogue').html('<option>Loading Catalogue Criteria...</option>');
            $('#catalogue').prop('disabled', 'disabled');

            $('#physicalLocation').html('<option>Loading Physical Location...</option>');
            $('#physicalLocation').prop('disabled', 'disabled');

            //Get Catalogues
            $.ajax({
                url: settingsManager.websiteURL + 'api/CatalogueCriteriaAPI/RetrieveCriterias',
                type: 'GET',
                async: true,
                cache: false,
                success: function (response) {
                    $('#catalogue').html('');
                    $('#catalogue').prop('disabled', false);
                    $('#catalogue').append('<option value="">Select Catalogue Criteria</option>');
                    var catalogue = response.data;
                    var html = '';
                    $.each(catalogue, function (key, value) {
                        $('#catalogue').append('<option value="' + value.ID + '">' + value.Name + '</option>');
                    });
                },
                error: function (xhr) {
                    displayMessage("error", 'Error experienced: ' + xhr.responseText);
                }
            });

            //Get locations
            $.ajax({
                url: settingsManager.websiteURL + 'api/PhysicalLocationAPI/RetrieveLocations',
                type: 'GET',
                async: true,
                cache: false,
                success: function (response) {
                    $('#physicalLocation').html('');
                    $('#physicalLocation').prop('disabled', false);
                    $('#physicalLocation').append('<option value="">Select Physical Location</option>');
                    var physicalLocation = response.data;
                    var html = '';
                    $.each(physicalLocation, function (key, value) {
                        $('#physicalLocation').append('<option value="' + value.ID + '">' + value.Name + '</option>');
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

function uploadDocument() {
    try {

        var formValid = $('#demo-form').parsley().validate();

        if (formValid) {

            var filesSelected = document.getElementById("docs").files;

            if (filesSelected.length > 0) {

                var fileToLoad = filesSelected[0];
                var fileReader = new FileReader();

                fileReader.onload = function (fileLoadedEvent) {

                    var doc = fileLoadedEvent.target.result;
                    doc = doc.split(',')[1];

                    $('#addBtn').html('Upload...');
                    $("#addBtn").attr("disabled", "disabled");

                    var docName = fileToLoad.name.toUpperCase();
                    var catalogue = $('#catalogue').val();
                    var physicalLocation = $('#physicalLocation').val();
                    
                    var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
                    var uploader = user.ID;
                    var currentUser = user.ID;

                    var data = { Catalogue: catalogue, Location: physicalLocation, Uploader: uploader, CurrentUser: currentUser, Name: docName, RawDocument: doc};
                    $.ajax({
                        url: settingsManager.websiteURL + 'api/DocumentAPI/SaveDocument',
                        type: 'POST',
                        data: data,
                        processData: true,
                        async: true,
                        cache: false,
                        success: function (response) {
                            displayMessage("success", response);
                            $('#catalogue').val('');
                            $('#physicalLocation').val('');
                            
                            $("#addBtn").removeAttr("disabled");
                            $('#addBtn').html('Upload');
                        },
                        error: function (xhr) {
                            displayMessage("error", 'Error experienced: ' + xhr.responseText);
                            $("#addBtn").removeAttr("disabled");
                            $('#addBtn').html('Upload');
                        }
                    });
                };
                fileReader.readAsDataURL(fileToLoad);
            } else {
                displayMessage("warning", "No document to upload");
            }
            
        } else {
            displayMessage("warning", "Fill the required values");
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
        $("#addBtn").removeAttr("disabled");
        $('#addBtn').html('Upload');
    }
}