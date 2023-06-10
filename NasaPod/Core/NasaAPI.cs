using Nasa.Model.Nasa;
using RestSharp;
using System.Net;
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
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Bad Request");
                case System.Net.HttpStatusCode.Unauthorized:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Unauthorized");
                case System.Net.HttpStatusCode.Forbidden:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Forbidden");
                case System.Net.HttpStatusCode.NotFound:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Not Found");
                case System.Net.HttpStatusCode.MethodNotAllowed:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Method Not Allowed");
                case System.Net.HttpStatusCode.NotAcceptable:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Not Acceptable");
                case System.Net.HttpStatusCode.ProxyAuthenticationRequired:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Proxy Authentication Required");
                case System.Net.HttpStatusCode.RequestTimeout:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Request Timeout");
                case System.Net.HttpStatusCode.Conflict:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Conflict");
                case System.Net.HttpStatusCode.Gone:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Gone");
                case System.Net.HttpStatusCode.LengthRequired:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Length Required");
                case System.Net.HttpStatusCode.PreconditionFailed:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Precondition Failed");
                case System.Net.HttpStatusCode.RequestEntityTooLarge:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Request Entity Too Large");
                case System.Net.HttpStatusCode.RequestUriTooLong:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Request Uri Too Long");
                case System.Net.HttpStatusCode.UnsupportedMediaType:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Unsupported Media Type");
                case System.Net.HttpStatusCode.RequestedRangeNotSatisfiable:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Requested Range Not Satisfiable");
                case System.Net.HttpStatusCode.ExpectationFailed:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Expectation Failed");
                case System.Net.HttpStatusCode.MisdirectedRequest:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Misdirected Request");
                case System.Net.HttpStatusCode.UnprocessableEntity:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Unprocessable Entity");
                case System.Net.HttpStatusCode.Locked:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Locked");
                case System.Net.HttpStatusCode.FailedDependency:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Failed Dependency");
                case System.Net.HttpStatusCode.UpgradeRequired:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Upgrade Required");
                case System.Net.HttpStatusCode.PreconditionRequired:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Precondition Required");
                case System.Net.HttpStatusCode.TooManyRequests:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Too Many Requests");
                case System.Net.HttpStatusCode.RequestHeaderFieldsTooLarge:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Request Header Fields Too Large");
                case System.Net.HttpStatusCode.UnavailableForLegalReasons:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Unavailable For Legal Reasons");
                case System.Net.HttpStatusCode.InternalServerError:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Internal Server Error");
                case System.Net.HttpStatusCode.NotImplemented:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Not Implemented");
                case System.Net.HttpStatusCode.BadGateway:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Bad Gateway");
                case System.Net.HttpStatusCode.ServiceUnavailable:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Service Unavailable");
                case System.Net.HttpStatusCode.GatewayTimeout:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Gateway Timeout");
                case System.Net.HttpStatusCode.HttpVersionNotSupported:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: HTTP Version Not Supported");
                case System.Net.HttpStatusCode.VariantAlsoNegotiates:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Variant Also Negotiates");
                case System.Net.HttpStatusCode.InsufficientStorage:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Insufficient Storage");
                case System.Net.HttpStatusCode.LoopDetected:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Loop Detected");
                case System.Net.HttpStatusCode.NotExtended:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Not Extended");
                case System.Net.HttpStatusCode.NetworkAuthenticationRequired:
                    throw new Exception($"Endpoint unavailable \n{(int)response.StatusCode}: Network Authentication Required");
                default:
                    throw new Exception($"Unknown status code: {response.StatusCode}");
            }
        }           
    }
}
