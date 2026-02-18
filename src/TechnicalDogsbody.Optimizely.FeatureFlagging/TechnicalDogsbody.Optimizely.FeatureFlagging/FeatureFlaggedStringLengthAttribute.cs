namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using EPiServer.ServiceLocation;
using Microsoft.FeatureManagement;
using System.ComponentModel.DataAnnotations;
using TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

/// <summary>
/// Conditionally applies StringLength validation based on a feature flag.
/// When the feature is enabled, uses enabledMaximumLength/enabledMinimumLength.
/// When the feature is disabled, uses disabledMaximumLength/disabledMinimumLength.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedStringLengthAttribute(
    string featureName,
    int enabledMaximumLength,
    int disabledMaximumLength)
    : ValidationAttribute
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public int EnabledMaximumLength { get; } = enabledMaximumLength;
    public int EnabledMinimumLength { get; set; }
    public int DisabledMaximumLength { get; } = disabledMaximumLength;
    public int DisabledMinimumLength { get; set; }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var featureFlagProvider = ServiceLocator.Current.GetInstance<IFeatureFlagProvider>();

        bool isFeatureEnabled = featureFlagProvider.IsEnabled(FeatureName);

        int maximumLength = isFeatureEnabled ? EnabledMaximumLength : DisabledMaximumLength;
        int minimumLength = isFeatureEnabled ? EnabledMinimumLength : DisabledMinimumLength;

        if (value == null)
        {
            return ValidationResult.Success;
        }

        if (value is not string stringValue)
        {
            return new ValidationResult($"{validationContext.DisplayName} must be a string.");
        }

        if (stringValue.Length < minimumLength)
        {
            return new ValidationResult(
                ErrorMessage ?? $"{validationContext.DisplayName} must be at least {minimumLength} characters long.");
        }

        if (stringValue.Length > maximumLength)
        {
            return new ValidationResult(
                ErrorMessage ?? $"{validationContext.DisplayName} must be no more than {maximumLength} characters long.");
        }

        return ValidationResult.Success;
    }
}
