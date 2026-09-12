using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace CoffeeNChill.Services
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _containerClient;

        public BlobStorageService()
        {
            string connectionString =
                Environment.GetEnvironmentVariable(
                    "StorageConnectionString")
                ?? throw new InvalidOperationException(
                    "StorageConnectionString is not configured.");

            var blobServiceClient =
                new BlobServiceClient(connectionString);

            _containerClient =
                blobServiceClient
                    .GetBlobContainerClient("staff-docs");

            _containerClient.CreateIfNotExists();
        }

        public async Task UploadDocumentAsync(
            string fileName,
            Stream fileStream,
            string contentType)
        {
            BlobClient blobClient =
                _containerClient
                    .GetBlobClient(fileName);

            var options =
                new BlobUploadOptions
                {
                    HttpHeaders =
                        new BlobHttpHeaders
                        {
                            ContentType = contentType
                        }
                };

            await blobClient.UploadAsync(
                fileStream,
                options);
        }

        public async Task<List<StaffDocumentInfo>>
            ListDocumentsAsync()
        {
            var documents =
                new List<StaffDocumentInfo>();

            await foreach (
                BlobItem blob in
                    _containerClient.GetBlobsAsync())
            {
                documents.Add(
                    new StaffDocumentInfo
                    {
                        FileName = blob.Name,

                        Size =
                            blob.Properties
                                .ContentLength ?? 0,

                        LastModified =
                            blob.Properties
                                .LastModified
                    });
            }

            return documents;
        }

        public async Task<BlobDownloadStreamingResult>
            DownloadDocumentAsync(string fileName)
        {
            BlobClient blobClient =
                _containerClient
                    .GetBlobClient(fileName);

            var response =
                await blobClient
                    .DownloadStreamingAsync();

            return response.Value;
        }

        public async Task<bool>
            DocumentExistsAsync(string fileName)
        {
            BlobClient blobClient =
                _containerClient
                    .GetBlobClient(fileName);

            return await blobClient.ExistsAsync();
        }
    }

    public class StaffDocumentInfo
    {
        public string FileName { get; set; }
            = string.Empty;

        public long Size { get; set; }

        public DateTimeOffset? LastModified
        {
            get;
            set;
        }
    }
}
