using Stossion.DbManagement.StossionDbManagement;
using Stossion.Domain;
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
                    var initialData = new List<Gender>
                        {
                            new Gender { Type = "Male" },
                            new Gender { Type = "Female" },
                            new Gender { Type = "Not Specified" }
                        };

                    context.Gender.AddRange(initialData);
                    context.SaveChanges();
                }
            }
        }
    }
}
