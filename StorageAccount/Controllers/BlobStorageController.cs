using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Mvc;

namespace StorageAccount.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobStorageController : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=traningudemy1sawe;AccountKey=vk1sMU/vOsbWe5bZOIoSOfhxeHWbm3DBMpqiOiQi3EJv9rQL/TQhfXdvodIwHZX36Upk741HV4hP+AStYXN2AQ==;EndpointSuffix=core.windows.net";

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            var containerName = "documents";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = containerClient.GetBlobClient(file.FileName);

            var blobHttpHeaders = new BlobHttpHeaders();
            blobHttpHeaders.ContentType = file.ContentType;

            await blobClient.UploadAsync(file.OpenReadStream()/*, overwrite: true*/, blobHttpHeaders);

            return Ok();
        }

        [HttpPost("download")]
        public async Task<IActionResult> Download([FromQuery]string blobName)
        {
            var connectionString = "DefaultEndpointsProtocol=https;AccountName=traningudemy1sawe;AccountKey=vk1sMU/vOsbWe5bZOIoSOfhxeHWbm3DBMpqiOiQi3EJv9rQL/TQhfXdvodIwHZX36Upk741HV4hP+AStYXN2AQ==;EndpointSuffix=core.windows.net";

            BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);

            var containerName = "documents";
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            await containerClient.CreateIfNotExistsAsync();

            BlobClient blobClient = containerClient.GetBlobClient(blobName);

            var downloadResponse = await blobClient.DownloadContentAsync();
            var content = downloadResponse.Value.Content.ToStream();
            var contentType = blobClient.GetProperties().Value.ContentType;

            return File(content, contentType, fileDownloadName: blobName);
        }
    }
}
