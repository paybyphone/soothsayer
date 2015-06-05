namespace soothsayer.Scripts
{
    public interface IScript
    {
		string Path { get; }
        string Name { get; }
        long Version { get; }
    }
}
