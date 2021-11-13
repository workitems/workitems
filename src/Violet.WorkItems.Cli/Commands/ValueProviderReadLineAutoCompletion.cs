using System;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;
using Violet.WorkItems.ValueProvider;

namespace Violet.WorkItems.Cli;

public class ValueProviderReadLineAutoCompletion : IAutoCompleteHandler
{
    private readonly IValueProvider _valueProvider;

    public ValueProviderReadLineAutoCompletion(IValueProvider valueProvider)
    {
        if (valueProvider is null)
        {
            throw new ArgumentNullException(nameof(valueProvider));
        }

        _valueProvider = valueProvider;
    }
    // characters to start completion from
    public char[] Separators { get; set; } = new char[] { ' ', '.', '/' };

    public string[] GetSuggestions(string text, int index)
    {
        var prefix = text[..index];

        return _valueProvider.SuggestionsAsync(prefix).Result.Select(pv => pv.Value).ToArray();
    }

    public static async Task<string> Readline(WorkItemManager manager, WorkItem wi, PropertyDescriptor propertyDescriptor, string currentValue)
    {
        ReadLine.ClearHistory();
        ReadLine.AutoCompletionHandler = null;

        if (propertyDescriptor.ValueProvider != null)
        {
            var valueProvider = manager.ValidationManager.CreateValueProvider(wi, propertyDescriptor.ValueProvider);
            if (valueProvider.IsUserExpierenceEnumerable)
            {
                foreach (var providedValue in await valueProvider.ProvideAllValuesAsync())
                {
                    ReadLine.AddHistory(providedValue.Value);
                }
            }

            ReadLine.AutoCompletionHandler = new ValueProviderReadLineAutoCompletion(valueProvider);
        }

        string result;

        if (string.IsNullOrWhiteSpace(currentValue))
        {
            result = ReadLine.Read($"{propertyDescriptor.Name}: ");
        }
        else
        {
            result = ReadLine.Read($"{propertyDescriptor.Name} [{currentValue}]: ");
        }

        return result;
    }
}
