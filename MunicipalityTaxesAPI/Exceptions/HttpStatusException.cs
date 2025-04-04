using System.Net;

namespace MunicipalityTaxesAPI.Exceptions
{
    public class HttpStatusException : Exception
    {
        public HttpStatusException(HttpStatusCode status) : this(status, status.ToString())
        {
        }

        public HttpStatusException(HttpStatusCode status, string msg) : base(msg)
        {
            Status = status;
        }

        public HttpStatusCode Status { get; }
    }
}
