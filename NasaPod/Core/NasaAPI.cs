using Nasa.Model.Nasa;
using Newtonsoft.Json;
using RestSharp;

namespace Nasa.Core
{
    internal class NasaAPI
    {
        protected string key { get; set; }
        public NasaAPI(string apikey) {
            key = apikey;
        }
        internal APOD PictureOfDay(AppSettings settings)
        {
            var client = new RestClient(settings.Endpoint);
            var request = new RestRequest("/planetary/apod", Method.Get);
            request.RequestFormat = DataFormat.Json;
            request.AddQueryParameter("api_key", settings.ApiKey);

            RestResponse response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                if (response.Content != null)
                {
                    APOD respObject = JsonConvert.DeserializeObject<APOD>(response.Content);
                    return respObject != null ? respObject : new APOD();
                }
                else
                {
                    throw new Exception("Content null");
                }
            }
            else
            {
                throw new Exception(response.ErrorMessage);
            }
        }
    }
}
