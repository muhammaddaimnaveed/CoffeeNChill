using System.Net;
using System.Text.Json;
using CoffeeNChill.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CoffeeNChill.Functions
{
    public class UpdateMenuItem
    {
        private class UpdateMenuItemRequest
        {
            public double? Price { get; set; }

            public bool? IsAvailable { get; set; }
        }

        [Function("UpdateMenuItem")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "put",
                Route = "menu/{category}/{id}")]
            HttpRequestData req,
            string category,
            string id)
        {
            try
            {
                var update =
                    await JsonSerializer
                        .DeserializeAsync<UpdateMenuItemRequest>(
                            req.Body,
                            new JsonSerializerOptions
                            {
                                PropertyNameCaseInsensitive = true
                            });

                if (update == null)
                {
                    var badResponse =
                        req.CreateResponse(
                            HttpStatusCode.BadRequest);

                    await badResponse.WriteStringAsync(
                        "Invalid update data.");

                    return badResponse;
                }

                if (update.Price == null &&
                    update.IsAvailable == null)
                {
                    var badResponse =
                        req.CreateResponse(
                            HttpStatusCode.BadRequest);

                    await badResponse.WriteStringAsync(
                        "Provide price or availability to update.");

                    return badResponse;
                }

                if (update.Price.HasValue &&
                    update.Price.Value < 0)
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

                var existingItem =
                    await service.GetMenuItemAsync(
                        category,
                        id);

                if (existingItem == null)
                {
                    var notFound =
                        req.CreateResponse(
                            HttpStatusCode.NotFound);

                    await notFound.WriteStringAsync(
                        "Menu item not found.");

                    return notFound;
                }

                if (update.Price.HasValue)
                    existingItem.Price =
                        update.Price.Value;

                if (update.IsAvailable.HasValue)
                    existingItem.IsAvailable =
                        update.IsAvailable.Value;

                await service
                    .UpdateMenuItemAsync(existingItem);

                var response =
                    req.CreateResponse(HttpStatusCode.OK);

                await response
                    .WriteAsJsonAsync(existingItem);

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
