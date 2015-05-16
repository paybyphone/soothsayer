namespace omt.Infrastructure
{
    public interface ISecureConsole
    {
        string ReadLine(char maskCharacter);
    }
}