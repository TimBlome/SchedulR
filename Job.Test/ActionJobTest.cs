namespace SchedulR.Job.Test;

using FluentAssertions;
using SchedulR.Job;

public class ActionJobTest
{
    [Fact]
    public void TheConstructor_WithNullAsName_ThrowsAnError()
    {
        var actionToFail = () => {
            IJob actionJob = new ActionJob(null, () => 1);
        };

        actionToFail.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void TheConstructor_WithEmptyName_ThrowsAnError()
    {
        var actionToFail = () => {
            IJob actionJob = new ActionJob("", () => 1);
        };

        actionToFail.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void TheConstructor_WithWhitespaceName_ThrowsAnError()
    {
        var actionToFail = () => {
            IJob actionJob = new ActionJob(" ", () => 1);
        };

        actionToFail.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void TheConstructor_WithNullFunction_ThrowsAnError()
    {
        var actionToFail = () => {
            IJob actionJob = new ActionJob("TestJob", null);
        };

        actionToFail.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void TheConstructor_WithValidParameters_CreatesANewJob()
    {
        IJob actionJob = new ActionJob("TestJob", () => 1);

        actionJob.Should().NotBeNull();
    }

    [Fact]
    public void ThePerformMethod_ShouldExecuteTheSuppliedAction()
    {
        bool executed = false;
        IJob actionJob = new ActionJob("TestJob", () => {
            executed = true;
            return 1;
        });

        actionJob.Perform().Should().Be(1);
        executed.Should().BeTrue();
    }
}