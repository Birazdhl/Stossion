$(document).ready(function () { 
    var isValidated = true;
    $("#register").on('click',function () {

        swal({
            title: 'Register',
            html: `
            <form id="registerFrm">
                <input id="profilePicture" hidden/>
                <div class="form-group d-flex">
                    <div>
                        <input type="text" class="form-control registerInput" id="firstName" placeholder="First Name" name="FirstName" >
                        <span class="errorTxt d-none" id="firstNameErrorField"></span>
                    </div>
                    <div>
                        <input type="text" class="form-control mx-4 registerInput" id="lastName" placeholder="Last Name" name="Lastname" >
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

                <div class="form-group d-flex">
                    <div>
                        <input type="file" id="userImage" name="UserImage" accept="image/*" style="display: none;">
                        <label for="userImage" class="custom-file-upload">
                            Upload Profile Image
                        </label>
                        <span class="errorTxt d-none" id="userImageErrorField"></span>
                    </div>
                    <div id="uploadedImagesContainer"></div>
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

                        var postData = {
                            "UserName": $("#Username").val(),
                            "FirstName": $("#firstName").val(),
                            "LastName": $("#lastName").val(),
                            "Country": $("#Country").val(),
                            "Password": $("#Password").val(),
                            "ConfirmPassword": $("#ConfirmPassword").val(),
                            "PhoneNumber": $("#PhoneNumber").val(),
                            "Email": $("#Email").val(),
                            "Birthday": $("#Birthday").val(),
                            "Gender": $("#Gender").val(),
                            "ProfilePicture": $("#profilePicture").val()
                        };
                        $.ajax({
                            url: "/Login/Register", // Replace with your controller and action
                            type: "POST",
                            dataType: "json",
                            data: postData,
                            success: function (success) {
                                debugger;
                                // Handle the success response
                                if (success.message == "Success") {
                                    var url = '/Common/ErrorMessage';
                                    var message = "Please Verify Email first to continue";
                                    window.location.href = url + "?message=" + message + "&userName=" + $("#Username").val();
                                    resolve();
                                }
                                else {
                                    toastr.error(success.message)
                                }
                                reject();
                            },
                            error: function (error) {
                                reject();
                                // Handle errors
                                console.error(error);
                            }
                        });

                    } else {
                        reject();
                    }
                });
            }
        }).then(function () {

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


        var date = new Date();
        date = getDateFormat(date.setFullYear(date.getFullYear() - 12));
        $("#Birthday").val(date);

        $(document).on('change', '#userImage', function (event) {

            var file = event.target.files[0];

            // Display the name of the uploaded file and a cross logo
            var fileName = file.name;
            var uploadedImageContainer = document.createElement('div');
            uploadedImageContainer.style.display = 'inline-block';

            var fileNameElement = document.createElement('span');
            fileNameElement.textContent = fileName;

            // Create a cross logo (HTML entity)
            var crossLogo = document.createElement('span');
            crossLogo.innerHTML = '&#10060;';
            crossLogo.style.cursor = 'pointer';
            crossLogo.style.marginLeft = '5px';
            crossLogo.onclick = function () {
                // Remove the uploaded image container
                uploadedImageContainer.remove();
                // Clear the file input
                document.getElementById('userImage').value = '';
                $("#profilePicture").val('');
            };

            uploadedImageContainer.appendChild(fileNameElement);
            uploadedImageContainer.appendChild(crossLogo);

            // Add the uploaded image container to the container
            var container = document.getElementById('uploadedImagesContainer');
            container.appendChild(uploadedImageContainer);
            getBase64(event, function (value) {
                $("#profilePicture").val(value);
            });

        });

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
            $('#PhoneNumberErrorField').css("color","green");
        } else {
            $('#PhoneNumberErrorField').addClass("d-none");
        }
    });

    $(document).on("input", '.registerInput', function () {
        //$(this).next('span.errorTxt').addClass('d-none');

        var errorSpan = $(this).next('span.errorTxt');

        // Check if the errorTxt class has the ID PhoneNumberErrorField
        if (errorSpan.attr("id") == 'PhoneNumberErrorField') {
            errorSpan.remove('d-none');
        } else {
            errorSpan.addClass('d-none');
        }
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
            var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#^()])[A-Za-z\d@$!%*?&#^()]{8,}$/;
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

    if ($("#emailInvalid").text() == "Invalid") {
        swal({
            title: 'Email Not Registered',
            html: `
           <div>
                <p>The email address is not registered into Stossion. </p>
                <br>
                <p>Please register it to use gmail login.(It is a one time process)</p>
           </div>
        `,
            showCancelButton: true,
            showCloseButton: true,
            showConfirmButton: false,
            allowOutsideClick: false,
            allowEscapeKey: false,
            allowEnterKey: false,
            cancelButtonText: 'Ok',
            preConfirm: function () {
                resolve()
            }
        }).then(function () {

        }).catch(swal.noop);
    }

    $("#forgotPassword").on('click', function () {

        var userName = $("#forgetPasswordUserName").val();
        if (userName == null || userName == '') {
            toastr.info("Please enter username");
            return false;
        }

        $.ajax({
            url: "/Login/EmailVerificationLink?username=" + $("#forgetPasswordUserName").val(), // Replace with your controller and action
            type: "GET",
            success: function (data) {
                // Handle the success response
                console.log(data);
                if (data == "Success") {
                    var text = "The password reset link is sent to the email associated with username: " + userName;
                    $("#emailBox").hide();
                    $("#emailSent").show();
                    $("#emailSentText").text(text);
                }
                else {
                    toastr.error(success)
                }
                reject();
            },
            error: function (error) {
                toastr.error("Error")
                // Handle errors
                console.error(error);
            }
        });
    });

    $("#resetPasswordButton").on('click', function () {

        var postData = {
            "Token": $("#token").val(),
            "Username": $("#resetUsername").val(),
            "Password": $("#resetNewPassword").val(),
            "ConfirmPassword": $("#resetConfirmNewPassword").val()
        }

        if ($("#resetNewPassword").val() == null || $("#resetNewPassword").val() == '') {
            toastr.error("Please enter password");
            return false;
        }

        if ($("#resetNewPassword").val() != $("#resetConfirmNewPassword").val()) {
            toastr.error("Password dosen't match");
            return false;
        }
     
      
        $.ajax({
            url: "/Login/ResetPassword", // Replace with your controller and action
            type: "POST",
            data: postData,
            success: function (data) {
                // Handle the success response
                if (data == "Success") {
                    toastr.success("Password reset Successfully!!!")
                    setTimeout(function () {
                        window.location.href = "/Login/Index";
                    }, 1000);
                }
                else {
                    toastr.error(success)
                }
            },
            error: function (error) {
                toastr.error("Error")
                // Handle errors
                console.error(error);
            }
        });
    });

   
});

