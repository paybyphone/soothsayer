namespace soothsayer.Oracle
{
    public class AppliedScriptDto
    {
        public long Version { get; set; }
        public string ScriptName{ get; set; }
        public string ForwardScript { get; set; }
        public string BackwardScript { get; set; }
    }
}