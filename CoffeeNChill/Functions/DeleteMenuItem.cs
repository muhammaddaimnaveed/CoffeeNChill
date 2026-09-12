using System.Net;
using CoffeeNChill.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CoffeeNChill.Functions
{
    public class DeleteMenuItem
    {
        [Function("DeleteMenuItem")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "delete",
                Route = "menu/{category}/{id}")]
            HttpRequestData req,
            string category,
            string id)
        {
            try
            {
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

                await service.DeleteMenuItemAsync(
                    category,
                    id);

                return req.CreateResponse(
                    HttpStatusCode.NoContent);
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
