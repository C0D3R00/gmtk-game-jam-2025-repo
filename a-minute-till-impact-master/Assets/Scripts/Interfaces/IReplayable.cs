public interface IReplayable
{
    string Id { get; }
    void ReplayAction(string action);
}
