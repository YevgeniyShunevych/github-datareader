﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Atata;
using GitHub.Components;

namespace GitHub.DataReader
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnReadIssuesButtonClick(object sender, RoutedEventArgs e)
        {
            string url = milestoneUrlTextBox.Text?.Trim();

            if (!string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
            {
                var issues = ReadIssues(url).OrderBy(x => x.Number);

                resultTextBox.Text = IssuesToString(issues);
            }
        }

        private IssueSummaryModel[] ReadIssues(string milestoneUrl)
        {
            using (AtataContext.Configure().UseChrome().Build())
            {
                var milestonePage = Go.To<MilestonePage>(url: milestoneUrl);
                List<IssueSummaryModel> issues = new List<IssueSummaryModel>();

                if (milestonePage.Issues.Exists(SearchOptions.SafelyAtOnce()))
                    issues.AddRange(milestonePage.Issues.ToModels().ToList());

                milestonePage.Filters.States.Toggle();

                if (milestonePage.Issues.Exists(SearchOptions.SafelyAtOnce()))
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
                StringBuilder builder = new StringBuilder();

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
            [Term("New Features")]
            Features,

            [Term("Changes and Enhancements")]
            Enhancements,

            Fixes,

            Other
        }
    }
}