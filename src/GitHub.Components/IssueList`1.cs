using System.Linq;
using Atata;

namespace GitHub.Components
{
    [ControlDefinition("ul", ContainingClass = "js-milestone-issues-container", ComponentTypeName = "issue list")]
    public class IssueList<TOwner> : UnorderedList<IssueSummary<TOwner>, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public IssueSummaryModel[] ToModels()
        {
            return Items.Select(x => x.ToModel()).ToArray();
        }
    }
}
