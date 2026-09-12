using System.Net;
using System.Text.Json;
using Azure;
using CoffeeNChill.Models;
using CoffeeNChill.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CoffeeNChill.Functions
{
    public class CreateMenuItem
    {
        [Function("CreateMenuItem")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = "menu")]
            HttpRequestData req)
        {
            try
            {
                var item =
                    await JsonSerializer.DeserializeAsync<MenuItem>(
                        req.Body,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                if (item == null)
                {
                    var badResponse =
                        req.CreateResponse(
                            HttpStatusCode.BadRequest);

                    await badResponse.WriteStringAsync(
                        "Invalid menu item data.");

                    return badResponse;
                }

                if (string.IsNullOrWhiteSpace(item.PartitionKey) ||
                    string.IsNullOrWhiteSpace(item.RowKey) ||
                    string.IsNullOrWhiteSpace(item.Name))
                {
                    var badResponse =
                        req.CreateResponse(
                            HttpStatusCode.BadRequest);

                    await badResponse.WriteStringAsync(
                        "Category, ID and name are required.");

                    return badResponse;
                }

                if (item.Price < 0)
                {
                    var badResponse =
                        req.CreateResponse(
                            HttpStatusCode.BadRequest);

                    await badResponse.WriteStringAsync(
                        "Price cannot be negative.");

                    return badResponse;
                }

                var service =
                    new TableStorageService();

                await service.AddMenuItemAsync(item);

                var response =
                    req.CreateResponse(
                        HttpStatusCode.Created);

                await response.WriteAsJsonAsync(item);

                return response;
            }
            catch (RequestFailedException ex)
                when (ex.Status == 409)
            {
                var response =
                    req.CreateResponse(
                        HttpStatusCode.Conflict);

                await response.WriteStringAsync(
                    "A menu item with this category and ID already exists.");

                return response;
            }
            catch (JsonException)
            {
                var response =
                    req.CreateResponse(
                        HttpStatusCode.BadRequest);

                await response.WriteStringAsync(
                    "Invalid JSON data.");

                return response;
            }
            catch (Exception ex)
            {
                var response =
                    req.CreateResponse(
                        HttpStatusCode.InternalServerError);

                await response.WriteStringAsync(ex.Message);

                return response;
            }
        }
    }
}
