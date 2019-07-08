using System;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace keyvault
{
    class Program
    {        
        static async Task Main(string[] args)
        {
            Console.WriteLine("Connecting to KeyVault through Proxy...");
            
            var _kv = new kvHelper(Environment.GetEnvironmentVariable("proxyUrl"));

            Console.WriteLine("Setting secret 'testSecret' to 'secretValue'...");
            var setSecret = await _kv.SetSecret("testSecret","secretValue");

            Console.WriteLine("Getting secret 'testSecret'... ");
            Console.WriteLine($"Value for testSecret is: {await _kv.GetSecret("testSecret")}");
        }
    }

    class kvHelper
    {
        private string proxyUrl;

        private string clientId = Environment.GetEnvironmentVariable("akvClientId");
        private string clientSecret = Environment.GetEnvironmentVariable("akvClientSecret");
        private string tenantId = Environment.GetEnvironmentVariable("akvTenantId");
        private string subscriptionId = Environment.GetEnvironmentVariable("akvSubscriptionId");
    
        private KeyVaultClient kvClient;

        public kvHelper(string proxyUrl){
            this.proxyUrl = proxyUrl;
            AzureCredentials credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(clientId, clientSecret, tenantId, AzureEnvironment.AzureGlobalCloud).WithDefaultSubscription(subscriptionId);
            kvClient = new KeyVaultClient(async (authority, resource, scope) =>
            {
                var adCredential = new ClientCredential(clientId, clientSecret);
                var authenticationContext = new AuthenticationContext(authority, null);
                return (await authenticationContext.AcquireTokenAsync(resource, adCredential)).AccessToken;
            });
        }
        public async Task<SecretBundle> SetSecret(string secretName, string secretValue)
        {
            var secretBundle = await kvClient.SetSecretAsync($"{proxyUrl}", secretName, secretValue);
            return secretBundle;
        }

        public async Task<string> GetSecret(string secretName)
        {
            var keyvaultSecret = await kvClient.GetSecretAsync($"{proxyUrl}/secrets/{secretName}").ConfigureAwait(false);
            return keyvaultSecret.Value;
        }
    }
}
