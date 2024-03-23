using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.Helpers.Enum
{
	public static class StossionConstants
	{
		public static string unverifiedEmail = "UnverifiedEmail";
		public static string success = "Success";
		public static string emailVerificationLink = "Email Verification Link";
        public static string forgetPasswordLink = "Password Reset Link";
        public static string emptyModel = "Model is Empty";
		public static string internalServerError = "Internal Server Error";
		public static string invalidParameter = "Invalid Parameter";
		public static string noContent = "No Content";
		public static string invalidUsername = "Invalid Username";
        public static string invalidEmail = "Invalid Email";






        #region Templates Name

        public static string VerifyEmail = "VerifyEmail";
		public static string ResetPassword = "ResetPassword";
        public static string ChangeEmail = "ChangeEmail";
        #endregion
    }
}
