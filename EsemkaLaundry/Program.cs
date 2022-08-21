global using Microsoft.OpenApi.Models;
global using EsemkaLaundry.Models;
global using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<EsemkaLaundryContext>(s => s.UseSqlServer(builder.Configuration.GetConnectionString("db")));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo { Title = "EsemkaLaundry", Version = "v1" });
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n" +
        "Enter your Token in the text input below. \r\n\r\n  Example : eyabcd123.",
        Type = SecuritySchemeType.Http,
        In = ParameterLocation.Header,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },new string []{}
        }
    });
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(s =>
{
    s.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(/*s => { s.SwaggerEndpoint("swagger/v1/swagger.json", "EsemkaLaundry"); }*/);
}
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
