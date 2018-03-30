using Atata;

namespace GitHub.Components
{
    [ControlDefinition("a", ContainingClass = "IssueLabel", ComponentTypeName = "label")]
    public class IssueLabel<TOwner> : Content<string, TOwner>
        where TOwner : PageObject<TOwner>
    {
    }
}
