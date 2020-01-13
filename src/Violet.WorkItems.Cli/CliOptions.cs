using System;

namespace Violet.WorkItems.Cli
{
    public class CliOptions
    {
        public DataSourceDescriptor[] Sources { get; set; } = Array.Empty<DataSourceDescriptor>();
    }

    public class DataSourceDescriptor
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
    }
}