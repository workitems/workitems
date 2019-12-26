## Value Provider Internals

**Behavior**

- ValueProviders are converted into validators

### EnumValueProvider

Provides the values for a property from a static enumeration (e.g. used for states).

- **ValueProvider**: not yet
- **Descriptor**: `EnumValueDescriptor`
- **Validator**: `EnumValidator`

### ProjectCollectionValueProvider

Provides the values for a property from a project specific collection (e.g. used for labels, milestones, ...)

- **ValueProvider**: not yet
- **Descriptor**: `ProjectCollectionValueProviderDescriptor`
- **Validator**: not yet

### ProjectUserValueProvider

Provides the values for a property from a project specific user source (e.g. the authentication system).

- **ValueProvider**: not yet
- **Descriptor**: `ProjectUserValueProviderDescriptor`
- **Validator**: not yet

### RelationshipValueProvider

Provides the values for a property from a relationship to another work item.

- **ValueProvider**: not yet
- **Descriptor**: `RelationshipValueProviderDescriptor`
- **Validator**: not yet