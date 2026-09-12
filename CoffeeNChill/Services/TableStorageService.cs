using Azure;
using Azure.Data.Tables;
using CoffeeNChill.Models;

namespace CoffeeNChill.Services
{
    public class TableStorageService
    {
        private readonly TableClient _tableClient;

        public TableStorageService()
        {
            string connectionString =
                Environment.GetEnvironmentVariable("StorageConnectionString")
                ?? throw new InvalidOperationException(
                    "StorageConnectionString is not configured.");

            _tableClient =
                new TableClient(connectionString, "MenuItems");

            _tableClient.CreateIfNotExists();
        }

        public async Task AddMenuItemAsync(MenuItem item)
        {
            await _tableClient.AddEntityAsync(item);
        }

        public async Task<List<MenuItem>> GetAllMenuItemsAsync()
        {
            var items = new List<MenuItem>();

            await foreach (
                MenuItem item in _tableClient.QueryAsync<MenuItem>())
            {
                items.Add(item);
            }

            return items;
        }

        public async Task<List<MenuItem>>
            GetMenuItemsByCategoryAsync(string category)
        {
            var items = new List<MenuItem>();

            await foreach (
                MenuItem item in _tableClient.QueryAsync<MenuItem>(
                    x => x.PartitionKey == category))
            {
                items.Add(item);
            }

            return items;
        }

        public async Task<MenuItem?> GetMenuItemAsync(
            string category,
            string id)
        {
            try
            {
                var response =
                    await _tableClient.GetEntityAsync<MenuItem>(
                        category,
                        id);

                return response.Value;
            }
            catch (RequestFailedException ex)
                when (ex.Status == 404)
            {
                return null;
            }
        }

        public async Task UpdateMenuItemAsync(MenuItem item)
        {
            await _tableClient.UpdateEntityAsync(
                item,
                ETag.All,
                TableUpdateMode.Replace);
        }

        public async Task DeleteMenuItemAsync(
            string category,
            string id)
        {
            await _tableClient.DeleteEntityAsync(
                category,
                id);
        }
    }
}
