$(window).on("load", function () {
    $(".loader-wrapper").fadeOut("slow");
});


$(document).on('invalid-form.validate', 'form', function () {
    var button = $(this).find(':submit');
    setTimeout(function () {
        $(".loader-wrapper").hide();
    }, 1);
});
$(document).on('submit', 'form', function () {
    var button = $(this).find(':submit');
    setTimeout(function () {
        $(".loader-wrapper").show();
    }, 0);
});