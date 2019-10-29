using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Morsley.UK.YearPlanner.Users.API.Models.v1.Response;
using Morsley.UK.YearPlanner.Users.Application.Commands;
using Morsley.UK.YearPlanner.Users.Application.Queries;
using Morsley.UK.YearPlanner.Users.Domain.Interfaces;
using System;
using System.Threading.Tasks;

namespace Morsley.UK.YearPlanner.Users.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UsersController : ControllerBase
    {
        #region Fields

        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        #endregion Fields

        #region Constructors

        public UsersController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        #endregion Constructors

        #region GET

        /// <summary>
        /// Get a page of users
        /// </summary>
        /// <param name="getUsersRequest">
        /// A GetUsersRequest object which contains fields for paging, searching, filtering, sorting and shaping user data</param>
        /// <returns>A page of users</returns>
        /// <response code="200">Success - OK - Returns the requested page of users</response>
        /// <response code="204">Success - No Content - No users matched given criteria</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromQuery] API.Models.v1.Request.GetUsersRequest getUsersRequest)
        {
            if (getUsersRequest == null) return BadRequest();

            var userResponses = await GetUserResponses(getUsersRequest);

            if (userResponses == null || !userResponses.Any()) return NoContent();

            return Ok(userResponses);
        }

        /// <summary>
        /// Get a user
        /// </summary>
        /// <param name="getUserRequest">The unique identifier of the user</param>
        /// <returns>The requested user</returns>
        /// <response code="200">Success - OK - Returns the requested user</response>
        /// <response code="204">Success - No Content - No user matched the given identifier</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get([FromRoute] API.Models.v1.Request.GetUserRequest getUserRequest)
        {
            if (getUserRequest == null) return BadRequest();

            var getUserResponse = await GetUserResponse(getUserRequest);

            if (getUserResponse == null) return NoContent();

            return Ok(getUserResponse);
        }

        #endregion GET

        #region POST

        /// <summary>
        /// Add a user
        /// </summary>
        /// <param name="request">A CreateUserRequest object which contains all the necessary data to create a user</param>
        /// <returns>A URI to the newly created user in the header (location)</returns>
        /// <response code="201">Success - Created - The user was successfully created</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response> 
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Add([FromBody] API.Models.v1.Request.CreateUserRequest request)
        {
            if (request == null) return BadRequest();

            var response = await AddUser(request);

            return CreatedAtRoute("GetUser", new { id = response.Id }, response);
        }

        #endregion POST

        #region PUT

        /// <summary>
        /// Fully update a user
        /// </summary>
        /// <param name="id">The unique identifier of the user</param>
        /// <param name="request">An UpdateUserRequest object which contains all the updates</param>
        /// <returns>The updated user</returns>
        /// <response code="200">Success - OK - The user was successfully updated</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response>
        /// <response code="404">Error - Not Found - No user matched the given identifier</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] API.Models.v1.Request.UpdateUserRequest request)
        {
            if (request == null) return BadRequest();
            request.Id = id;

            var user = await UpdateUser(request);

            return Ok(user);
        }

        #endregion PUT

        #region PATCH

        /// <summary>
        /// Fully or partially update a user
        /// </summary>
        /// <param name="id">The unique identifier of the user</param>
        /// <param name="patchDocument">
        /// A JSON Patch Document detailing the full or partial updates to the user
        /// </param>
        /// <returns>The updated user</returns>
        /// <response code="200">Success - OK - The user was successfully updated</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response>
        /// <response code="404">Error - Not Found - No user matched the given identifier</response>
        /// <response code="422">Error - Unprocessable Entity - Unable to process the contained instructions.</response>
        /// <remarks>
        /// Sample request (this request updates the user's first name): \
        /// PATCH /users/id \
        /// [ \
        ///     { \
        ///         "op": "replace", \
        ///         "path": "/firstname", \
        ///         "value": "Dave" \
        ///     } \
        /// ] \
        /// </remarks>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Update(
            [FromRoute] Guid id,
            [FromBody] API.Models.v1.Request.PartiallyUpdateUserRequest request)
        {
            if (request == null) return BadRequest();
            request.Id = id;

            var updatedUser = await UpdateUser(request);

            return Ok(updatedUser);
        }

        #endregion PATCH

        #region DELETE

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">The unique identifier of the user</param>
        /// <returns>Nothing</returns>
        /// <response code="204">Success - No Content - User was successfully deleted</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response>
        /// <response code="404">Error - Not Found - No user matched the given identifier</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] API.Models.v1.Request.DeleteUserRequest request)
        {
            if (request == null) return BadRequest();

            await DeleteUser(request);

            return NoContent();
        }

        #endregion DELETE

        #region Methods

        private async Task<API.Models.v1.Response.UserResponse> AddUser(API.Models.v1.Request.CreateUserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var addUserCommand = _mapper.Map<AddUserCommand>(request);

            var addedUserId = await _mediator.Send(addUserCommand);

            return new UserResponse { Id = addedUserId };
        }

        private async Task DeleteUser(API.Models.v1.Request.DeleteUserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // ToDo --> Use AutoMapper!
            var deleteUserCommand = new DeleteUserCommand();

            await _mediator.Send(deleteUserCommand);

            return;
        }

        private async Task<API.Models.v1.Response.UserResponse?> GetUserResponse(API.Models.v1.Request.GetUserRequest getUserRequest)
        {
            if (getUserRequest == null) throw new ArgumentNullException(nameof(getUserRequest));

            var getUserQuery = _mapper.Map<GetUserQuery>(getUserRequest);

            var user = await _mediator.Send(getUserQuery);
            if (user == null) return null;

            var userResponse = _mapper.Map<UserResponse>(user);

            return userResponse;
        }

        private async Task<IPagedList<API.Models.v1.Response.UserResponse>> GetUserResponses(API.Models.v1.Request.GetUsersRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // ToDo --> Use AutoMapper!
            var query = new GetUsersQuery();

            var pageOfUsers = await _mediator.Send(query);

            // ToDo --> Use AutoMapper!
            var pageOfUserResponses = API.Models.v1.PagedList.Create(pageOfUsers);

            return pageOfUserResponses;
        }

        private async Task<API.Models.v1.Response.UserResponse> UpdateUser(API.Models.v1.Request.UpdateUserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // ToDo --> Use AutoMapper!
            var updateUserCommand = new UpdateUserCommand();

            var updatedUser = await _mediator.Send(updateUserCommand);

            // ToDo --> Use AutoMapper!
            var updatedUserResponse = new UserResponse();

            return updatedUserResponse;
        }

        private async Task<API.Models.v1.Response.UserResponse> UpdateUser(API.Models.v1.Request.PartiallyUpdateUserRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // ToDo --> Use AutoMapper!
            var partialUpdateUserCommand = new PartialUpdateUserCommand();

            var updatedUser = await _mediator.Send(partialUpdateUserCommand);

            // ToDo --> Use AutoMapper!
            var updatedUserResponse = new UserResponse();

            return updatedUserResponse;
        }

        #endregion Methods
    }
}