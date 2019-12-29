using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.ValueProvider
{
    public interface IValueProvider
    {
        Task<IEnumerable<ProvidedValue>> ProvideAllValuesAsync();
        Task<bool> ValueExists(string value);
        Task<IEnumerable<ProvidedValue>> SuggestionsAsync(string input);
    }
}