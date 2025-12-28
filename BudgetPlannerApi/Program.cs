using Bpst.API.DB;
using Bpst.API.Services.UserAccount;
using Bpst.API.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text;
using System.Text.Json.Serialization;
using Bpst.API.Repositories;
using Bpst.API.Middleware;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// ✅ Correct CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("*")
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(10));

// ✅ JWT Authentication setup
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true
    };

    x.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                context.Response.Headers["Token-Expired"] = "true";
            return System.Threading.Tasks.Task.CompletedTask;
        }
    };
});

// Database connection (switchable provider)
// Use MySQL (Pomelo) for LiveDB in this branch. Connection string name: "LiveDB"
var liveConn = builder.Configuration.GetConnectionString("LiveDB");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(liveConn, ServerVersion.AutoDetect(liveConn)));
// Register services

builder.Services.AddHttpContextAccessor();  // ✅ this line is missing

// Services registration
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
// Register repository and service
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
// register wish list service with fully-qualified types to avoid namespace lookup issues
builder.Services.AddScoped<Bpst.API.Services.WishLists.IWishListService, Bpst.API.Services.WishLists.WishListService>();
// register income source service
builder.Services.AddScoped<Bpst.API.Services.IncomeSources.IIncomeSourceService, Bpst.API.Services.IncomeSources.IncomeSourceService>();

// Controllers and Swagger config
builder.Services.AddControllers()
 .AddJsonOptions(options =>
  {
      options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
      options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; // instead of Preserve
      options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;

  });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

var app = builder.Build();

// Global exception logging in non-development environments so production exceptions are captured
if (!app.Environment.IsDevelopment())
{
    app.UseGlobalExceptionLogging();
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseDeveloperExceptionPage();


// Swagger setup
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// ✅ Middleware order — very important!
app.UseCors(MyAllowSpecificOrigins);

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
