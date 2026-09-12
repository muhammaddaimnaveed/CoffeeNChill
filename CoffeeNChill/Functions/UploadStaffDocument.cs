using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using CoffeeNChill.Services;

namespace CoffeeNChill.Functions
{
    public class UploadStaffDocument
    {
        [Function("UploadStaffDocument")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "post",
                Route = "documents/upload")]
            HttpRequestData req)
        {
            try
            {
                if (!req.Headers.TryGetValues("Content-Type", out var values))
                {
                    var badResponse =
                        req.CreateResponse(HttpStatusCode.BadRequest);

                    await badResponse.WriteStringAsync(
                        "Content-Type is required.");

                    return badResponse;
                }

                string contentType = values.First();

                if (!contentType.StartsWith(
                    "multipart/form-data",
                    StringComparison.OrdinalIgnoreCase))
                {
                    var badResponse =
                        req.CreateResponse(HttpStatusCode.BadRequest);

                    await badResponse.WriteStringAsync(
                        "Request must use multipart/form-data.");

                    return badResponse;
                }

                var mediaType =
                    MediaTypeHeaderValue.Parse(contentType);

                string? boundary =
                    HeaderUtilities.RemoveQuotes(
                        mediaType.Boundary).Value;

                if (string.IsNullOrWhiteSpace(boundary))
                {
                    var badResponse =
                        req.CreateResponse(HttpStatusCode.BadRequest);

                    await badResponse.WriteStringAsync(
                        "Multipart boundary is missing.");

                    return badResponse;
                }

                var reader =
                    new MultipartReader(boundary, req.Body);

                var section = await reader.ReadNextSectionAsync();

                while (section != null)
                {
                    if (ContentDispositionHeaderValue.TryParse(
                        section.ContentDisposition,
                        out var contentDisposition))
                    {
                        bool hasFileName =
                            !string.IsNullOrWhiteSpace(
                                contentDisposition.FileName.Value) ||
                            !string.IsNullOrWhiteSpace(
                                contentDisposition.FileNameStar.Value);

                        if (hasFileName)
                        {
                            string fileName =
                                HeaderUtilities.RemoveQuotes(
                                    contentDisposition.FileNameStar.HasValue
                                        ? contentDisposition.FileNameStar
                                        : contentDisposition.FileName).Value
                                ?? "document.pdf";

                            string fileContentType =
                                section.ContentType
                                ?? "application/octet-stream";

                            // Only PDF staff documents are accepted.
                            if (!fileContentType.Equals(
                                "application/pdf",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                var unsupportedResponse =
                                    req.CreateResponse(
                                        HttpStatusCode.BadRequest);

                                await unsupportedResponse.WriteStringAsync(
                                    "Only PDF documents are allowed.");

                                return unsupportedResponse;
                            }

                            var blobStorageService =
                                new BlobStorageService();

                            await blobStorageService.UploadDocumentAsync(
                                fileName,
                                section.Body,
                                fileContentType);

                            var response =
                                req.CreateResponse(HttpStatusCode.Created);

                            await response.WriteStringAsync(
                                $"Document '{fileName}' uploaded successfully.");

                            return response;
                        }
                    }

                    section = await reader.ReadNextSectionAsync();
                }

                var noFileResponse =
                    req.CreateResponse(HttpStatusCode.BadRequest);

                await noFileResponse.WriteStringAsync(
                    "No file was provided.");

                return noFileResponse;
            }
            catch (Exception ex)
            {
                var response =
                    req.CreateResponse(
                        HttpStatusCode.InternalServerError);

                await response.WriteStringAsync(
                    $"Error uploading document: {ex.Message}");

                return response;
            }
        }
    }
}


