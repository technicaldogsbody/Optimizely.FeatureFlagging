namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.FeatureManagement;
using System.ComponentModel.DataAnnotations;
using TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedRequiredAttribute(string featureName, bool requiredWhenEnabled = true) : ValidationAttribute, IDisplayMetadataProvider
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public bool RequiredWhenEnabled { get; } = requiredWhenEnabled;

    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        var featureFlagProvider = ServiceLocator.Current.GetInstance<IFeatureFlagProvider>();

        bool isFeatureEnabled = featureFlagProvider.IsEnabled(FeatureName);

        bool shouldBeRequired = RequiredWhenEnabled ? isFeatureEnabled : !isFeatureEnabled;

        if (shouldBeRequired)
        {
            if (context.DisplayMetadata.AdditionalValues[ExtendedMetadata.ExtendedMetadataDisplayKey] is not ExtendedMetadata extendedMetadata)
            {
                return;
            }

            extendedMetadata.IsRequired = true;
        }
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var featureFlagProvider = ServiceLocator.Current.GetInstance<IFeatureFlagProvider>();

        bool isFeatureEnabled = featureFlagProvider.IsEnabled(FeatureName);

        bool shouldBeRequired = RequiredWhenEnabled ? isFeatureEnabled : !isFeatureEnabled;

        if (!shouldBeRequired)
        {
            return ValidationResult.Success!;
        }

        if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
        {
            return new ValidationResult(ErrorMessage ?? $"{validationContext.DisplayName} is required.");
        }

        return ValidationResult.Success!;
    }
}
