namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using System.Reflection;
using EPiServer.DataAbstraction.RuntimeModel;

public class FeatureFlaggedDisplayContentScannerExtension(IFeatureFlagProvider featureFlagProvider) : ContentScannerExtension
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

            var featureDisplay = propertyInfo.GetCustomAttribute<FeatureFlaggedDisplayAttribute>();
            if (featureDisplay == null)
            {
                continue;
            }

            bool isFeatureEnabled = featureFlagProvider.IsEnabled(featureDisplay.FeatureName);

            if (isFeatureEnabled)
            {
                if (!string.IsNullOrWhiteSpace(featureDisplay.EnabledName))
                {
                    propertyDefinition.Name = featureDisplay.EnabledName;
                }

                if (!string.IsNullOrWhiteSpace(featureDisplay.EnabledDescription))
                {
                    propertyDefinition.Description = featureDisplay.EnabledDescription;
                }

                if (!string.IsNullOrWhiteSpace(featureDisplay.EnabledGroupName))
                {
                    propertyDefinition.TabName = featureDisplay.EnabledGroupName;
                }

                if (featureDisplay.HasEnabledOrder)
                {
                    propertyDefinition.Order = featureDisplay.EnabledOrder;
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(featureDisplay.DisabledName))
                {
                    propertyDefinition.Name = featureDisplay.DisabledName;
                }

                if (!string.IsNullOrWhiteSpace(featureDisplay.DisabledDescription))
                {
                    propertyDefinition.Description = featureDisplay.DisabledDescription;
                }

                if (!string.IsNullOrWhiteSpace(featureDisplay.DisabledGroupName))
                {
                    propertyDefinition.TabName = featureDisplay.DisabledGroupName;
                }

                if (featureDisplay.HasDisabledOrder)
                {
                    propertyDefinition.Order = featureDisplay.DisabledOrder;
                }
            }
        }
    }
}
