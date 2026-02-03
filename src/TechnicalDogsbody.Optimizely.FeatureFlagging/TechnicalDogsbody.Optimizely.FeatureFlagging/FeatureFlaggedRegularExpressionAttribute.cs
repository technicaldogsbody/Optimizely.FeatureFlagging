namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using EPiServer.ServiceLocation;
using Microsoft.FeatureManagement;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

/// <summary>
/// Conditionally applies RegularExpression validation based on a feature flag.
/// When the feature is enabled, uses enabledPattern.
/// When the feature is disabled, uses disabledPattern.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedRegularExpressionAttribute(string featureName, string enabledPattern, string disabledPattern)
    : ValidationAttribute
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public string EnabledPattern { get; } = enabledPattern ?? throw new ArgumentNullException(nameof(enabledPattern));
    public string DisabledPattern { get; } = disabledPattern ?? throw new ArgumentNullException(nameof(disabledPattern));

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var featureManager = ServiceLocator.Current.GetInstance<IFeatureManager>();

        bool isFeatureEnabled = featureManager.IsEnabled(FeatureName);

        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is not string stringValue)
        {
            return new ValidationResult($"{validationContext.DisplayName} must be a string.");
        }

        var regex = isFeatureEnabled
            ? new Regex(EnabledPattern, RegexOptions.Compiled)
            : new Regex(DisabledPattern, RegexOptions.Compiled);

        if (!regex.IsMatch(stringValue))
        {
            return new ValidationResult(
                ErrorMessage ?? $"{validationContext.DisplayName} does not match the required pattern.");
        }

        return ValidationResult.Success;
    }
}
