using Assiginment.Models;
using Assiginment.Services;
using ContactsApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Jose;
//var builder i am using this to chech the commit
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Register your services
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILeaveService, LeaveService>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Register DbContext with connection string
builder.Services.AddDbContext<DevContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Attendance API",
        Version = "v1"
    });

    // Add Bearer token support
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' followed by space and your JWT token.\nExample: Bearer abc123..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Optional: Configure Email service (if needed)
//builder.Services.AddScoped<IEmailService, EmailService>();
//builder.Services.AddTransient<SmtpClient>(sp => new SmtpClient("smtp.gmail.com")
//{
//    Port = 587,
//    Credentials = new NetworkCredential("your-email", "your-app-password"),
//    EnableSsl = true
//});

// Enable Authorization (for [Authorize] attributes)
builder.Services.AddAuthorization();

// Enable CORS (Angular or frontend support)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policyBuilder => policyBuilder
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod());
});



//Login Service//
var jwtSection = builder.Configuration.GetSection("JwtSettings");
builder.Services.Configure<JwtSettings>(jwtSection);

var secretKey = jwtSection["SecretKey"];
if (string.IsNullOrEmpty(secretKey))
    throw new Exception("JWT SecretKey is missing in configuration");

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateLifetime = true
        };
    });


var app = builder.Build();

// Global error handling middleware
app.UseMiddleware<ErrorHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS before routing
app.UseCors("AllowSpecificOrigin");

app.UseAuthentication();

app.UseAuthorization();

// Map controller routes
app.MapControllers();

app.Run();
