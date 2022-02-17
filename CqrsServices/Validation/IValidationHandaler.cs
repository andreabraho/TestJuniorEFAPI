using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CqrsServices.Validation
{
    /// <summary>
    /// defines validate methods
    /// </summary>
    public interface IValidationHandler { }

    public interface IValidationHandler<T> : IValidationHandler
    {
        Task<ValidationResult> Validate(T request);
    }
}
