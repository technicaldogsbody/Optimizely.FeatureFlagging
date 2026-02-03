# Contributing to TechnicalDogsbody.Optimizely.FeatureFlagging

Thank you for your interest in contributing! This document provides guidelines for contributing to this project.

## Code of Conduct

Please be respectful and constructive in all interactions. We aim to maintain a welcoming environment for all contributors.

## How to Contribute

### Reporting Issues

Before creating an issue:
1. Search existing issues to avoid duplicates
2. Use the issue template if available
3. Include clear reproduction steps
4. Specify your Optimizely CMS and .NET versions

### Submitting Pull Requests

1. **Fork the repository** and create your branch from `main`
2. **Follow the coding standards** defined in `.editorconfig`
3. **Write tests** for new functionality
4. **Update documentation** including README and XML comments
5. **Run all tests** and ensure they pass
6. **Update CHANGELOG.md** with your changes

### Development Setup

```bash
# Clone your fork
git clone https://github.com/YOUR_USERNAME/Optimizely.FeatureFlagging.git
cd Optimizely.FeatureFlagging

# Restore packages
dotnet restore

# Build
dotnet build

# Run tests
dotnet test
```

### Coding Standards

- Follow the `.editorconfig` settings in the repository
- Use British English spelling in code and comments
- Write clear, descriptive commit messages
- Keep functions focused and single-purpose
- Add XML documentation comments for public APIs
- Use meaningful variable and method names

### Testing Requirements

- **Unit Tests**: All new attributes must have comprehensive unit tests
- **Coverage**: Aim for 100% coverage of functional code
- **Test Naming**: Use descriptive names following AAA pattern
- **Mocking**: Use Moq for dependency mocking

Example test structure:

```csharp
[Test]
public void Attribute_Should_BehaveLikeThis_When_ConditionMet()
{
    // Arrange
    var featureManager = new Mock<IFeatureManager>();
    featureManager.Setup(x => x.IsEnabledAsync("FeatureName"))
        .ReturnsAsync(true);

    // Act
    var result = // test action

    // Assert
    Assert.That(result, Is.EqualTo(expected));
}
```

### Documentation

- Add XML documentation to all public classes, methods, and properties
- Update README.md with examples for new features
- Include usage examples in code comments
- Update CHANGELOG.md following Keep a Changelog format

### Pull Request Process

1. Update documentation and tests
2. Ensure CI builds pass
3. Request review from maintainers
4. Address review feedback
5. Squash commits if requested
6. Maintainers will merge approved PRs

### Commit Message Format

Use clear, descriptive commit messages:

```
feat: Add FeatureFlaggedNewAttribute for X functionality
fix: Resolve issue with Y when Z condition
docs: Update README with new examples
test: Add unit tests for A scenario
refactor: Simplify B implementation
```

Prefixes:
- `feat:` New features
- `fix:` Bug fixes
- `docs:` Documentation changes
- `test:` Test additions or changes
- `refactor:` Code refactoring
- `chore:` Build process or tooling changes

### Feature Requests

Feature requests are welcome! Please:
1. Check existing issues first
2. Describe the use case clearly
3. Explain why this would benefit other users
4. Consider submitting a PR if you can implement it

### Questions

For questions about usage or implementation:
- Check the [Wiki](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/wiki)
- Search existing [issues](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/issues)
- Open a new issue with the "question" label

## Project Structure

```
TechnicalDogsbody.Optimizely.FeatureFlagging/
├── Extensions/                    # Content scanner extensions
│   ├── FeatureFlaggedBackingTypeContentScannerExtension.cs
│   ├── FeatureFlaggedCultureSpecificContentScannerExtension.cs
│   └── ...
├── FeatureFlaggedBackingTypeAttribute.cs
├── FeatureFlaggedClientEditorAttribute.cs
├── ...                           # All attribute classes
├── ServiceCollectionExtensions.cs
└── TechnicalDogsbody.Optimizely.FeatureFlagging.csproj

TechnicalDogsbody.Optimizely.FeatureFlagging.Tests/
├── Attributes/                   # Attribute tests
├── Extensions/                   # Scanner extension tests
└── TechnicalDogsbody.Optimizely.FeatureFlagging.Tests.csproj
```

## Release Process

Maintainers handle releases:

1. Update version in `.csproj`
2. Update CHANGELOG.md
3. Create release tag
4. Build NuGet package
5. Publish to NuGet.org
6. Create GitHub release with notes

## License

By contributing, you agree that your contributions will be licensed under the MIT License.

## Questions?

Feel free to open an issue or reach out to the maintainers.

Thank you for contributing! 🎉
