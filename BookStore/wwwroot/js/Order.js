var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/admin/order/getall', },
        "columns": [
            { data: 'ID', "width": "5%" },
            { data: 'Name', "width": "15%" },
            { data: 'PhoneNumber', "width": "20%" },
            { data: 'ApplicationUser.Email', "width": "15%" },
            { data: 'OrderStatus', "width": "10%" },
            { data: 'OrderTotal', "width": "10%" },
            {
                data: 'ID',
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                     <a href="/Admin/Order/Detail?OrderID=${data}" class="btn btn-primary mx-2"> <i class="bi bi-pencil-square"></i> Edit</a>               
                    </div>`
                },
                "width": "25%"
            }
        ]
    });
}


