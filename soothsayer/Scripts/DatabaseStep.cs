namespace soothsayer.Scripts
{
    public class DatabaseStep : IStep
    {
        public static DatabaseStep BackwardOnly(IScript backwardScript)
        {
            return new DatabaseStep(null, backwardScript);
        }
        public static DatabaseStep ForwardOnly(IScript forwardScript)
        {
            return new DatabaseStep(forwardScript, null);
        }

        public DatabaseStep(IScript forwardScript, IScript backwardScript)
        {
            ForwardScript = forwardScript;
            BackwardScript = backwardScript;
        }
        
        public IScript ForwardScript { get; private set; }

        public IScript BackwardScript { get; private set; }
    }
}