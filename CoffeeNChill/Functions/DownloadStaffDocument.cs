using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using CoffeeNChill.Services;

namespace CoffeeNChill.Functions
{
    public class DownloadStaffDocument
    {
        [Function("DownloadStaffDocument")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "get",
                Route = "documents/download/{fileName}")]
            HttpRequestData req,
            string fileName)
        {
            try
            {
                var blobStorageService =
                    new BlobStorageService();

                bool exists =
                    await blobStorageService.DocumentExistsAsync(fileName);

                if (!exists)
                {
                    var notFoundResponse =
                        req.CreateResponse(HttpStatusCode.NotFound);

                    await notFoundResponse.WriteStringAsync(
                        $"Document '{fileName}' was not found.");

                    return notFoundResponse;
                }

                var download =
                    await blobStorageService.DownloadDocumentAsync(fileName);

                var response =
                    req.CreateResponse(HttpStatusCode.OK);

                response.Headers.Add(
                    "Content-Type",
                    download.Details.ContentType
                    ?? "application/octet-stream");

                response.Headers.Add(
                    "Content-Disposition",
                    $"attachment; filename=\"{fileName}\"");

                await download.Content.CopyToAsync(
                    response.Body);

                return response;
            }
            catch (Exception ex)
            {
                var response =
                    req.CreateResponse(
                        HttpStatusCode.InternalServerError);

                await response.WriteStringAsync(
                    $"Error downloading document: {ex.Message}");

                return response;
            }
        }
    }
}

