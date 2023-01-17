// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



/***********FORM VIEW JQUERY*************************/
/****************************************************/

$('.div-col-income').click(function () {
    //toggle radio button checked prop
    $('input[name=Income]').prop('checked', true);
    $('input[name=Type]').prop('checked', false);

    //changing classes
    $('.radio-css').removeClass('div-col-income');
    $('.div-col-expanse').addClass('div-col-income');
});

$('.div-col-expanse').click(function () {
    $('input[name=Type]').prop('checked', true);
    $('input[name=Income]').prop('checked', false);

    $('.div-col-expanse').removeClass('div-col-income');
    $('.radio-css').addClass('div-col-income');
});

/****************************************************/
/****************************************************/