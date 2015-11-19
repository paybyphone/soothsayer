namespace soothsayer.Scripts
{
    public interface IManoeuvre
    {
        IScript ForwardScript { get; }

        IScript BackwardScript { get; }
    }
}