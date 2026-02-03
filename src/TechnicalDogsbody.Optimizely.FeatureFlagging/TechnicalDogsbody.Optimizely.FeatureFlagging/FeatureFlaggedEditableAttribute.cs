namespace TechnicalDogsbody.Optimizely.FeatureFlagging;

using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.FeatureManagement;
using TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

/// <summary>
/// Conditionally controls whether a property is editable based on a feature flag.
/// When the feature is enabled, editability matches editableWhenEnabled parameter.
/// When the feature is disabled, editability is the inverse.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class FeatureFlaggedEditableAttribute(string featureName, bool editableWhenEnabled = true) : Attribute, IDisplayMetadataProvider
{
    public string FeatureName { get; } = featureName ?? throw new ArgumentNullException(nameof(featureName));
    public bool EditableWhenEnabled { get; } = editableWhenEnabled;

    public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
    {
        var featureManager = ServiceLocator.Current.GetInstance<IFeatureManager>();

        bool isFeatureEnabled = featureManager.IsEnabled(FeatureName);

        bool shouldBeEditable = EditableWhenEnabled ? isFeatureEnabled : !isFeatureEnabled;

        if (!shouldBeEditable)
        {
            if (context.DisplayMetadata.AdditionalValues[ExtendedMetadata.ExtendedMetadataDisplayKey] is not ExtendedMetadata extendedMetadata)
            {
                return;
            }

            extendedMetadata.IsReadOnly = true;
        }
    }
}
