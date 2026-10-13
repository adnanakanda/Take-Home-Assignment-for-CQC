using Json.Schema;
using System.Text.Json;

namespace SeniorCQCAssignment.Framework.HTTP;

public static class JsonSchemaValidator
{
    private static readonly EvaluationOptions EveryMismatch = new()
    {
        OutputFormat = OutputFormat.List
    };

    public static IReadOnlyList<string> GetValidationErrors(string document, string schema)
    {
        if (string.IsNullOrWhiteSpace(document))
        {
            throw new ArgumentException("There is nothing to validate: the document is empty.", nameof(document));
        }

        using var json = JsonDocument.Parse(document);

        var result = JsonSchema.FromText(schema).Evaluate(json.RootElement, EveryMismatch);

        return result.IsValid ? [] : GetMismatches(result);
    }

    private static IReadOnlyList<string> GetMismatches(EvaluationResults result) =>
        result.Details?
            .Where(detail => !detail.IsValid && detail.Errors is { Count: > 0 })
            .SelectMany(detail => detail.Errors!.Values.Select(message => $"{PlaceIn(detail)} {message}"))
            .ToList() ?? [];

    private static string PlaceIn(EvaluationResults detail)
    {
        var place = detail.InstanceLocation.ToString();

        return string.IsNullOrEmpty(place) ? "$" : place;
    }
}