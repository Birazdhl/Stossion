// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function validateEmail(emailId) {
    var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test($('#' + emailId).val())) {
        return false;
    }
    return true;
}
function getDateFormat(dateTime) {
    const monthNames = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'];
    var date = new Date(dateTime);
    const day = date.getDate(date);

    const monthIndex = date.getMonth();
    const monthName = monthNames[monthIndex];

    const year = date.getFullYear();

    return `${monthName} ${day} ${year}`;
}

function getBase64(event, callback) {
    var file = event.target.files[0];
    var reader = new FileReader();
    reader.readAsDataURL(file);
    reader.onload = function () {
        callback(reader.result); // Pass the result to the callback function
    };
    reader.onerror = function (error) {
        console.log('Error: ', error);
    };
}

//function getBase64(event) {

//    var file = event.target.files[0];
//    var reader = new FileReader();
//    reader.readAsDataURL(file);
//    reader.onload = function () {
//        callback(reader.result);
//    };
//    reader.onerror = function (error) {
//        console.log('Error: ', error);
//    };
//}
