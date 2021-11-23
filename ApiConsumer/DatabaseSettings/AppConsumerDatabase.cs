using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiConsumer.DatabaseSettings
{
    public class AppConsumerDatabase : IAppConsumerDatabase
    {
        public string ProductCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
