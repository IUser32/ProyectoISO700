$(function () {
    $('#frmBusqueda').submit();
});

function onSuccessLoading() {
    $('#loading').hide(200);
    $('#result').show(200);
    jQuery('html, body').animate({ scrollTop: 500 }, 500);

}

function onBeginLoading() {
    $('#loading').show(200);
    $('#result').hide(200);
}

