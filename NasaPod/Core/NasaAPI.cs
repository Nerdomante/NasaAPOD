using Nasa.Model.Nasa;
using RestSharp;
using System.Text.Json;

namespace Nasa.Core
{
    internal class NasaAPI
    {
        protected string key { get; set; }
        public NasaAPI(string apikey) {
            key = apikey;
        }

        internal async Task<APOD> PictureOfDayAsync(AppSettings settings)
        {
            var client = new RestClient(settings.Endpoint);
            var request = new RestRequest("/planetary/apod", Method.Get);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("api_key", settings.ApiKey);

            RestResponse response = await client.ExecuteAsync(request);
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    if (response.Content != null)
                    {
                        APOD respObject = JsonSerializer.Deserialize<APOD>(response.Content);
                        return respObject != null ? respObject : new APOD();
                    }
                    else
                    {
                        throw new Exception("Content null");
                    }
                case System.Net.HttpStatusCode.BadRequest:
                    throw new Exception("Bad request");
                case System.Net.HttpStatusCode.Unauthorized:
                    throw new Exception("Unauthorized");
                case System.Net.HttpStatusCode.Forbidden:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.NotFound:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.MethodNotAllowed:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.NotAcceptable:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.RequestTimeout:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.Conflict:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.Gone:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.LengthRequired:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.PreconditionFailed:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.RequestEntityTooLarge:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.RequestUriTooLong:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.UnsupportedMediaType:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.RequestedRangeNotSatisfiable:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.ExpectationFailed:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.MisdirectedRequest:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.UnprocessableEntity:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.Locked:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.FailedDependency:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.UpgradeRequired:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.PreconditionRequired:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.TooManyRequests:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.RequestHeaderFieldsTooLarge:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.UnavailableForLegalReasons:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.InternalServerError:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.NotImplemented:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.BadGateway:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.ServiceUnavailable:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.GatewayTimeout:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.HttpVersionNotSupported:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.VariantAlsoNegotiates:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.InsufficientStorage:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.LoopDetected:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.NotExtended:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                case System.Net.HttpStatusCode.NetworkAuthenticationRequired:
                    throw new Exception($"Service temporary unavaiable ({response.StatusCode})");
                default:
                    throw new Exception(response.StatusCode.ToString());
            }
        }
    }
}
