using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApplication2.Controller
{
    public class GameController : ApiController
    {
        private HttpClient httpClient;

        public GameController()
        {
            httpClient = new HttpClient();
        }

        public async Task<string> ConsumeExternalApi(string apiUrl)
        {
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                // Process the response data
                return responseData;
            }
            else
            {
                // Handle the error response
                throw new HttpResponseException(response.StatusCode);
            }
        }
    }
}