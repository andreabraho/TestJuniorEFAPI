using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace CqrsServices.Validation
{
    public static class ValidationExtensions
    {
        public static void AddValidators(this IServiceCollection services)
        {
            services.Scan(scan => scan//scan from scrutor library
              .FromAssemblyOf<IValidationHandler>()//scan from assembnly of the IVALIDATIONHANDLER
                .AddClasses(classes => classes.AssignableTo<IValidationHandler>())//add classes assignable to IVALIDATIONHANDLER 
                  .AsImplementedInterfaces()//this registers the IValitationHandler as a generic type not as the concrete version
                  .WithTransientLifetime());
        }
    }
}
