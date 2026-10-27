using System.Net;

namespace SeniorCQCAssignment.Tests.TestData;

public static class HttpStatusCodeData
{
    public static IEnumerable<HttpStatusCodeCase> StatusCodes =>
    [
        new((HttpStatusCode)199, false, "the last of the 1xx answers"),
        new(HttpStatusCode.OK, true, "the usual answer"),
        new(HttpStatusCode.Created, true, "the answer to a registration"),
        new(HttpStatusCode.NoContent, true, "an answer with nothing in its body"),
        new((HttpStatusCode)299, true, "the last of the 2xx answers"),
        new((HttpStatusCode)300, false, "a redirect, on the first code of the 3xx range"),
        new(HttpStatusCode.MovedPermanently, false, "a redirect"),
        new(HttpStatusCode.BadRequest, false, "the application refusing the call"),
        new(HttpStatusCode.Unauthorized, false, "a call made without a signed in customer"),
        new(HttpStatusCode.InternalServerError, false, "the application breaking")
    ];
}