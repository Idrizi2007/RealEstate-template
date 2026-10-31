// Shared delete for every admin page.
// url: the Delete endpoint to call
// onSuccess: what this page does after a delete (reload a table, remove a card...)
function confirmDelete(url, onSuccess) {
    Swal.fire({
        title: "Je i sigurt?",
        text: "Kjo nuk mund te kthehet mbrapsht.",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#d33",
        cancelButtonColor: "#6c757d",
        confirmButtonText: "Po, fshije",
        cancelButtonText: "Anulo"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (data) {
                    toastr.success(data.message);
                    if (onSuccess) {
                        onSuccess();
                    }
                },
                error: function (xhr) {
                    // 400 or 404 from the controller: show its message
                    toastr.error(xhr.responseJSON?.message ?? "Dicka shkoi keq");
                }
            });
        }
    });
}
