using Atata;

namespace GitHub.Components
{
    using _ = MilestonePage;

    [WaitForLoadingIndicator(on: TriggerEvents.Init)]
    public class MilestonePage : Page<_>
    {
        public IssueFilters<_> Filters { get; private set; }

        public IssueList<_> Issues { get; private set; }
    }
}
