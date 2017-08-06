using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Today.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        string subscriptionKey;
        string token;
        const int RefreshTokenDuration = 9;

        public AuthenticationService(string apiKey)
        {
            subscriptionKey = apiKey;
        }

        public async Task InitializeAsync()
        {
            token = await FetchTokenAsync(Constants.AuthenticationTokenEndpoint, subscriptionKey);
       }

        public string GetAccessToken()
        {
            return token;
        }

        async Task RenewAccessToken()
        {
            token = await FetchTokenAsync(Constants.AuthenticationTokenEndpoint, subscriptionKey);
            Debug.WriteLine("Renewed token.");
        }

        async Task<string> FetchTokenAsync(string fetchUri, string apiKey)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
                UriBuilder uriBuilder = new UriBuilder(fetchUri);
                uriBuilder.Path += "/issueToken";

                var result = await client.PostAsync(uriBuilder.Uri.AbsoluteUri, null);
                return await result.Content.ReadAsStringAsync();
            }
        }
    }
}
