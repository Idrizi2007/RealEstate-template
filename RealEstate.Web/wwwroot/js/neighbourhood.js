var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: { url: '/Admin/Neighbourhood/GetAll' },
        columns: [
            { data: 'name', width: '30%', render: $.fn.dataTable.render.text() },
            {
                data: 'city.name',
                width: '25%',
                defaultContent: '',
                render: $.fn.dataTable.render.text()
            },
            {
                data: 'isActive',
                width: '15%',
                render: function (data) {
                    return data
                        ? '<span class="badge bg-success">Po</span>'
                        : '<span class="badge bg-secondary">Jo</span>';
                }
            },
            {
                data: 'id',
                width: '30%',
                orderable: false,
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/Admin/Neighbourhood/Upsert?id=${data}" class="btn btn-primary mx-2">Ndrysho</a>
                        <a onClick="confirmDelete('/Admin/Neighbourhood/Delete/${data}', () => dataTable.ajax.reload())" class="btn btn-danger mx-2">Fshi</a>
                    </div>`;
                }
            }
        ]
    });
}
