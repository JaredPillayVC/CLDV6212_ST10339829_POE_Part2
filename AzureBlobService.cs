using Azure.Storage.Blobs;

namespace CLDV6212_ST10339829_POE
{
    public class AzureBlobService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobContainerClient;


        public AzureBlobService(string connectionString) 
        { 
            _blobServiceClient = new BlobServiceClient(connectionString);
            _blobContainerClient = _blobServiceClient.GetBlobContainerClient("Images");
            _blobContainerClient.CreateIfNotExists();
        }

        public async Task uploadImageAsync(IFormFile file) 
        {
            var imageBlobClient = _blobContainerClient.GetBlobClient(file.FileName);
            using var fileStream = file.OpenReadStream();
            await imageBlobClient.UploadAsync(fileStream, true);
        }
        public async Task<List<string>> FilesAsync() 
        { 
            var files = new List<string>();
            await foreach (var item in _blobContainerClient.GetBlobsAsync()) 
            { 
                files.Add(item.Name);
            }
            return files;
        }

    
    }
}
