using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation
{
    public abstract class PropertyWithPropertyDescriptorValidatorBase : IValidator
    {
        public PropertyDescriptor PropertyDescriptor { get; }
        public string Source { get; }

        public PropertyWithPropertyDescriptorValidatorBase(PropertyDescriptor propertyDescriptor, string source)
        {
            PropertyDescriptor = propertyDescriptor ?? throw new ArgumentNullException(nameof(propertyDescriptor));
            Source = source;
        }

        public Task<IEnumerable<ErrorMessage>> ValidateAsync(WorkItem workItem, IEnumerable<PropertyChange> appliedChanges)
        {
            var result = Array.Empty<ErrorMessage>();

            var property = workItem.Properties.FirstOrDefault(p => p.Name == PropertyDescriptor.Name);

            if (!ValidateProperty(property, out var code, out var message))
            {
                result = new ErrorMessage[] {
                    new ErrorMessage(Source, code, message, workItem.ProjectCode, workItem.Id, property.Name),
                };
            }

            return Task.FromResult((IEnumerable<ErrorMessage>)result);
        }

        protected abstract bool ValidateProperty(Property property, out string code, out string message);
    }
}