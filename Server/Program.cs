global using Microsoft.AspNetCore.Authorization;
global using Shared;
using System.Text;
using DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using NpgsqlTypes;
using Server.Contexts;
using Server.Entities;
using Server.Repositories;
using Shared.Models;
using Swashbuckle.AspNetCore.Filters;


// MUST HAVE IT LIKE THIS FOR NLOG TO RECOGNIZE DOTNET USER-SECRETS INSTEAD OF HARDCODED DELIMIT PLACEHOLDER VALUE FROM APPSETTINGS.JSON

/*

    dotnet ef dbcontext scaffold "Name=ConnectionStrings:PortfolioDB" Npgsql.EntityFrameworkCore.PostgreSQL -t blog -t contacttable -t project_card -t skill -t skill_description -o Entities -c PortfolioDBContext --context-dir Contexts -f

    to test api in swagger:
        run "dotnet watch run" and look at
        http://localhost:5224/swagger/index.html


    to test entire app from blazor wasm:
        run "dotnet watch run --launch-profile https"

bulk insert:
\copy table_name FROM 'path/to/csv_file' WITH (FORMAT csv, HEADER true);

in the PasswordManagerDbContext, make sure this is commented out or removed otherwise the deployed version of this app won't work
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    => optionsBuilder.UseNpgsql("Name=ConnectionStrings:DB_CONN");


*/

// #if DEBUG
//     var logger = LogManager.Setup().LoadConfigurationFromFile("nlog_dev.config").GetCurrentClassLogger();
// #else
//     var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();
// #endif

// try
// {

Run(args);


// }
//  catch (Exception ex)
// {
//     logger.Error(ex, "Stopped program because of exception: " + ex);
//     throw ex;
// }
// finally {
//     LogManager.Shutdown();
// }


static void Run(string[] args)
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    // builder.Logging.ClearProviders();
    // builder.Host.UseNLog();


    builder.Services.AddControllers();
    builder.Services.AddRazorPages();
    builder.Services.AddHttpClient();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });

    builder.Services.AddScoped<IAccountRepository, AccountRepository>();
    // builder.Services.AddScoped<IBlogsDataRepository<BlogDTO>, BlogsDataRepository>();
    // rich text editor support
    builder.Services.AddScoped<IBlogsDataRepository<BlogDTO>, BlogsDataTinymceRepository>();
    builder.Services.AddScoped<IContactsDataRepository<ContactMeDTO>, ContactsDataRepository>();
    builder.Services.AddScoped<IProjectsDataRepository<ProjectCardDTO>, ProjectsDataRepository>();
    builder.Services.AddScoped<ISkillsDataRepository<SkillDTO>, SkillsDataRepository>();

    // add your custom db contexts here
    builder.Services.AddDbContext<PortfolioDBContext>(options =>
    {
        options.UseNpgsql(builder.Environment.IsDevelopment()
                    ?
                        builder.Configuration.GetConnectionString("PortfolioDB")
                    :
                        Environment.GetEnvironmentVariable("PortfolioDB")
                    ).EnableSensitiveDataLogging();
    });

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Environment.IsDevelopment() ? builder.Configuration["Jwt:Issuer"] : Environment.GetEnvironmentVariable("Jwt_Issuer"),
            ValidAudience = builder.Environment.IsDevelopment() ? builder.Configuration["Jwt:Audience"] : Environment.GetEnvironmentVariable("Jwt_Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Environment.IsDevelopment() ? builder.Configuration["Jwt:Key"] : Environment.GetEnvironmentVariable("Jwt_Key")))
        };
    });

    if (!builder.Environment.IsDevelopment())
    {
        var port = Environment.GetEnvironmentVariable("PORT") ?? "8081";
        builder.WebHost.UseUrls($"http://*:{port}");
    }

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseWebAssemblyDebugging();
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseBlazorFrameworkFiles();
    app.UseStaticFiles();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapRazorPages();
    app.MapControllers();
    app.MapFallbackToFile("index.html");


    app.Run();
}