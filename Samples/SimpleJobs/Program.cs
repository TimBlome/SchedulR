using SchedulR.Jobs;

var job = Job.New("Demo", () => {
    
    Console.WriteLine("Hello from Job");

    return 0;
});

Timer t = new(state => job.Perform(), null, 0, 500);

Console.ReadLine();