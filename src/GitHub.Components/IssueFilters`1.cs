using System.Linq;
using Atata;

namespace GitHub.Components
{
    [ControlDefinition("div", ContainingClass = "table-list-filters")]
    public class IssueFilters<TOwner> : Control<TOwner>
        where TOwner : PageObject<TOwner>
    {
        public StatesFilter States { get; private set; }

        [ControlDefinition("div", ContainingClass = "states")]
        public class StatesFilter : Control<TOwner>
        {
            [FindByIndex(0)]
            [WaitForLoadingIndicator]
            public ButtonLink Open { get; private set; }

            [FindByIndex(1)]
            [WaitForLoadingIndicator]
            public ButtonLink Closed { get; private set; }

            public TOwner Toggle()
            {
                (Open.IsSelected ? Closed : Open).Click();

                return Owner;
            }

            [ControlDefinition("a", ContainingClass = "btn-link")]
            public class ButtonLink : Link<TOwner>
            {
                public ValueProvider<bool, TOwner> IsSelected => CreateValueProvider(
                    "selected",
                    () => DomClasses.Value.Contains("selected"));
            }
        }
    }
}
