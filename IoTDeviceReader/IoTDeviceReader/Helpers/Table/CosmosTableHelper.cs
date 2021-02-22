using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading.Tasks;

namespace IoTDeviceReader.Helpers.Table
{
    public class CosmosTableHelper<T> where T : TableEntity, new()
    {
        private volatile CloudTable _cloudTable;
        private readonly object _cloudTableLock = new object();

        private readonly string _storageConnectionString;
        private readonly string _tableName;
        private readonly TableClientConfiguration _tableClientConfiguration;

        public CosmosTableHelper(
            string storageConnectionString,
            string tableName,
            TableClientConfiguration tableClientConfiguration = null)
        {
            _storageConnectionString = storageConnectionString;
            _tableName = tableName;
            _tableClientConfiguration = tableClientConfiguration == null ? new TableClientConfiguration() : tableClientConfiguration;
        }

        public virtual async Task<T> InsertOrMerge(T tableEntity)
        {
            var insertOrReplaceOperation = TableOperation.InsertOrMerge(tableEntity);
            var result = await CloudTable.ExecuteAsync(insertOrReplaceOperation);
            var returnedValue = result.Result as T;
            return returnedValue;
        }

        private CloudTable CloudTable
        {
            get
            {
                if (_cloudTable == null)
                {
                    lock (_cloudTableLock)
                    {
                        if (_cloudTable == null)
                        {
                            var storageAccount = CloudStorageAccount.Parse(_storageConnectionString);

                            var cloudTableClient = storageAccount.CreateCloudTableClient(_tableClientConfiguration);

                            _cloudTable = cloudTableClient.GetTableReference(_tableName);

                            if (_cloudTable.Exists() == false)
                            {
                                throw new ArgumentException($"Table {_tableName} does not exist in storage account");
                            }
                        }
                    }
                }
                return _cloudTable;
            }
        }
    }
}
