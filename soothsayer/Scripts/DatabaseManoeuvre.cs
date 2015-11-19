namespace soothsayer.Scripts
{
    public class DatabaseManoeuvre : IManoeuvre
    {
        public DatabaseManoeuvre(IScript forwardScript, IScript backwardScript)
        {
            ForwardScript = forwardScript;
            BackwardScript = backwardScript;
        }
        
        public IScript ForwardScript { get; private set; }

        public IScript BackwardScript { get; private set; }
    }
}