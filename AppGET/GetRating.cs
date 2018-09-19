using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AppGET
{
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("C# HTTP trigger function processed a request.");
            Document response=new Document();
            string jresponse=string.Empty;
            // parse query parameter
            string id = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "ratingId", true) == 0)
                .Value;
            DocumentClient client = new DocumentClient(new Uri("https://samsopenhack.documents.azure.com:443/"),"GJsORfwkpHePHLgR46fCMBxGb2AqAFVSznrnDudriKhvIqh0Tor6XLomIkGOPA5SjFLTdA0P8Ifc4vHeIE0uFA==");
            if (id != null)
            {
                Doc dresponse = await client.ReadDocumentAsync<Doc>(UriFactory.CreateDocumentUri("OpenHack", "BFYOC", id));
                jresponse = Newtonsoft.Json.JsonConvert.SerializeObject(dresponse);
            }
            return id == null
                ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass an id on the query string or in the request body")
                : req.CreateResponse(HttpStatusCode.OK, jresponse);
        }
    }
}
