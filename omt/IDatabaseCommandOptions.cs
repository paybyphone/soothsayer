namespace omt
{
    public interface IDatabaseCommandOptions : IOptions
    {
        string Username { get; set; }
        string Password { get; set; }
    }
}