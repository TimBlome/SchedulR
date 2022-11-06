namespace SchedulR.Jobs;

public interface IJob {
    Guid Id { get; set; }
    string Name { get; set; }
    public int Perform();
}