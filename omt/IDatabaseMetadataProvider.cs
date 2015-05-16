namespace omt
{
    public interface IDatabaseMetadataProvider
    {
        bool SchemaExists(string schema);
    }
}