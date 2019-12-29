using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.ValueProvider
{
    public class EnumValueProvider : IValueProvider
    {
        public EnumValueProviderDescriptor Descriptor { get; }

        public EnumValueProvider(EnumValueProviderDescriptor descriptor)
        {
            Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor));
        }

        public Task<IEnumerable<ProvidedValue>> ProvideAllValuesAsync()
            => Task.FromResult(Descriptor.Values.Select(ev => new ProvidedValue(ev.Value, ev.DisplayText)));

        public Task<IEnumerable<ProvidedValue>> SuggestionsAsync(string input)
            => Task.FromResult(
                    Descriptor.Values
                    .Where(ev => ev.DisplayText.StartsWith(input, StringComparison.InvariantCultureIgnoreCase))
                    .Select(ev => new ProvidedValue(ev.Value, ev.DisplayText))
                );

        public Task<bool> ValueExistsAsync(string value)
            => Task.FromResult(Descriptor.Values.Any(ev => ev.Value == value));

        public Task<string> GetDefaultValueAsync()
            => Task.FromResult(Descriptor.Values.FirstOrDefault()?.Value);
    }
}