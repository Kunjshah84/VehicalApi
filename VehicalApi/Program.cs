using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using VehicalApi.Data;
using VehicalApi.Domain.Auth.Interfaces;
using VehicalApi.Infrastructure.Repositories;
using VehicalApi.Domain.Auth.Services;
using VehicalApi.Business.Auth.Interfaces;
using VehicalApi.Business.Auth.Services;
using VehicalApi.Services.Interfaces;
using VehicalApi.Services.Implementations;
using VehicalApi.Domain.Vehicles.Interfaces;
using VehicalApi.Domain.Vehicles.Services;
using VehicalApi.Business.Vehicles.Interfaces;
using VehicalApi.Business.Vehicles.Services;
using VehicalApi.Domain.Manager.Interfaces;
using VehicalApi.Domain.Manager.Services;
using VehicalApi.Business.Manager.Interfaces;
using VehicalApi.Business.Manager.Services;
using VehicalApi.Services.BookingServiecs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthDomainService, AuthDomainService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleDomainService, VehicleDomainService>();
builder.Services.AddScoped<IVehicleQueryService, VehicleQueryService>();
builder.Services.AddScoped<IManagerRepository, ManagerRepository>();
builder.Services.AddScoped<IManagerDomainService, ManagerDomainService>();
builder.Services.AddScoped<IManagerService, ManagerService>();
builder.Services.AddScoped<RideBookingService>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:4200" , "http://localhost:5001" , "http://vehiclewebcomp.lovestoblog.com")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


// This is the section of the JWT seetiongs 
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"]!);


builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
    db.Database.Migrate();
}


// app.UseHttpsRedirection();
if (app.Environment.IsProduction() || app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseCors("AllowAngular");

app.UseAuthentication(); 
app.UseAuthorization();

app.MapGet("/", () => "After the changin in the code");
app.MapControllers();
app.Run();


    //   "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=VehicleShowroom;Trusted_Connection=True;TrustServerCertificate=True;"
//   "DefaultConnection": "Server=db37270.public.databaseasp.net; Database=db37270; User Id=db37270; Password=4f_C+Hc9N6=h; Encrypt=True; TrustServerCertificate=True;"
