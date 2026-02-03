# TechnicalDogsbody.Optimizely.FeatureFlagging

<p align="center">
  <img src="logo_and_title.png" alt="Optimizely Feature Flagging" width="400"/>
</p>

<p align="center">
  <a href="https://www.nuget.org/packages/TechnicalDogsbody.Optimizely.FeatureFlagging/"><img src="https://img.shields.io/nuget/v/TechnicalDogsbody.Optimizely.FeatureFlagging.svg" alt="NuGet"></a>
  <a href="https://www.nuget.org/packages/TechnicalDogsbody.Optimizely.FeatureFlagging/"><img src="https://img.shields.io/nuget/dt/TechnicalDogsbody.Optimizely.FeatureFlagging.svg" alt="NuGet Downloads"></a>
  <a href="https://opensource.org/licenses/MIT"><img src="https://img.shields.io/badge/License-MIT-yellow.svg" alt="License: MIT"></a>
  <br />
  <a href="https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/build.yml"><img src="https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/build.yml/badge.svg" alt="Build"></a>
  <a href="https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/code-quality.yml"><img src="https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/code-quality.yml/badge.svg" alt="Code Quality"></a>
  <a href="https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/codeql.yml"><img src="https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/codeql.yml/badge.svg" alt="CodeQL"></a>
</p>

Feature flag your Optimizely CMS content properties without code changes. Control property behaviour, validation, display settings, and metadata dynamically using Microsoft Feature Management.

## Features

- **Full Property Control**: Manage visibility, validation, editors, and metadata
- **Microsoft Feature Management**: Built on industry-standard feature flagging
- **Type Safe**: Compile-time validation of feature flag configurations
- **Performance Optimised**: Scanner extensions run during content type initialisation

## Installation

```bash
dotnet add package TechnicalDogsbody.Optimizely.FeatureFlagging
```

Or via Package Manager Console:

```powershell
Install-Package TechnicalDogsbody.Optimizely.FeatureFlagging
```

## Quick Start

### 1. Register Services

In your `Program.cs` or `Startup.cs`:

```csharp
using TechnicalDogsbody.Optimizely.FeatureFlagging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCms();
builder.Services.AddOptimizelyFeatureFlagging();

var app = builder.Build();
```

### 2. Configure Feature Flags

Add feature flags to your `appsettings.json`:

```json
{
  "FeatureManagement": {
    "NewPropertyEditor": true,
    "BetaValidation": false,
    "ShowAdvancedOptions": true
  }
}
```

### 3. Use in Content Types

Apply feature-flagged attributes to your properties:

```csharp
using EPiServer.Core;
using EPiServer.DataAnnotations;
using TechnicalDogsbody.Optimizely.FeatureFlagging;

[ContentType(DisplayName = "Article Page", GUID = "...")]
public class ArticlePage : PageData
{
    // Show/hide property based on feature flag
    [FeatureFlaggedScaffoldColumn("ShowAdvancedOptions", scaffoldWhenEnabled: true)]
    [Display(Name = "SEO Keywords", GroupName = "SEO")]
    public virtual string SeoKeywords { get; set; }

    // Change validation rules
    [FeatureFlaggedStringLength("BetaValidation",
        enabledMaximumLength: 500,
        disabledMaximumLength: 200)]
    [Display(Name = "Summary")]
    public virtual string Summary { get; set; }

    // Switch property editor
    [FeatureFlaggedClientEditor("NewPropertyEditor",
        EnabledClientEditor = "epi-cms/widget/HtmlEditor",
        DisabledClientEditor = "epi-cms/widget/Textarea")]
    [Display(Name = "Body Text")]
    public virtual string BodyText { get; set; }
}
```

## Available Attributes

### Display & Visibility

#### FeatureFlaggedDisplay
Change property name, description, tab, and order based on feature flags.

```csharp
[FeatureFlaggedDisplay("RebrandFeature",
    EnabledName = "Hero Image",
    DisabledName = "Main Image",
    EnabledDescription = "Main hero banner image",
    DisabledDescription = "Primary content image",
    EnabledGroupName = "Content",
    DisabledGroupName = "Media")]
public virtual ContentReference Image { get; set; }
```

#### FeatureFlaggedScaffoldColumn
Show or hide properties in the CMS editor.

```csharp
[FeatureFlaggedScaffoldColumn("ShowAdvancedSettings", scaffoldWhenEnabled: true)]
public virtual string AdvancedConfig { get; set; }
```

#### FeatureFlaggedIgnore
Completely ignore a property during content type scanning when the feature is disabled.

```csharp
[FeatureFlaggedIgnore("ExperimentalFeatures")]
public virtual string ExperimentalProperty { get; set; }
```

### Validation

#### FeatureFlaggedRequired
Toggle whether a property is required.

```csharp
[FeatureFlaggedRequired("StrictValidation", requiredWhenEnabled: true)]
public virtual string AuthorName { get; set; }
```

#### FeatureFlaggedStringLength
Change string length validation rules.

```csharp
[FeatureFlaggedStringLength("ExtendedContent",
    enabledMaximumLength: 1000,
    disabledMaximumLength: 500,
    EnabledMinimumLength = 10,
    DisabledMinimumLength = 5)]
public virtual string Description { get; set; }
```

#### FeatureFlaggedRange
Toggle numeric range validation.

```csharp
[FeatureFlaggedRange("ExpandedLimits",
    enabledMinimum: 0,
    enabledMaximum: 1000,
    disabledMinimum: 0,
    disabledMaximum: 100)]
public virtual int Quantity { get; set; }
```

#### FeatureFlaggedRegularExpression
Switch validation patterns.

```csharp
[FeatureFlaggedRegularExpression("InternationalFormat",
    enabledPattern: @"^\+?[1-9]\d{1,14}$",  // E.164 international
    disabledPattern: @"^\d{3}-\d{3}-\d{4}$")] // US format
public virtual string PhoneNumber { get; set; }
```

### Editors & UI

#### FeatureFlaggedClientEditor
Switch between different property editors.

```csharp
[FeatureFlaggedClientEditor("RichTextEditor",
    EnabledClientEditor = "epi-cms/widget/HtmlEditor",
    DisabledClientEditor = "epi-cms/widget/Textarea")]
public virtual string Content { get; set; }
```

#### FeatureFlaggedUIHint
Toggle UI hints for property rendering.

```csharp
[FeatureFlaggedUiHint("DateTimePicker",
    EnabledUiHint = "epi-cms/widget/DateTimeEditor",
    DisabledUiHint = "epi-cms/widget/Textarea")]
public virtual string ScheduledDate { get; set; }
```

#### FeatureFlaggedEditable
Control whether properties are editable or read-only.

```csharp
[FeatureFlaggedEditable("AllowEditing", editableWhenEnabled: true)]
public virtual string SystemGenerated { get; set; }
```

### Content Model Settings

#### FeatureFlaggedBackingType
Switch the underlying PropertyData type.

```csharp
[FeatureFlaggedBackingType("LongContent",
    typeof(PropertyLongString),
    typeof(PropertyString))]
public virtual string FlexibleContent { get; set; }
```

#### FeatureFlaggedCultureSpecific
Toggle whether properties are culture-specific.

```csharp
[FeatureFlaggedCultureSpecific("MultiLanguage", cultureSpecificWhenEnabled: true)]
public virtual string LocalizedContent { get; set; }
```

#### FeatureFlaggedSearchable
Control property searchability.

```csharp
[FeatureFlaggedSearchable("SearchOptimization", searchableWhenEnabled: true)]
public virtual string ProductDescription { get; set; }
```

## Advanced Configuration

### Custom Feature Management Setup

If you need custom Feature Management configuration, register extensions separately:

```csharp
using Microsoft.FeatureManagement;
using TechnicalDogsbody.Optimizely.FeatureFlagging;

var builder = WebApplication.CreateBuilder(args);

// Custom Feature Management setup
builder.Services.AddFeatureManagement(builder.Configuration.GetSection("Features"))
    .WithTargeting<CustomTargetingContextAccessor>();

// Register only the scanner extensions
builder.Services.AddOptimizelyFeatureFlaggingExtensions();
```

### Environment-Specific Configuration

Override feature flags per environment:

**appsettings.Development.json:**
```json
{
  "FeatureManagement": {
    "ExperimentalFeatures": true,
    "BetaValidation": true
  }
}
```

**appsettings.Production.json:**
```json
{
  "FeatureManagement": {
    "ExperimentalFeatures": false,
    "BetaValidation": false
  }
}
```

## Best Practices

### Feature Flag Naming

Use descriptive, purposeful names:

```csharp
// Good
[FeatureFlaggedRequired("EnforceGdprCompliance", requiredWhenEnabled: true)]

// Avoid
[FeatureFlaggedRequired("Flag1", requiredWhenEnabled: true)]
```

### Combining Attributes

Apply multiple feature-flagged attributes to the same property:

```csharp
[FeatureFlaggedRequired("StrictMode", requiredWhenEnabled: true)]
[FeatureFlaggedStringLength("StrictMode",
    enabledMaximumLength: 100,
    disabledMaximumLength: 500)]
[FeatureFlaggedEditable("AllowEdits", editableWhenEnabled: true)]
public virtual string Title { get; set; }
```

### Testing

Test content types with different feature flag configurations:

```csharp
[Test]
public void Property_Should_Be_Required_When_Feature_Enabled()
{
    // Arrange
    var featureManager = new Mock<IFeatureManager>();
    featureManager.Setup(x => x.IsEnabledAsync("StrictValidation"))
        .ReturnsAsync(true);

    // Act & Assert
    var attribute = new FeatureFlaggedRequiredAttribute("StrictValidation");
    // Test validation logic
}
```

### Performance

- Scanner extensions run once during content type initialisation
- Feature flag checks are cached by Microsoft Feature Management
- No runtime performance impact on content rendering

## Migration Guide

### From Standard Attributes

Replace standard Optimizely attributes with feature-flagged versions:

**Before:**
```csharp
[Required]
[Display(Name = "Title", GroupName = "Content")]
public virtual string Title { get; set; }
```

**After:**
```csharp
[FeatureFlaggedRequired("RequireTitle", requiredWhenEnabled: true)]
[FeatureFlaggedDisplay("ContentRestructure",
    EnabledName = "Title",
    EnabledGroupName = "Content")]
public virtual string Title { get; set; }
```

### Gradual Rollout

Introduce feature flags gradually:

1. Add feature flag to `appsettings.json` (disabled)
2. Apply feature-flagged attribute
3. Test with flag enabled in development
4. Enable flag in production when ready

## Troubleshooting

### Properties Not Updating

**Problem**: Changes to feature flags not reflected in CMS.

**Solution**: Content type metadata is cached. Restart the application or clear Optimizely's content type cache.

### Feature Manager Not Found

**Problem**: `IFeatureManager` cannot be resolved.

**Solution**: Ensure `AddOptimizelyFeatureFlagging()` is called in service registration.

### Attributes Not Applied

**Problem**: Feature-flagged attributes have no effect.

**Solution**: Check that scanner extensions are registered. Use `AddOptimizelyFeatureFlagging()` or manually register extensions.

## Requirements

- Optimizely CMS 12.0 or higher
- .NET 8.0, 9.0, or 10.0
- Microsoft.FeatureManagement.AspNetCore 3.0 or higher

## Contributing

Contributions are welcome! Please submit issues and pull requests to the [GitHub repository](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging).

### Development Setup

1. Clone the repository: `git clone https://github.com/technicaldogsbody/Optimizely.FeatureFlagging.git`
2. Restore packages: `dotnet restore`
3. Build: `dotnet build`
4. Run tests: `dotnet test`

## License

This project is licensed under the MIT License. See the [LICENSE](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/blob/main/LICENSE) file for details.

## Support

For issues, questions, or feature requests:
- Open an issue on [GitHub](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/issues)

## Changelog

See [CHANGELOG.md](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/blob/main/CHANGELOG.md) for version history and release notes.

## Acknowledgements

Built on:
- [Microsoft Feature Management](https://github.com/microsoft/FeatureManagement-Dotnet)
- [Optimizely CMS](https://www.optimizely.com/)

---

Made with ❤️ by [TechnicalDogsbody](https://github.com/technicaldogsbody)
