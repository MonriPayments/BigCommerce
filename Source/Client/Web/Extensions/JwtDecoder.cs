using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace BigCommerceApi.Client.Web.Extensions
{
    public class JwtDecoder
    {
        public static T? DecodeJwt<T>(string jwt, string secretKey) where T : class, new()
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);

                var keyId = token.Header.Kid;
                var audience = token.Audiences.ToList();
                var claims = token.Claims.Select(claim => (claim.Type, claim.Value)).ToList();
                var payload = new T();

                foreach (var claim in claims)
                {
                    var property = typeof(T).GetProperties()
                        .FirstOrDefault(p => string.Equals(p.Name, claim.Type, StringComparison.OrdinalIgnoreCase));

                    if (property != null && property.CanWrite)
                    {
                        // If the property is a complex type, deserialize the claim value
                        if (property.PropertyType != typeof(string))
                        {
                            var deserializedValue = JsonConvert.DeserializeObject(claim.Value, property.PropertyType);
                            property.SetValue(payload, deserializedValue);
                        }
                        else
                        {
                            property.SetValue(payload, claim.Value);
                        }
                    }
                }

                return payload;
            }
            catch (SecurityTokenException ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return null;
        }
    }
}
