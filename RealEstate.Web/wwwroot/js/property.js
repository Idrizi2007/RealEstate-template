var allProperties = [];

$(document).ready(function () {
    loadProperties();
    $('#propertySearch').on('input', render);
    $('input[name="statusFilter"]').on('change', render);
});

function esc(value) {
    return $('<span>').text(value ?? '').html();
}

function formatPrice(value) {
    return Number(value).toLocaleString('sq-AL', { maximumFractionDigits: 0 });
}

function loadProperties() {
    $.get('/Admin/Property/GetAll')
        .done(function (response) {
            allProperties = response.data || [];
            render();
        })
        .fail(function () {
            $('#propertyGrid').html('<div class="gd-empty">Pronat nuk u ngarkuan. Rifresko faqen.</div>');
            $('#propertyCount').text('');
        });
}

function primaryImage(property) {
    var images = (property.images || []).slice().sort(function (a, b) { return a.sortOrder - b.sortOrder; });
    var primary = images.find(function (i) { return i.isPrimary; }) || images[0];
    return primary ? primary.imageUrl : null;
}

function matches(property, term, status) {
    if (status === 'active' && !property.isActive) return false;
    if (status === 'inactive' && property.isActive) return false;
    if (!term) return true;
    var haystack = [
        property.referenceCode,
        property.title,
        property.city && property.city.name,
        property.neighbourhood && property.neighbourhood.name,
        property.propertyType && property.propertyType.name
    ].join(' ').toLowerCase();
    return haystack.indexOf(term) !== -1;
}

function card(p) {
    var image = primaryImage(p);
    var photo = image
        ? '<img src="' + esc(image) + '" alt="" loading="lazy" />'
        : '<span class="gd-photo-empty"><i class="bi bi-image"></i></span>';

    var place = [p.city && p.city.name, p.neighbourhood && p.neighbourhood.name].filter(Boolean).map(esc).join(' · ');

    var prices = [];
    if (p.salePrice != null) {
        prices.push('<div><span>Shitje</span><strong>' + formatPrice(p.salePrice) + '</strong></div>');
    }
    if (p.rentPrice != null) {
        prices.push('<div><span>Qira</span><strong>' + formatPrice(p.rentPrice) + '</strong> / muaj</div>');
    }

    var facts = [];
    if (p.areaM2 != null) facts.push(esc(p.areaM2) + ' m²');
    if (p.bedrooms != null) facts.push(esc(p.bedrooms) + ' dhoma');
    if (p.bathrooms != null) facts.push(esc(p.bathrooms) + ' banjo');

    var editUrl = '/Admin/Property/Upsert?id=' + p.id;

    return '<article class="gd-prop-card' + (p.isActive ? '' : ' is-inactive') + '">'
        + '<a class="gd-photo" href="' + editUrl + '" tabindex="-1" aria-hidden="true">'
        + '<span class="gd-chip gd-status' + (p.isActive ? ' is-on' : '') + '">' + (p.isActive ? 'Aktive' : 'Joaktive') + '</span>'
        + '<span class="gd-ref">' + esc(p.referenceCode) + '</span>'
        + photo
        + '</a>'
        + '<div class="gd-prop-body">'
        + '<div class="gd-eyebrow">' + esc(p.propertyType ? p.propertyType.name : '') + '</div>'
        + '<h2 class="gd-prop-title"><a href="' + editUrl + '">' + esc(p.title) + '</a></h2>'
        + (place ? '<div class="gd-prop-place"><i class="bi bi-geo-alt"></i>' + place + '</div>' : '')
        + (facts.length ? '<div class="gd-facts">' + facts.join('<span aria-hidden="true">·</span>') + '</div>' : '')
        + '<div class="gd-prop-prices">' + (prices.length ? prices.join('') : '<div class="gd-muted">Pa çmim</div>') + '</div>'
        + '</div>'
        + '<div class="gd-prop-actions">'
        + '<a href="' + editUrl + '" class="btn-gd-line text-decoration-none"><i class="bi bi-pencil"></i>Ndrysho</a>'
        + '<button type="button" class="gd-icon-btn is-danger ms-auto" aria-label="Fshi pronën ' + esc(p.referenceCode) + '" title="Fshi" data-delete="' + p.id + '"><i class="bi bi-trash"></i></button>'
        + '</div>'
        + '</article>';
}

function render() {
    var term = $.trim($('#propertySearch').val()).toLowerCase();
    var status = $('input[name="statusFilter"]:checked').val();
    var list = allProperties.filter(function (p) { return matches(p, term, status); });

    $('#propertyCount').text(list.length + ' nga ' + allProperties.length);

    if (!allProperties.length) {
        $('#propertyGrid').html('<div class="gd-empty">Ende nuk ka prona. <a href="/Admin/Property/Upsert">Shto të parën</a>.</div>');
        return;
    }
    if (!list.length) {
        $('#propertyGrid').html('<div class="gd-empty">Asnjë pronë nuk përputhet me kërkimin.</div>');
        return;
    }

    $('#propertyGrid').html(list.map(card).join(''));
    $('#propertyGrid [data-delete]').on('click', function () {
        var id = $(this).data('delete');
        confirmDelete('/Admin/Property/Delete/' + id, loadProperties);
    });
}
