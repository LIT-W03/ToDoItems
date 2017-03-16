$(function () {
    $("#title").on('keyup', function () {
        setButtonValidState();
    });

    $("#description").on('keyup', function () {
        setButtonValidState();
    });

    $("#date").on('change', function () {
        setButtonValidState();
    });
});

function setButtonValidState() {
    $("#submit").attr('disabled', !isValid());
}

function isValid() {
    var title = $("#title").val();
    var desc = $("#description").val();
    var date = $("#date").val();

    return title && desc && date;
}