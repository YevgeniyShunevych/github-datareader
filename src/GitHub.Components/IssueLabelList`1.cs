using Atata;

namespace GitHub.Components
{
    [ControlDefinition("*", ContainingClass = "labels", ComponentTypeName = "label list")]
    [FindFirst]
    public class IssueLabelList<TOwner> : ItemsControl<IssueLabel<TOwner>, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
