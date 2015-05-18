namespace soothsayer
{
    public class MigrationInfo
    {
        public MigrationInfo(MigrationDirection direction, string scriptFolder, string targetSchema, string targetTablespace, string targetEnvironment,
            long? targetVersion, bool forced = false)
        {
            Direction = direction;
            ScriptFolder = scriptFolder;
            TargetSchema = targetSchema;
            TargetTablespace = targetTablespace;
            TargetEnvironment = targetEnvironment;
            TargetVersion = targetVersion;
            Forced = forced;
        }

        public MigrationDirection Direction { get; private set; }
        public string ScriptFolder { get; private set; }

        public string TargetSchema { get; private set; }
        public string TargetTablespace { get; private set; }
        public string TargetEnvironment { get; private set; }
        public long? TargetVersion { get; private set; }

        public bool Forced { get; private set; }
    }
}
