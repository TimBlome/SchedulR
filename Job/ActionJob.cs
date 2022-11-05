using System.Diagnostics;

namespace SchedulR.Job;

[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
internal class ActionJob : IJob
{
    private readonly Func<int> performAction;

    public ActionJob(string name, Func<int> performAction)
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

    public int Perform()
    {
        return performAction();
    }

    private string GetDebuggerDisplay()
    {
        return $"{Name}:[{Id}]";
    }
}