namespace soothsayer.Scripts
{
    public class DatabaseManoeuvre : IManoeuvre
    {
        public DatabaseManoeuvre(long version, IScript forwardScript, IScript backwardScript)
        {
            Version = version;
            ForwardScript = forwardScript;
            BackwardScript = backwardScript;
        }

        public long Version { get; private set; }

        public IScript ForwardScript { get; private set; }

        public IScript BackwardScript { get; private set; }
    }
}