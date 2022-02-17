using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace CqrsServices
{
    /// <summary>
    /// base for all responses to get status code and error message
    /// </summary>
    public class CQRSResponse
    {
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string ErrorMessage { get; set; } = null;
        
    }
}
