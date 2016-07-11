$(document).ready(function () {

    if (window.sessionStorage.getItem("loggedInUser") === null) {
        window.location = '../';
        alert("Your session has expired. Kindly login again.");
    } else {

        var user = JSON.parse(window.sessionStorage.getItem("loggedInUser"));
        $('#theLoggedInUser').html(user.Username);
        $('#theLoggedInUserRole').html(user.Role);
    }
});

function logout() {
    window.sessionStorage.removeItem("loggedInUser");
    window.location = ("../");
}

function lockscreen() {
    window.location = ("../User/LockScreen");
}

function searchDocument() {
    if (window.sessionStorage.getItem("searchValue") != null)
        window.sessionStorage.removeItem("searchValue");

    var searchValue = prompt("Enter your search terms", "");

    if (searchValue != null) {
        if (searchValue == "") {
            displayMessage("error", "Enter your search terms");
        } else {
            window.sessionStorage.setItem("searchValue", searchValue);
            window.location = ("../Document/Search");
        }
    }
}