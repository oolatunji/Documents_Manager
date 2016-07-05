$(document).ready(function () {
    try {
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
        if (!authorized)
            window.location.href = '../System/UnAuthorized';
        else {
            if (window.sessionStorage.getItem('criteria') !== null) {
                window.sessionStorage.removeItem('criteria');
            }
            getCriteria();
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


function getCriteria() {

    $('#example tfoot th').each(function () {
        var title = $('#example tfoot th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": settingsManager.websiteURL + 'api/CatalogueCriteriaAPI/RetrieveCriterias',

        "columns": [
            { "data": "Name" },
            { "data": "Description" },
            { "data": "Date" },
            {
                "className": 'edit-control',
                "orderable": false,
                "data": null,
                "defaultContent": ''
            },
            {
                "data": "ID",
                "visible": false
            },
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

    $('#example tbody').on('click', 'td.edit-control', function () {
        var tr = $(this).closest('tr');
        var row = table.row(tr);
        var data = row.data();
        var update = confirm("Are you sure you want to update criteria: " + data.Name + "?");
        if (update == true) {
            window.sessionStorage.setItem('criteria', JSON.stringify(data));
            window.location.href = '../CatalogueCriteria/UpdateCriteria';
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

