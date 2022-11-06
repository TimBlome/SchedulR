using System.Diagnostics;
using SchedulR.Jobs.Execution;

namespace SchedulR.Jobs;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
internal class ActionJob : IJob
{
    private readonly Func<JobExecutionInfo, int> performAction;

    public ActionJob(string name, Func<JobExecutionInfo, int> performAction)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException($"'{nameof(name)}' cannot be null or whitespace.", nameof(name));
        }
        Name = name;
        this.performAction = performAction ?? throw new ArgumentNullException(nameof(performAction));
    }
    public Guid Id { get; set; }
    public string Name { get; set; }

    public int Perform(JobExecutionInfo executionInfo)
    {
        Debug.Assert(executionInfo is not null);
        return performAction(executionInfo);
    }

    private string GetDebuggerDisplay()
    {
        return $"{Name}:[{Id}]";
    }
}