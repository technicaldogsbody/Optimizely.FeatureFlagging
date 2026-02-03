namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using System.Reflection;
using EPiServer.DataAbstraction.RuntimeModel;
using Microsoft.FeatureManagement;

public class FeatureFlaggedScaffoldColumnContentScannerExtension(IFeatureManager featureManager) : ContentScannerExtension
{
    private readonly IFeatureManager _featureManager = featureManager;

    public override bool ShouldIgnoreProperty(ContentTypeModel contentTypeModel, PropertyInfo propertyInfo)
    {
        var featureScaffold = propertyInfo.GetCustomAttribute<FeatureFlaggedScaffoldColumnAttribute>();

        if (featureScaffold == null)
        {
            return false;
        }

        bool isFeatureEnabled = _featureManager
            .IsEnabledAsync(featureScaffold.FeatureName)
            .GetAwaiter()
            .GetResult();

        bool shouldScaffold = featureScaffold.ScaffoldWhenEnabled ? isFeatureEnabled : !isFeatureEnabled;

        return !shouldScaffold;
    }
}
