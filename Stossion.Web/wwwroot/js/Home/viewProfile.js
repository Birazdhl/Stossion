$(document).ready(function () {

    $('.updateProfileInputField').addClass('d-none');
    $('.edit-link').show();

    $("#Gender option").filter(function () {
        return $(this).text() == '@Model.Gender';
    }).prop("selected", true);

    // For each edit link
    $('.edit-link').click(function (e) {
        e.preventDefault();

        // Assuming you want to select the option with the text "France"
        var countryText = $("#countryNameLbl").text();
        var $select = $(".CountrySelection");

        // Find the option with the specified text
        var $option = $select.find("option").filter(function () {
            return $(this).text() === countryText;
        });

        // Check if the option is found
        if ($option.length > 0) {
            // Set the found option as selected
            $option.prop("selected", true);

            // Trigger the change event to update Select2 UI
            $select.trigger("change");
        } else {
            console.log("Option with text '" + countryText + "' not found.");
        }


        var fieldId = $(this).attr('id').replace('edit-', '');

        // Toggle visibility of text field and edit link for current field
        $(this).siblings('.text-field').toggle();
        $(this).toggle();

        // Show the corresponding updateProfileInputField for current field
        var currentProfileInfo = $(this).closest('.profile-info');
        currentProfileInfo.find('.updateProfileInputField').toggleClass('d-none');

        // Hide updateProfileInputField and show text-field for other fields
        $('.profile-info').not(currentProfileInfo).find('.updateProfileInputField').addClass('d-none');
        $('.profile-info').not(currentProfileInfo).find('.text-field').show();
        $('.profile-info').not(currentProfileInfo).find('.edit-link').show();

        //for profile picture
        $(".profile-picture").attr("src", "data:image/png;base64," + localStorage.getItem('profilePicture'));
        $('.updateProfileImage').addClass('d-none');
        $('.edit-picture').removeClass('d-none');
    });

    // When cancel button is clicked
    $('.stossionCancelBtn').click(function (e) {
        e.preventDefault();

        // Hide the updateProfileInputField
        $(this).closest('.updateProfileInputField').addClass('d-none');

        // Toggle visibility of text field and edit link
        $(this).closest('.profile-info').find('.text-field').toggle();
        $(this).closest('.profile-info').find('.edit-link').toggle();
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


    // When the "Edit" link is clicked
    $(".edit-picture").click(function (e) {
        e.preventDefault(); // Prevent default link behavior
        $('.updateProfileInputField').addClass('d-none');
        $("#profile-picture-upload").click(); // Trigger click event on the hidden file input
        $(".text-field").show();
        $(".edit-link").show();
    });

    // When a file is selected using the file input
    $("#profile-picture-upload").change(function () {
        var file = this.files[0];

        if (file) {
            $('.updateProfileImage').removeClass('d-none');
            $('.edit-picture').addClass('d-none');
            // Read the selected file as a data URL
            var reader = new FileReader();
            reader.onload = function (event) {
                // Display the selected image
                $(".profile-picture").attr("src", event.target.result);

            };
            reader.readAsDataURL(file);
        }
    });

    // when the cancel button is clicked
    $(".cancel-btn").click(function () {
        // reset the profile picture to the original image
        $(".profile-picture").attr("src", "data:image/png;base64," + localStorage.getItem('profilePicture'));
        $('.updateProfileImage').addClass('d-none');
        $('.edit-picture').removeClass('d-none');

    });

    // When the update button is clicked
    $(".update-btn").click(function () {
        // Get the updated image source
        var updatedImageSrc = $(".profile-picture").attr("src");
        // Log the updated image source to the console
        console.log("Updated image source:", updatedImageSrc);
        // Perform any other actions, such as submitting the updated image to the server
    });

    $(".updateUser").on("click", function () {

        var currentEvent = $(this);
        var key = $(this).attr("name");
       
        if (key == "name") {

            if ($("#userFirstName").val() == null || $("#userLastName").val() == null) {
                toastr.error("Both FirstName and LastName is required");
                return false;
            }

            value = $("#userFirstName").val() + ',' + $("#userLastName").val()
            jqueryCommand = "$(\"#labelName\").text(value.replace(\",\", \" \"))";
            
        }
        else if (key == "country") {
            value = $("#updateProfileCountry").val()

            // Get the Select2 dropdown instance
            var select2Instance = $(".CountrySelection").data('select2');
            // Get the selected data
            var selectedData = select2Instance.data()[0];

            var logo = selectedData.element.getAttribute("logo");
            var name = selectedData.text

            jqueryCommand = "$('#countryNameLbl').text('" + name +"'); $('.profile-logo').attr('src', '" + logo + "');";

            // Update image source
            
        }
        else if (key == "gender") {
            value = $("#updateProfileGender").val()
            text = $("#updateProfileGender option:selected").text()
            jqueryCommand = "$(\"#labelGender\").text(text)";
        }


        else if (key == "email") {
            if (!$("#userEmail").val()) {
                toastr.error("Email is required");
                return false;
            }


            //change Password PoUp
            swal({
                title: 'Password Verification',
                html: `
                        <div class="flex-column">
                         <div class="">
                             <div class="col-12">
                                 <input type="password" id="password" placeholder="Password" class="form-control stextBox required" />
                             </div>
                             <div class="col-12 mt-3">
                                 <input type="password" id="confirmpassword" placeholder="ConfirmPassword" class="form-control stextBox required" />
                             </div>
                         </div>
                        </div>`,

                showCancelButton: true,
                showCloseButton: true,
                showConfirmButton: true,
                allowOutsideClick: false,
                allowEscapeKey: false,
                allowEnterKey: false,
                confirmButtonText: 'Confirm',
                confirmButtonColor: "purple",
              
                preConfirm: function () {
                    return new Promise(function (resolve, reject) {
                        debugger;
                        var swalPassword = $("#password").val();
                        var swalConfirmPassword = $("#confirmpassword").val();

                        if (swalPassword != swalConfirmPassword) {
                            toastr.error("Password Dosen't Match");
                            reject();
                        }
                        value = $("#userEmail").val()
                        jqueryCommand = "$(\"#labelEmail\").text(value)";

                        updateUser(key, value, currentEvent, jqueryCommand, swalPassword, swalConfirmPassword)
                        resolve();
                    });
                }
            }).then(function () {

               

            }).catch(swal.noop);

            $(".swal2-modal").css('background-color', 'white');//Optional changes the color of the sweetalert 
            $(".swal2-modal").css('width', '500px');//Optional changes the color of the sweetalert 
           
            $(".swal2-modal").css('margin-top', '75px');
            $(".swal2-modal").addClass('stossionRadius');
            $(".swal2-container.in").css('background-color', 'rgba(43, 165, 137, 0.45)');//changes the color of the overlay
            $(".swal2-title").css("color", "black");

            $(".swal2-confirm").addClass('stossionBtn')
            $(".swal2-cancel").addClass('stossionCancelBtn');

            $(".swal2-confirm").css('border-top-left-radius', '25px').css('border-bottom-right-radius', '25px');
            $(".swal2-cancel").css('border-top-left-radius', '25px').css('border-bottom-right-radius', '25px');

            return false;

        }


        else if (key == "phonenumber") {
            value = $("#userPhoneNumber").val()
            jqueryCommand = "$(\"#labelPhoneNumber\").text(value)";
        }


        else if (key == "profilepicture") {
            value = $('.profile-picture').attr('src').split(",")[1];
        }

        updateUser(key, value, currentEvent,jqueryCommand);

    });

    function updateUser(key, value, currentEvnt, jqueryCommand, password = null, confirmPassword = null) {


        var postData = {
            "Key": key,
            "Value": value,
            "Password": password,
            "ConfirmPassword": confirmPassword
        }

        $.ajax({
            url: "/Home/UpdateUserProfile", // Replace with your controller and action
            type: "POST",
            data: postData,
            success: function (success) {
                // Handle the success response
                if (success == "Success") {
                    if (key == "email") {
                        toastr.info("Success! Please verify your email")
                    }
                    else {
                        toastr.success("User Update successfully")
                    }
                    debugger;
                    var cancelButton = (currentEvnt).next('.stossionCancelBtn');
                    cancelButton.trigger("click");
                    eval(jqueryCommand);

                }
                else {
                    var cancelButton = (currentEvnt).next('.stossionCancelBtn');
                    cancelButton.trigger("click");
                    toastr.error(success)
                }
            },
            error: function (error) {
                // Handle errors
                console.error(error);
            }
        });


    }
});