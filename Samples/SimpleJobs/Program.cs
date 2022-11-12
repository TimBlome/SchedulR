using Cronos;
using Jobs.Execution;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SchedulR.Jobs;
using SchedulR.Jobs.Execution;

var job = Job.New("Demo", (_) => {
    
    Console.WriteLine("Hello from Job");
    Thread.Sleep(5000);

    return 0;
});

var loggerFactory = LoggerFactory.Create(b => b.AddConsole());

Executor executor = new(loggerFactory.CreateLogger<Executor>());
executor.RegisterJob(job, CronExpression.Parse("* * * * * *", CronFormat.IncludeSeconds));
var token = new CancellationToken();
executor.Run(token).GetAwaiter().GetResult();

Console.ReadLine();