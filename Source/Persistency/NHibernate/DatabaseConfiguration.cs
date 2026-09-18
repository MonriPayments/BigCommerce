namespace BigCommerceApi.Persistency.NHibernate
{
    public class DatabaseConfiguration
    {
        public string? ConnectionString { get; set; }
        public string? DefaultSchema { get; set; }
        public int CommandTimeout { get; set; }
    }
}
