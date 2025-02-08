namespace Falcon.CosmosDb.Gremlin.Configurations;

public class GremlinDbOptions
{
    public string HostName { get; set; } = string.Empty;
    public string PrimaryKey { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public string GraphName { get; set; } = string.Empty;

    public class Builder
    {
        private readonly GremlinDbOptions _options = new GremlinDbOptions();

        public Builder HostName(string hostName)
        {
            _options.HostName = hostName;
            return this;
        }

        public Builder PrimaryKey(string primaryKey)
        {
            _options.PrimaryKey = primaryKey;
            return this;
        }

        public Builder Database(string database)
        {
            _options.Database = database;
            return this;
        }

        public Builder GraphName(string graphName)
        {
            _options.GraphName = graphName;
            return this;
        }

        public GremlinDbOptions Build()
        {
            return _options;
        }
    }
}
