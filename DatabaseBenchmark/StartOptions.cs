namespace DatabaseBenchmark
{
    public class StartOptions
    {
        public StartOptions(int concurrent, int readPageSize, int readCount, int insertCount, int insertPageSize, string[] databases)
        {
            Concurrent = concurrent;
            ReadPageSize = readPageSize;
            ReadCount = readCount;
            InsertCount = insertCount;
            InsertPageSize = insertPageSize;
            Databases = databases;
        }

        public int Concurrent { get; }
        public int ReadPageSize { get; }
        public int ReadCount { get; }
        public int InsertCount { get; }
        public int InsertPageSize { get; }
        public string[] Databases { get; }
    }
}
