using SeniorCQCAssignment.Automation.Pages;

namespace SeniorCQCAssignment.Automation.Steps.Ui;

public class SearchSteps
{
    private readonly SearchPage _searchPage;

    public SearchSteps(SearchPage searchPage)
    {
        _searchPage = searchPage;
    }

    public IReadOnlyList<string> SearchFor(string query)
    {
        _searchPage.SearchFor(query);

        return _searchPage.GetProductNames();
    }
}
