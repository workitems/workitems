namespace Violet.WorkItems.Cli
{
    public class CliOptions
    {
        public DataSourceDescriptor[] Sources { get; set; }
    }
    public class DataSourceDescriptor
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ConnectionString { get; set; }
    }
}