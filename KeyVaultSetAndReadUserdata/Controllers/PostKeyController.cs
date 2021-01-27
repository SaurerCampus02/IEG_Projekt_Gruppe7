using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace KeyVaultService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostKeyController : ControllerBase
    {
        [HttpPost]
        public IActionResult LoginWithUsernameAndPassword(string username, string password)
        {          
            var client = new SecretClient(new Uri($"https://IEGKeyVaultService.vault.azure.net/"),
                                                             new DefaultAzureCredential());

            // https://docs.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing?view=aspnetcore-5.0
            // generate a 128-bit salt using PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // derive a 256-bit subkey (use HMACSHA256 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            
            var secret = new KeyVaultSecret(username, hashed);
            secret.Properties.ExpiresOn = DateTimeOffset.Now.AddYears(1);

            client.SetSecret(secret);

            return Ok(username + " wurde angelegt");
        }
    }
}
