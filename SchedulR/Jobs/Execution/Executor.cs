using System.Collections.Concurrent;
using Cronos;
using SchedulR.Jobs;
using SchedulR.Jobs.Execution;

namespace Jobs.Execution
{
    public class Executor
    {
        private readonly ConcurrentDictionary<Guid, ExecutionJob> _jobs = new();
        public Executor()
        {
            
        }

        public async Task Run(CancellationToken cancellationToken){

            while(!cancellationToken.IsCancellationRequested){
                var executeJobsBefore = DateTime.UtcNow;
                var nextExecutions = _jobs.Where(job => job.Value.NextExecution < executeJobsBefore).ToList();

                await Parallel.ForEachAsync(nextExecutions, async (job, cancellationToken) => {
                    job.Value.Job.Perform(new JobExecutionInfo(null, null));
                    job.Value.CalculateNewNextExecution();
                    await Task.Delay(5000, cancellationToken);
                });

                await Task.Delay(500);
            }
        }

        public void RegisterJob(IJob job, CronExpression cron){
            _ = cron.GetNextOccurrence(DateTime.UtcNow) ?? throw new InvalidOperationException("Job will never execute");
            _jobs.TryAdd(job.Id, new ExecutionJob(job, cron));
        }

        struct ExecutionJob{

            public ExecutionJob(IJob job, CronExpression cron)
            {
                this.Job = job;
                this.Cron = cron;
                this.NextExecution = cron.GetNextOccurrence(DateTime.UtcNow);
            }

            public readonly IJob Job {get; init;}
            public readonly CronExpression Cron {get; init;}

            public DateTime? NextExecution {get; private set;}

            public void CalculateNewNextExecution(){
                NextExecution = Cron.GetNextOccurrence(DateTime.UtcNow);
            }
        }
    }
}