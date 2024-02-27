using Azure.Storage.Blobs;
using DocsBlobStorage.Configuration;
using DocsBlobStorage.Services.Abstractions;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;

namespace DocsBlobStorage.Services.Implementations
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly string _connectionString;
        private readonly string _containerName;

        public BlobStorageService(IOptions<BlobStorageOptions> blobOptions)
        {
            _connectionString = blobOptions.Value.ConnectionString;
            _containerName = blobOptions.Value.ContainerName;
        }

        public async Task UploadFileAsync(IBrowserFile file, string email)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file), "File cannot be null.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(nameof(email), "Email cannot be null or empty.");
            }

            try
            {
                var blobServiceClient = new BlobServiceClient(_connectionString);
                var blobContainerClient = blobServiceClient.GetBlobContainerClient(_containerName);

                await using var ms = new MemoryStream();
                await file.OpenReadStream(maxAllowedSize: 104857600).CopyToAsync(ms);
                ms.Position = 0;

                var blobClient = blobContainerClient.GetBlobClient(file.Name);
                var metadata = new Dictionary<string, string> { { "email", email } };

                await blobClient.UploadAsync(ms, overwrite: true);
                await blobClient.SetMetadataAsync(metadata);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error uploading file to Blob Storage: {ex.Message}", ex);
            }
        }
    }
}
