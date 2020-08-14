namespace FluentBehaviourTree
{
    public interface ITickData
    {
        int[] RunningSequences { get; }
        int[] RunningSelectors { get; }
    }
}
