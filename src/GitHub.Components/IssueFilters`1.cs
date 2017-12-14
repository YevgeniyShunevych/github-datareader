﻿using System.Linq;
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
            public ButtonLink Open { get; private set; }

            [FindByIndex(1)]
            public ButtonLink Closed { get; private set; }

            [ControlDefinition("a", ContainingClass = "btn-link")]
            public class ButtonLink : Link<TOwner>
            {
                public DataProvider<bool, TOwner> IsSelected => GetOrCreateDataProvider(
                    "selected",
                    () => Attributes.Class.Value.Contains("selected"));
            }
        }
    }
}
