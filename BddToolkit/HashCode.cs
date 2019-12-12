using System.Collections.Immutable;

namespace ITLIBRIUM.BddToolkit
{
    internal static class HashCode
    {
        public static int Combine<T>(ImmutableArray<T> array)
        {
            var length = array.Length;
            unchecked
            {
                var hash = array[0].GetHashCode();
                for (var i = 1; i < length; i++)
                {
                    var item = array[i];
                    hash = (hash * 397) ^ (item?.GetHashCode() ?? 0);
                }
                return hash;
            }
        }
    }
}