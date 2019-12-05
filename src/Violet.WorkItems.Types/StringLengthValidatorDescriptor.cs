namespace Violet.WorkItems.Types
{
    public class StringLengthValidatorDescriptor : ValidatorDescriptor
    {
        public StringLengthValidatorDescriptor(int min, int max)
            : base("StringLength")
        {
            Min = min;
            Max = max;
        }

        public int Min { get; }
        public int Max { get; }
    }
}