#!/bin/bash

# Documentation Generation Script for libtwee
# This script builds the documentation using DocFX

echo "📚 Building libtwee documentation..."

# Check if DocFX is installed
if ! command -v docfx &> /dev/null; then
    echo "🔧 Installing DocFX..."
    dotnet tool install -g docfx
fi

# Navigate to docs directory
cd docs

echo "🏗️  Building documentation with DocFX..."

# Build the documentation
docfx docfx.json

if [ $? -eq 0 ]; then
    echo "✅ Documentation built successfully!"
    echo ""
    echo "📁 Documentation files generated in: docs/_site/"
    echo ""
    echo "🌐 Open the documentation:"
    echo "   file://$(pwd)/_site/index.html"
    echo ""
    echo "💡 To serve locally:"
    echo "   cd docs && docfx serve _site"
    echo "   Then visit: http://localhost:8080"
else
    echo "❌ Failed to build documentation!"
    exit 1
fi