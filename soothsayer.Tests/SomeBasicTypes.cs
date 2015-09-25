namespace soothsayer.Tests
{
    public sealed partial class Some
    {
        public static T Value<T>(T value = default(T))
        {
            return value;
        }

        public static string String(string value = @"")
        {
            return value;
        }
    }
}
