﻿$(document).ready(function () {
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
        else
            getCriteria();

    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};


function getCriteria() {

    var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
    var userID = user.ID;

    $('#example tfoot th').each(function () {
        var title = $('#example tfoot th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": settingsManager.websiteURL + 'api/DocumentAPI/RetrieveDocumentTransaction',

        "columns": [
            { "data": "Name" },
            { "data": "FromUser" },
            { "data": "ToUser" },
            { "data": "Date" },
            { "data": "Status" },
            {
                "className": 'document-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            {
                "data": "DocumentID",
                "visible": false
            }
        ],

        "order": [[0, "asc"]],

        dom: 'Brtip',

        buttons: [
            {
                extend: 'copyHtml5',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'csvHtml5',
                exportOptions: {
                    columns: ':visible'
                }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: {
                    columns: ':visible'
                }
            }
        ]
    });

    $('#example tbody').on('click', 'td.document-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        var data = row.data();
        var viewdocument = confirm("Are you sure you want to view document: " + data.Name + "?");
        if (viewdocument == true) {
            try {
                $('#loadicon').removeClass("hide");

                var name = data.Name;
                var documentID = data.DocumentID;

                var data = { Name: name, DocumentID: documentID };
                $.ajax({
                    url: settingsManager.websiteURL + 'api/DocumentAPI/ViewDocument',
                    type: 'POST',
                    data: data,
                    processData: true,
                    async: true,
                    cache: false,
                    success: function (response) {
                        $('#loadicon').addClass("hide");
                        window.open("data:application/pdf;base64, " + response);
                    },
                    error: function (xhr) {
                        displayMessage("error", 'Error experienced: ' + xhr.responseText);
                    }
                });

            } catch (err) {
                displayMessage("error", "Error encountered: " + err);
            }
        } else {
            return;
        }
    });

    $("#example tfoot input").on('keyup change', function () {
        table
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
}

function refreshResult() {
    try {
        var table = $('#example').DataTable();
        table.ajax.reload();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
    }
}

$(document).ready(function () {
    $('#dataTables-example').DataTable({
        responsive: true
    });
});

