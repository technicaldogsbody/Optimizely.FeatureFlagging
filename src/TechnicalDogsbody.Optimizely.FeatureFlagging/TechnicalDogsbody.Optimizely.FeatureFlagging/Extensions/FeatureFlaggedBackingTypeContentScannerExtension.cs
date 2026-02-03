namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using System.Reflection;
using EPiServer.DataAbstraction.RuntimeModel;
using Microsoft.FeatureManagement;

public class FeatureFlaggedBackingTypeContentScannerExtension(IFeatureManager featureManager) : ContentScannerExtension
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

            var featureBackingType = propertyInfo.GetCustomAttribute<FeatureFlaggedBackingTypeAttribute>();
            if (featureBackingType == null)
            {
                continue;
            }

            bool isFeatureEnabled = featureManager
                .IsEnabledAsync(featureBackingType.FeatureName)
                .GetAwaiter()
                .GetResult();

            var backingType = isFeatureEnabled
                ? featureBackingType.EnabledBackingType
                : featureBackingType.DisabledBackingType;

            propertyDefinition.BackingType = backingType;
        }
    }
}
