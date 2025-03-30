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
            "url": "/api/fee",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { data: "name" },
            { data: "feeAmount" },
            {
                data: "id", width: "30%",
                "render": function (data) {
                    return `<div class="text-center">
                            <a href="/Admin/Fees/Upsert?id=${data}"
                            class ="btn btn-success text-white style="cursor:pointer; width=100px;"> <i class="far fa-edit"></i>Edit</a>
                            <a onClick="Delete('/api/fee/'+${data})"
                            class="btn btn-danger text-white" style="cursor:pointer; width:100px;"> 
                            <i class="far fa-trash-alt"></i> Delete</a>
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