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
                    foreach (var vd in pd.Validators)
                    {
                        var validator = CreateValidator(pd, vd);

                        var errorsOfProperty = await validator.ValidatePropertyAsync(workItem, appliedChanges);

                        errors.AddRange(errorsOfProperty);
                    }
                }
            }

            return errors;
        }

        private IValidator CreateValidator(PropertyDescriptor propertyDescriptor, ValidatorDescriptor validatorDescriptor)
            => validatorDescriptor switch
            {
                MandatoryValidatorDescriptor mvd => new MandatoryValidator(propertyDescriptor, mvd),
                _ => null,
            };
    }
}