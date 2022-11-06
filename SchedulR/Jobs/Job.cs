using SchedulR.Jobs.Execution;

namespace SchedulR.Jobs;

public static class Job{
    public static IJob New(string name, Func<JobExecutionInfo, int> job) => new ActionJob(name, job);
}