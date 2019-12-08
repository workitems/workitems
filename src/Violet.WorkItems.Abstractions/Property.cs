namespace Violet.WorkItems
{
    public class Property
    {
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Value { get; set; }

        public Property() { }
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
}