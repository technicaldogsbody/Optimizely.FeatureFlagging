# TechnicalDogsbody.Optimizely.FeatureFlagging

![Optimizely Feature Flagging](https://raw.githubusercontent.com/technicaldogsbody/Optimizely.FeatureFlagging/refs/heads/main/logo_and_title.png)

[![NuGet](https://img.shields.io/nuget/v/TechnicalDogsbody.Optimizely.FeatureFlagging.svg)](https://www.nuget.org/packages/TechnicalDogsbody.Optimizely.FeatureFlagging/)
[![NuGet Downloads](https://img.shields.io/nuget/dt/TechnicalDogsbody.Optimizely.FeatureFlagging.svg)](https://www.nuget.org/packages/TechnicalDogsbody.Optimizely.FeatureFlagging/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Build](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/build.yml/badge.svg)](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/build.yml)
[![Code Quality](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/code-quality.yml/badge.svg)](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/code-quality.yml)
[![CodeQL](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/codeql.yml/badge.svg)](https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/actions/workflows/codeql.yml)

Feature flag your Optimizely CMS content properties without code changes. Control property behaviour, validation, display settings, and metadata dynamically using any feature flag provider.

## Features

- **Full Property Control**: Manage visibility, validation, editors, and metadata
- **Provider Agnostic**: Use Microsoft Feature Management, LaunchDarkly, Optimizely Feature Experimentation, or any custom provider
- **Type Safe**: Compile-time validation of feature flag configurations
- **Performance Optimised**: Scanner extensions run during content type initialisation
- **Change Notifications**: Optional support for dynamic flag updates (provider-dependent)

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
builder.Services.AddFeatureFlaggedContentTypes(); // Uses Microsoft Feature Management by default

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

#### FeatureFlaggedContentType
Show or hide entire content types based on feature flags.

```csharp
// Content type visible when feature is enabled
[ContentType(DisplayName = "New Article Page", GUID = "...")]
[FeatureFlaggedContentType("NewArticleFeature", VisibleWhenEnabled = true)]
public class NewArticlePage : PageData
{
    // Properties...
}

// Content type visible when feature is disabled (for deprecation)
[ContentType(DisplayName = "Old Article Page", GUID = "...")]
[FeatureFlaggedContentType("NewArticleFeature", VisibleWhenEnabled = false)]
public class OldArticlePage : PageData
{
    // Properties...
}
```

**Use cases:**
- Gradual rollout of new content types
- A/B testing different content structures
- Deprecating old content types while maintaining data
- Environment-specific content types

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

## Custom Feature Flag Providers

### Using a Different Provider

The library supports any feature flag provider through the `IFeatureFlagProvider` interface.

#### LaunchDarkly Example

```csharp
using LaunchDarkly.Sdk.Server;
using TechnicalDogsbody.Optimizely.FeatureFlagging;

var builder = WebApplication.CreateBuilder(args);

// Register LaunchDarkly client
builder.Services.AddSingleton<ILdClient>(sp =>
{
    var config = Configuration.Builder("your-sdk-key").Build();
    return new LdClient(config);
});

// Register custom provider
builder.Services.AddFeatureFlaggedContentTypes<LaunchDarklyFeatureFlagProvider>();
```

**LaunchDarkly Provider Implementation:**

```csharp
public class LaunchDarklyFeatureFlagProvider : IFeatureFlagProvider
{
    private readonly ILdClient _client;
    
    public LaunchDarklyFeatureFlagProvider(ILdClient client)
    {
        _client = client;
    }
    
    public bool IsEnabled(string featureName)
    {
        var context = Context.New("default");
        return _client.BoolVariation(featureName, context, false);
    }
    
    public IDisposable? OnFlagChanged(Action<string> callback)
    {
        _client.FlagTracker.FlagChanged += (sender, args) =>
        {
            callback(args.Key);
        };
        
        return new FlagChangeSubscription(_client);
    }
}
```

#### Optimizely Feature Experimentation Example

```csharp
using Optimizely;
using TechnicalDogsbody.Optimizely.FeatureFlagging;

var builder = WebApplication.CreateBuilder(args);

// Register Optimizely Feature Experimentation client
builder.Services.AddSingleton<OptimizelyClient>(sp =>
{
    return new OptimizelyClient(
        datafile: "your-datafile",
        configManager: new HttpProjectConfigManager.Builder()
            .WithSdkKey("your-sdk-key")
            .Build()
    );
});

// Register custom provider
builder.Services.AddFeatureFlaggedContentTypes<OptimizelyFeatureExperimentationProvider>();
```

**Optimizely Feature Experimentation Provider Implementation:**

```csharp
public class OptimizelyFeatureExperimentationProvider : IFeatureFlagProvider
{
    private readonly OptimizelyClient _client;
    
    public OptimizelyFeatureExperimentationProvider(OptimizelyClient client)
    {
        _client = client;
    }
    
    public bool IsEnabled(string featureName)
    {
        var userId = "default-user";
        return _client.IsFeatureEnabled(featureName, userId);
    }
    
    public IDisposable? OnFlagChanged(Action<string> callback)
    {
        // Optimizely Feature Experimentation supports change notifications
        _client.NotificationCenter.AddNotification(
            NotificationCenter.NotificationType.OptimizelyConfigUpdate,
            (args) => callback("*") // Signal all flags may have changed
        );
        
        return null; // Or return proper disposable
    }
}
```

### Creating Your Own Provider

Implement the `IFeatureFlagProvider` interface:

```csharp
public interface IFeatureFlagProvider
{
    /// <summary>
    /// Checks if a feature is enabled.
    /// </summary>
    /// <param name="featureName">The feature name.</param>
    /// <returns>True if enabled, otherwise false.</returns>
    bool IsEnabled(string featureName);
    
    /// <summary>
    /// Subscribes to feature flag changes.
    /// Not all providers support this - returns null if unsupported.
    /// </summary>
    /// <param name="callback">Called when any flag changes.</param>
    /// <returns>Disposable subscription, or null if provider doesn't support change notifications.</returns>
    IDisposable? OnFlagChanged(Action<string> callback);
}
```

**Simple Database Provider Example:**

```csharp
public class DatabaseFeatureFlagProvider : IFeatureFlagProvider
{
    private readonly IDbConnection _connection;
    
    public DatabaseFeatureFlagProvider(IDbConnection connection)
    {
        _connection = connection;
    }
    
    public bool IsEnabled(string featureName)
    {
        var sql = "SELECT IsEnabled FROM FeatureFlags WHERE Name = @Name";
        return _connection.QuerySingleOrDefault<bool>(sql, new { Name = featureName });
    }
    
    public IDisposable? OnFlagChanged(Action<string> callback)
    {
        // Database polling not supported in this simple example
        return null;
    }
}
```

**Configuration File Provider Example:**

```csharp
public class JsonFileFeatureFlagProvider : IFeatureFlagProvider
{
    private readonly string _filePath;
    private readonly Dictionary<string, bool> _flags;
    
    public JsonFileFeatureFlagProvider(string filePath)
    {
        _filePath = filePath;
        _flags = LoadFlags();
    }
    
    public bool IsEnabled(string featureName)
    {
        return _flags.TryGetValue(featureName, out var enabled) && enabled;
    }
    
    public IDisposable? OnFlagChanged(Action<string> callback)
    {
        // File watching could be implemented here
        return null;
    }
    
    private Dictionary<string, bool> LoadFlags()
    {
        var json = File.ReadAllText(_filePath);
        return JsonSerializer.Deserialize<Dictionary<string, bool>>(json) 
            ?? new Dictionary<string, bool>();
    }
}
```

## Advanced Configuration

### Custom Feature Management Setup

If you need custom Feature Management configuration, register your provider manually:

```csharp
using Microsoft.FeatureManagement;
using TechnicalDogsbody.Optimizely.FeatureFlagging;
using TechnicalDogsbody.Optimizely.FeatureFlagging.Providers;

var builder = WebApplication.CreateBuilder(args);

// Custom Feature Management setup
builder.Services.AddFeatureManagement(builder.Configuration.GetSection("Features"))
    .WithTargeting<CustomTargetingContextAccessor>();

// Register Microsoft provider manually
builder.Services.AddSingleton<IFeatureFlagProvider, MicrosoftFeatureFlagProvider>();

// Register feature flagged content types
builder.Services.AddFeatureFlaggedContentTypes<MicrosoftFeatureFlagProvider>();
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

### Dynamic Flag Updates

Some providers support dynamic flag changes. Implement `OnFlagChanged` to respond to updates:

```csharp
public class FeatureFlagMonitor : IHostedService
{
    private readonly IFeatureFlagProvider _provider;
    private IDisposable? _subscription;
    
    public FeatureFlagMonitor(IFeatureFlagProvider provider)
    {
        _provider = provider;
    }
    
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _subscription = _provider.OnFlagChanged(flagName =>
        {
            // Handle flag change
            // Note: ContentScannerExtensions only run at startup
            // Dynamic changes only affect runtime behaviour like FeatureFlaggedContentTypeAvailabilityService
        });
        
        return Task.CompletedTask;
    }
    
    public Task StopAsync(CancellationToken cancellationToken)
    {
        _subscription?.Dispose();
        return Task.CompletedTask;
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
    var provider = new Mock<IFeatureFlagProvider>();
    provider.Setup(x => x.IsEnabled("StrictValidation"))
        .Returns(true);

    // Act & Assert
    var attribute = new FeatureFlaggedRequiredAttribute("StrictValidation");
    // Test validation logic
}
```

### Performance

- Scanner extensions run once during content type initialisation
- Feature flag checks should be cached by your provider
- No runtime performance impact on content rendering
- Property metadata changes require application restart

## Important Limitations

### Startup-Only Property Metadata

Content type scanning happens once at application startup. Changes to the following require an application restart:

- Property names, descriptions, tabs (`FeatureFlaggedDisplay`)
- Property visibility (`FeatureFlaggedScaffoldColumn`, `FeatureFlaggedIgnore`)
- Backing types (`FeatureFlaggedBackingType`)
- Culture-specific settings (`FeatureFlaggedCultureSpecific`)
- Searchable settings (`FeatureFlaggedSearchable`)

### Runtime-Dynamic Behaviour

The following respond to flag changes without restart:

- Content type availability (`FeatureFlaggedContentType`)
- Property editability (`FeatureFlaggedEditable`)

### Provider Change Notification Support

Not all providers support `OnFlagChanged`:

- **Microsoft Feature Management**: No (returns null)
- **LaunchDarkly**: Yes
- **Optimizely Feature Experimentation**: Yes
- **Flagsmith**: Yes
- **Custom providers**: Optional

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

1. Add feature flag to your provider configuration (disabled)
2. Apply feature-flagged attribute
3. Test with flag enabled in development
4. Enable flag in production when ready

### Upgrading from Previous Versions

**Breaking Changes in v1.1:**

- `AddOptimizelyFeatureFlagging()` is now obsolete, use `AddFeatureFlaggedContentTypes()`
- `AddOptimizelyFeatureFlaggingExtensions()` is now obsolete, use `AddFeatureFlaggedContentTypes<TProvider>()`
- Direct `IFeatureManager` usage replaced with `IFeatureFlagProvider` abstraction

**Migration Steps:**

```csharp
// Old (still works but obsolete)
services.AddOptimizelyFeatureFlagging();

// New
services.AddFeatureFlaggedContentTypes(); // Uses Microsoft Feature Management by default

// Old (custom setup)
services.AddFeatureManagement();
services.AddOptimizelyFeatureFlaggingExtensions();

// New (custom provider)
services.AddSingleton<ILdClient>(...);
services.AddFeatureFlaggedContentTypes<LaunchDarklyFeatureFlagProvider>();
```

## Troubleshooting

### Properties Not Updating

**Problem**: Changes to feature flags not reflected in CMS.

**Solution**: Content type metadata is cached and only loaded at startup. Restart the application to see property-level changes (names, visibility, etc.). Runtime changes like content type availability work without restart.

### Feature Provider Not Found

**Problem**: `IFeatureFlagProvider` cannot be resolved.

**Solution**: Ensure `AddFeatureFlaggedContentTypes()` or `AddFeatureFlaggedContentTypes<TProvider>()` is called in service registration.

### Attributes Not Applied

**Problem**: Feature-flagged attributes have no effect.

**Solution**: Check that scanner extensions are registered. Use `AddFeatureFlaggedContentTypes()` or manually register extensions.

### Change Notifications Not Working

**Problem**: `OnFlagChanged` returns null or doesn't fire.

**Solution**: Not all providers support change notifications. Microsoft Feature Management doesn't support this by default. Consider LaunchDarkly or Optimizely Feature Experimentation for dynamic updates.

## Requirements

- Optimizely CMS 12.0 or higher
- .NET 8.0, 9.0, or 10.0
- Microsoft.FeatureManagement.AspNetCore 3.0 or higher (if using default provider)

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
- [Optimizely CMS](https://www.optimizely.com/)
- [Microsoft Feature Management](https://github.com/microsoft/FeatureManagement-Dotnet) (default provider)

Supports integration with:
- [LaunchDarkly](https://launchdarkly.com/)
- [Optimizely Feature Experimentation](https://www.optimizely.com/products/experiment/feature-experimentation/)
- [Flagsmith](https://www.flagsmith.com/)
- [Unleash](https://www.getunleash.io/)
- Any custom feature flag provider

---

Made with ❤️ by [TechnicalDogsbody](https://github.com/technicaldogsbody)