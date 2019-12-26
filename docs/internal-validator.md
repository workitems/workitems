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