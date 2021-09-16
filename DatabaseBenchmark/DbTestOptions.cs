namespace DatabaseBenchmark
{
    public class DbTestOptions
    {
        public int ReadPageSize { get; set; } = 100;
        public int ConcurrentCount { get; set; } = 1;
        public int InsertPageSize { get; set; } = 1;
        public int RecordCount { get; set; } = 1000;
        public int ReadCount { get; set; } = 1000;
    }
}
