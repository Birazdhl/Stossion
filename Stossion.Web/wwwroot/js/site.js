$(document).ready(function () {
    $(document).on("click", function (event) {
        var submenuToggle = $("#submenuToggle");
        var submenu = $("#submenu");

        if (!submenuToggle.is(event.target) && !submenu.has(event.target).length) {
            submenu.hide();
        }
    });

    $("#submenuToggle").on("click", function () {
        var submenu = $("#submenu");

        if (submenu.is(":hidden")) {
            submenu.show();
        } else {
            submenu.hide();
        }
    });
});
