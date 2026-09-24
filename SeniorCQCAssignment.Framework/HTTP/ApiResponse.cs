using System.Net;

namespace SeniorCQCAssignment.Framework.HTTP
{
    public sealed class ApiResponse<T>
    {
        public required HttpStatusCode StatusCode { get; init; }
        public required string RequestBody { get; init; }

        public required string ResponseBody { get; init; }

        public T? Data { get; init; }

        public bool IsSuccessStatusCode => (int)StatusCode is >= 200 and <= 299;
    }
}