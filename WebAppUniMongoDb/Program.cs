
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAppUniMongoDb.BULogic.BasicAuthentication;
using WebAppUniMongoDb.Model;

namespace WebAppUniMongoDb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddAuthentication()
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>
                ("BasicAuthentication", opt => { });

            builder.Services.AddAuthorizationBuilder()
                .AddPolicy("BasicAuthentication", new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder("BasicAuthentication")
                        .RequireAuthenticatedUser().Build());

            builder.Services.AddCors(opts =>
            {
                opts.AddPolicy("CorsPolicy",
                    builder =>
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            builder.Services.AddControllers();

            builder.Services.AddDbContext<UniversityContext>(
               options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDbConnectionString")));

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.Configure<StudentDbConfig>(builder.Configuration.GetSection("StudentsMongoDb"));
            builder.Services.AddSingleton<StudentDbConfig>();
            builder.Services.Configure<FacultyDbConfig>(builder.Configuration.GetSection("FacultiesMongoDb"));
            builder.Services.AddSingleton<FacultyDbConfig>();
            builder.Services.Configure<ProfessorDbConfig>(builder.Configuration.GetSection("ProfessorMongoDb"));
            builder.Services.AddSingleton<ProfessorDbConfig>();
            builder.Services.Configure<CourseDbConfig>(builder.Configuration.GetSection("CourseMongoDb"));
            builder.Services.AddSingleton<CourseDbConfig>();
            builder.Services.Configure<ExamDbConfig>(builder.Configuration.GetSection("ExamMongoDb"));
            builder.Services.AddSingleton<ExamDbConfig>();

            //Autenticazione Jwt
            JwtSettings jwtSettings = new();
            builder.Configuration.GetSection("JwtSettings").Bind(jwtSettings);
            _ = builder.Services.AddSingleton(jwtSettings);


            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opts =>
                {
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        RequireExpirationTime = true,
                        IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                    };
                });

            var app = builder.Build();
            app.UseCors("CorsPolicy");
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
