var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function formatPrice(value) {
    return Number(value).toLocaleString('sq-AL', { maximumFractionDigits: 2 });
}

function esc(value) {
    return $('<span>').text(value ?? '').html();
}

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: { url: '/Admin/Property/GetAll' },
        columns: [
            {
                data: 'referenceCode',
                width: '10%',
                render: function (data) {
                    return '<span class="gd-mono gd-ref-cell">' + esc(data) + '</span>';
                }
            },
            {
                data: 'title',
                width: '24%',
                render: function (data, type, row) {
                    if (type !== 'display') {
                        return data;
                    }
                    return '<strong class="fw-semibold">' + esc(data) + '</strong>'
                        + (row.propertyType ? '<span class="gd-cell-sub">' + esc(row.propertyType.name) + '</span>' : '');
                }
            },
            {
                data: 'propertyType.name',
                visible: false,
                defaultContent: ''
            },
            {
                data: null,
                width: '18%',
                orderable: false,
                render: function (data, type, row) {
                    var city = esc(row.city ? row.city.name : '');
                    var neighbourhood = row.neighbourhood
                        ? '<span class="gd-cell-sub">' + esc(row.neighbourhood.name) + '</span>'
                        : '';
                    return city + neighbourhood;
                }
            },
            {
                data: null,
                width: '20%',
                orderable: false,
                render: function (data, type, row) {
                    var parts = [];
                    if (row.salePrice != null) {
                        parts.push('<div><span>Shitje</span>' + formatPrice(row.salePrice) + '</div>');
                    }
                    if (row.rentPrice != null) {
                        parts.push('<div><span>Qira</span>' + formatPrice(row.rentPrice) + ' / muaj</div>');
                    }
                    return '<div class="gd-price-cell">' + parts.join('') + '</div>';
                }
            },
            {
                data: 'isActive',
                width: '10%',
                render: function (data, type) {
                    if (type !== 'display') {
                        return data ? 1 : 0;
                    }
                    return data
                        ? '<span class="gd-pill is-on">Aktive</span>'
                        : '<span class="gd-pill">Joaktive</span>';
                }
            },
            {
                data: 'id',
                width: '18%',
                orderable: false,
                render: function (data) {
                    return `<div class="gd-row-actions">
                        <a href="/Admin/Property/Upsert?id=${data}" class="btn btn-outline-secondary"><i class="bi bi-pencil me-1"></i>Ndrysho</a>
                        <button type="button" class="gd-icon-btn is-danger" aria-label="Fshi pronën" title="Fshi"
                                onclick="confirmDelete('/Admin/Property/Delete/${data}', () => dataTable.ajax.reload())"><i class="bi bi-trash"></i></button>
                    </div>`;
                }
            }
        ]
    });
}
