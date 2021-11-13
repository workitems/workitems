using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;
using Violet.WorkItems.ValueProvider;

namespace Violet.WorkItems.Validation;

public class ValidationManager
{
    private readonly WorkItemManager _workItemManager;
    private readonly DescriptorManager _descriptorManager;

    public ValidationManager(WorkItemManager workItemManager, DescriptorManager descriptorManager)
    {
        _workItemManager = workItemManager;
        _descriptorManager = descriptorManager;
    }

    public async Task<IEnumerable<ErrorMessage>> ValidateAsync(WorkItem workItem, IEnumerable<PropertyChange> appliedChanges, bool internalEdit)
    {
        var errors = new List<ErrorMessage>();
        var validators = CreateValidators(workItem);

        foreach (var validator in validators)
        {
            var errorsOfProperty = await validator.ValidateAsync(new ValidationContext(_workItemManager, workItem, appliedChanges, internalEdit));

            errors.AddRange(errorsOfProperty);
        }

        return errors;
    }

    private IEnumerable<IValidator> CreateValidators(WorkItem workItem)
    {
        var validators = new List<IValidator?>();

        validators.AddRange(CreateWorkItemValidators(workItem));

        var propertyDescriptors = _descriptorManager.GetCurrentPropertyDescriptors(workItem);
        // validate all properties, even if no change applied. context may change.
        foreach (var pd in propertyDescriptors)
        {
            validators.AddRange(CreatePropertyValidatorByPropertyDescriptor(pd));

            validators.AddRange(pd.Validators.Select(vd => CreatePropertyValidatorByValidatorDescriptor(pd, vd)));

            validators.Add(CreateValueProviderValidator(workItem, pd, pd.ValueProvider));
        }

        return validators.Where(v => v != null).Cast<IValidator>();
    }

    private IEnumerable<IValidator> CreateWorkItemValidators(WorkItem workItem)
    {
        yield return new CompletenessValidator(_descriptorManager.GetCurrentPropertyDescriptors(workItem));
    }

    private static IEnumerable<IValidator> CreatePropertyValidatorByPropertyDescriptor(PropertyDescriptor propertyDescriptor)
    {
        if (propertyDescriptor.IsEditable == false)
        {
            yield return new ImmutableValidator(propertyDescriptor);
        }
        if (propertyDescriptor.PropertyType == PropertyType.SingleRaw)
        {
            yield return new RawDataTypeValidator(propertyDescriptor.Name);
        }
    }

    private static IValidator? CreatePropertyValidatorByValidatorDescriptor(PropertyDescriptor propertyDescriptor, ValidatorDescriptor validatorDescriptor)
        => validatorDescriptor switch
        {
            MandatoryValidatorDescriptor mvd => new MandatoryValidator(propertyDescriptor, mvd),
            StringLengthValidatorDescriptor slvd => new StringLengthValidator(propertyDescriptor, slvd),
            _ => null,
        };

    private IValidator? CreateValueProviderValidator(WorkItem workItem, PropertyDescriptor propertyDescriptor, ValueProviderDescriptor? valueProviderDescriptor)
        => valueProviderDescriptor is null ? null : new ValueProviderValidator(propertyDescriptor, CreateValueProvider(workItem, valueProviderDescriptor));

    public IValueProvider CreateValueProvider(WorkItem workItem, ValueProviderDescriptor valueProviderDescriptor)
        => valueProviderDescriptor switch
        {
            EnumValueProviderDescriptor evpd => new EnumValueProvider(evpd),
            ProjectCollectionValueProviderDescriptor => throw new NotImplementedException(),
            ProjectUserValueProviderDescriptor => throw new NotImplementedException(),
            RelationshipValueProviderDescriptor rvpd => new RelationshipValueProvider(rvpd, _workItemManager, workItem.ProjectCode),
            _ => throw new NotImplementedException(),
        };
}
