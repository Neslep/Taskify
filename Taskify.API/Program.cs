using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Taskify.API.Configurations;
using Taskify.API.Data;
using Taskify.API.Services.Filters;
using Taskify.API.Services.Repositories.IRepositories;
using Taskify.API.Services.Repositories.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Lấy cấu hình JWT
var jwtSettings = builder.Configuration.GetSection("JWT");

// Lấy connection string
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Thiết lập secret JWT
JwtConfig.SetSecret(jwtSettings);

// Thêm cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.WithOrigins("https://taskifyvn.vercel.app") // Địa chỉ frontend
              .AllowAnyHeader()
              .AllowAnyMethod()
        // Nếu cần cookie hay thông tin đăng nhập:
              .AllowCredentials();
    });
});

// Thêm controllers + custom exception filter
builder.Services.AddControllers(cfg =>
{
    cfg.Filters.Add(typeof(ExceptionFilter));
});

// Tạo endpoint explorer
builder.Services.AddEndpointsApiExplorer();

// Cấu hình Swagger + Authen trong Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

// Cấu hình JWT
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["ValidIssuer"],
        ValidAudience = jwtSettings["ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConfig.secret))
    };
});

// Kết nối SQL Server
builder.Services.AddDbContext<TaskifyContext>(options =>
    options.UseSqlServer(connectionString));

// Đăng ký các Repository
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Hiển thị Swagger khi ở môi trường Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Bắt buộc dùng HTTPS
app.UseHttpsRedirection();

// Kích hoạt CORS (phải gọi trước khi map controllers)
app.UseCors("AllowClient");

// Kích hoạt xác thực
app.UseAuthentication();

// Kích hoạt xác thực quyền
app.UseAuthorization();

// Định tuyến controller
app.MapControllers();

// Chạy ứng dụng
app.Run();
