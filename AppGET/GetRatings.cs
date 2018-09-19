using System;
using System.Collections.Generic;
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
    public static class GetRatings
    {
        //public class Doc
        //{
        //    public string id { get; set; }
        //    public string userId { get; set; }
        //    public string productId { get; set; }
        //    public string timestamp { get; set; }
        //    public string locationName { get; set; }
        //    public string rating { get; set; }
        //    public string userNotes { get; set; }

        //}
        [FunctionName("GetRatings")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            Doc[] response=new Doc[0]; 
        // parse query parameter
        string id = req.GetQueryNameValuePairs()
                .FirstOrDefault(q => string.Compare(q.Key, "userId", true) == 0)
                .Value;
            var collectionLink = UriFactory.CreateDocumentCollectionUri("OpenHack", "BFYOC");
            DocumentClient client = new DocumentClient(new Uri("https://samsopenhack.documents.azure.com:443/"), "GJsORfwkpHePHLgR46fCMBxGb2AqAFVSznrnDudriKhvIqh0Tor6XLomIkGOPA5SjFLTdA0P8Ifc4vHeIE0uFA==");
            if (id != null)
            {
                response = client.CreateDocumentQuery<Doc>(collectionLink)
                                            .Where(so => so.userId == id)
                                            .AsEnumerable().ToArray<Doc>();
                                            
            }

            return id == null
            ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a userId on the query string or in the request body")
            : req.CreateResponse(HttpStatusCode.OK, response);
        }
    }
}
