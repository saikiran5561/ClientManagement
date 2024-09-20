namespace ClientManagement.Caching
{
    public static class CacheKeys
    {
        public static string Client = "Client_{0}";
        public const double AbsoluteExpiration = 5000;
        public const double SlidingExpiration = 5000;
        public const long EntryOptionsSize = 1024;
    }
}
