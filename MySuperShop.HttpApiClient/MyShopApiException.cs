using OnlineShop.HttpModels.Responses;
using System.Net;

namespace OnlineShop.HttpApiClient
{
    public class MyShopApiException : Exception
    {
        public ErrorResponse? Error { get; }
        public ValidationProblemDetails? Details { get; }
        public HttpStatusCode? StatusCode { get; }
       
        public MyShopApiException()
        {
        }

        public MyShopApiException(HttpStatusCode statusCode, ValidationProblemDetails details) : base(details.Title)
        {
            StatusCode = statusCode;
            Details = details;
        }

        public MyShopApiException(ErrorResponse error) : base(error.Message)
        {
            Error = error;
            StatusCode = error.StatusCode!;
        }

        public MyShopApiException(HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
        }

        public MyShopApiException(string? message) : base(message)
        {
        }

        public MyShopApiException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}