using System;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation;

public abstract class PropertyWithValidatorDescriptorValidatorBase<TValidatorDescriptor> : PropertyWithPropertyDescriptorValidatorBase
{
    public TValidatorDescriptor ValidatorDescriptor { get; }

    public PropertyWithValidatorDescriptorValidatorBase(PropertyDescriptor propertyDescriptor, TValidatorDescriptor validatorDescriptor, string source)
        : base(propertyDescriptor, source)
    {
        ValidatorDescriptor = validatorDescriptor ?? throw new ArgumentNullException(nameof(validatorDescriptor));
    }
}
