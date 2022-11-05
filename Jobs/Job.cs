namespace SchedulR.Jobs;

public static class Job{
    public static IJob New(string name, Func<int> job) => new ActionJob(name, job);
}