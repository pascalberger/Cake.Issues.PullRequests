﻿namespace Cake.Issues.PullRequests.Tests
{
    using System.Collections.Generic;
    using Cake.Core.Diagnostics;
    using Cake.Issues.Testing;
    using Cake.Testing;

    internal class PullRequestsFixture
    {
        public PullRequestsFixture()
        {
            this.Log = new FakeLog { Verbosity = Verbosity.Normal };
            this.IssueProviders = new List<FakeIssueProvider> { new FakeIssueProvider(this.Log) };
            this.PullRequestSystem = new FakePullRequestSystem(this.Log);
            this.Settings =
                new ReportIssuesToPullRequestSettings(
                    new Core.IO.DirectoryPath(@"c:\Source\Cake.Issues"));
        }

        public FakeLog Log { get; set; }

        public IList<FakeIssueProvider> IssueProviders { get; set; }

        public FakePullRequestSystem PullRequestSystem { get; set; }

        public RepositorySettings Settings { get; set; }

        public ReportIssuesToPullRequestSettings ReportIssuesToPullRequestSettings =>
            this.Settings as ReportIssuesToPullRequestSettings;

        public PullRequestIssueResult RunOrchestrator()
        {
            var orchestrator =
                new Orchestrator(
                    this.Log,
                    this.IssueProviders,
                    this.PullRequestSystem,
                    this.ReportIssuesToPullRequestSettings);
            return orchestrator.Run();
        }

        public IEnumerable<IIssue> FilterIssues(
            IEnumerable<IIssue> issues,
            IDictionary<IIssue, IssueCommentInfo> issueComments)
        {
            this.PullRequestSystem?.Initialize(this.ReportIssuesToPullRequestSettings);

            var issueFilterer =
                new IssueFilterer(
                    this.Log,
                    this.PullRequestSystem,
                    this.ReportIssuesToPullRequestSettings);
            return issueFilterer.FilterIssues(issues, issueComments);
        }
    }
}
