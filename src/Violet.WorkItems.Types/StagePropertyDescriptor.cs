using System;
using System.Collections.Generic;

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
            Validators = validators ?? Array.Empty<ValidatorDescriptor>();
        }

        public string Name { get; }
        public bool? IsVisible { get; }
        public bool? IsEditable { get; }
        public IEnumerable<ValidatorDescriptor> Validators { get; }
    }
}