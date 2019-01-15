using System.Linq;
using Atata;

namespace GitHub.Components
{
    [ControlDefinition("div", ContainingClass = "js-milestone-issues-container", ComponentTypeName = "issue list")]
    public class IssueList<TOwner> : ItemsControl<IssueSummary<TOwner>, TOwner>
        where TOwner : PageObject<TOwner>
    {
        public IssueSummaryModel[] ToModels()
        {
            return Items.Where(x => x.Labels.IsVisible).Select(x => x.ToModel()).ToArray();
        }
    }
}
