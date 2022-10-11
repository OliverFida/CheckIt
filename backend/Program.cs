using awl_raumreservierung;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using awl_raumreservierung.Controllers;
using System.Reflection;
using Microsoft.OpenApi.Models;
using awl_raumreservierung.core;

var builder = WebApplication.CreateBuilder(args);
loginController.builder = builder;
Globals.AppBuilder = builder;
Globals.DbContext = new checkITContext();

// leere DB inkl Admin User anlegen falls keine existiert
if (!File.Exists(builder.Configuration["Database:Path"]))
{
	string sqlComm = @"
		BEGIN TRANSACTION;
		CREATE TABLE IF NOT EXISTS '__EFMigrationsHistory' (
			'MigrationId'	TEXT NOT NULL,
			'ProductVersion'	TEXT NOT NULL,
			CONSTRAINT 'PK___EFMigrationsHistory' PRIMARY KEY('MigrationId')
		);
		CREATE TABLE IF NOT EXISTS 'AspNetRoles' (
			'Id'	TEXT NOT NULL,
			'Name'	TEXT,
			'NormalizedName'	TEXT,
			'ConcurrencyStamp'	TEXT,
			CONSTRAINT 'PK_AspNetRoles' PRIMARY KEY('Id')
		);
		CREATE TABLE IF NOT EXISTS 'AspNetUsers' (
			'Id'	TEXT NOT NULL,
			'UserName'	TEXT,
			'NormalizedUserName'	TEXT,
			'Email'	TEXT,
			'NormalizedEmail'	TEXT,
			'EmailConfirmed'	INTEGER NOT NULL,
			'PasswordHash'	TEXT,
			'SecurityStamp'	TEXT,
			'ConcurrencyStamp'	TEXT,
			'PhoneNumber'	TEXT,
			'PhoneNumberConfirmed'	INTEGER NOT NULL,
			'TwoFactorEnabled'	INTEGER NOT NULL,
			'LockoutEnd'	TEXT,
			'LockoutEnabled'	INTEGER NOT NULL,
			'AccessFailedCount'	INTEGER NOT NULL,
			CONSTRAINT 'PK_AspNetUsers' PRIMARY KEY('Id')
		);
		CREATE TABLE IF NOT EXISTS 'AspNetRoleClaims' (
			'Id'	INTEGER NOT NULL,
			'RoleId'	TEXT NOT NULL,
			'ClaimType'	TEXT,
			'ClaimValue'	TEXT,
			CONSTRAINT 'PK_AspNetRoleClaims' PRIMARY KEY('Id' AUTOINCREMENT),
			CONSTRAINT 'FK_AspNetRoleClaims_AspNetRoles_RoleId' FOREIGN KEY('RoleId') REFERENCES 'AspNetRoles'('Id') ON DELETE CASCADE
		);
		CREATE TABLE IF NOT EXISTS 'AspNetUserClaims' (
			'Id'	INTEGER NOT NULL,
			'UserId'	TEXT NOT NULL,
			'ClaimType'	TEXT,
			'ClaimValue'	TEXT,
			CONSTRAINT 'PK_AspNetUserClaims' PRIMARY KEY('Id' AUTOINCREMENT),
			CONSTRAINT 'FK_AspNetUserClaims_AspNetUsers_UserId' FOREIGN KEY('UserId') REFERENCES 'AspNetUsers'('Id') ON DELETE CASCADE
		);
		CREATE TABLE IF NOT EXISTS 'AspNetUserLogins' (
			'LoginProvider'	TEXT NOT NULL,
			'ProviderKey'	TEXT NOT NULL,
			'ProviderDisplayName'	TEXT,
			'UserId'	TEXT NOT NULL,
			CONSTRAINT 'PK_AspNetUserLogins' PRIMARY KEY('LoginProvider','ProviderKey'),
			CONSTRAINT 'FK_AspNetUserLogins_AspNetUsers_UserId' FOREIGN KEY('UserId') REFERENCES 'AspNetUsers'('Id') ON DELETE CASCADE
		);
		CREATE TABLE IF NOT EXISTS 'AspNetUserRoles' (
			'UserId'	TEXT NOT NULL,
			'RoleId'	TEXT NOT NULL,
			CONSTRAINT 'PK_AspNetUserRoles' PRIMARY KEY('UserId','RoleId'),
			CONSTRAINT 'FK_AspNetUserRoles_AspNetUsers_UserId' FOREIGN KEY('UserId') REFERENCES 'AspNetUsers'('Id') ON DELETE CASCADE,
			CONSTRAINT 'FK_AspNetUserRoles_AspNetRoles_RoleId' FOREIGN KEY('RoleId') REFERENCES 'AspNetRoles'('Id') ON DELETE CASCADE
		);
		CREATE TABLE IF NOT EXISTS 'AspNetUserTokens' (
			'UserId'	TEXT NOT NULL,
			'LoginProvider'	TEXT NOT NULL,
			'Name'	TEXT NOT NULL,
			'Value'	TEXT,
			CONSTRAINT 'PK_AspNetUserTokens' PRIMARY KEY('UserId','LoginProvider','Name'),
			CONSTRAINT 'FK_AspNetUserTokens_AspNetUsers_UserId' FOREIGN KEY('UserId') REFERENCES 'AspNetUsers'('Id') ON DELETE CASCADE
		);
		CREATE TABLE IF NOT EXISTS 'Room' (
			'ID'	INTEGER NOT NULL UNIQUE,
			'Number'	TEXT NOT NULL,
			'Name'	TEXT NOT NULL,
			'Active'	INTEGER NOT NULL DEFAULT 1,
			PRIMARY KEY('ID' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'Bookings' (
			'ID'	INTEGER NOT NULL UNIQUE,
			'StartTime'	TEXT NOT NULL,
			'EndTime'	TEXT NOT NULL,
			'Room'	INTEGER NOT NULL,
			'UserID'	INTEGER NOT NULL,
			'CreateTime'	TEXT NOT NULL,
			'CreatedBy'	INTEGER NOT NULL,
			'Note'	TEXT,
			PRIMARY KEY('ID' AUTOINCREMENT)
		);
		CREATE TABLE IF NOT EXISTS 'User' (
			'ID'	INTEGER NOT NULL UNIQUE,
			'Username'	TEXT NOT NULL UNIQUE,
			'Firstname'	TEXT,
			'Lastname'	TEXT,
			'Passwd'	TEXT NOT NULL,
			'Active'	INTEGER NOT NULL,
			'Role'	INTEGER NOT NULL,
			'Lastchange'	nvarchar,
			'Lastlogon'	nvarchar,
			PRIMARY KEY('ID' AUTOINCREMENT)
		);
		CREATE INDEX IF NOT EXISTS 'IX_AspNetRoleClaims_RoleId' ON 'AspNetRoleClaims' (
			'RoleId'
		);
		CREATE UNIQUE INDEX IF NOT EXISTS 'RoleNameIndex' ON 'AspNetRoles' (
			'NormalizedName'
		);
		CREATE INDEX IF NOT EXISTS 'IX_AspNetUserClaims_UserId' ON 'AspNetUserClaims' (
			'UserId'
		);
		CREATE INDEX IF NOT EXISTS 'IX_AspNetUserLogins_UserId' ON 'AspNetUserLogins' (
			'UserId'
		);
		CREATE INDEX IF NOT EXISTS 'IX_AspNetUserRoles_RoleId' ON 'AspNetUserRoles' (
			'RoleId'
		);
		CREATE INDEX IF NOT EXISTS 'EmailIndex' ON 'AspNetUsers' (
			'NormalizedEmail'
		);
		CREATE UNIQUE INDEX IF NOT EXISTS 'UserNameIndex' ON 'AspNetUsers' (
			'NormalizedUserName'
		);
		INSERT INTO User (Username, Passwd, Active, Role)
		VALUES ('Admin', 'admin', 1, 1); 
		COMMIT;";

	System.Data.SQLite.SQLiteConnection.CreateFile(builder.Configuration["Database:Path"]);
	System.Data.SQLite.SQLiteConnection dbConn = new System.Data.SQLite.SQLiteConnection($"Datasource={Globals.AppBuilder.Configuration["Database:Path"]}");
	dbConn.Open();

	System.Data.SQLite.SQLiteCommand command = new System.Data.SQLite.SQLiteCommand(sqlComm, dbConn);
	command.ExecuteNonQuery();

	dbConn.Close();
}

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(o =>
    {
        o.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
        //o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
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
				if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
				{
					context.Response.Headers.Add("IS-TOKEN-EXPIRED", "true");
				}
				return Task.CompletedTask;
			}
		};
    });

builder.Services.AddAuthorization();

builder.Services.AddSwaggerGen(setup =>
{
    OpenApiSecurityScheme jwtSecurityScheme = new()
    {
        BearerFormat = "JWT",
        Name = "JWT Authentification",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Setze hier den JWT Token ein",

        Reference = new OpenApiReference
        {
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
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
