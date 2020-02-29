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

        public async Task<IEnumerable<ErrorMessage>> ValidateAsync(ValidationContext context)
        {
            var result = Array.Empty<ErrorMessage>();

            var property = context.WorkItem.Properties.FirstOrDefault(p => p.Name == PropertyDescriptor.Name);

            if (property != null)
            {
                var propertyContext = new PropertyValidationContext(context, property);
                var (success, code, message) = await ValidatePropertyAsync(propertyContext);

                if (!success)
                {
                    result = new ErrorMessage[] {
                        new ErrorMessage(Source, code, message, context.WorkItem.ProjectCode, context.WorkItem.Id, property.Name),
                    };
                }
            }

            return result;
        }

        protected virtual Task<(bool success, string code, string message)> ValidatePropertyAsync(PropertyValidationContext context)
            => Task.FromResult(ValidateProperty(context));

        protected virtual (bool success, string code, string message) ValidateProperty(PropertyValidationContext context)
            => (true, string.Empty, string.Empty);
    }
}