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

        public ValueProvider<int, TOwner> Number =>
            CreateValueProvider("number", GetNumber);

        private int GetNumber()
        {
            string numberAsString = Title.Href.Value.Split('/').Last();
            return int.Parse(numberAsString);
        }

        public IssueSummaryModel ToModel()
        {
            var title = Title.GetContent(ContentSource.InnerHtml)
                .Value.Replace("<code>", "`").Replace("</code>", "`");

            return new IssueSummaryModel
            {
                Number = Number,
                Title = title,
                Labels = Labels.Items.Select(x => x.Value).ToArray()
            };
        }
    }
}
