using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ids.Client.Consoles
{
    public class PasswordClientDemo
    {
        void Log(string msg)
        {
            Console.WriteLine(msg);
        }
        async Task<string> GetAccessTokenAsync()
        {
            Log("[AccessToken]开始获取");
            var accessToken = "";
            // 从元数据发现端点
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Log("[AccessToken]" + disco.Error);
                return accessToken;
            }

            // 请求令牌
            var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = disco.TokenEndpoint,

                ClientId = "PasswordClient",
                ClientSecret = "PasswordClient",
                UserName = "zhangsan",
                Password = "password",
                Scope = "userCenter",
            });

            if (tokenResponse.IsError)
            {
                Log("[AccessToken]" + tokenResponse.Error);
                return accessToken;
            }
            accessToken = tokenResponse.AccessToken;
            return accessToken;
        }

        async Task<string> GetApiResponseAsync(string token, string apiUrl = "http://localhost:5010/api/identity")
        {
            Log("[ApiRequest]开始获取资源:" + apiUrl);
            //调用api
            var clientApi = new HttpClient();
            clientApi.SetBearerToken(token);

            var response = await clientApi.GetAsync(apiUrl);
            if (!response.IsSuccessStatusCode)
            {
                Log("[ApiRequest]失败:" + response.StatusCode);
                return "";
            }
            var content = await response.Content.ReadAsStringAsync();
            Log("[ApiRequest]成功:" + content);
            return content;
        }


        public async Task RunAsync()
        {
            var preFix = $"[{nameof(PasswordClientDemo)}]";
            Log(preFix + "1.获取accesstoken");
            Log(preFix + "2.根据accesstoken请求受保护的资源");
            Log(preFix + "======================开始====================");
            var accesstoken = await GetAccessTokenAsync();
            var response = await GetApiResponseAsync(accesstoken);
            Log(preFix + "======================结束====================");
        }
    }
}
