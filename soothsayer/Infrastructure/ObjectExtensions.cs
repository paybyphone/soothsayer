namespace soothsayer.Infrastructure
{
    public static class ObjectExtensions
    {
        [ContractAnnotation("null => false")]
        public static bool IsNotNull(this object obj)
        {
            return obj != null;
        }

        [ContractAnnotation("null => true")]
        public static bool IsNull(this object obj)
        {
            return obj == null;
        }
    }
}
