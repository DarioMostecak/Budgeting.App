using Budgeting.App.Api.Models.IdentityRequests;
using Budgeting.App.Api.Models.IdentityRequests.Exceptions;
using Budgeting.App.Api.Models.IdentityResponses;
using Budgeting.App.Api.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budgeting.App.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [AllowAnonymous]
    public class IdentityController : BaseController
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost]
        public async ValueTask<IActionResult> LogInAsync([FromBody] IdentityRequest identityRequest)
        {
            try
            {
                IdentityResponse identityResponse =
                    await this.identityService.AuthenticateUserAsync(identityRequest);

                return Ok(identityResponse);
            }
            catch (IdentityRequestValidationException identityRequestException)
              when (identityRequestException.InnerException is FailAuthenticationIdentityRequestException)
            {
                return Unauthorized(identityRequestException.InnerException);
            }
            catch (IdentityRequestValidationException identityRequestValidationException)
            {
                return BadRequest(identityRequestValidationException);
            }
            catch (IdentityRequestDependencyException identityRequestDependencyException)
            {
                return InternalServerError(identityRequestDependencyException);
            }
            catch (IdentityRequestServiceException identityRequestServiceException)
            {
                return InternalServerError(identityRequestServiceException);
            }



        }
    }
}
