using Budgeting.App.Api.Models.Users;
using Budgeting.App.Api.Models.Users.Exceptions;
using Budgeting.App.Api.Services.Foundations.Users;
using Microsoft.AspNetCore.Mvc;

namespace Budgeting.App.Api.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
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
        public async ValueTask<IActionResult> PostUserAsync([FromBody] User user, string password)
        {
            try
            {
                User createdUser =
                    await this.userService.AddUserAsync(user, password);

                return CreatedAtAction(nameof(GetUserByIdAsync)
                    , new { userId = user.Id }, user);
            }
            catch (UserValidationException userValidationException)
               when (userValidationException.InnerException is AlreadyExistsUserException)
            {
                return Conflict(userValidationException.InnerException);
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
