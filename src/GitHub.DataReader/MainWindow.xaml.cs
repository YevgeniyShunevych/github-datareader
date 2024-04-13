using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Atata;
using Atata.WebDriverSetup;
using GitHub.Components;

namespace GitHub.DataReader;

public partial class MainWindow : Window
{
    public MainWindow() =>
        InitializeComponent();

    private void OnReadIssuesButtonClick(object sender, RoutedEventArgs e)
    {
        string url = milestoneUrlTextBox.Text?.Trim();

        if (!string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
        {
            var issues = ReadIssues(url).OrderBy(x => x.Number);

            resultTextBox.Text = IssuesToString(issues);
        }
    }

    private static IssueSummaryModel[] ReadIssues(string milestoneUrl)
    {
        DriverSetup.AutoSetUp(BrowserNames.Chrome);

        using (AtataContext.Configure().UseChrome().WithArguments("headless").Build())
        {
            var milestonePage = Go.To<MilestonePage>(url: milestoneUrl);
            List<IssueSummaryModel> issues = new();

            if (milestonePage.Issues.IsVisible)
                issues.AddRange(milestonePage.Issues.ToModels());

            milestonePage.Filters.States.Toggle();

            if (milestonePage.Issues.IsVisible)
                issues.AddRange(milestonePage.Issues.ToModels());

            return issues.ToArray();
        }
    }

    private static string IssuesToString(IEnumerable<IssueSummaryModel> issues)
    {
        var issueGroups = issues.ToLookup(ResolveIssueGroup);

        if (issueGroups.Count == 1 && issueGroups.Single().Key == IssueGroup.Other)
        {
            return string.Join(Environment.NewLine, issues.Select(IssueToString));
        }
        else
        {
            StringBuilder builder = new();

            foreach (var group in issueGroups.OrderBy(x => x.Key))
            {
                if (builder.Length > 0)
                    builder.AppendLine().AppendLine();

                string gorupName = TermResolver.ToString(group.Key);

                builder.AppendLine($"## {gorupName}").AppendLine();
                builder.Append(string.Join(Environment.NewLine, group.Select(IssueToString)));
            }

            return builder.ToString();
        }
    }

    private static string IssueToString(IssueSummaryModel issue)
        => $"- #{issue.Number} {issue.Title}";

    private static IssueGroup ResolveIssueGroup(IssueSummaryModel issue)
    {
        if (issue.Labels.Contains("breaking-change"))
            return IssueGroup.BreakingChanges;

        if (issue.Labels.Contains("feature"))
            return IssueGroup.Features;

        if (issue.Labels.Contains("enhancement"))
            return IssueGroup.Enhancements;

        if (issue.Labels.Contains("bug"))
            return IssueGroup.Fixes;

        return IssueGroup.Other;
    }

    public enum IssueGroup
    {
        [Term("Breaking changes")]
        BreakingChanges,

        [Term("New features")]
        Features,

        [Term("Changes and enhancements")]
        Enhancements,

        Fixes,

        Other
    }
}
