using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Validation
{
    public class ValidationManager
    {
        private readonly DescriptorManager descriptorManager;

        public ValidationManager(DescriptorManager descriptorManager)
        {
            this.descriptorManager = descriptorManager;
        }

        public async Task<IEnumerable<ErrorMessage>> ValidateAsync(WorkItem workItem, IEnumerable<PropertyChange> appliedChanges)
        {
            var errors = new List<ErrorMessage>();
            var propertyDescriptors = descriptorManager.GetCurrentPropertyDescriptors(workItem);

            var validators = new List<IValidator>();

            // validate all properties, even if no change applied. context may change.
            foreach (var pd in propertyDescriptors)
            {
                var propertyChange = appliedChanges.FirstOrDefault(c => c.Name == pd.Name);

                if (!pd.IsEditable && !(propertyChange is null))
                {
                    errors.Add(new ErrorMessage(nameof(WorkItemManager), string.Empty, "The field is currently not editable", workItem.ProjectCode, workItem.Id, pd.Name));
                }
                else
                {
                    validators.AddRange(pd.Validators.Select(vd => CreatePropertyValidator(pd, vd)).Where(v => v != null));
                }

                if (pd.ValueProvider != null)
                {
                    var validator = CreateValueProviderValidator(pd, pd.ValueProvider);

                    if (!(validator is null))
                    {
                        validators.Add(validator);
                    }
                }
            }

            foreach (var validator in validators)
            {
                var errorsOfProperty = await validator.ValidatePropertyAsync(workItem, appliedChanges);

                errors.AddRange(errorsOfProperty);
            }

            return errors;
        }

        private IValidator CreatePropertyValidator(PropertyDescriptor propertyDescriptor, ValidatorDescriptor validatorDescriptor)
            => validatorDescriptor switch
            {
                MandatoryValidatorDescriptor mvd => new MandatoryValidator(propertyDescriptor, mvd),
                _ => null,
            };

        private IValidator CreateValueProviderValidator(PropertyDescriptor propertyDescriptor, ValueProviderDescriptor valueProviderDescriptor)
            => valueProviderDescriptor switch
            {
                EnumValueProviderDescriptor evpd => new EnumValidator(propertyDescriptor, evpd),
                ProjectCollectionValueProviderDescriptor pcvpd => null,
                ProjectUserValueProviderDescriptor puvpd => null,
                RelationshipValueProviderDescriptor rvpd => null,
                _ => null,
            };
    }
}