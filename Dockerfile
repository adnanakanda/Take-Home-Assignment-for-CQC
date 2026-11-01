# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files for restore
COPY Directory.Build.props ./
COPY Directory.Packages.props ./
COPY SeniorCQCAssignment.slnx ./
COPY SeniorCQCAssignment.Framework/SeniorCQCAssignment.Framework.csproj SeniorCQCAssignment.Framework/
COPY SeniorCQCAssignment.Automation/SeniorCQCAssignment.Automation.csproj SeniorCQCAssignment.Automation/
COPY SeniorCQCAssignment.Tests/SeniorCQCAssignment.Tests.csproj SeniorCQCAssignment.Tests/

# Restore dependencies
RUN dotnet restore SeniorCQCAssignment.slnx

# Copy everything else
COPY . .

# Build
RUN dotnet build SeniorCQCAssignment.slnx --no-restore -c Release

# Runtime stage with Chrome and dependencies for UI tests
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS test
WORKDIR /app

# Install Chrome and dependencies
RUN apt-get update && apt-get install -y \
    wget \
    gnupg \
    ca-certificates \
    fonts-liberation \
    libasound2 \
    libatk-bridge2.0-0 \
    libatk1.0-0 \
    libatspi2.0-0 \
    libcups2 \
    libdbus-1-3 \
    libdrm2 \
    libgbm1 \
    libgtk-3-0 \
    libnspr4 \
    libnss3 \
    libwayland-client0 \
    libxcomposite1 \
    libxdamage1 \
    libxfixes3 \
    libxkbcommon0 \
    libxrandr2 \
    xdg-utils \
    --no-install-recommends && \
    wget -q -O - https://dl.google.com/linux/linux_signing_key.pub | gpg --dearmor -o /usr/share/keyrings/googlechrome-linux-keyring.gpg && \
    echo "deb [arch=amd64 signed-by=/usr/share/keyrings/googlechrome-linux-keyring.gpg] https://dl.google.com/linux/chrome/deb/ stable main" | tee /etc/apt/sources.list.d/google-chrome.list > /dev/null && \
    apt-get update && apt-get install -y google-chrome-stable --no-install-recommends && \
    rm -rf /var/lib/apt/lists/*

# Copy built app
COPY --from=build /src /app

WORKDIR /app

# Create directories for test results
RUN mkdir -p /app/TestResults /app/allure-results

ENV TestSettings__Environment=ci

ENTRYPOINT ["dotnet", "test", "SeniorCQCAssignment.slnx", "--no-restore", "-c", "Release", "--logger", "console;verbosity=normal", "--results-directory", "/app/TestResults"]
