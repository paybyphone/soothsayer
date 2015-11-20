namespace soothsayer.Scripts
{
    public class DatabaseManoeuvre : IManoeuvre
    {
        public static DatabaseManoeuvre BackwardOnly(IScript backwardScript)
        {
            return new DatabaseManoeuvre(null, backwardScript);
        }
        public static DatabaseManoeuvre ForwardOnly(IScript forwardScript)
        {
            return new DatabaseManoeuvre(forwardScript, null);
        }

        public DatabaseManoeuvre(IScript forwardScript, IScript backwardScript)
        {
            ForwardScript = forwardScript;
            BackwardScript = backwardScript;
        }
        
        public IScript ForwardScript { get; private set; }

        public IScript BackwardScript { get; private set; }
    }
}