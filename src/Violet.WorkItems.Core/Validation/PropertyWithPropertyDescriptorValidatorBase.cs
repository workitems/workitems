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

        public async Task<IEnumerable<ErrorMessage>> ValidateAsync(WorkItem workItem, IEnumerable<PropertyChange> appliedChanges)
        {
            var result = Array.Empty<ErrorMessage>();

            var property = workItem.Properties.FirstOrDefault(p => p.Name == PropertyDescriptor.Name);

            if (property != null)
            {
                var (success, code, message) = await ValidatePropertyAsync(property);

                if (!success)
                {
                    result = new ErrorMessage[] {
                        new ErrorMessage(Source, code, message, workItem.ProjectCode, workItem.Id, property.Name),
                    };
                }
            }

            return result;
        }

        protected virtual Task<(bool success, string code, string message)> ValidatePropertyAsync(Property property)
            => Task.FromResult(ValidateProperty(property));

        protected virtual (bool success, string code, string message) ValidateProperty(Property property)
            => (true, string.Empty, string.Empty);
    }
}