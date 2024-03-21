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


    $("#logout").on("click", function () {
        localStorage.clear();
    });


    var profilePicture = localStorage.getItem('profilePicture');
    // If profile picture exists, make AJAX call to fetch the latest picture
    if (!profilePicture) {
        $.ajax({
            url: '/Login/GetProfilePicture',
            type: 'GET',
            success: function (data) {
                localStorage.setItem('profilePicture', data);
                $(".rounded-circle").attr("src", 'data:image/png;base64,' + data);
            },
            error: function (xhr, status, error) {
                console.error('Error fetching profile picture:', error);
            }
        });
    } else {
        $(".rounded-circle").attr("src", 'data:image/png;base64,' + profilePicture);
    }

    $(".stextBox").on("focus", function () {
        $(this).siblings(".errorTxt").addClass("d-none");
    });

    $(".stextBox.required").blur(function () {
        var inputValue = $(this).val().trim();
        if (inputValue === "") {
            $(this).siblings(".errorTxt").remove(); // Remove any existing error span
            $(this).after('<span class="errorTxt">This field is required</span>'); // Add error span
        }
    });

    $(".passwordType").blur(function () {
        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#^()])[A-Za-z\d@$!%*?&#^()]{8,}$/;
        if (!regex.test($(this).val())) {
            toggleErrorMessage($(this), "Password must be 8 digits and contains [A-Z] [a-z] [1-9] [!@#$%^&*()_]");
        }
    });

});
