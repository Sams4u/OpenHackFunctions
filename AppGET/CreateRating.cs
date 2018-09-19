using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AppGET
{
    public static class CreateRating
    {
        [FunctionName("CreateRating")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get","post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            var isSuccess = false;
            // parse query parameter
            string name = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
                .Value;
            string response="";
            if (name == null)
            {
                // Get request body
                dynamic data = await req.Content.ReadAsAsync<object>();
                var productID = data?.productId;
                var userID = data?.userId;
                var rating = data?.rating;
                try
                {
                    WebClient userClient = new WebClient();
                    userClient.Headers.Add("Content-Type", "application/json");
                    var userResponse = userClient.DownloadString("https://serverlessohuser.trafficmanager.net/api/GetUser?userId=" + userID);
                }
                catch
                {
                    isSuccess = false;
                }
                try
                {
                    WebClient userClient = new WebClient();
                    userClient.Headers.Add("Content-Type", "application/json");
                    var userResponse = userClient.DownloadString("https://serverlessohproduct.trafficmanager.net/api/GetProduct?productID=" + productID);
                }
                catch
                {
                    isSuccess = false;
                }
                if (rating > 0 && rating <= 5)
                {
                    data["id"] = Guid.NewGuid();
                    data["timestamp"] = DateTime.UtcNow.ToString();
                    WebClient userClient = new WebClient();
                    var userResponse = userClient.DownloadString($"https://samsopenhack.azurewebsites.net/api/CreateCosmos?id=" +data["id"]+ "&userId="+userID+ "&productId="+productID+ "&timestamp="+ data["timestamp"]+ "&locationName="+ data["locationName"] +"&rating="+rating+ "&userNotes="+data["userNotes"]);
                }
                else
                    isSuccess = false;               
                response = Convert.ToString(data);
                //response = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                isSuccess = true;
            }

            return isSuccess == false
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
