namespace soothsayer.Infrastructure
{
    public static class SliceExtensions
    {
        public static BetweenConfigurator<T> Between<T>(this T[] array, int startIndex)
        {
            return new BetweenConfigurator<T>(array, startIndex);
        }
    }

    public class BetweenConfigurator<T>
    {
        private readonly T[] _array;
        private readonly int _startIndex;

        public BetweenConfigurator(T[] array, int startIndex)
        {
            _array = array;
            _startIndex = startIndex;
        }

		public Slice<T> And(int endIndex)
        {
			return new Slice<T>(_array, _startIndex, endIndex);
        }
    }
}
