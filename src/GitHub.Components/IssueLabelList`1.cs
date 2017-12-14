using Atata;

namespace GitHub.Components
{
    [ControlDefinition("*", ContainingClass = "labels", ComponentTypeName = "label list")]
    [ControlFinding(typeof(FindFirstAttribute))]
    public class IssueLabelList<TOwner> : ItemsControl<IssueLabel<TOwner>, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
