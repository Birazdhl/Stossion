using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stossion.API.ApplicationService;
using Stossion.API.ExceptionHandler;
using Stossion.BusinessLayers.Interfaces;
using Stossion.BusinessLayers.Services;
using Stossion.DbManagement.StossionDbManagement;
using System;
using System.Text;
using static Stossion.DbManagement.Seeder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Initiliazing the Database
builder.Services.AddDbContext<StossionDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("StossionConnectionString") ??
        throw new InvalidOperationException("Connection String is not found"));
});

builder.Services.AddHttpContextAccessor();
//Adding Authentication
//Identity
builder.Services.AddIdentity<StossionUser, IdentityRole>()
    .AddEntityFrameworkStores<StossionDbContext>()
    .AddSignInManager()
    .AddRoles<IdentityRole>();

// JWT 
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
		.AddJwtBearer(options =>
		{
			options.TokenValidationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidateAudience = true,
				ValidateLifetime = true,
				ValidateIssuerSigningKey = true,
				ClockSkew = TimeSpan.Zero
			};

			options.Events = new JwtBearerEvents
			{
				OnAuthenticationFailed = context =>
				{
					// Handle authentication failure, if needed
					return Task.CompletedTask;
				}
			};
		});


//builder.Services.AddScoped<IUserInterface, UserService>();

builder.Services.AddServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var serviceScope = app.Services.CreateScope())
{
    var context = serviceScope.ServiceProvider.GetRequiredService<StossionDbContext>();
    DbInitializer.Initialize(context);
}
app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
