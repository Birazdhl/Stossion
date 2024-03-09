
$(document).ready(function () { 
    $("#register").on('click', function () {

        swal({
            title: 'Register',
            html: `
            <form id="registerFrm">

                <div class="form-group d-flex">
                    <div>
                        <input type="text" class="form-control registerInput" placeholder="First Name" name="Firstname" required>
                        <span class="errorTxt d-none" id="firstNameField">This field is required</span>
                    </div>
                    <div>
                        <input type="text" class="form-control mx-4 registerInput" placeholder="Last Name" name="Lastname" required>
                         <span class="errorTxt d-none" style="padding-left:36px;" id="lastNameField">This field is required</span>
                    </div>
                </div>

                <div class="form-group d-flex">
                    <div>
                        <input type="text" class="form-control registerInput" placeholder="Username" name="Username" required>
                        <span class="errorTxt d-none" id="UsernameField">This field is required</span>
                    </div>
                    <div>
                        <input type="text" class="form-control mx-4 registerInput" placeholder="Email" name="Email" required>
                         <span class="errorTxt d-none" style="padding-left:36px;" id="EmailField">This field is required</span>
                    </div>
                </div>

                <div class="form-group d-flex">
                    <div>
                        <input type="password" class="form-control registerInput" placeholder="Password" name="Password" required>
                        <span class="errorTxt d-none" id="PasswordField">This field is required</span>
                    </div>
                    <div>
                         <input type="password" class="form-control mx-4 registerInput" placeholder="Confirm Password" name="ConfirmPassword" required>
                         <span class="errorTxt d-none" style="padding-left:36px;" id="ConfirmPasswordField">This field is required</span>
                    </div>
                </div>

                <div class="form-group d-flex">
                    <div>
                        <select class="form-control loginSelect registerInput" name="Country" placeholder="Select a Country" id="Country"></select>
                        <span class="errorTxt d-none" id="CountryField">This field is required</span>
                    </div>
                    <div>
                         <input type="text" class="form-control mx-4 registerInput" placeholder="Birthday" id="Birthday" name="Birthday" required>
                         <span class="errorTxt d-none" style="padding-left:36px;" id="BirthdayField">This field is required</span>
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
                        <span class="errorTxt d-none" id="Gender">This field is required</span>
                    </div>
                     <div>
                          <input type="string" class="form-control mx-4 registerInput" placeholder="PhoneNumber" name="PhoneNumber" required>
                          <span class="errorTxt d-none" style="padding-left:36px;" id="PhoneNumberFeild">This field is required</span>
                    </div>
                </div>

                <div class="swal2-actions d-flex">
                    <button type="button" class="swal2-cancel btn btn-secondary mx-2">Cancel</button>
                    <button type="button" class="swal2-confirm btn btn-primary d-flex" id="registerUser">Register</button>
                </div>
            </form>
        `,
            showCancelButton: false,
            showCloseButton: true,
            showConfirmButton: false
        });
        $(".swal2-modal").css('background-color', '#000');//Optional changes the color of the sweetalert 
        $(".swal2-modal").css('width', 'fit-content');//Optional changes the color of the sweetalert 
        $(".swal2-modal").css('margin-top', '2rem');
        $(".swal2-container.in").css('background-color', 'rgba(43, 165, 137, 0.45)');//changes the color of the overlay
        $("#Birthday").datepicker();

        $.ajax({
            url: "/Common/GetCountryList", // Replace with your controller and action
            type: "GET",
            dataType: "json",
            success: function (data) {
                // Handle the success response
                if (data.length > 0) {
                    var dropdown = $("#Country");

                    // Iterate through the data and append options to the dropdown
                    $.each(data, function (index, country) {
                        var optionText = '<img src="' + country.logo + '" alt="' + encodeURIComponent(country.name) + '" class="country-flag" /> ' + country.name;
                        var optionValue = country.symbol; // You can change this to country.id if needed
                        dropdown.append($("<option class=\"loginOption\">").val(optionValue).html(optionText));
                    });
                }
            },
            error: function (error) {
                // Handle errors
                console.error(error);
            }
        });

        $("#registerUser").on('click', function () {
            var form = $("#registerFrm");
            if (!form.valid()) {

            }
        });
    });

   

});

//<input type="text" class="form-control registerInput" placeholder="Country" name="Country" required>
