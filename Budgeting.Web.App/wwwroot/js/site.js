// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.



/***********FORM VIEW JQUERY*************************/
/****************************************************/

$('.div-col-income').click(function () {
    //toggle radio button checked prop
    $('input[value=Income]').prop('checked', true);
    $('input[value=Expense]').prop('checked', false);

    //changing classes
    $('.radio-css').removeClass('div-col-income');
    $('.div-col-expense').addClass('div-col-income');
});

$('.div-col-expense').click(function () {
    $('input[value=Expense]').prop('checked', true);
    $('input[value=Income]').prop('checked', false);

    $('.div-col-expense').removeClass('div-col-income');
    $('.radio-css').addClass('div-col-income');
});



/**Category Form Validation*/
function validateCategory() {
    event = event || window.event || event.srcElement;

    let title = $('#title').val();
    var IsValid;

    if (title < 3) {
        $('#title').next('span').text('Title must be between 3 and 20 charachters.').show();
        isValid = false;
    }
    else {
        $('#title').next('span').hide();
    }

    if (!isValid) {
        event.preventDefault();
    }
}



/****************************************************/
/****************************************************/