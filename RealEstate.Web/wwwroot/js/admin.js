(function () {
    if ($.fn.dataTable) {
        $.extend(true, $.fn.dataTable.defaults, {
            language: {
                search: '',
                searchPlaceholder: 'Kërko…',
                lengthMenu: 'Shfaq _MENU_ për faqe',
                info: '_START_–_END_ nga _TOTAL_',
                infoEmpty: '0 rezultate',
                infoFiltered: '(nga _MAX_ gjithsej)',
                zeroRecords: 'Nuk u gjet asgjë',
                emptyTable: 'Ende nuk ka asgjë këtu',
                loadingRecords: 'Duke ngarkuar…',
                processing: 'Duke punuar…',
                paginate: { first: 'E para', last: 'E fundit', next: '›', previous: '‹' },
                aria: { sortAscending: ': rendit në rritje', sortDescending: ': rendit në zbritje' }
            }
        });
    }

    document.querySelectorAll('[data-gd-side]').forEach(function (el) {
        el.addEventListener('click', function () {
            document.body.classList.toggle('gd-side-open');
        });
    });

    document.addEventListener('keydown', function (e) {
        if (e.key === 'Escape') {
            document.body.classList.remove('gd-side-open');
        }
    });
})();
