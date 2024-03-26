using Stossion.DbManagement.StossionDbManagement;
using Stossion.Domain;
using Stossion.Helpers.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stossion.DbManagement
{
    public class Seeder
    {
        public static class DbInitializer
        {
            public static void Initialize(StossionDbContext context)
            {
                context.Database.EnsureCreated();

                if (!context.Gender.Any())
                {
                    // Add initial data
                    var gender = new List<Domain.Gender>
                        {
                            new Domain.Gender { Type = "Male" },
                            new Domain.Gender { Type = "Female" },
                            new Domain.Gender { Type = "Not Specified" }
                        };

                    context.Gender.AddRange(gender);
                    context.SaveChanges();
                }

                if (!context.Templates.Any())
                {
                    // Add initial data
                    var templates = new List<Templates>
                        {
                            new Templates { Name = StossionConstants.VerifyEmail,
                                Value = "<html> <div> <h3>Stossion Email Verification</h3> </div> <div> <p>Please click the following link to verify your Email Address.</p> <a href=\"@verificationLink\">Verify Email Address for Stossion</a> </div> <br/> <br/> <div>Regards,</div> <div>Stossion Team</div> </html>" },

                            new Templates { Name = StossionConstants.ResetPassword,
                                Value = "<html><div><h3>Password Reset Link</h3></div><div><p>Please click the following link to reset your Password.</p><a href=\"@forgetPasswordLink\">Password Reset Link</a></div><br/> <br/> <div>Regards,</div><div>Stossion Team</div></html>\r\n"},

                             new Templates { Name = StossionConstants.ChangeEmail,
                                Value = "<html><div><h3>Stossion Change Email Confirmation</h3></div><div><p>Please click the following link to confirm to change your Email Address.</p><a href=\"@verificationLink\">Change Email Address for Stossion</a> </div><br/><br/> <div>Regards,</div><div>Stossion Team</div></html>\r\n"}

                        };

                    context.Templates.AddRange(templates);
                    context.SaveChanges();
                }
            }
        }
    }
}
