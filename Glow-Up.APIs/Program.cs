
using Glow_Up.APIs.Extensions;
using Glow_Up.APIs.Hubs;
using Glow_Up.Core.Mappers;
using Glow_Up.Repositories._Data;
using Microsoft.EntityFrameworkCore;

namespace Glow_Up.APIs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddApplicationServices(builder.Configuration);

            builder.Services.AddIdentityServices(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                    // Do NOT use .AllowCredentials() with AllowAnyOrigin()
                });
            });

            builder.Services.AddSignalR();

            builder.Services.AddAutoMapper(typeof(NotificationProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            //if (app.Environment.IsDevelopment())
            //{
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notificationHub");
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
