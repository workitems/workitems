namespace Violet.WorkItems;

public class Property
{
    public static readonly string NullValue = string.Empty;

    public string Name { get; set; }
    public string DataType { get; set; }
    public string Value { get; set; }

    public Property()
    {
        Name = string.Empty;
        DataType = string.Empty;
        Value = string.Empty;
    }
    public Property(string name, string dataType, string value)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new System.ArgumentException("message", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(dataType))
        {
            throw new System.ArgumentException("message", nameof(dataType));
        }

        Name = name;
        DataType = dataType;
        Value = value;
    }
}
