using System;
using System.Collections.Generic;
using System.Text;

namespace CqrsServices.Validation
{
    /// <summary>
    /// result of a validation
    /// </summary>
    public class ValidationResult
    {
        public bool IsSuccessful { get; set; }=true;
        public string Error { get; set; }
        /// <summary>
        /// method to fast create a validation result success
        /// </summary>
        public static ValidationResult Success => new ValidationResult();
        /// <summary>
        /// method to fast create a validation result success
        /// </summary>
        public static ValidationResult Fail(string error) => new ValidationResult { IsSuccessful = false, Error = error };
    }
}
