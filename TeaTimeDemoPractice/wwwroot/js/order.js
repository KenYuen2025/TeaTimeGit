﻿var dataTable;
$(document).ready(function () {
    var url = window.location.search;
    console.log(url);
    if (url.includes("Processing")) {
        loadDataTable("Processing");
    } else {
        if (url.includes("Pending")) {
            loadDataTable("Pending");
        } else {
            if (url.includes("Ready")) {
                loadDataTable("Ready");
            } else {
                if (url.includes("Completed")) {
                    loadDataTable("Completed");
                } else {
                    loadDataTable("all");
                }
            }
        }
    }
    //loadDataTable();
});

function loadDataTable(status) {
    dataTable = $('#tblData').dataTable({
        "ajax": {
            url: '/admin/order/getall?status=' + status
        },
        "columns": [
            { data: 'id', "width": "10%" },
            { data: 'name', "width": "15%" },
            { data: 'phoneNumber', "width": "20%" },
            { data: 'applicationUser.email', "width": "20%" },
            { data: 'orderStatus', "width": "10%" },
            { data: 'orderTotal', "width": "10%" },
            {
                data: 'id',
                "render": function (data) {
                return `<div class="w-75 btn-group" role="group">
                    <a href="/admin/order/details?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil-square"></i>Edit</a>
                </div>`
                },
                "width": "15%"
            }
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    $('#tblData').DataTable().ajax.reload();
                    toastr.success(data.message);
                    //alert(data.message);
                }
            })
        }
    });
}
