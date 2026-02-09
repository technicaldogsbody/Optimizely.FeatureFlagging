namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using EPiServer.ServiceLocation;
using Microsoft.FeatureManagement;
using System.ComponentModel.DataAnnotations;
using TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

/// <summary>
/// Conditionally applies Range validation based on a feature flag.
/// When the feature is enabled, uses enabledMinimum/enabledMaximum.
/// When the feature is disabled, uses disabledMinimum/disabledMaximum.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedRangeAttribute(
    string featureName,
    double enabledMinimum,
    double enabledMaximum,
    double disabledMinimum,
    double disabledMaximum)
    : ValidationAttribute
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public double EnabledMinimum { get; } = enabledMinimum;
    public double EnabledMaximum { get; } = enabledMaximum;
    public double DisabledMinimum { get; } = disabledMinimum;
    public double DisabledMaximum { get; } = disabledMaximum;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var featureFlagProvider = ServiceLocator.Current.GetInstance<IFeatureFlagProvider>();

        bool isFeatureEnabled = featureFlagProvider.IsEnabled(FeatureName);

        double minimum = isFeatureEnabled ? EnabledMinimum : DisabledMinimum;
        double maximum = isFeatureEnabled ? EnabledMaximum : DisabledMaximum;

        if (value == null)
        {
            return ValidationResult.Success;
        }

        double numericValue;
        try
        {
            numericValue = Convert.ToDouble(value);
        }
        catch(InvalidCastException)
        {
            return new ValidationResult($"{validationContext.DisplayName} must be a numeric value.");
        }

        if (numericValue < minimum || numericValue > maximum)
        {
            return new ValidationResult(
                ErrorMessage ?? $"{validationContext.DisplayName} must be between {minimum} and {maximum}.");
        }

        return ValidationResult.Success;
    }
}
