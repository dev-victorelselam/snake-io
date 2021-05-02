using System.Collections;

namespace Context
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this ICollection collection) =>
            collection == null || collection.Count == 0;
    }

    public static class StaticValues
    {
        public static float UpdateTime = 0.5f;
        public static float BlockSize = 1f;
    }
}