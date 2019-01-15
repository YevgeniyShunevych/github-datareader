using System.Linq;
using Atata;

namespace GitHub.Components
{
    [ControlDefinition("div", ContainingClass = "js-issue-row", ComponentTypeName = "issue")]
    public class IssueSummary<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        [FindByClass("h4")]
        public Link<TOwner> Title { get; private set; }

        public IssueLabelList<TOwner> Labels { get; private set; }

        public DataProvider<int, TOwner> Number => GetOrCreateDataProvider("number", GetNumber);

        private int GetNumber()
        {
            string numberAsString = Title.Attributes.Href.Value.Split('/').Last();
            return int.Parse(numberAsString);
        }

        public IssueSummaryModel ToModel()
        {
            return new IssueSummaryModel
            {
                Number = Number,
                Title = Title.Content,
                Labels = Labels.Items.Select(x => x.Value).ToArray()
            };
        }
    }
}
