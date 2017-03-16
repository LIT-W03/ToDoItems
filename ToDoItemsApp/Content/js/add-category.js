$(function() {
    $("#category-name").on('keyup', function() {
            var value = $("#category-name").val();
            $("#submit").attr('disabled', !value || value.length < 5);
        });
});