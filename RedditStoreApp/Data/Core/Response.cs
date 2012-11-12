using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp.Data.Core
{
    public class Response
    {
        public Response(
            String content, 
            DateTime sent, 
            HttpStatusCode statusCode, 
            bool isSuccess)
        {
            this.Content = content;
            this.Sent = sent;
            this.StatusCode = statusCode;
            this.IsSuccess = isSuccess;
        }

        public string Content { get; private set; }
        public DateTime Sent { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public bool IsSuccess { get; private set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Data.Core.Response:");
            sb.AppendFormat("- Sent: {0}\n", this.Sent);
            sb.AppendFormat("- StatusCode: {0}\n", this.StatusCode);
            sb.AppendFormat("- Content: {0}\n", this.Content);
            return sb.ToString();
        }
    }
}
