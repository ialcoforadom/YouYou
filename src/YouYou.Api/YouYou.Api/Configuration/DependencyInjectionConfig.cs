﻿using YouYou.Api.Extensions;
using YouYou.Business.ErrorNotifications;
using YouYou.Business.Interfaces;
using YouYou.Business.Interfaces.BackOfficeUsers;
using YouYou.Business.Interfaces.ExtraPhones;
using YouYou.Business.Interfaces.PhysicalPersons;
using YouYou.Business.Interfaces.Users;
using YouYou.Business.Services;
using YouYou.Data.Context;
using YouYou.Data.Repository;

namespace YouYou.Api.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<YouYouContext>();

            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            services.AddScoped<IErrorNotifier, ErrorNotifier>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IEmailSettings, EmailSettings>();
            services.AddScoped<IUser, AspNetUser>();
            services.AddScoped<ILogExceptionRepository, LogExceptionRepository>();

            //services.AddTransient<IEmailSender, EmailSender>();

            services.AddScoped<IBackOfficeUserRepository, BackOfficeUserRepository>();
            services.AddScoped<IBackOfficeUserService, BackOfficeUserService>();

            services.AddScoped<IPhysicalPersonRepository, PhysicalPersonRepository>();
            services.AddScoped<IPhysicalPersonService, PhysicalPersonService>();

            //services.AddScoped<ITranshipmentRepository, TranshipmentRepository>();
            //services.AddScoped<IManageTranshipmentService, ManageTranshipmentService>();
            //services.AddScoped<ITranshipmentPhysicalPersonService, TranshipmentPhysicalPersonService>();
            //services.AddScoped<ITranshipmentJuridicalPersonService, TranshipmentJuridicalPersonService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IExtraPhoneRepository, ExtraPhoneRepository>();
            services.AddScoped<IExtraPhoneService, ExtraPhoneService>();

            return services;
        }
    }
}
