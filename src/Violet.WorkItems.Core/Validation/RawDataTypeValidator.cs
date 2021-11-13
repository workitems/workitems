using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Violet.WorkItems.Validation;

public class RawDataTypeValidator : IValidator
{
    public string PropertyName { get; }

    public RawDataTypeValidator(string propertyName)
    {
        PropertyName = propertyName ?? throw new ArgumentNullException(nameof(propertyName));
    }

    public Task<IEnumerable<ErrorMessage>> ValidateAsync(ValidationContext context)
    {
        var result = new List<ErrorMessage>();

        var workItem = context.WorkItem;

        var property = workItem.Properties.FirstOrDefault(p => p.Name == PropertyName);

        if (property != null)
        {
            if (!ValueTypesManager.IsValidDataType(property.DataType))
            {
                result.Add(new ErrorMessage(nameof(RawDataTypeValidator), string.Empty, $"DataType '{property.DataType}' on Property '{property.Name}' is not a recognized property type.", workItem.ProjectCode, workItem.Id, property.Name));
            }
            else if (!ValueTypesManager.IsValidData(property.DataType, property.Value))
            {
                result.Add(new ErrorMessage(nameof(RawDataTypeValidator), string.Empty, $"DataType '{property.DataType}' on Property '{property.Name}' does not match property value.", workItem.ProjectCode, workItem.Id, property.Name));
            }
        }

        return Task.FromResult((IEnumerable<ErrorMessage>)result);
    }
}
