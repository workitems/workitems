using System.Collections.Generic;
using System.Collections.Immutable;

namespace Violet.WorkItems.Types
{
    public class StagePropertyDescriptor
    {
        public StagePropertyDescriptor(string name, bool? isVisible, bool? isEditable, IEnumerable<ValidatorDescriptor> validators)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("message", nameof(name));
            }

            Name = name;
            IsVisible = isVisible;
            IsEditable = isEditable;
            Validators = validators?.ToImmutableArray() ?? ImmutableArray<ValidatorDescriptor>.Empty;
        }

        public string Name { get; }
        public bool? IsVisible { get; }
        public bool? IsEditable { get; }
        public ImmutableArray<ValidatorDescriptor> Validators { get; }
    }
}