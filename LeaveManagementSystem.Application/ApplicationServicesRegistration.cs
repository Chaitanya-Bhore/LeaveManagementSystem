using LeaveManagementSystem.Application.Services.Email;
using LeaveManagementSystem.Application.Services.LeaveAllocation;
using LeaveManagementSystem.Application.Services.LeaveRequests;
using LeaveManagementSystem.Application.Services.LeaveTypes;
using LeaveManagementSystem.Application.Services.Periods;
using LeaveManagementSystem.Application.Services.Users;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace LeaveManagementSystem.Application
{
    public static class ApplicationServicesRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, Assembly.GetExecutingAssembly());


            services.AddScoped<ILeaveTypesService, LeaveTypesService>();
            
            services.AddScoped<ILeaveAllocationService, LeaveAllocationsService>();
            
            services.AddScoped<ILeaveRequestsService, LeaveRequestsService>();
            
            services.AddTransient<IEmailSender, EmailSender>();
            
            services.AddScoped<IPeriodsService, PeriodsService>();
            
            services.AddScoped<IUserService, UserService>();
            return services;
        }
    }
}
