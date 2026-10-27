using System.Net;

namespace SeniorCQCAssignment.Tests.TestData;

public sealed record HttpStatusCodeCase(HttpStatusCode StatusCode, bool IsSuccessStatusCode, string Meaning)
{
    public override string ToString() => Meaning;
}