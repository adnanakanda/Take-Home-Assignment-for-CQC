using SeniorCQCAssignment.Framework.HTTP;
using SeniorCQCAssignment.Tests.Constants;

namespace SeniorCQCAssignment.Tests.Unit;

public class JsonSchemaValidatorTests
{
    private const string Schema = """
    {
      "type": "object",
      "required": ["status", "data"],
      "properties": {
        "status": { "type": "string" },
        "data": {
          "type": "array",
          "items": {
            "type": "object",
            "required": ["name", "price"],
            "properties": {
              "name": { "type": "string" },
              "price": { "type": "number" }
            }
          }
        }
      }
    }
    """;

    [Test]
    [Category(Categories.Unit)]
    public void GetValidationErrors_ReturnsEmptyForValidDocument()
    {
        const string document = """{"status":"success","data":[{"name":"Apple Juice","price":1.99}]}""";

        var mismatches = JsonSchemaValidator.GetValidationErrors(document, Schema);

        Assert.That(mismatches, Is.Empty, $"A document that fits the schema was reported as broken: {string.Join("; ", mismatches)}");
    }

    [Test]
    [Category(Categories.Unit)]
    public void GetValidationErrors_ReturnsErrorsForInvalidType()
    {
        const string document = """{"status":"success","data":[{"name":"Apple Juice","price":"1.99"}]}""";

        var mismatches = JsonSchemaValidator.GetValidationErrors(document, Schema);

        Assert.Multiple(() =>
        {
            Assert.That(mismatches, Is.Not.Empty, "A price that came back as text was accepted as a number.");

            Assert.That(
                mismatches.Any(mismatch => mismatch.Contains("/data/0/price")),
                Is.True,
                $"Nothing in the failure names the field that was wrong: {string.Join("; ", mismatches)}");

            Assert.That(
                mismatches.Any(mismatch => mismatch.Contains("/data/0/price") && mismatch.Contains("number")),
                Is.True,
                $"Nothing in the failure says what was expected of the field: {string.Join("; ", mismatches)}");
        });
    }

    [Test]
    [Category(Categories.Unit)]
    public void GetValidationErrors_ReturnsErrorsForMissingRequiredProperty()
    {
        const string document = """{"status":"success","data":[{"name":"Apple Juice"}]}""";

        var mismatches = JsonSchemaValidator.GetValidationErrors(document, Schema);

        Assert.Multiple(() =>
        {
            Assert.That(mismatches, Is.Not.Empty, "A product without a price was accepted.");

            Assert.That(
                mismatches.Any(mismatch => mismatch.Contains("price")),
                Is.True,
                $"Nothing in the failure names the property that is missing: {string.Join("; ", mismatches)}");
        });
    }

    [Test]
    [Category(Categories.Unit)]
    public void GetValidationErrors_ThrowsForNullOrEmptyDocument()
    {
        Assert.That(
            () => JsonSchemaValidator.GetValidationErrors(string.Empty, Schema),
            Throws.ArgumentException,
            "An empty answer was passed off as one that fits the schema.");
    }
}