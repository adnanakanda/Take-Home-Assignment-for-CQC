namespace SeniorCQCAssignment.Tests.TestData;

public static class ProductSearchData
{
    public static IEnumerable<ProductSearchCase> Queries =>
    [
        new("Apple", "Apple Juice"),
        new("Orange", "Orange Juice"),
        new("Juice", "Juice")
    ];
}