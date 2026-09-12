using System.Net;
using CoffeeNChill.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;

namespace CoffeeNChill.Functions
{
    public class GetAllMenuItems
    {
        [Function("GetAllMenuItems")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "get",
                Route = "menu")]
            HttpRequestData req)
        {
            try
            {
                var service =
                    new TableStorageService();

                var items =
                    await service.GetAllMenuItemsAsync();

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
