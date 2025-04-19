// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {
    if ($.fn.DataTable.isDataTable('#DT_load')) {
        dataTable.destroy();
        $('#DT_load').empty(); // empty in case the columns change
    }

    dataTable = $('#DT_load').DataTable({
        "ajax": {
            "url": "/api/site",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { data: "name" },
            { data: "maxLength", "render": $.fn.dataTable.render.number(',', '.', 2, '', ' ft'), width: "15%" },
            { data: "siteType.name" },
            { data: "description" },

            {
                data: "id", width: "30%",
                "render": function (data, type, row) {
                    return `<div class="text-center">
                            <a href="/Admin/Sites/Upsert?id=${data}"
                            class ="btn btn-success text-white style="cursor:pointer; width=100px;"> <i class="far fa-edit"></i>Edit</a>
                            <a href="javascript:void(0);" onClick="lockUnlockSite(${data})"
                            class="btn btn-danger text-white" style="cursor:pointer; width:100px;"> 
                            <i class="fa fa-lock"></i> ${row.isLocked ? "Unlock" : "Lock"}</a>
                    </div>`;
                }
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%"
    });
}

function Delete(url) {
    swal(
        {
            title: "Are you sure you want to delete?",
            text: "You will not be able to restore this data.",
            icon: "warning",
            buttons: true,
            dangerMode: true
        }).then((willDelete) => {
            if (willDelete) {
                $.ajax({
                    type: 'DELETE',
                    url: url,
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message);
                            dataTable.ajax.reload();
                        }
                        else
                            toastr.error(data.message);

                    }
                })
            }
        })
}


function lockUnlockSite(siteId) {
    $.ajax({
        url: `/api/site/lockunlock/${siteId}`, // API endpoint for lock/unlock
        type: "POST", // Use POST for modifying data
        success: function (response) {
            if (response.success) {
                toastr.success(response.message); // Show success notification
                dataTable.ajax.reload(); // Reload the DataTable to reflect changes
            } else {
                toastr.error(response.message); // Show error notification
            }
        },
        error: function (xhr) {
            console.error("Error locking/unlocking site:", xhr.responseText);
            toastr.error("An error occurred while processing the request.");
        }
    });
}
