namespace Violet.WorkItems.Validation
{
    public class PropertyValidationContext : ValidationContext
    {
        public PropertyValidationContext(ValidationContext context, Property property)
            : base(context.WorkItemManager, context.WorkItem, context.AppliedChanges, context.InternalEdit)
        {
            Property = property;
        }

        public Property Property { get; }
    }
}