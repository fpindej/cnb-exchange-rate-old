using System.Net;

namespace Logging.Utils;

public static class LoggingHelper
{
    public static BaseExceptionModel CreateBaseExceptionModel(string message, int statusCode, string correlationId)
    {
        return new BaseExceptionModel
        {
            Message = message,
            StatusCode = (HttpStatusCode)statusCode,
            CorrelationId = correlationId
        };
    }
}
