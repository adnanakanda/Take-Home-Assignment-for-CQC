# Senior CQC Automation Assignment - OWASP Juice Shop

This project is a test automation framework for [OWASP Juice Shop](https://owasp.org/www-project-juice-shop/) v17.1.1, built as a senior QA take-home assignment. It demonstrates a layered, maintainable automation architecture with API, UI, E2E, and unit tests, fully containerized with Docker and integrated with GitHub Actions CI.

## Table of Contents

- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Quick Start (Docker - Recommended)](#quick-start-docker---recommended)
- [Local Development Setup](#local-development-setup)
- [Running Tests](#running-tests)
- [Project Structure](#project-structure)
- [How to Add Tests](#how-to-add-tests)
- [Test Categories & Tags](#test-categories--tags)
- [Reporting](#reporting)
- [CI/CD Pipeline](#cicd-pipeline)
- [Known Defects](#known-defects)
- [Design Decisions & Trade-offs](#design-decisions--trade-offs)
- [Tech Stack](#tech-stack)

## Architecture

The framework follows a strict layered architecture with clear separation of concerns:

```
Tests (NUnit) → Business Steps → API Clients / Page Objects → Infrastructure (HTTP, WebDriver, Config, Logging)
```

### Key Principles

- **Single Responsibility**: Each class has one clear purpose
- **No Sleeps**: Only explicit waits (`WebDriverWait`) are used
- **No Shared State**: Tests run in parallel with unique test data per test
- **Layered Dependencies**: Only Tests project references NUnit; assertions stay in test layer
- **Explicit Waits**: `Open()`/`GoTo()` only navigates, separate `WaitUntilDisplayed()` called by tests
- **Logging**: All actions and assertions are logged with sensitive data masked

## Prerequisites

### For Docker (Recommended)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) 29+ with Docker Compose v5+

### For Local Development
- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Google Chrome](https://www.google.com/chrome/) (for UI tests)
- [OWASP Juice Shop](https://github.com/juice-shop/juice-shop) running locally or accessible via URL

## Quick Start (Docker - Recommended)

The entire suite runs with a single command - no local .NET or browser installation required.

```bash
# Clone the repository
git clone <repository-url>
cd Git

# Run all tests (builds images, starts SUT with health check, runs tests)
docker compose up --build --exit-code-from tests
```

### Run Specific Test Suites

```bash
# Run only API tests
docker compose --profile api up --build --exit-code-from tests-api

# Run only UI tests
docker compose --profile ui up --build --exit-code-from tests-ui

# Run only framework unit tests (no SUT required)
docker compose --profile unit up --build --exit-code-from tests-unit
```

### Run with Custom Filter

```bash
# Filter by test name
TEST_FILTER="FullyQualifiedName~Search_Product" docker compose up --build --exit-code-from tests
```

Test results will be available in `./TestResults/` and `./allure-results/` on your host machine.

## Local Development Setup

1. **Start OWASP Juice Shop**
   ```bash
   docker run -d -p 3000:3000 bkimminich/juice-shop:v17.1.1
   ```

2. **Configure settings** (optional - defaults are in `appsettings.json`)
   - Edit `SeniorCQCAssignment.Framework/Configuration/appsettings.json` for local settings
   - Or use environment variables: `TestSettings__BaseUrl`, `TestSettings__Browser`, etc.

3. **Build the solution**
   ```bash
   dotnet build SeniorCQCAssignment.slnx
   ```

4. **Run tests**
   ```bash
   dotnet test SeniorCQCAssignment.slnx
   ```

## Running Tests

### By Category

```bash
# Unit tests only
dotnet test SeniorCQCAssignment.slnx --filter "Category=unit"

# API tests only
dotnet test SeniorCQCAssignment.slnx --filter "Category=api"

# UI tests only
dotnet test SeniorCQCAssignment.slnx --filter "Category=ui"

# Smoke tests
dotnet test SeniorCQCAssignment.slnx --filter "Category=smoke"

# Regression tests
dotnet test SeniorCQCAssignment.slnx --filter "Category=regression"
```

### With Code Coverage (optional)

```bash
dotnet test SeniorCQCAssignment.slnx --collect:"XPlat Code Coverage"
```

### Linting

```bash
# Check formatting
dotnet format SeniorCQCAssignment.slnx --verify-no-changes --no-restore

# Auto-fix formatting
dotnet format SeniorCQCAssignment.slnx --no-restore
```

## Project Structure

```
.
├── .github/workflows/ci.yml      # GitHub Actions CI pipeline
├── Dockerfile                    # Multi-stage Docker build with Chrome
├── docker-compose.yml            # Orchestration with SUT health checks
├── Directory.Build.props         # Common build props (analyzers enabled)
├── Directory.Packages.props      # Central package management
├── .editorconfig                 # Code style rules
├── README.md                     # This file
├── STRATEGY.md                   # Test strategy documentation
├── BUGS.md                       # Known defects in SUT
│
├── SeniorCQCAssignment.Framework/    # Core framework
│   ├── Configuration/               # Config loading & validation
│   ├── Elements/                    # Web elements (TextBox, Button, Dropdown, etc.)
│   ├── HTTP/                        # HTTP client, API response, JSON schema validator
│   ├── Logging/                     # Logging abstraction
│   └── WebDriver/                   # WebDriver factory, waits, navigator
│
├── SeniorCQCAssignment.Automation/   # Business logic layer
│   ├── ApiClients/                  # Feature-specific API clients
│   ├── Models/                      # Domain, API request/response models
│   ├── Pages/                       # Page Objects with locators
│   ├── Steps/                       # Business step implementations
│   └── TestData/                    # Test data factories
│
└── SeniorCQCAssignment.Tests/        # Test layer (NUnit only here)
    ├── Api/                         # API tests
    ├── Ui/                          # UI tests
    ├── E2E/                         # End-to-end tests
    ├── Unit/                        # Framework unit tests
    ├── Fixtures/                    # Test bases, contexts
    ├── Constants/                   # Categories, constants
    └── Schemas/                     # JSON schemas for validation
```

## How to Add Tests

### Adding an API Test

1. Create/use API client in `SeniorCQCAssignment.Automation/ApiClients/`
2. Add request/response models in `SeniorCQCAssignment.Automation/Models/`
3. Create test in `SeniorCQCAssignment.Tests/Api/` inheriting from `ApiTestBase`
4. Add appropriate categories `[Category(Categories.Api)]`, `[Category(Categories.Smoke)]` etc.

```csharp
public class MyApiTests : ApiTestBase
{
    [Test]
    [Category(Categories.Api)]
    [Category(Categories.Regression)]
    public async Task GetSomething_ReturnsExpectedData()
    {
        var response = await Api.MyClient.GetSomethingAsync();
        Assert.That(response.IsSuccessStatusCode, Is.True);
    }
}
```

### Adding a UI Test

1. Create Page Object in `SeniorCQCAssignment.Automation/Pages/` with a corresponding Locators class
2. Add to `Pages/Components/` if it's a reusable component
3. Create test in `SeniorCQCAssignment.Tests/Ui/` inheriting from `UiTestBase`
4. Always call `WaitUntilDisplayed()` after navigation, use explicit waits only
5. UI tests must register customers through the UI (see existing patterns)

```csharp
public class MyUiTests : UiTestBase
{
    [Test]
    [Category(Categories.Ui)]
    [Category(Categories.Smoke)]
    public void DoSomething_ThroughUi()
    {
        // Register user through UI
        var user = UserFactory.Create();
        RegistrationPage.GoTo();
        RegistrationPage.WaitUntilDisplayed();
        RegistrationPage.Register(user);
        
        // Continue with test...
    }
}
```

### Adding a Page Object

```csharp
public class MyPage : BasePage
{
    private MyPageLocators Locators => new(Driver);
    
    public MyPage(IWebDriver driver, TimeSpan timeout, ILogger logger, ElementFactory elements, NavigationBar header, Navigator navigator)
        : base(driver, timeout, logger, elements, header, navigator)
    {
    }
    
    public void GoTo() => Navigator.GoToPage("/#/my-route");
    
    public override void WaitUntilDisplayed() => Wait.Until(d => Elements.Button(Locators.SubmitButton, "Submit").IsDisplayed);
    
    public void DoAction() => Elements.Button(Locators.SubmitButton, "Submit").Click();
}
```

### Adding Framework Unit Tests

Add to `SeniorCQCAssignment.Tests/Unit/` inheriting from nothing or as appropriate. Follow the naming convention: `MethodName_ExpectedBehavior`.

## Test Categories & Tags

| Category | Purpose | Usage |
| :-- | :-- | :-- |
| `Api` | API-level tests | All tests in `Tests/Api/` |
| `Ui` | UI-level tests | All tests in `Tests/Ui/` |
| `Unit` | Framework unit tests | All tests in `Tests/Unit/` |
| `Smoke` | Critical path tests | Fast, essential tests for quick feedback |
| `Regression` | Full regression coverage | Comprehensive tests run nightly |
| `KnownDefect` | Tests for known bugs | Tests marked as inconclusive due to SUT defects |
| `E2E` | End-to-end scenarios | Cross-layer critical journeys |

## Reporting

### Allure Reports

Allure is integrated for rich reporting with screenshots, logs, and attachments on failure.

```bash
# Generate report locally (requires Allure CLI)
allure serve allure-results
```

In CI, Allure reports are automatically published to GitHub Pages on the `main` branch.

### Failure Attachments

- **UI test failures**: Automatic screenshot attached as `screenshot.png`
- **API test failures**: Request body (`api-request-body.txt`) and response body (`api-response-<status>.txt`) attached
- **Logs**: Comprehensive logging throughout test execution

## CI/CD Pipeline

The GitHub Actions workflow (`.github/workflows/ci.yml`) runs on:
- Push to `main`
- Pull requests to `main`
- Nightly schedule (2 AM UTC)
- Manual trigger with workflow_dispatch

### Pipeline Stages

1. **Build & Lint** - Build Docker image, run `dotnet format --verify-no-changes`, run framework unit tests
2. **API Tests** - Run API tests in parallel with UI tests (after build)
3. **UI Tests** - Run UI tests in parallel with API tests (after build)
4. **Report** - Collect Allure results, generate report, publish to GitHub Pages (main only)
5. **Notify** - Send email notification on schedule, main branch, or PR failures

### Required Secrets

For email notifications and Pages deployment, configure these secrets in GitHub:
- `SMTP_SERVER`, `SMTP_PORT`, `SMTP_USERNAME`, `SMTP_PASSWORD`, `NOTIFICATION_EMAIL`
- `GITHUB_TOKEN` (automatically provided)

## Known Defects

See [BUGS.md](BUGS.md) for details on known issues in the SUT:

- **BUG-001**: Registration accepts invalid email addresses (no `@` symbol)
- **BUG-002**: Registration accepts mismatched password confirmation

These are tracked with `Assert.Inconclusive` in tests to document them without failing the build.

## Design Decisions & Trade-offs

| Decision | Rationale | Trade-off |
| :-- | :-- | :-- |
| **Unique users per test** | Avoids test data conflicts, no cleanup needed | Tests create more data in SUT, but isolated and reliable |
| **UI tests register via UI** | Tests the actual registration flow | Slower than API setup, but validates real UI behavior (E2E is exception) |
| **Explicit waits only, no sleeps** | More reliable, faster when conditions met | Requires careful wait conditions, more verbose |
| **Fixture-level parallelism** | Good balance of speed and isolation | UI tests must run non-parallel due to browser constraints |
| **Docker-first approach** | Reproducible across environments, no setup hassle | Larger initial download, slightly slower on first run |
| **Allure over plain reports** | Rich attachments, better failure analysis | Additional dependency and setup complexity |
| **Known defects as Inconclusive** | Documents bugs without blocking CI | VSTest shows as NotExecuted, must check Allure/NUnit results |
| **Locators in separate classes per page** | Clear organization, easy to maintain | More files, but better structure |
| **ElementFactory pattern** | Consistent element creation with logging | Extra abstraction layer |

## Tech Stack

- **Framework**: .NET 10, C# 13
- **Test Runner**: NUnit 4.x
- **UI Automation**: Selenium WebDriver 4.x
- **API Testing**: HttpClient with custom clients
- **Reporting**: Allure NUnit
- **Containerization**: Docker, Docker Compose
- **CI/CD**: GitHub Actions
- **SUT**: OWASP Juice Shop v17.1.1

---

*For more details on test strategy, see [STRATEGY.md](STRATEGY.md). For known defects, see [BUGS.md](BUGS.md).*
