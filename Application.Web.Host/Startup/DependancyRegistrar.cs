using Application.Service.PdfHandler;
using Application.Service.Setting;
using Application.Services.AppUser;
using Application.Services.Orders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;

namespace Application
{
    public static class DependancyRegistrar
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
          
            //DI 
            services.AddTransient<IAppUsersService, AppUsersService>();

            services.AddTransient<IPdfHandlerService, PdfHandlerService>();
            services.AddTransient<IFileProvider, PhysicalFileProvider>(); 
            services.AddTransient<ISettingService, SettingService>(); 
            services.AddTransient<IFileProcessedLogService, FileProcessedLogService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ITagService, TagService>();
             
        } 
    }
     
}
