// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
(function () {

    'use strict';

    $('.input-file').each(function () {
        var $input = $(this),
            $label = $input.next('.js-labelFile'),
            labelVal = $label.html();

        $input.on('change', function (element) {
            var fileName = '';
            if (element.target.value) fileName = element.target.value.split('\\').pop();
            fileName ? $label.addClass('has-file').find('.js-fileName').html(fileName) : $label.removeClass('has-file').html(labelVal);
        });
    });

})();

document.getElementById('addUserButton').addEventListener('click', function () {
    var selectedOptions = Array.from(document.getElementById('selectedUsers').selectedOptions).map(option => option.value);
    var usersField = document.getElementById('users');
    var currentUsers = usersField.value.split(',').map(user => user.trim());
    selectedOptions.forEach(option => {
        if (!currentUsers.includes(option)) {
            currentUsers.push(option);
        }
    });
    usersField.value = currentUsers.join(', ');
});

document.getElementById('removeUserButton').addEventListener('click', function () {
    var selectedOptions = Array.from(document.getElementById('selectedUsers').selectedOptions).map(option => option.value);
    var usersField = document.getElementById('users');
    var currentUsers = usersField.value.split(',').map(user => user.trim());
    var hasTestUser = currentUsers.includes('test');
    selectedOptions = selectedOptions.filter(option => !(hasTestUser && option === 'test'));

    selectedOptions.forEach(option => {
        var index = currentUsers.indexOf(option);
        if (index !== -1) {
            currentUsers.splice(index, 1);
        }
    });

    usersField.value = currentUsers.join(', ');
});

document.getElementById('uploadButton').addEventListener('click', function () {
    var description = document.getElementById('description').value;
    var selectedUsers = Array.from(document.getElementById('selectedUsers').selectedOptions).map(option => option.value);
    $.ajax({
        url: '/DriveSec/UploadFile',
        method: 'POST',
        data: {
            description: description,
            selectedUsers: selectedUsers.join(',')
        },
        success: function (response) {
            // Обработка успешного ответа от сервера
        },
        error: function (xhr, status, error) {
            // Обработка ошибки
        }
    });
});