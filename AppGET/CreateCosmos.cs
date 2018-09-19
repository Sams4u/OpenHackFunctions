using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;


namespace AppGET
{
    public static class CreateCosmos
    {
        [FunctionName("CreateCosmos")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous,"get",  Route = null)]HttpRequestMessage req, TraceWriter log, [DocumentDB("OpenHack", "BFYOC",CreateIfNotExists =true,
                        ConnectionStringSetting = "myCosmosDBConnection")]
                        out dynamic outputDocument)
        {
            string id = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "id", true) == 0)
        .Value;
            string userId = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "userId", true) == 0)
                .Value;
            string productId = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "productId", true) == 0)
                .Value;
            string locationName = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "locationName", true) == 0)
                .Value;
            string rating = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "rating", true) == 0)
                .Value;
            string userNotes = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "userNotes", true) == 0)
                .Value;
            string timestamp = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "timestamp", true) == 0)
                .Value;
            outputDocument = new
            {
                id = id,
                userId = userId,
                productId = productId,
                locationName = locationName,
                rating = rating,
                userNotes = userNotes,
                timestamp = timestamp
            };
            if (id != "")
            {
                return req.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

        }
    }
}
