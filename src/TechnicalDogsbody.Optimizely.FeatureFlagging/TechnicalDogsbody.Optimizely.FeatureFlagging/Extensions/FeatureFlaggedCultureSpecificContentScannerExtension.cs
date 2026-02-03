namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using System.Reflection;
using EPiServer.DataAbstraction.RuntimeModel;
using Microsoft.FeatureManagement;

public class FeatureFlaggedCultureSpecificContentScannerExtension(IFeatureManager featureManager) : ContentScannerExtension
{
    public override void AssignValuesToProperties(ContentTypeModel contentTypeModel)
    {
        base.AssignValuesToProperties(contentTypeModel);

        foreach (var propertyDefinition in contentTypeModel.PropertyDefinitionModels)
        {
            var propertyInfo = contentTypeModel.ModelType.GetProperty(propertyDefinition.Name);
            if (propertyInfo == null)
            {
                continue;
            }

            var featureCultureSpecific = propertyInfo.GetCustomAttribute<FeatureFlaggedCultureSpecificAttribute>();
            if (featureCultureSpecific == null)
            {
                continue;
            }

            bool isFeatureEnabled = featureManager
                .IsEnabledAsync(featureCultureSpecific.FeatureName)
                .GetAwaiter()
                .GetResult();

            bool shouldBeCultureSpecific = featureCultureSpecific.CultureSpecificWhenEnabled ? isFeatureEnabled : !isFeatureEnabled;

            propertyDefinition.CultureSpecific = shouldBeCultureSpecific;
        }
    }
}
