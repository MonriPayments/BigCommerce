using BigCommerceApi.Domain.Model.Administrators;
using BigCommerceApi.Domain.Model.Shops;
using BigCommerceApi.Domain.Projections.Auth;
using BigCommerceApi.Domain.Projections.StoreConfigurations;
using BigCommerceApi.Domain.Services;
using BigCommerceApi.Domain.Services.Helpers;
using BigCommerceApi.Domain.Services.Jwt;
using BigCommerceApi.Domain.Services.Logging;
using BigCommerceApi.Domain.Services.Shops.UpdateShop;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Remotion.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebStudio.Entities.Core;
using WebStudio.Entities.Interaction;
using ILogger = WebStudio.Logging.Abstractions.ILogger;

namespace BigCommerceApi.Persistency.NHibernate.QueryHandlers.Auth
{
    public class LoginHandler : BaseCommandHandler, IAsyncQueryHandler<LoginQuery, Response<LoginProjection>>
    {
        private readonly ILogger _logger;
        private readonly JwtTokenGenerator _tokenGenerator;
        public LoginHandler(WebStudio.Entities.Interaction.IQueryExecutor queryExecutor, UnitOfWork unitOfWork, ILogger logger, JwtTokenGenerator jwtTokenGenerator) : base(queryExecutor, unitOfWork)
        {
            _logger = logger;
            _tokenGenerator = jwtTokenGenerator ?? throw new ArgumentNullException(nameof(jwtTokenGenerator));
        }

        public async Task<Response<LoginProjection>> ExecuteAsync(LoginQuery query, CancellationToken cancellationToken)
        {
            try
            {
                if (query == null)
                {
                    throw new ArgumentNullException(nameof(query));
                }
                // retrieve Administrator by email
                Administrator admin = await QueryExecutor.GetOneAsync<Administrator>(a => a.Email == query.Email, cancellationToken);
                if (admin == null)
                {
                    throw new InvalidOperationException();
                }
                // hash the password and compare
                var hashedPass = HashPasswordSha512(query.Password).ToLower();
                if (!admin.Password.Equals(hashedPass))
                {
                    throw new InvalidOperationException();
                }
                // generate JWT token
                string token = _tokenGenerator.GenerateToken(admin.Email);
                return new Response<LoginProjection>(new LoginProjection
                {
                    Token = token
                });

            }
            catch (Exception ex)
            {
                IDictionary<string, string>? additionalData = new Dictionary<string, string>
                    {
                        { "Handler", "LoginHandler" },
                    };
                _logger.LogApplicationError(ex, additionalData);
                return ResponseHelper.CreateErrorResponse<LoginProjection>(ErrorMessages.GenericError);
            }
        }
        public static string HashPasswordSha512(string password)
        {
            using var sha512 = SHA512.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(password);
            byte[] hash = sha512.ComputeHash(bytes);
            return Convert.ToHexString(hash); // .NET 5+
        }
    }
}
