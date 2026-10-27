using SeniorCQCAssignment.Framework.HTTP;
using SeniorCQCAssignment.Tests.Constants;
using SeniorCQCAssignment.Tests.TestData;

namespace SeniorCQCAssignment.Tests.Unit;

public class ApiResponseTests
{
    [TestCaseSource(typeof(HttpStatusCodeData), nameof(HttpStatusCodeData.StatusCodes))]
    [Category(Categories.Unit)]
    public void IsSuccessStatusCode_Only_Says_Yes_To_A_2xx_Answer(HttpStatusCodeCase statusCodeCase)
    {
        var response = new ApiResponse<string>
        {
            StatusCode = statusCodeCase.StatusCode,
            RequestBody = string.Empty,
            ResponseBody = string.Empty
        };

        Assert.That(
            response.IsSuccessStatusCode,
            Is.EqualTo(statusCodeCase.IsSuccessStatusCode),
            $"HTTP {(int)statusCodeCase.StatusCode} came back as {statusCodeCase.Meaning} and was read the wrong way round.");
    }
}