using Microsoft.AspNetCore.Http;

namespace DatingApp.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            // using reponse.Headers allows a dev to add on additional header information to a server response
            response.Headers.Add("Application-Error", message);
            // the following headers are added to simply allow the first custom header to be displayed
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }
    }
}