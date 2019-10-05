using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FL.Common.Base.Configuration;
using FL.Common.Base.Exceptions;
using FL.Services.DatabaseAccessors.ApiKeys;
using FL.Shared.ClientHttp;
using FL.Shared.ClientHttp.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FootballLiga.API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected ILogger _logger;
        protected readonly AppConfiguration _configuration;
        private readonly IAuthenticationService _authenticationService;
        private readonly JsonSerializerSettings _serializationSettings;

        protected const string SUCCES_STATUS = "SUCCES";
        protected const string FAILED_STATUS = "FAILED";

        public BaseController(
            ILogger logger,
            AppConfiguration configuration, 
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _configuration = configuration;
            _authenticationService = authenticationService;
            _serializationSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        }

        protected async Task<Tuple<bool, List<ErrorMessageDC>>> Authenticate()
        {
            var errors = new List<ErrorMessageDC>();

            if (!Request.Headers["ApiKey"].Any())
                errors.Add(new ErrorMessageDC { Message = "Authentication failed.", Description = "No ApiKey is provided." });

            if (!Request.Headers["Username"].Any())
                errors.Add(new ErrorMessageDC { Message = "Authentication failed.", Description = "No Username is provided." });

            if (errors.Any())
                return new Tuple<bool, List<ErrorMessageDC>>(false, errors);

            var username = Request.Headers["Username"].FirstOrDefault().ToLower();
            var apiKey = Request.Headers["ApiKey"].FirstOrDefault().ToLower();

            var authenticateResponse = await _authenticationService.Authenticate(new AuthenticateRequest(username, apiKey));

            if (authenticateResponse != null)
                return new Tuple<bool, List<ErrorMessageDC>>(true, new List<ErrorMessageDC>());

            errors.Add(new ErrorMessageDC { Message = "Authentication failed.", Description = "Authentication has failed." });
            return new Tuple<bool, List<ErrorMessageDC>>(false, errors);
        }

        protected async Task<ResponseDC<T>> CreateResponse<T>(List<ErrorMessageDC> errorMessages = null)
        {
            var response = new ResponseDC<T>();

            try
            {
                var result = await Authenticate();

                if (result.Item1)
                    response.ErrorMessages = result.Item2;

                if (errorMessages != null && errorMessages.Any())
                {
                    var errorMessagesResponse = response.ErrorMessages.Select(e => new ErrorMessageDC { Message = e.Message, Description = e.Description });
                    response.ErrorMessages.AddRange(errorMessagesResponse);
                }
            }
            catch (Exception exception)
            {
                throw new DatabaseServiceException();
            }

            return response;
        }

        protected async Task WriteResponse(object responseToSerialize)
        {
            byte[] data = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(responseToSerialize, _serializationSettings));
            Response.ContentType = "application/json";
            await Response.Body.WriteAsync(data, 0, data.Length);
        }
    }
}