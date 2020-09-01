namespace Dal.Impl.Configurations
{
    public class DatabaseConfiguration
    {
        public string ConnectionString { get; }
        public string DatabaseName { get; }
        public string StuffCollectionName { get; set; }
    }
}
