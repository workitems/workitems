## Validator Internals

**Behavior**

- Validators are executed before the work item is handed over to the IDataProvider.
- Validators are applied if specified directly on the work item or in a work item stage (stage is evaluated on the current values)

**Property Validators (root or staged)**

- Property Validator validate the value of an individual property value.

### Mandatory

Checks if the contextual property is present.

- **Scope**: Property
- **Type**: `MandatoryValidator`
- **Descriptor**: `MandatoryValidatorDescriptor`

### Immutable

Checks if the property has not be changed. Reacts to the IsEditable field.

- **Scope**: Property
- **Type**: `ImmutableValidator`
- **Descriptor**: None

### Completeness

Checks if the workitem has a full property set according to its type definition

- **Scope**: WorkItem
- **Type**: `CompletenessValidator`
- **Descriptor**: List of PropertyDescriptor.

### ValueProvider (Validator)

Checks if the value of the property is found in the ValueProvider

- **Scope**: Property
- **Type**: `ValueProviderValidator`
- **Descriptor**: ValueProvider descriptor