using static OnlineShop.Common.Errors.ApiResponseErrors;

namespace OnlineShop.Web.ViewModels
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => BadRequestMessage,
                401 => UnauthorizedMessage,
                404 => ResourceNotFoundMessage,
                500 => InternalServerErrorMessage,
                _ => null
            };;
        }

    }
}
