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
            "url": "/api/role",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            { data: "name" },
            { data: "description" },
            {
                data: "id", width: "30%",
                "render": function (data, type, row) {
                    let lockBtnClass = row.isLocked ? "btn-secondary" : "btn-warning";
                    let lockIcon = row.isLocked ? "fa-lock" : "fa-unlock";
                    let lockText = row.isLocked ? "Unlock" : "Lock";

                    return `<div class="text-center">
                <a href="/Admin/Roles/Upsert?id=${data}"
                    class ="btn btn-success text-white" style="cursor:pointer; width=100px;">
                    <i class="far fa-edit"></i> Edit</a>
               
                <button onClick="LockRole(${data})"
                    class="btn ${lockBtnClass} text-white" style="cursor:pointer; width:100px;">
                    <i class="fas ${lockIcon}"></i> ${lockText}</button>
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
function LockRole(id) {
    $.post("/Role/Lock", { id: id }, function (data) {
        if (data.success) {
            toastr.success(data.message);
            dataTable.ajax.reload(); // Refresh the table
        } else {
            toastr.error(data.message);
        }
    });
}
