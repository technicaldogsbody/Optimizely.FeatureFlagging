# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

## [1.0.0] - 2026-02-03

### Added
- Initial release
- `FeatureFlaggedBackingTypeAttribute` for dynamic property backing types
- `FeatureFlaggedClientEditorAttribute` for switching property editors
- `FeatureFlaggedCultureSpecificAttribute` for culture-specific property control
- `FeatureFlaggedDisplayAttribute` for dynamic display metadata
- `FeatureFlaggedEditableAttribute` for read-only property control
- `FeatureFlaggedIgnoreAttribute` for conditional property exclusion
- `FeatureFlaggedRangeAttribute` for dynamic numeric validation
- `FeatureFlaggedRegularExpressionAttribute` for pattern validation switching
- `FeatureFlaggedRequiredAttribute` for conditional required validation
- `FeatureFlaggedScaffoldColumnAttribute` for property visibility control
- `FeatureFlaggedSearchableAttribute` for search indexing control
- `FeatureFlaggedStringLengthAttribute` for dynamic string length validation
- `FeatureFlaggedUIHintAttribute` for UI hint switching
- Content scanner extensions for all attributes
- `AddOptimizelyFeatureFlagging()` service collection extension
- `AddOptimizelyFeatureFlaggingExtensions()` for custom Feature Management setup
- Support for .NET 8.0, 9.0, and 10.0
- Support for Optimizely CMS 12+
- Microsoft Feature Management integration

[Unreleased]: https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/compare/v1.0.0...HEAD
[1.0.0]: https://github.com/technicaldogsbody/Optimizely.FeatureFlagging/releases/tag/v1.0.0
