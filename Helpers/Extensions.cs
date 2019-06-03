using Microsoft.AspNetCore.Http;

namespace SportsCentre.API.Helpers
{
    public static class Extensions
    {
        /*
         * These headers are provided to the startup class for use with authenticating the client.
         */
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}