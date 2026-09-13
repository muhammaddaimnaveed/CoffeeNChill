using System.Net;
using System.Text;
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
                AuthorizationLevel.Anonymous,
                "post",
                Route = "documents/upload")]
            HttpRequestData req)
        {
            try
            {

                var bodyStream = new MemoryStream();
                await req.Body.CopyToAsync(bodyStream);
                bodyStream.Position = 0;

                string? boundary = null;

                if (req.Headers.TryGetValues(
                    "Content-Type",
                    out var contentTypeValues))
                {
                    string contentType = contentTypeValues.First();

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

                    boundary =
                        HeaderUtilities.RemoveQuotes(
                            mediaType.Boundary).Value;
                }

                if (string.IsNullOrWhiteSpace(boundary))
                {
                    bodyStream.Position = 0;

                    using var reader = new StreamReader(
                        bodyStream,
                        Encoding.UTF8,
                        detectEncodingFromByteOrderMarks: true,
                        bufferSize: 1024,
                        leaveOpen: true);

                    string? firstLine =
                        await reader.ReadLineAsync();

                    if (!string.IsNullOrWhiteSpace(firstLine) &&
                        firstLine.StartsWith("--"))
                    {
                        boundary = firstLine.Substring(2).Trim();
                    }

                    bodyStream.Position = 0;
                }

                if (string.IsNullOrWhiteSpace(boundary))
                {
                    var badResponse =
                        req.CreateResponse(HttpStatusCode.BadRequest);

                    await badResponse.WriteStringAsync(
                        "Multipart boundary is missing.");

                    return badResponse;
                }

                var multipartReader =
                    new MultipartReader(boundary, bodyStream);

                var section =
                    await multipartReader.ReadNextSectionAsync();

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

                            if (!fileName.EndsWith(
                                ".pdf",
                                StringComparison.OrdinalIgnoreCase))
                            {
                                var unsupportedResponse =
                                    req.CreateResponse(
                                        HttpStatusCode.BadRequest);

                                await unsupportedResponse.WriteStringAsync(
                                    "Only PDF documents are allowed.");

                                return unsupportedResponse;
                            }

                            string? fileContentType =
                                section.ContentType;

                            if (!string.IsNullOrWhiteSpace(fileContentType) &&
                                !fileContentType.Equals(
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

                            fileContentType = "application/pdf";

                            var blobStorageService =
                            new BlobStorageService();

                            using var fileStream = new MemoryStream();

                            await section.Body.CopyToAsync(fileStream);

                            fileStream.Position = 0;

                            await blobStorageService.UploadDocumentAsync(
                                fileName,
                                fileStream,
                                fileContentType);

                            var response =
                                req.CreateResponse(HttpStatusCode.Created);

                            await response.WriteStringAsync(
                                $"Document '{fileName}' uploaded successfully.");

                            return response;
                        }
                    }

                    section =
                        await multipartReader.ReadNextSectionAsync();
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
