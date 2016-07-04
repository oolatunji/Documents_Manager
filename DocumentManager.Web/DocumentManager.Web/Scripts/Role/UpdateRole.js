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
        if (window.sessionStorage.getItem('role') === null) {
            window.location.href = '../Role/ViewRole';
        } else {
            var data = JSON.parse(window.sessionStorage.getItem('role'));
            $('#roleName').val(data.Name);
            $('#dynamicfunctions').html('<p style="font-family:Calibri;color:green;"><i class="fa fa-cog fa-spin"></i> Loading functions...</p>');
            $.ajax({
                url: settingsManager.websiteURL + 'api/FunctionAPI/RetrieveFunctions',
                type: 'GET',
                async: true,
                cache: false,
                success: function (response) {
                    var functions = response.data;
                    var html = '';
                    $.each(functions, function (key, value) {
                        var checked = false;
                        $.each(data.Functions, function (key1, value1) {
                            if (value.ID == value1) {
                                checked = true;
                            }
                        });
                        if (checked)
                            html += '<input type="checkbox" name="functions" checked="checked" style="font-size:12px" value="' + value.ID + '" />' + value.Name + '<br/>';
                        else
                            html += '<input type="checkbox" name="functions" style="font-size:12px" value="' + value.ID + '"/>' + value.Name + '<br/>';
                    });
                    $('#dynamicfunctions').html('');
                    $('#dynamicfunctions').append(html);
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

function updateRole() {
    try {
        var formValid = $('#demo-form').parsley().validate();
        if (formValid) {
            $('#updateBtn').html('Update...');
            $("#updateBtn").attr("disabled", "disabled");

            var name = $('#roleName').val();
            var roleFunctions = [];
            $("input:checkbox[name=functions]:checked").each(function () {
                var roleFunction = {};
                var _function = $(this).val();
                roleFunction = { FunctionID: _function };
                roleFunctions.push(roleFunction);
            });

            var role = JSON.parse(window.sessionStorage.getItem('role'));
            var id = role.ID;

            var data = { Name: name, RoleFunctions: roleFunctions, ID: id };
            $.ajax({
                url: settingsManager.websiteURL + 'api/RoleAPI/UpdateRole',
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
