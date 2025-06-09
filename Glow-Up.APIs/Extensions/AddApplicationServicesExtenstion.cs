using Glow_Up.APIs.Hubs;
using Glow_Up.Core.Repositories;
using Glow_Up.Core.Services.BlackHat;
using Glow_Up.Core.Services.Comment;
using Glow_Up.Core.Services.Files;
using Glow_Up.Core.Services.Messages;
using Glow_Up.Core.Services.Notifications;
using Glow_Up.Core.Services.Posts;
using Glow_Up.Core.Services.Users;
using Glow_Up.Repositories._Data;
using Glow_Up.Repositories._Identity;
using Glow_Up.Repositories.Repositories;
using Glow_Up.Services.BlackHat;
using Glow_Up.Services.Comments;
using Glow_Up.Services.Files;
using Glow_Up.Services.Messages;
using Glow_Up.Services.Notifications;
using Glow_Up.Services.Posts;
using Glow_Up.Services.Users;
using Microsoft.EntityFrameworkCore;

namespace Glow_Up.APIs.Extensions
{
    public static class AddApplicationServicesExtenstion
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection Services, IConfiguration configuration)
        {
            Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            Services.AddScoped<IUnitOfWork, UnitOfWork>();


            Services.AddScoped<IFileUploadService, FileUploadService>();
            Services.AddScoped<IPostService, PostService>();
            Services.AddScoped<ICommentService, CommentService>();

            Services.AddScoped<IUserService, UserService>();
            Services.AddScoped<IBlackHatService, BlackHatService>();

            Services.AddScoped<INotificationService, NotificationService>();
            Services.AddScoped<INotificationPublisher, NotificationPublisher>();

            Services.AddScoped<IMessageService, MessageService>();
            Services.AddScoped<IMessagePublisher, MessagePublisher>();

            return Services;
        }
    }
}
