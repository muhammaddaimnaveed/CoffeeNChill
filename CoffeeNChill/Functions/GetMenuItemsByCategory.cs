using System.Net;
using CoffeeNChill.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CoffeeNChill.Functions
{
    public class GetMenuItemsByCategory
    {
        [Function("GetMenuItemsByCategory")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = "menu/category/{category}")]
            HttpRequestData req,
            string category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category))
                {
                    var badResponse =
                        req.CreateResponse(
                            HttpStatusCode.BadRequest);

                    await badResponse.WriteStringAsync(
                        "Category is required.");

                    return badResponse;
                }

                var service =
                    new TableStorageService();

                var items =
                    await service
                        .GetMenuItemsByCategoryAsync(category);

                var response =
                    req.CreateResponse(HttpStatusCode.OK);

                await response.WriteAsJsonAsync(items);

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