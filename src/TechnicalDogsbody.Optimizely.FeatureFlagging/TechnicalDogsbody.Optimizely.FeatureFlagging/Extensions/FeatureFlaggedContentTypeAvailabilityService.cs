namespace TechnicalDogsbody.Optimizely.FeatureFlagging.Extensions;

using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Security;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

public class FeatureFlaggedContentTypeAvailabilityService(
    ContentTypeAvailabilityService defaultService,
    IContentTypeRepository contentTypeRepository,
    IFeatureFlagProvider featureFlagProvider)
    : ContentTypeAvailabilityService
{
    public override AvailableSetting GetSetting(string contentTypeName) => defaultService.GetSetting(contentTypeName);

    public override bool IsAllowed(string parentContentTypeName, string childContentTypeName)
    {
        if (!IsFeatureEnabled(childContentTypeName))
        {
            return false;
        }

        return defaultService.IsAllowed(parentContentTypeName, childContentTypeName);
    }

    public override IList<ContentType> ListAvailable(string contentTypeName, IPrincipal user)
    {
        var availableTypes = defaultService.ListAvailable(contentTypeName, user);

        return availableTypes.Where(ct => IsFeatureEnabled(ct.Name)).ToList();
    }

    public override IList<ContentType> ListAvailable(IContent content, bool contentFolder, IPrincipal user)
    {
        var availableTypes = defaultService.ListAvailable(content, contentFolder, user);

        return availableTypes.Where(ct => IsFeatureEnabled(ct.Name)).ToList();
    }

    private bool IsFeatureEnabled(string contentTypeName)
    {
        var contentType = contentTypeRepository.Load(contentTypeName);
        var modelType = contentType?.ModelType;

        if (modelType == null)
        {
            return true;
        }

        if (modelType.GetCustomAttributes(typeof(FeatureFlaggedContentTypeAttribute), false)
                .FirstOrDefault() is FeatureFlaggedContentTypeAttribute contentTypeAttr)
        {
            bool featureEnabled = featureFlagProvider.IsEnabled(contentTypeAttr.FeatureFlag);
            return contentTypeAttr.VisibleWhenEnabled ? featureEnabled : !featureEnabled;
        }

        if (modelType.GetCustomAttributes(typeof(FeatureFlaggedVisualBuilderTypeAttribute), false)
                .FirstOrDefault() is FeatureFlaggedVisualBuilderTypeAttribute vbAttr)
        {
            bool featureEnabled = featureFlagProvider.IsEnabled(vbAttr.FeatureFlag);
            return vbAttr.VisibleWhenEnabled ? featureEnabled : !featureEnabled;
        }

        return true;
    }
}
