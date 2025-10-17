#!/bin/bash

# Code Coverage Analysis Script for libtwee
# This script runs tests with code coverage and generates HTML reports

echo "🧪 Running tests with code coverage collection..."

# Clean previous results
rm -rf TestTwee/TestResults
rm -rf TestTwee/CoverageReport

# Run tests with coverage collection
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestTwee/TestResults

# Check if coverage was collected
if [ ! -d "TestTwee/TestResults" ]; then
    echo "❌ No coverage data was collected!"
    exit 1
fi

echo "📊 Generating HTML coverage report..."

# Generate HTML report using ReportGenerator
reportgenerator \
    -reports:"TestTwee/TestResults/*/coverage.cobertura.xml" \
    -targetdir:"TestTwee/CoverageReport" \
    -reporttypes:"Html;TextSummary"

if [ $? -eq 0 ]; then
    echo "✅ Coverage report generated successfully!"
    echo ""
    echo "📈 Coverage Summary:"
    cat TestTwee/CoverageReport/Summary.txt | head -20
    echo ""
    echo "🌐 Open the detailed HTML report:"
    echo "   file://$(pwd)/TestTwee/CoverageReport/index.html"
    echo ""
    echo "💡 To open in browser:"
    echo "   open TestTwee/CoverageReport/index.html"
else
    echo "❌ Failed to generate coverage report!"
    exit 1
fi