namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using System.ComponentModel.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.FeatureManagement;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedRequiredAttribute(string featureName, bool requiredWhenEnabled = true) : ValidationAttribute, IDisplayMetadataProvider
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public bool RequiredWhenEnabled { get; } = requiredWhenEnabled;

    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        var featureManager = ServiceLocator.Current.GetInstance<IFeatureManager>();

        bool isFeatureEnabled = featureManager
            .IsEnabledAsync(FeatureName)
            .GetAwaiter()
            .GetResult();

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
        var featureManager = ServiceLocator.Current.GetInstance<IFeatureManager>();

        bool isFeatureEnabled = featureManager
            .IsEnabledAsync(FeatureName)
            .GetAwaiter()
            .GetResult();

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
