using System.Collections.Generic;
using System.Threading.Tasks;

namespace Violet.WorkItems.ValueProvider
{
    public interface IValueProvider
    {
        Task<string> GetDefaultValueAsync();
        Task<IEnumerable<ProvidedValue>> ProvideAllValuesAsync();
        Task<bool> ValueExistsAsync(string value);
        Task<IEnumerable<ProvidedValue>> SuggestionsAsync(string input);
        bool IsValidEncoding(string value);
        bool IsUserExpierenceEnumerable { get; }
    }
}