using System.Net;

public class BaseExceptionModel : Exception
{
    public string Message { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public string CorrelationId { get; set; }
}
