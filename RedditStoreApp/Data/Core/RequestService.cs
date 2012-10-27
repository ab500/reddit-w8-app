using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Net;

namespace RedditStoreApp.Data.Core
{
    class RequestService
    {
        private HttpClient _client;
        private HttpClientHandler _handler;
        private CookieContainer _cookiejar;


        private TimeSpan _requestTimeout;
        private TimeSpan _cacheTimeout;
        private Dictionary<string, Response> _cache;
        private string _cookie;

        public RequestService()
        {
            _cookiejar = new CookieContainer();

            _handler = new HttpClientHandler()
            {
                CookieContainer = _cookiejar,
                UseCookies = true
            };

            _client = new HttpClient(_handler);
            _client.DefaultRequestHeaders.Add("user-agent", "wastingtime1's Prototype Windows Store Reddit App");

            _requestTimeout = TimeSpan.FromSeconds(5);
            _cacheTimeout = TimeSpan.FromSeconds(30);
            _cache = new Dictionary<string, Response>();

            _client.BaseAddress = new Uri("http://www.reddit.com/");
        }

        public async Task<Response> GetAsync(string resource, bool useCache)
        {
            FormatResource(ref resource);

            if (useCache && IsCached(resource))
            {
                return _cache[resource];
            }

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(_requestTimeout);

            HttpResponseMessage httpResp = null;
            string httpRespContent = null;

            try
            {
                httpResp = await _client.GetAsync(
                    new Uri(resource, UriKind.RelativeOrAbsolute),
                    HttpCompletionOption.ResponseContentRead,
                    cts.Token
                );
                if (httpResp.IsSuccessStatusCode)
                {
                    httpRespContent = await httpResp.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                httpRespContent = ex.Message;
            }

            Response resp = BuildResponse(httpRespContent, httpResp);
                
            if (resp.IsSuccess && useCache)
            {
                _cache[resource] = resp;
            }

            return resp;
        }

        public async Task<Response> PostAsync(string resource, List<KeyValuePair<String, String>> paramas)
        {
            FormUrlEncodedContent httpContent = new FormUrlEncodedContent(paramas);
            FormatResource(ref resource);

            CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(_requestTimeout);

            HttpResponseMessage httpResp = null;
            string httpRespContent = null;

            try
            {
                httpResp = await _client.PostAsync(
                    new Uri(resource, UriKind.RelativeOrAbsolute),
                    httpContent,
                    cts.Token
                );
                if (httpResp.IsSuccessStatusCode)
                {
                    httpRespContent = await httpResp.Content.ReadAsStringAsync();
                }
            }
            catch (HttpRequestException ex)
            {
                httpRespContent = ex.Message;
            }

            return BuildResponse(httpRespContent, httpResp);
        }

        public TimeSpan Timeout
        {
            get
            {
                return _requestTimeout;
            }
            set
            {
                _requestTimeout = value;
            }
        }

        private Response BuildResponse(string content, HttpResponseMessage httpResp)
        {
            if (httpResp != null)
            {
                return new Response(content, DateTime.Now, httpResp.StatusCode, httpResp.IsSuccessStatusCode);
            }
            else
            {
                // If the request fialed we use a fake status code. Is this best practice?
                return new Response(content, DateTime.Now, HttpStatusCode.RequestTimeout, false);
            }
        }

        private bool IsCached(string resource)
        {
            if (_cache.ContainsKey(resource))
            {
                if (DateTime.Now - _cache[resource].Sent < _cacheTimeout)
                {
                    return true;
                }
                _cache.Remove(resource);
            }
            return false; 
        }

        private void FormatResource(ref string resource)
        {
            string queryParams = "";
            if (resource.Contains("?"))
            {
                var pieces = resource.Split(new char[] { '?' });
                resource = pieces[0];
                queryParams = pieces[1];
            }

            if (resource[0] == '/' && resource.Length > 1)
            {
                resource = resource.Substring(1, resource.Length - 1);
            }

            if (resource[resource.Length - 1] == '/' && resource.Length > 1)
            {
                resource = resource.Substring(0, resource.Length - 1);
            }

            resource = resource + ".json?" + queryParams;
        }
    }
}
