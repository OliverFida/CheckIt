global using awl_raumreservierung.core;
global using awl_raumreservierung.db;
using awl_raumreservierung;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
awl_raumreservierung.core.Globals.AppBuilder = builder;

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<checkITContext>(option => option.UseSqlite($"Datasource={awl_raumreservierung.core.Globals.AppBuilder.Configuration["Database:Path"]}"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
   c.EnableAnnotations();
   var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
   c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(o => {
       o.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
       //o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });
builder.Services
    .AddAuthentication(options => {
       options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
       options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
       options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o => {
       o.TokenValidationParameters = new TokenValidationParameters {
          ValidIssuer = builder.Configuration["Jwt:Issuer"],
          ValidAudience = builder.Configuration["Jwt:Audience"],
          IssuerSigningKey = new SymmetricSecurityKey(
               Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
           ),
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = false,
          ValidateIssuerSigningKey = true
       };
       o.Events = new JwtBearerEvents {
          OnAuthenticationFailed = context => {
             if(context.Exception.GetType() == typeof(SecurityTokenExpiredException)) {
                context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
             }

             return Task.CompletedTask;
          }
       };
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(setup => {
   OpenApiSecurityScheme jwtSecurityScheme = new() {
      BearerFormat = "JWT",
      Name = "JWT Authentification",
      In = ParameterLocation.Header,
      Type = SecuritySchemeType.Http,
      Scheme = JwtBearerDefaults.AuthenticationScheme,
      Description = "Setze hier den JWT Token ein",

      Reference = new OpenApiReference {
         Id = JwtBearerDefaults.AuthenticationScheme,
         Type = ReferenceType.SecurityScheme
      }
   };

   setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

   setup.AddSecurityRequirement(new OpenApiSecurityRequirement
   {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

var app = builder.Build();

using(var scope = app.Services.CreateScope()) {
   var db = scope.ServiceProvider.GetRequiredService<checkITContext>();
   db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment()) {
   app.UseSwagger();
   app.UseSwaggerUI();
}

app.UseCors(
    x =>
        x.AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true) // allow any origin
            .AllowCredentials()
); // allow credentials
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
