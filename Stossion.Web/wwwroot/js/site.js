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

    $('.nav_btn').click(function () {
        $('.mobile_nav_items').toggleClass('active');
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

    $(".emailType").blur(function () {
        var regex = validateEmail($(this).val());
        if (!regex) {
            toggleErrorMessage($(this), "Invalid Email Format");
        }
    });

    $.ajax({
        url: "/Common/GetCountryList", // Replace with your controller and action
        type: "GET",
        dataType: "json",
        success: function (data) {
            // Handle the success response
            if (data.length > 0) {
                var dropdown = $(".CountrySelection");
                dropdown.empty().trigger("change");
                // Iterate through the data and append options to the dropdown
                $.each(data, function (index, country) {
                    var optionText = country.name;
                    var optionValue = country.symbol; // You can change this to country.id if needed
                    dropdown.append($("<option class=\"loginOption\" logo='" + country.logo + "'>").val(optionValue).html(optionText));
                });
            }
        },
        error: function (error) {
            // Handle errors
            console.error(error);
        }
    });

    $(".CountrySelection").select2({
        placeholder: "Select Country",
        containerCssClass: "countrySelectContainer",
        dropdownCssClass: "countryDropdown",
        templateResult: function (data, container) {

            var flagImage = $("<img src=\"" + $(data.element).attr("logo") + "\" style=\"height: 1rem; width: 2rem; border: 1px solid black;\" />");
            var countryName = $("<span style=\"padding-left: 1rem;color: black;\">" + data.text + "</span>")
            // Create a container (span) to hold both the image and the text
            var resultContainer = $("<span></span>");

            // Append the image and the text to the container
            resultContainer.append(flagImage).append(countryName);

            // Return the container
            return resultContainer;
        },
        templateSelection: function (data, contianer) {
            var flagImage = $("<img src=\"" + $(data.element).attr("logo") + "\" style=\"height: 2rem; border: 1px solid black;\" />");
            var countryName = $("<span style=\"padding-left: 1rem;color: blackl;\">" + data.text + "</span>")
            var resultContainer = $("<span></span>");

            // Append the image and the text to the container
            resultContainer.append(flagImage).append(countryName);

            // Return the container
            return resultContainer;
        },
    }).on("select2:open", function () {
        $(".select2-search__field").on("keydown", function (e) {
            if (e.keyCode === 13) {
                e.preventDefault();
                e.stopPropagation();
            }
        });
    });  


    $(document).on('input', '.phoneNumberValidation', function () {
        // Get the current value of the input field
        var inputValue = $(this).val();

        // Use a regular expression to check if the input contains only numbers
        var numbersOnly = /^\d{1,15}$/;

        // If the input does not match the regular expression, clear the input field
        if (!numbersOnly.test(inputValue) || inputValue.length > 15) {
            $(this).val(inputValue.replace(/.$/, ""));
            //var value = inputValue.replace(/.$/, "");
            //$(this).val(value);
        }

        if ($(this).val().length > 0) {
            $('.PhoneNumberErrorField').text("Please enter with extension(numbers only)");
            $('.PhoneNumberErrorField').removeClass("d-none");
            $('.PhoneNumberErrorField').css("color", "green");
        } else {
            $('.PhoneNumberErrorField').addClass("d-none");
        }
    });

});
