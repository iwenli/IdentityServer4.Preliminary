using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Ids.Client.Consoles
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // client credentials
            await new ClientCredentialsDemo().RunAsync();
            // password client
            await new PasswordClientDemo().RunAsync();

            Console.ReadLine();
        }
    }
}
