using Atata;

namespace GitHub.Components
{
    public class WaitForLoadingIndicatorAttribute : WaitForElementAttribute
    {
        public WaitForLoadingIndicatorAttribute(Until until = Until.VisibleThenMissingOrHidden, TriggerEvents on = TriggerEvents.AfterClick, TriggerPriority priority = TriggerPriority.Medium)
            : base(WaitBy.Css, "include-fragment.is-error svg", until, on, priority)
        {
            ScopeSource = ScopeSource.Page;
            PresenceTimeout = 2;
            ThrowOnPresenceFailure = false;
        }
    }
}
