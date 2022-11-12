using System.Collections.Concurrent;
using Cronos;
using Microsoft.Extensions.Logging;
using SchedulR.Jobs;
using SchedulR.Jobs.Execution;

namespace Jobs.Execution
{
    public class Executor
    {
        private readonly ConcurrentDictionary<Guid, ExecutionJob> _jobs = new();
        private readonly ILogger<Executor> logger;

        public Executor(ILogger<Executor> logger)
        {
            this.logger = logger;
        }

        public async Task Run(CancellationToken cancellationToken){

            logger.LogInformation("Starting execution of scheduled jobs");
            while(!cancellationToken.IsCancellationRequested){
                var executeJobsBefore = DateTime.UtcNow;
                var nextExecutions = _jobs.Where(job => job.Value.NextExecution < executeJobsBefore).ToList();
                
                await Parallel.ForEachAsync(nextExecutions, async (job, cancellationToken) => {
                    logger.LogInformation("Beginning execution of {jobId}[{jobName}]", job.Key, job.Value.Job.Name);
                    job.Value.Job.Perform(new JobExecutionInfo(null, null));
                    var nextExecutionJob = job.Value.CalculateNewNextExecution();
                    _jobs[job.Key] = nextExecutionJob;
                    var jobToLog = job.Value.Job;
                    logger.LogInformation("Finished execution of {jobId}[{jobName}]. Next execution is at {nextExecution}", jobToLog.Id, jobToLog.Name, nextExecutionJob.NextExecution);
                });

                await Task.Delay(500);
            }
            logger.LogInformation("Execution terminated");
        }

        public void RegisterJob(IJob job, CronExpression cron){
            _ = cron.GetNextOccurrence(DateTime.UtcNow) ?? throw new InvalidOperationException("Job will never execute");
            _jobs.TryAdd(job.Id, new ExecutionJob(job, cron));
        }

        record struct ExecutionJob{

            public ExecutionJob(IJob job, CronExpression cron)
            {
                this.Job = job;
                this.Cron = cron;
                this.NextExecution = cron.GetNextOccurrence(DateTime.UtcNow);
            }

            public readonly IJob Job {get; init;}
            public readonly CronExpression Cron {get; init;}

            public readonly DateTime? NextExecution {get; init;}

            public ExecutionJob CalculateNewNextExecution(){
                return this with {NextExecution = Cron.GetNextOccurrence(DateTime.UtcNow)};
            }
        }
    }
}