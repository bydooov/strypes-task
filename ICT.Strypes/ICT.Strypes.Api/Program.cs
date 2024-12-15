using FluentValidation;
using FluentValidation.AspNetCore;
using ICT.Strypes.Api.Filters;
using ICT.Strypes.Business.Interfaces;
using ICT.Strypes.Business.Services;
using ICT.Strypes.Business.Validators;
using ICT.Strypes.Infrastructure.Data;
using ICT.Strypes.Infrastructure.Interfaces;
using ICT.Strypes.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ICT.Strypes.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(new GlobalExceptionFilter());
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationDbContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddTransient<IChargePointRepository, ChargePointRepository>();
            builder.Services.AddTransient<ILocationRepository, LocationRepository>();
            builder.Services.AddTransient<ILocationService, LocationService>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddFluentValidationAutoValidation();
            builder.Services.AddFluentValidationClientsideAdapters();

            builder.Services.AddValidatorsFromAssemblyContaining<LocationRequestModelValidator>();

            var app = builder.Build();

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
