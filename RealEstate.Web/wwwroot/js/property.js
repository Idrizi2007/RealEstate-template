var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function formatPrice(value) {
    return Number(value).toLocaleString('sq-AL', { maximumFractionDigits: 2 });
}

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: { url: '/Admin/Property/GetAll' },
        columns: [
            { data: 'referenceCode', width: '10%', render: $.fn.dataTable.render.text() },
            { data: 'title', width: '22%', render: $.fn.dataTable.render.text() },
            {
                data: 'propertyType.name',
                width: '12%',
                defaultContent: '',
                render: $.fn.dataTable.render.text()
            },
            {
                data: null,
                width: '16%',
                orderable: false,
                render: function (data, type, row) {
                    var city = $('<span>').text(row.city ? row.city.name : '').html();
                    var neighbourhood = row.neighbourhood
                        ? '<br><small class="text-muted">' + $('<span>').text(row.neighbourhood.name).html() + '</small>'
                        : '';
                    return city + neighbourhood;
                }
            },
            {
                data: null,
                width: '15%',
                orderable: false,
                render: function (data, type, row) {
                    var parts = [];
                    if (row.salePrice != null) {
                        parts.push('Shitje: ' + formatPrice(row.salePrice));
                    }
                    if (row.rentPrice != null) {
                        parts.push('Qira: ' + formatPrice(row.rentPrice) + ' / muaj');
                    }
                    return parts.join('<br>');
                }
            },
            {
                data: 'isActive',
                width: '8%',
                render: function (data) {
                    return data
                        ? '<span class="badge bg-success">Po</span>'
                        : '<span class="badge bg-secondary">Jo</span>';
                }
            },
            {
                data: 'id',
                width: '17%',
                orderable: false,
                render: function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/Admin/Property/Upsert?id=${data}" class="btn btn-primary mx-2">Ndrysho</a>
                        <a onClick="confirmDelete('/Admin/Property/Delete/${data}', () => dataTable.ajax.reload())" class="btn btn-danger mx-2">Fshi</a>
                    </div>`;
                }
            }
        ]
    });
}
