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
            "url": "/api/sitetype",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { data: "name" },
            { data: "price", "render": $.fn.dataTable.render.number(',', '.', 2, "$"), width: "15%" },

            {
                data: "id", width: "25%",
                "render": function (data, type, row) {
                    var lockBtnClass = row.isLocked ? "btn-secondary" : "btn-warning";
                    var lockIcon = row.isLocked ? "fa-lock" : "fa-unlock";
                    var lockText = row.isLocked ? "Unlock" : "Lock";

                    if (lockText === "Lock") {
                        return `<div class="text-center">
                            <a href="/Admin/SiteTypes/Upsert?id=${data}" class ="btn btn-success text-white style="cursor:pointer; width=100px;"> 
                                <i class="far fa-edit"></i>
                                Edit
                            </a>
                            <button onClick="LockSiteType(${data}, '${lockText}')" class="btn ${lockBtnClass} text-white" style="cursor:pointer; width:100px;">
                                <i class="fas ${lockIcon}"></i> 
                                ${lockText}
                            </button>
                            </div>`;
                    } else {
                        return `<div class="text-center">
                            <button href="/Admin/SiteTypes/Upsert?id=${data}" class ="btn btn-secondary text-white style="cursor:pointer; width=100px; disabled"> 
                                <i class="far fa-edit"></i>
                                Edit
                            </button>
                            <button onClick="LockSiteType(${data}, '${lockText}')" class="btn ${lockBtnClass} text-white" style="cursor:pointer; width:100px;">
                                <i class="fas ${lockIcon}"></i> 
                                ${lockText}
                            </button>
                            </div>`;
                    }
                }
            }
        ],
        "language": {
            "emptyTable": "no data found."
        },
        "width": "100%"
    });
}

function LockSiteType(id, lockState) {
    if (lockState === "Lock") {
        swal({
            title: "Are you sure?",
            text: "You will not be able to edit this data while it is locked.",
            icon: "warning",
            buttons: true,
            dangerMode: true

        }).then((willLock) => {
            if (willLock) {
                $.ajax({
                    type: "POST",
                    url: "/api/siteType/lock/" + id,  // Match the API pattern
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message);
                            dataTable.ajax.reload(); // Refresh the table
                        } else {
                            toastr.error(data.message);
                        }
                    },
                    error: function (xhr) {
                        console.error("Error:", xhr.responseText);
                        toastr.error("An error occurred.");
                    }
                });
            }
        });
    } else {
        swal({
            title: "Are you sure?",
            text: "You will allow editing for this data.",
            icon: "warning",
            buttons: true,
            dangerMode: true

        }).then((willLock) => {
            if (willLock) {
                $.ajax({
                    type: "POST",
                    url: "/api/siteType/lock/" + id,  // Match the API pattern
                    success: function (data) {
                        if (data.success) {
                            toastr.success(data.message);
                            dataTable.ajax.reload(); // Refresh the table
                        } else {
                            toastr.error(data.message);
                        }
                    },
                    error: function (xhr) {
                        console.error("Error:", xhr.responseText);
                        toastr.error("An error occurred.");
                    }
                });
            }
        });
    }
}