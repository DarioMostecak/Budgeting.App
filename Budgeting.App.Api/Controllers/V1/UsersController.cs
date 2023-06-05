using Budgeting.App.Api.Models.Accounts.Exceptions;
using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using Budgeting.App.Api.Services.Foundations.Users;
using Budgeting.App.Api.Services.Orchestrations.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Budgeting.App.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : BaseController
    {
        private readonly IUserService userService;
        private readonly IUserOrchestrationService userOrchestrationService;

        public UsersController(
            IUserService userService,
            IUserOrchestrationService userOrchestrationService)
        {
            this.userService = userService;
            this.userOrchestrationService = userOrchestrationService;
        }

        [HttpGet]
        [Route("{userId}")]
        public async ValueTask<IActionResult> GetUserByIdAsync(Guid userId)
        {
            try
            {
                User user =
                    await this.userService.RetrieveUserByIdAsync(userId);

                return Ok(user);

            }
            catch (UserValidationException userValidationException)
               when (userValidationException.InnerException is NotFoundUserException)
            {
                return NotFound(userValidationException.InnerException);
            }
            catch (UserValidationException userValidationException)
            {
                return BadRequest(userValidationException);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                return InternalServerError(userServiceException);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async ValueTask<IActionResult> PostUserAsync([FromBody] User user, string password)
        {
            try
            {
                User createdUser =
                    await this.userOrchestrationService.RegisterUserAsync(user, password);

                return Created(createdUser);
            }
            catch (UserOrchestrationValidationException userOrchestrationValidationException)
            {
                return BadRequest(userOrchestrationValidationException);
            }
            catch (UserOrchestrationDependencyValidationException userOrchestrationDependencyValidationException)
              when (userOrchestrationDependencyValidationException.InnerException is AlreadyExistsUserException)
            {
                return Conflict(userOrchestrationDependencyValidationException.InnerException);
            }
            catch (UserOrchestrationDependencyValidationException userOrchestrationDependencyValidationException)
              when (userOrchestrationDependencyValidationException.InnerException is AlreadyExistsAccountExceptions)
            {
                return Conflict(userOrchestrationDependencyValidationException.InnerException);
            }
            catch (UserOrchestrationDependencyValidationException userOrchestrationDependencyValidationException)
            {
                return BadRequest(userOrchestrationDependencyValidationException);
            }
            catch (UserOrchestrationDependencyException userOrchestrationDependencyException)
            {
                return InternalServerError(userOrchestrationDependencyException);
            }
        }

        [HttpPut]
        public async ValueTask<IActionResult> PutUserAsync([FromBody] User user)
        {
            try
            {
                User updateUser =
                    await this.userService.ModifyUserAsync(user);

                return Ok(updateUser);
            }
            catch (UserValidationException userValidationException)
               when (userValidationException.InnerException is NotFoundUserException)
            {
                return NotFound(userValidationException.InnerException);
            }
            catch (UserValidationException userValidationException)
            {
                return BadRequest(userValidationException);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserServiceException userServiceException)
            {
                return InternalServerError(userServiceException);
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        public async ValueTask<IActionResult> DeleteUserAsync(Guid userId)
        {
            try
            {
                User deletedUser =
                    await this.userService.RemoveUserByIdAsync(userId);

                return Ok(deletedUser);
            }
            catch (UserValidationException userValidationException)
               when (userValidationException.InnerException is NotFoundUserException)
            {
                return NotFound(userValidationException.InnerException);
            }
            catch (UserValidationException userValidationException)
            {
                return BadRequest(userValidationException);
            }
            catch (UserDependencyException userDependencyException)
            {
                return InternalServerError(userDependencyException);
            }
            catch (UserServiceException userServiceExceptions)
            {
                return InternalServerError(userServiceExceptions);
            }
        }
    }
}
