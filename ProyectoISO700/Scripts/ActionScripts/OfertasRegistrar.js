﻿$(function () {
    $('#frmBusqueda').submit();
    jQuery('html, body').animate({ scrollTop: 500 }, 500);
    $('#btnRegistrar').on('click', function (event) { btnRegistrar_OnClick($(this), event); });
});

function btnRegistrar_OnClick(sender, e)
{
    if (!validation())
    {
        e.preventDefault();
        return;
    }
}

function validation()
{
    if ($('#Categoria').val().trim() == '' || $('#Categoria').val() == undefined)
    {
        toastr.error('Debe seleccionar una categoria.');
        return false;
    }
    if ($('#Tipo').val().trim() == '' || $('#Tipo').val() == undefined) {
        toastr.error('Debe seleccionar un tipo.');
        return false;
    }
    if ($('#Compañia').val().trim() == '' || $('#Compañia').val() == undefined) {
        toastr.error('Debe completar el campo compañia');
        return false;
    }
    /*if ($('#Logo').val().trim() == '' || $('#Logo').val() == undefined) {
        toastr.error('Debe seleccionar un logo.');
        return false;
    }
    if ($('#URL').val().trim() == '' || $('#URL').val() == undefined) {
        toastr.error('Debe completar una URL.');
        return false;
    }*/
    if ($('#Posicion').val().trim() == '' || $('#Posicion').val() == undefined) {
        toastr.error('Debe completar el nombre de la posición.');
        return false;
    }
    if ($('#Ubicacion').val().trim() == '' || $('#Ubicacion').val() == undefined) {
        toastr.error('Debe completar el nombre de la ubicación.');
        return false;
    }
    if ($('#Descripcion').val().trim() == '' || $('#Descripcion').val() == undefined) {
        toastr.error('Debe completar una descripción.');
        return false;
    }

    return true;
}