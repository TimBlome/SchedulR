using Cronos;
using Jobs.Execution;
using SchedulR.Jobs;
using SchedulR.Jobs.Execution;

var job = Job.New("Demo", (_) => {
    
    Console.WriteLine("Hello from Job");

    return 0;
});

Executor executor = new();
executor.RegisterJob(job, CronExpression.Parse("* * * * * *", CronFormat.IncludeSeconds));
var token = new CancellationToken();
executor.Run(token).GetAwaiter().GetResult();

Console.ReadLine();