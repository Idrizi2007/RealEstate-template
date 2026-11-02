$('#iconPicker').on('click', '.icon-option', function () {
    $('#iconPicker .icon-option').removeClass('active');
    $(this).addClass('active');
    $('#IconClass').val($(this).data('icon'));
});