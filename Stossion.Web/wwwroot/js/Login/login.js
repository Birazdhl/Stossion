
$(document).ready(function () { 
    var isValidated = true;
    $("#register").on('click',function () {

        swal({
            title: 'Register',
            html: `
            <form id="registerFrm">

                <div class="form-group d-flex">
                    <div>
                        <input type="text" class="form-control registerInput" id="firstName" placeholder="First Name" name="FirstName" >
                        <span class="errorTxt d-none" id="firstNameErrorField"></span>
                    </div>
                    <div>
                        <input type="text" class="form-control mx-4 registerInput" placeholder="Last Name" name="Lastname" >
                    </div>
                </div>

                <div class="form-group d-flex">
                    <div>
                        <input type="text" class="form-control registerInput" placeholder="Username" id="Username" name="Username" >
                        <span class="errorTxt d-none" id="UsernameErrorField"></span>
                    </div>
                    <div>
                        <input type="text" class="form-control mx-4 registerInput" id="Email" placeholder="Email" name="Email" >
                         <span class="errorTxt d-none" style="padding-left:36px;" id="EmailErrorField"></span>
                    </div>
                </div>

                <div class="form-group d-flex">
                    <div>
                        <input type="password" class="form-control registerInput" placeholder="Password" id="Password" name="Password" >
                        <span class="errorTxt d-none" id="PasswordErrorField"></span>
                    </div>
                    <div>
                         <input type="password" class="form-control mx-4 registerInput" id="ConfirmPassword" placeholder="Confirm Password" name="ConfirmPassword" >
                         <span class="errorTxt d-none" style="padding-left:36px;" id="ConfirmPasswordErrorField"></span>
                    </div>
                </div>

                <div class="form-group d-flex">
                    <div id="countryContainer">
                        <select class="form-control loginSelect registerInput" name="Country" placeholder="Select a Country" id="Country"></select>
                        <span class="errorTxt d-none" id="CountryErrorField"></span>
                    </div>
                    <div>
                         <input type="text" class="form-control mx-4 registerInput" placeholder="Birthday" id="Birthday" name="Birthday" >
                         <span class="errorTxt d-none" style="padding-left:36px;" id="BirthdayErrorField"></span>
                    </div>
                </div>

                <div class="form-group d-flex">
                    <div>
                        <select class="form-control loginSelect registerInput" name="Gender" placeholder="Select a Gender" id="Gender">
                          <option class="loginOption" value="0"> Gender</option>
                          <option class="loginOption" value="1">Male</option>
                          <option class="loginOption" value="2">Female</option>
                          <option class="loginOption" value="3">Other</option>
                        </select>
                        <span class="errorTxt d-none" id="GenderErrorField"></span>
                    </div>
                     <div>
                          <input type="string" class="form-control mx-4 registerInput" id="PhoneNumber" placeholder="PhoneNumber" name="PhoneNumber" >
                          <span class="errorTxt d-none" style="padding-left:36px;" id="PhoneNumberErrorField"></span>
                    </div>
                </div>

                
            </form>
        `,
            showCancelButton: true,
            showCloseButton: true,
            showConfirmButton: true,
            allowOutsideClick: false,
            allowEscapeKey: false,
            allowEnterKey: false,
            confirmButtonText: 'Register',
            confirmButtonColor: "green",
            preConfirm: function () {
                return new Promise(function (resolve, reject) {
                    var form = $("#registerFrm");
                    isValidated = true;

                    // Validation functions
                    if (!validateFirstName()) {
                        isValidated = false;
                    }

                    if (!validateUsername()) {
                        isValidated = false;
                    }

                    if (!validateEmailInput()) {
                        isValidated = false;
                    }

                    if (!validatePassword()) {
                        isValidated = false;
                    }

                    if (!validateConfirmPassword()) {
                        isValidated = false;
                    }

                    if (!validateGender()) {
                        isValidated = false;
                    }

                    if (isValidated) {
                        resolve();
                    } else {
                        reject();
                    }
                });
            }
        }).then(function () {
            alert("message");
        }).catch(swal.noop);

        $(".swal2-modal").css('background-color', '#000');//Optional changes the color of the sweetalert 
        $(".swal2-modal").css('width', 'fit-content');//Optional changes the color of the sweetalert 
        $(".swal2-modal").css('margin-top', '2rem');
        $(".swal2-container.in").css('background-color', 'rgba(43, 165, 137, 0.45)');//changes the color of the overlay
        $(".swal2-title").css("color", "white");
        $(".swal2-confirm").css({ "padding": "0px", "height": "3rem", "width": "8rem" });
        $(".swal2-cancel").css({ "padding": "0px", "height": "3rem", "width": "8rem" });
       
        $('#Birthday').bootstrapMaterialDatePicker({           
            weekStart: 0,
            time: false,
            format: 'MMM DD YYYY'
        });

        $.ajax({
            url: "/Common/GetCountryList", // Replace with your controller and action
            type: "GET",
            dataType: "json",
            success: function (data) {
                // Handle the success response
                if (data.length > 0) {
                    var dropdown = $("#Country");
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

        $("#Country").select2({
            placeholder: "Select Country",
            containerCssClass: "countrySelectContainer",
            dropdownCssClass: "countryDropdown",
            templateResult: function (data, container) {

                var flagImage = $("<img src=\"" + $(data.element).attr("logo") + "\" style=\"height: 1rem; width: 2rem;\" />");
                var countryName = $("<span style=\"padding-left: 1rem;color: white;\">" + data.text + "</span>")
                // Create a container (span) to hold both the image and the text
                var resultContainer = $("<span></span>");

                // Append the image and the text to the container
                resultContainer.append(flagImage).append(countryName);

                // Return the container
                return resultContainer;
            },
            templateSelection: function (data, contianer) {
                var flagImage = $("<img src=\"" + $(data.element).attr("logo") + "\" style=\"height: 2rem;\" />");
                var countryName = $("<span style=\"padding-left: 1rem;color: white;\">" + data.text + "</span>")
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

        var date = new Date();
        date = getDateFormat(date.setFullYear(date.getFullYear() - 12));
        $("#Birthday").val(date);
    });

    $(document).on('focusout', '#firstName', function () {
        validateFirstName();
    });
    $(document).on('focusout', '#Email', function () {
        validateEmailInput();
    });

    $(document).on('focusout', '#Username', function () {
        validateUsername();
    });

    $(document).on('focusout', '#Password', function () {
        validatePassword();
    });
    $(document).on('focusout', '#ConfirmPassword', function () {
        validateConfirmPassword();
    });

    $(document).on('change', '#Gender', function () {
        validateGender();
    });

    $(document).on('input', '#PhoneNumber', function () {
        debugger;
        // Get the current value of the input field
        var inputValue = $('#PhoneNumber').val();

        // Use a regular expression to check if the input contains only numbers
        var numbersOnly = /^\d{1,15}$/;

        // If the input does not match the regular expression, clear the input field
        if (!numbersOnly.test(inputValue) || inputValue.length > 15) {
            $(this).val(inputValue.replace(/.$/, "")); 
            //var value = inputValue.replace(/.$/, "");
            //$(this).val(value);
        }

        if ($(this).val().length > 0) {
            $('#PhoneNumberErrorField').text("Please enter with extension(numbers only)");
            $('#PhoneNumberErrorField').removeClass("d-none");
        } else {
            $('#PhoneNumberErrorField').addClass("d-none");
        }
    });

    $(document).on("input", '.registerInput',function () {
        $(this).next('span.errorTxt').addClass('d-none');
    });
    function validateFirstName() {

        //validate firstname that is requried
        if ($('#firstName').val().length <= 0) {
            $('#firstNameErrorField').text("First Name is required");
            $('#firstNameErrorField').removeClass("d-none");
            return false;
        } else {
            $('#firstNameErrorField').addClass("d-none");
            return true;
        }
    }
    function validateEmailInput() {
        var isEmailValid = validateEmail("Email");
        if (!isEmailValid) {
            $('#EmailErrorField').text("Invalid! Email Id");
            $('#EmailErrorField').removeClass("d-none");
            return false;
        } else { $('#EmailErrorField').addClass("d-none"); return true; }
    }
    function validateUsername() {
        if ($('#Username').val().length <= 0) {
            $('#UsernameErrorField').text("User Name is required");
            $('#UsernameErrorField').removeClass("d-none");
            return false;
        } else {
            if ($('#Username').val().length > 0) {
                var regex = /^[a-zA-Z0-9_!@#$%^&*()-+=]{5,20}$/;
                var value = $('#Username').val();
                if (regex.test(value)) {
                    $('#UsernameErrorField').addClass("d-none");
                    return true;
                }
                else {
                    $('#UsernameErrorField').text("Invalid! Username");
                    $('#UsernameErrorField').removeClass("d-none");
                    return false;
                }
            }

            else { $('#UsernameErrorField').addClass("d-none"); return true; }
        }
    }
    function validatePassword() {
        if ($('#Password').val().length <= 0) {
            $('#PasswordErrorField').text("Password is required");
            $('#PasswordErrorField').removeClass("d-none");
            return false;
        } else {
            var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$/;
            if (regex.test($('#Password').val())) {
                $('#PasswordErrorField').addClass("d-none");
                return true;
            }
            else {
                $('#PasswordErrorField').text("Password must be 8 digits and contains [A-Z] [a-z] [1-9] [!@#$%^&*()_]");
                $('#PasswordErrorField').removeClass("d-none");
                return false;
            }
        }
    }
    function validateConfirmPassword() {
        if ($('#ConfirmPassword').val() != $('#Password').val()) {
            $('#ConfirmPasswordErrorField').text("Password dosen't match");
            $('#ConfirmPasswordErrorField').removeClass("d-none");
        } else {
            $('#ConfirmPasswordErrorField').addClass("d-none");
            return true;
        }
    }
    function validateGender() {
        if ($('#Gender').val() == "0") {
            $('#GenderErrorField').text("Please select a Gender");
            $('#GenderErrorField').removeClass("d-none");
            return false;
        } else {
            $('#GenderErrorField').addClass("d-none");
            return true;
        }
    }

   
});

