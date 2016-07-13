$(document).ready(function () {
    try {
        getCriteria();
        setTimeout(autoSuggest, 3000);
    } catch (err) {
        displayMessage("error", "Error encountered: " + err);
    }
});

String.prototype.trimRight = function (charlist) {
    if (charlist === undefined)
        charlist = "\s";

    return this.replace(new RegExp("[" + charlist + "]+$"), "");
};

function autoSuggest() {
    $.ajax({
        url: settingsManager.websiteURL + 'api/DocumentAPI/RetrieveSearchLists',
        type: 'GET',
        async: true,
        cache: false,
        success: function (response) {
            var options = {
                data: response,
                list: {
                    maxNumberOfElements: 15,
                    match: {
                        enabled: true
                    }
                }
            };
            $("#searchValue").easyAutocomplete(options);
        },
        error: function (xhr) {
            displayMessage("error", 'Error experienced: ' + xhr.responseText);
        }
    });
}

function search() {

    var searchValue = $('#searchValue').val();

    if (searchValue.length != "") {
        if (window.sessionStorage.getItem("searchValue") != null)
            window.sessionStorage.removeItem("searchValue");

        window.sessionStorage.setItem("searchValue", searchValue);
        refreshResult();
        setTimeout(autoSuggest, 3000);
    } else {
        displayMessage("error", "Error encountered: Search criteria is required.");
    }

   
}

function getCriteria() {

    var searchValue = window.sessionStorage.getItem("searchValue");
    $('#searchValue').val(searchValue);

    $('#example tfoot th').each(function () {
        var title = $('#example tfoot th').eq($(this).index()).text();
        if (title != "")
            $(this).html('<input type="text" placeholder="Search ' + title + '" />');
    });

    var table = $('#example').DataTable({

        "processing": true,

        "ajax": {
            "url": settingsManager.websiteURL + 'api/DocumentAPI/SearchDocument',
            "type": "POST",
            "data": function (d) {
                d.SearchValue = window.sessionStorage.getItem("searchValue");
            },
        },

        "columns": [
            { "data": "Name" },
            { "data": "CatalogueCriteria" },
            { "data": "PhysicalLocation" },
            { "data": "Uploader" },
            { "data": "CurrentUser" },
            { "data": "Date" },
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

