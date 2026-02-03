namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using System.Reflection;
using EPiServer.DataAbstraction.RuntimeModel;
using Microsoft.FeatureManagement;

public class FeatureFlaggedIgnoreContentScannerExtension(IFeatureManager featureManager) : ContentScannerExtension
{
    public override bool ShouldIgnoreProperty(ContentTypeModel contentTypeModel, PropertyInfo propertyInfo)
    {
        var featureScaffold = propertyInfo.GetCustomAttribute<FeatureFlaggedIgnoreAttribute>();

        if (featureScaffold == null)
        {
            return false;
        }

        bool isFeatureEnabled = featureManager.IsEnabled(featureScaffold.FeatureName);

        return !isFeatureEnabled;
    }
}
