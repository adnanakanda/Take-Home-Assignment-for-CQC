# Test Strategy

## 1. Objective
This document outlines the test strategy for the automated test suite covering the OWASP Juice Shop application. The goal is to provide fast feedback, reliable regression coverage, and clear visibility into defects while keeping the framework maintainable and scalable.

## 2. Test Coverage by Level

| Level | Purpose | Scope | Execution | Notes
| :-- | :-- | :-- | :-- | :-- |
| **Unit** | Validate framework logic in isolation | `ConfigurationProvider`, `UserFactory`, `ApiResponse`, `JsonSchemaValidator` | Parallel, fast (seconds), no external dependencies | Located in `SeniorCQCAssignment.Tests/Unit/`. Run in CI before starting SUT. |
| **API** | Validate business logic and contract | User registration/login, basket operations, product search, schema validation, negative scenarios | Parallel (fixtures), against live SUT | Uses API clients and steps. Tests verify status codes, responses, and JSON schema. Known defects are marked as `[Ignore]`/`Assert.Inconclusive` and tracked in `BUGS.md`. |
| **UI** | Validate end-to-end user workflows through the browser | Login, basket management, registration flows | Non-parallel (single browser session constraints), headless Chrome | Uses Page Object Model with explicit waits only. UI tests register their own customers through the UI (except the deliberate E2E exception). |
| **End-to-End (E2E)** | Validate critical business journey across layers | Full customer journey (registration via API where appropriate, login via UI/API as needed, basket operations) | Non-parallel, part of UI suite | Deliberately uses API for setup where it makes sense to isolate the UI under test. | 

## 3. What Is Deliberately Not Automated

| Area | Reason | Alternative |
| :-- | :-- | :-- |
| **Full checkout/payment flows** | Juice Shop has complex payment methods, third-party integrations, and CAPTCHA-like elements that are flaky and time-consuming to automate. | Manual exploratory testing for payment paths. |
| **Admin functionality** | Requires elevated permissions, complex setup, and is outside the core customer journey scope for this assignment. | Covered by exploratory testing if needed. |
| **File uploads with malicious payloads** | Security scanning is better handled by dedicated security tools (ZAP, Burp) rather than UI automation. | Out of scope for functional test automation. |
| **Cross-browser testing** | Chrome is sufficient for this assignment. Additional browsers would add complexity without adding significant value. | Chrome headless is used in CI and locally. |
| **Mobile responsive testing** | Focus is on core functionality, not responsive layouts. | Not prioritized for this exercise. |
| **Performance/load testing** | Requires dedicated tools (k6, JMeter) and different infrastructure. | Not in scope. |

## 4. Risks, Mitigations, and Considerations

| Risk | Impact | Likelihood | Mitigation |
| :-- | :-- | :-- | :-- |
| **Test data pollution** | High | Medium | Each test creates unique users (via `UserFactory` with timestamps/guids). No shared state between tests. SUT can be restarted between runs if needed. |
| **Flaky UI tests** | High | Medium | Explicit waits only (`WebDriverWait`), no `Thread.Sleep`. Locators are stable (IDs preferred). Browser is maximized/headless with proper flags. UI suite runs non-parallel. |
| **Parallel execution conflicts** | High | Medium | Assembly-level parallelism with `ParallelScope.Fixtures`. UI tests and `ConfigurationProviderTests` are marked `[NonParallelizable]`. API/unit tests run in parallel. |
| **SUT startup time** | Medium | High | Docker Compose uses health checks (`wget` spider) to ensure Juice Shop is ready before tests start. No arbitrary sleeps. |
| **Known defects** | Medium | High | BUG-001 and BUG-002 are tracked in `BUGS.md`. Corresponding tests use `Assert.Inconclusive` to fail gracefully without blocking CI, while still documenting the defects. |
| **Browser dependencies in CI** | Medium | Medium | Chrome is installed in the test Docker image with all required dependencies. Headless mode is enforced. |
| **Environment differences** | Medium | Medium | Configuration via `appsettings.json`, environment-specific files (`appsettings.ci.json`), and environment variables. `ConfigurationProvider` validates and fails fast on invalid config. |

## 5. Scaling Strategy (1000+ Tests, 5+ Authors)

| Area | Strategy | Rationale |
| :-- | :-- | :-- |
| **Test organization** | Feature-based structure (`Tests/Api/`, `Tests/Ui/`, `Tests/Unit/`) with shared steps and clients | Clear ownership boundaries, easy for multiple authors to work in parallel. |
| **Parallelism** | Fixture-level parallelism with careful non-parallel marking | Maximizes throughput while avoiding shared state issues. Can scale to more parallel workers. |
| **Test data** | Factory pattern with unique data generation | Eliminates collisions without needing complex cleanup infrastructure. |
| **Page Object Model** | Strict separation (Pages, Components, Locators) with `ElementFactory` | Reduces duplication, makes UI changes localized to single files. |
| **API client abstraction** | Layered clients (`ApiClientBase`, feature clients) with `ApiTestContext` | Reusable across tests, easy to extend for new endpoints. |
| **Logging & reporting** | Structured logging with Allure attachments | Critical for debugging failures at scale without re-running tests. |
| **Code quality** | `.editorconfig`, analyzers, `dotnet format` in CI | Enforces consistency across multiple contributors. |
| **Tagging** | Categories (`Smoke`, `Regression`, `Api`, `Ui`, `Unit`, `KnownDefect`) | Enables selective execution (e.g., smoke tests on PRs, full regression nightly). |
| **CI optimization** | Docker layer caching, parallel jobs (API/UI), early unit test feedback | Keeps feedback loops fast even as test count grows. |

## 6. Retry Policy

| Scenario | Retry | Justification |
| :-- | :-- | :-- |
| **Unit tests** | None | Deterministic, no external dependencies. Failures indicate real issues. |
| **API tests** | None (by default) | Should be deterministic against a controlled SUT. Retries can mask real defects. |
| **UI tests** | None (by default) | With explicit waits and proper synchronisation, retries are rarely needed. If flakiness appears, investigate root cause first. |
| **Infrastructure failures** | Handled by CI | Docker/SUT startup issues are infrastructure-related. CI will fail fast with clear logs. Health checks prevent tests from running against unready SUT. |

## 7. Quality Gates

- **Build**: Must compile with no errors
- **Lint**: `dotnet format --verify-no-changes` must pass
- **Unit tests**: All framework unit tests must pass
- **API tests**: Must pass (known defects marked as inconclusive, not failures)
- **UI tests**: Must pass (known defects marked as inconclusive, not failures)
- **Exit code**: `docker compose up --exit-code-from tests` must exit with 0 for a successful run
- **Reports**: Allure results must be generated and published (on main branch)

## 8. Tools & Technologies

| Tool | Purpose |
| :-- | :-- |
| **.NET 10 / C#** | Test framework and automation code |
| **NUnit** | Test runner and assertions |
| **Selenium WebDriver** | UI automation |
| **HttpClient** | API automation |
| **JsonSchema.Net** | JSON schema validation |
| **Allure** | Rich test reporting with attachments |
| **Docker & Docker Compose** | Containerized, reproducible test execution |
| **GitHub Actions** | CI/CD pipeline |
| **OWASP Juice Shop v17.1.1** | System under test |
