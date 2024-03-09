
$(document).ready(function () { 
    $("#register").on('click', function () {

        swal({
            title: 'Register',
            html: `
            <form id="registerFrm">

                <div class="form-group d-flex">
                    <div>
                        <input type="text" class="form-control registerInput" placeholder="First Name" name="Firstname" required>
                        <span class="errorTxt" id="firstNameField">This field is required</span>
                    </div>
                    <div>
                        <input type="text" class="form-control mx-4 registerInput" placeholder="Last Name" name="Lastname" required>
                         <span class="errorTxt" style="padding-left:36px;" id="lastNameField">This field is required</span>
                    </div>
                </div>

                <div class="form-group d-flex">
                    <div>
                        <input type="text" class="form-control registerInput" placeholder="Username" name="Username" required>
                        <span class="errorTxt" id="UsernameField">This field is required</span>
                    </div>
                    <div>
                        <input type="text" class="form-control mx-4 registerInput" placeholder="Email" name="Email" required>
                         <span class="errorTxt" style="padding-left:36px;" id="EmailField">This field is required</span>
                    </div>
                </div>

                <div class="form-group d-flex">
                    <div>
                        <input type="password" class="form-control registerInput" placeholder="Password" name="Password" required>
                        <span class="errorTxt" id="PasswordField">This field is required</span>
                    </div>
                    <div>
                         <input type="password" class="form-control mx-4 registerInput" placeholder="Confirm Password" name="ConfirmPassword" required>
                         <span class="errorTxt" style="padding-left:36px;" id="ConfirmPasswordField">This field is required</span>
                    </div>
                </div>

                <div class="form-group d-flex">
                    <div>
                        <input type="text" class="form-control registerInput" placeholder="Country" name="Country" required>
                        <span class="errorTxt" id="CountryField">This field is required</span>
                    </div>
                    <div>
                         <input type="text" class="form-control mx-4 registerInput" placeholder="Birthday" name="Birthday" required>
                         <span class="errorTxt" style="padding-left:36px;" id="BirthdayField">This field is required</span>
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
                        <span class="errorTxt" id="PhoneNumberFeild">This field is required</span>
                    </div>
                     <div>
                          <input type="string" class="form-control mx-4 registerInput" placeholder="PhoneNumber" name="PhoneNumber" required>
                          <span class="errorTxt" style="padding-left:36px;" id="PhoneNumberFeild">This field is required</span>
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

        $("#registerUser").on('click', function () {
            alert("message");
        });
    });

   

});