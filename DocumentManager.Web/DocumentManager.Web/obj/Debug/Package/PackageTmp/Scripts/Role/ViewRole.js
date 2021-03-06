﻿$(document).ready(function () {
    try {
        var currentUrl = window.location.href;
        var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
        var userFunctions = user.Function;

        var exist = false;
        $.each(userFunctions, function (key, userfunction) {
            var link = settingsManager.websiteURL.trimRight('/') + userfunction.PageLink;
            if (currentUrl == link) {
                exist = true;
            }
        });
        
        if (!exist)
            window.location.href = '../System/UnAuthorized';
        else {
            if (window.sessionStorage.getItem('role') !== null) {
                window.sessionStorage.removeItem('role');
            }
            getFunctionsAndDisplayRoles();
        }
    } catch (err) {
        displayMessage("error", "Error encountered: " + err, "Roles Management");
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};


function getFunctionsAndDisplayRoles() {
    try {        
        $.ajax({
            url: settingsManager.websiteURL + 'api/FunctionAPI/RetrieveFunctions',
            type: 'GET',
            async: true,
            cache: false,
            success: function (response) {
                var functions = [];
                functions = response.data;
                getRoles(functions);
            },
            error: function (xhr) {
                displayMessage("error", 'Error experienced: ' + xhr.responseText);
            }            
        });
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);       
    }
}

function getRoles(functions) {
    $('#example tfoot th').each(function () {
        var title = $('#example thead th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": settingsManager.websiteURL + 'api/RoleAPI/RetrieveRoles',

        "columns": [
            { "data": "Name" },
            {
                "className": 'details-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            {
                "className": 'edit-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            {
                "data": "Functions",
                "visible": false
            },
            {
                "data": "ID",
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

    $('#example tbody').on('click', 'td.details-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);

        function closeAll() {
            var e = $('#example tbody tr.shown');
            var rows = table.row(e);
            if (tr != e) {
                e.removeClass('shown');
                rows.child.hide();
            }
        }

        if (row.child.isShown()) {
            closeAll();
        }
        else {
            closeAll();

            row.child(formatDetails(row.data(), functions)).show();
            tr.addClass('shown');
        }
    });

    $('#example tbody').on('click', 'td.edit-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        var data = row.data();
        var update = confirm("Are you sure you want to update role: " + data.Name + "?");
        if (update == true) {
            window.sessionStorage.setItem('role', JSON.stringify(data));
            window.location.href = '../Role/UpdateRole';
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
};

function refreshResult() {
    try {
        var table = $('#example').DataTable();
        table.ajax.reload();
    } catch (err) {
        displayMessage("error", "Error encountered: " + err,);
    }
}

$(document).ready(function () {
    $('#dataTables-example').DataTable({
        responsive: true
    });
});

function formatDetails(d, allfunctions) {
    var table = '<table width="100%" class="cell-border" cellpadding="5" cellspacing="0" border="2" style="padding-left:50px;">';
    table += '<tr>';
    table += '<th style="color:navy;width:20%;font-family:Arial;">Functions</th>';
    table += '</tr>';
    table += '<tr>';
    table += '<td style="font-family:Arial;">';   
    $.each(allfunctions, function (key, value) {
        var checked = false;
        $.each(d.Functions, function (key1, value1) {
            if (value.ID == value1) {
                checked = true;
            }
        });
        if (checked)
            table += value.Name + '<br/>';
    });
    table += '</td>';
    table += '</tr>';
    table += '</table>';
    return table;
}