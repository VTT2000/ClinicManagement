using System.Net;
using System.Security.Claims;
using System.Text;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using web_api_base.Models.ClinicManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

// Thêm dịch vụ cho Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Đọc connection string từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
//Kết nối db
builder.Services.AddDbContext<ClinicContext>(options => options.UseLazyLoadingProxies(false).UseSqlServer(connectionString));


builder.Services.AddAuthorization();


builder.Services.AddHttpClient();
//Add blazor storage
// builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredLocalStorage();

//Sử dụng httpcontext từ blazor server
builder.Services.AddHttpContextAccessor();

//Repository pattern & unit of work pattern
//repo
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
//unit
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
//service
builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();




//blazor    
app.UseStaticFiles();
app.UseRouting();
//Phân quyền
app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub(); // SignalR hub cho Blazor Server
app.MapFallbackToPage("/_Host"); // Trang mặc định cho Blazor

app.Run();


