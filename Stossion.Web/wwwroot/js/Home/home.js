
$(document).ready(function () {

    $("#changePasswordButton").on("click", function () {

        var valid = true;

        if (!$('#oldPassword').val() || !$('#newPassword').val() || !$('#confirmNewPassowrd').val()) {
            valid = false;
            return false;
        }

        var regex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#^()])[A-Za-z\d@$!%*?&#^()]{8,}$/;
        if (!regex.test($('#newPassword').val()) ||
            !regex.test($('#confirmNewPassowrd').val())) {
            valid = false;
            return false;
        } 

        if ($('#newPassword').val() != $('#confirmNewPassowrd').val()) {
            toggleErrorMessage($("#confirmNewPassowrd"), "Password dosen't match");
            valid = false;
            return false;
        }

        if (valid) {

            $(".loading").fadeIn();
            var postData = {
                "OldPassword": $("#oldPassword").val(),
                "NewPassword": $("#newPassword").val(),
                "ConfirmNewPassowrd": $("#confirmNewPassowrd").val()
            }

            $.ajax({
                url: "/Login/ChangePassword", // Replace with your controller and action
                type: "POST",
                data: postData,
                success: function (data) {
                    // Handle the success response
                    if (data == "Success") {
                        toastr.success("Password Changed Successfully!!!")
                        setTimeout(function () {
                            window.location.href = "/Login/Index";
                        }, 2000);
                    }
                    else {
                        toastr.error(success)
                    }
                    $(".loading").fadeOut();
                },
                error: function (error) {
                    toastr.error("Error")
                    // Handle errors
                    console.error(error);
                }
            });
        }

        
    }); 


});