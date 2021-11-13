using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Violet.WorkItems.Types;

namespace Violet.WorkItems.Text;

public class WorkItemFormatter
{
    public static async Task FormatAsync(DescriptorManager manager, WorkItem workItem, TextWriter writer)
    {
        var logEntryTypeDescriptors = manager.GetCurrentLogEntryTypeDescriptors(workItem);
        var commands = manager.GetCurrentCommands(workItem);

        await writer.WriteLineAsync(FormatShortLine(workItem));

        foreach (var property in workItem.Properties)
        {
            await writer.WriteLineAsync($"> {FormatProperty(property)}");
        }

        if (commands.Any())
        {
            writer.WriteLine($"? Commands: {string.Join(", ", commands.Select(c => c.Label))}");
        }

        foreach (var logEntry in workItem.Log.OrderByDescending(l => l.Date))
        {
            var formatDescription = GetFormatTemplate(logEntryTypeDescriptors, logEntry);
            var text = formatDescription
                .Replace("{date}", $"{logEntry.Date}")
                .Replace("{user}", $"{logEntry.User}")
                .Replace("{comment}", $"{logEntry.Comment}");

            await writer.WriteLineAsync(text);

            foreach (var propertyChange in logEntry.Changes)
            {
                await writer.WriteAsync("|  > ");
                await writer.WriteLineAsync(FormatPropertyChange(propertyChange));
            }
        }
    }

    private static string GetFormatTemplate(IEnumerable<LogEntryTypeDescriptor> logEntryTypeDescriptors, LogEntry logEntry)
    {
        string result = "✍  Change at {date} by {user} with comment: {comment}";

        foreach (var descriptor in logEntryTypeDescriptors)
        {
            if (IsLogEntryTypeMatch(descriptor, logEntry))
            {
                result = CreateTemplate(descriptor);

                break;
            }
        }

        return result;
    }

    private static string CreateTemplate(LogEntryTypeDescriptor descriptor)
        => descriptor switch
        {
            PropertyChangeLogEntryTypeDescriptor pcletc => $"{pcletc.Emoji}  {pcletc.Message}",
            _ => string.Empty,
        };

    private static bool IsLogEntryTypeMatch(LogEntryTypeDescriptor descriptor, LogEntry logEntry)
        => (descriptor, logEntry) switch
        {
            (PropertyChangeLogEntryTypeDescriptor pcletd, _) when (logEntry.Changes.Any(c => c.Name == pcletd.PropertyName && (pcletd.TargetValue == null || pcletd.TargetValue == c.NewValue))) => true,
            _ => false,
        };

    public static string FormatShortLine(WorkItem workItem)
        => $"{FormatId(workItem)} ({workItem.WorkItemType}): {GetTitleProperty(workItem)}";

    private static string FormatProperty(Property property)
        => $"{property.Name}: {property.Value}";

    private static string FormatPropertyChange(PropertyChange propertyChange)
        => $"{propertyChange.Name}: {propertyChange.OldValue} => {propertyChange.NewValue}";

    private static string GetTitleProperty(WorkItem workItem)
    {
        var result = string.Empty;

        foreach (var propertyName in new string[] {
                "Title",
                "Name",
            })
        {
            var property = workItem.Properties.FirstOrDefault(p => p.Name == propertyName);

            if (property != null)
            {
                property.Value(out result);

                result ??= string.Empty;

                break;
            }
        }

        return result;
    }

    public static string FormatId(WorkItem workItem)
        => $"{workItem.ProjectCode}-{workItem.Id}";
}
