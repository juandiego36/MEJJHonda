$(document).on('invalid-form.validate', 'form', function () {
    var button = $(this).find(':submit');
    console.log('jp;a');
    setTimeout(function () {
        button.removeAttr('disabled');
    }, 1);
});
$(document).on('submit', 'form', function () {
    var button = $(this).find(':submit');
    setTimeout(function () {
        button.attr('disabled', 'disabled');
    }, 0);
});