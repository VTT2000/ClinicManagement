using System.Net;
using System.Security.Claims;
using System.Text;
using Blazored.LocalStorage;
using Blazored.Toast;
using web_api_base.Service_FE;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Microsoft.OpenApi.Models;
using web_api_base.Models.ClinicManagement;

using web_api_base.Service_FE.Services;
using web_api_base.Models.Configuration;
using Microsoft.AspNetCore.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
    // 🔥 Thêm hỗ trợ Authorization header tất cả api
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token vào ô bên dưới theo định dạng: Bearer {token}"
    });

    // 🔥 Định nghĩa yêu cầu sử dụng Authorization trên từng api
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.AddControllers();

// Thêm dịch vụ cho Blazor Server
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


// Đọc connection string từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("ConnectionString0");
//Kết nối db
builder.Services.AddDbContext<ClinicContext>(options => options.UseLazyLoadingProxies(false).UseSqlServer(connectionString));

//Thêm middleware authentication
var privateKey = builder.Configuration["jwt:Serect-Key"];
var Issuer = builder.Configuration["jwt:Issuer"];
var Audience = builder.Configuration["jwt:Audience"];
// Thêm dịch vụ Authentication vào ứng dụng, sử dụng JWT Bearer làm phương thức xác thực
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    // Thiết lập các tham số xác thực token
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        // Kiểm tra và xác nhận Issuer (nguồn phát hành token)
        ValidateIssuer = true,
        ValidIssuer = Issuer, // Biến `Issuer` chứa giá trị của Issuer hợp lệ
                              // Kiểm tra và xác nhận Audience (đối tượng nhận token)
        ValidateAudience = true,
        ValidAudience = Audience, // Biến `Audience` chứa giá trị của Audience hợp lệ
                                  // Kiểm tra và xác nhận khóa bí mật được sử dụng để ký token
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKey)),
        // Sử dụng khóa bí mật (`privateKey`) để tạo SymmetricSecurityKey nhằm xác thực chữ ký của token
        // Giảm độ trễ (skew time) của token xuống 0, đảm bảo token hết hạn chính xác
        ClockSkew = TimeSpan.Zero,
        // Xác định claim chứa vai trò của user (để phân quyền)
        RoleClaimType = ClaimTypes.Role,
        // Xác định claim chứa tên của user
        NameClaimType = ClaimTypes.Name,
        // Kiểm tra thời gian hết hạn của token, không cho phép sử dụng token hết hạn
        ValidateLifetime = true
    };
        
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Cho phép token từ query string nếu là WebSocket (SignalR)
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;
            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});
// Thêm dịch vụ Authorization để hỗ trợ phân quyền người dùng
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// Cấu hình HttpClient để gọi API trong cùng project
// builder.Services.AddHttpClient("LocalApi", client =>
// {
//     client.BaseAddress = new Uri("http://localhost:5208"); // Đặt URL cơ sở của API
// });

builder.Services.AddHttpClient("LocalApi", client =>
{
    var config = builder.Configuration.GetSection("ApiSettings");
    var apiBaseUrl = config.GetValue<string>("ApiBaseUrl");

    if (string.IsNullOrWhiteSpace(apiBaseUrl))
        throw new Exception("Missing ApiBaseUrl in configuration.");

    client.BaseAddress = new Uri(apiBaseUrl);
});

// Đọc cấu hình API settings
// builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));


// Cấu hình State Management và Custom Auth Provider

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
//Add blazor storage
// builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredLocalStorage();

//Sử dụng httpcontext từ blazor server
builder.Services.AddHttpContextAccessor();
//DI Service JWT
builder.Services.AddScoped<JwtAuthService>();
// Thêm dịch vụ Authorization để hỗ trợ phân quyền người dùng
builder.Services.AddAuthorization();

//Repository pattern & unit of work pattern
//repo
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IWorkScheduleRepository, WorkScheduleRepository>();
builder.Services.AddScoped<IDiagnosisRepository, DiagnosisRepository>();
builder.Services.AddScoped<IDiagnosisServiceRepository, DiagnosisServiceRepository>();
builder.Services.AddScoped<IPrescriptionRepository, PrescriptionRepository>();
builder.Services.AddScoped<IPrescriptionDetailRepository, PrescriptionDetailRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IMedicineRepository, MedicineRepository>();
builder.Services.AddScoped<IRoomRepository, RoomRepository>();
builder.Services.AddScoped<IDiagnosisServiceRepository, DiagnosisServiceRepository>();
//unit
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IWorkScheduleService, WorkScheduleService>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IDiagnosisServiceBE, DiagnosisServiceBE>();
builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IMedicineService, MedicineService>();
builder.Services.AddScoped<IRoomService, RoomService>();
builder.Services.AddScoped<IDiagnosisServiceService, DiagnosisServiceService>();

//service FE
builder.Services.AddScoped<ILoginService, LoginService>();

//service FE
builder.Services.AddScoped<ReceptionistService>();
builder.Services.AddScoped<DoctorFEService>();
builder.Services.AddScoped<RoomServiceFE>();
builder.Services.AddScoped<TechnicianService>();
builder.Services.AddScoped<ProfileService>();
builder.Services.AddBlazoredToast();

builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

// Phục vụ file tĩnh từ wwwroot (mặc định)
app.UseStaticFiles();

// Cấu hình phục vụ file từ uploads
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")),
    RequestPath = "/images" // Đường dẫn này sẽ tương ứng với thư mục wwwroot/images
});

app.UseRouting();

//Phân quyền
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


app.MapBlazorHub(); // SignalR hub cho Blazor Server
app.MapFallbackToPage("/_Host"); // Trang mặc định cho Blazor

app.MapHub<ServiceHub>("/hub/service"); // Đăng ký route cho Hub

app.Run();


