var dataTable;

$(document).ready(function () {
    loadList();
});

function loadList() {
    if ($.fn.DataTable.isDataTable('#DT_load')) {
        dataTable.destroy();
        $('#DT_load').empty(); // empty in case the columns change
    }

    var activeUser = $(".customer").attr("id");

    if (activeUser === "" || typeof activeUser === "undefined") {
        dataTable = $('#DT_load').DataTable({
            "ajax": {
                "url": "/api/reservation",
                "type": "GET",
                "datatype": "json"

            },
            "columns": [
                { data: "startDate", render: $.fn.dataTable.render.date() },
                { data: "endDate", render: $.fn.dataTable.render.date() },
                { data: "site.name" },
                { data: "user.fullName" },
                {
                    data: "id",
                    width: "30%",
                    "render": function(data) {
                        return `
                        <div class="text-center">
                            <a href="/Admin/Reservations/Upsert?id=${data
                            }" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                <i class="far fa-edit"></i> Edit
                            </a>
                            <a onClick="Delete('/api/reservation/${data
                            }')" class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                <i class="far fa-trash-alt"></i> Delete
                            </a>
                        </div>`;
                    }
                }
            ],
            "columnDefs": [
                {
                    "targets": "_all",
                    "width": "16%",
                    "orderable": false
                }
            ],
            "language": {
                "emptyTable": "No data found."
            },
            "width": "100%",
            "searching": true
        });
    } else {
        dataTable = $('#DT_load').DataTable({
            "ajax": {
                "url": "/api/reservation",
                "type": "GET",
                "datatype": "json"

            },
            "columns": [
                { data: "startDate", render: $.fn.dataTable.render.date() },
                { data: "endDate", render: $.fn.dataTable.render.date() },
                { data: "site.name" },
                { data: "user.fullName" },
                {
                    data: "id",
                    width: "30%",
                    "render": function(data) {
                        return `
                        <div class="text-center">
                            <a href="/Admin/Reservations/Upsert?id=${data
                            }" class="btn btn-success text-white" style="cursor:pointer; width:100px;">
                                <i class="far fa-edit"></i> Edit
                            </a>
                            <a onClick="Delete('/api/reservation/${data
                            }')" class="btn btn-danger text-white" style="cursor:pointer; width:100px;">
                                <i class="far fa-trash-alt"></i> Delete
                            </a>
                        </div>`;
                    }
                }
            ],
            "columnDefs": [
                {
                    "targets": "_all",
                    "width": "16%",
                    "orderable": false
                }
            ],
            "language": {
                "emptyTable": "No data found."
            },
            "width": "100%",
            "search": {
                "search": activeUser
            },
            "layout":
            {
                topStart: 'pageLength',
                topEnd: '',
                bottomStart: 'paging',
                bottomEnd: ''
            }
        });
    }
}

function Delete(url) {
    swal({
        title: "Are you sure you want to delete?",
        text: "You will not be able to restore this data!",
        icon: "warning",
        buttons: true,
        dangerMode: true
    }).then((willDelete) => {
        if (willDelete) {
            $.ajax({
                type: 'DELETE',
                url: url,
                success: function(data) {
                    if (data.success) {
                        toastr.success(data.message);
                        dataTable.ajax.reload();
                    } else {
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}