namespace soothsayer.Scripts
{
    public interface IStep
    {
        IScript ForwardScript { get; }

        IScript BackwardScript { get; }
    }
}