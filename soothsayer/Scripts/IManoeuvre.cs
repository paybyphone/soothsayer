namespace soothsayer.Scripts
{
    public interface IManoeuvre
    {
        long Version { get; }

        IScript ForwardScript { get; }

        IScript BackwardScript { get; }
    }
}