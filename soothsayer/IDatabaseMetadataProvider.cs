namespace soothsayer
{
    public interface IDatabaseMetadataProvider
    {
        bool SchemaExists(string schema);
    }
}
