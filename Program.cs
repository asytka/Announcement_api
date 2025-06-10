using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Announcement_api.Data;
namespace Announcement_api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<Announcement_apiContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Announcement_apiContext"),

            sqlOptions => sqlOptions.EnableRetryOnFailure()));
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "My Cool API",
                    Version = "v1"
                });
            });           // adds Swagger

                        builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();
            app.UseCors();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
