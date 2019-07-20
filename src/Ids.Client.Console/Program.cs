using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ids.Client1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // 从元数据发现端点
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return;
            }

            // 请求令牌
            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "ConsoleClientCredentials",
                ClientSecret = "ConsoleClientCredentials",
                Scope = "userCenter"
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }

            Console.WriteLine(tokenResponse.Json);

            //调用api
            var clientApi = new HttpClient();
            clientApi.SetBearerToken(tokenResponse.AccessToken);

            var response = await clientApi.GetAsync("http://localhost:5010/api/users");
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
            }
        }
    }
}
