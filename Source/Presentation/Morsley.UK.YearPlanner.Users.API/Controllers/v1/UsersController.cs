using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Morsley.UK.YearPlanner.Users.API.Models;
using System;
using System.Collections.Generic;

namespace Morsley.UK.YearPlanner.Users.API.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/users")]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UsersController : ControllerBase
    {
        #region GET

        /// <summary>
        /// Get a page of users
        /// </summary>
        /// <param name="request">
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
        public IActionResult Get(API.Models.GetUsersRequest request)
        {
            if (request == null) return BadRequest();

            var users = GetUsers(request);

            if (users == null || !users.Any()) return NoContent();

            return Ok(users);
        }

        /// <summary>
        /// Get a user
        /// </summary>
        /// <param name="id">The unique identifier of the user</param>
        /// <returns>The requested user</returns>
        /// <response code="200">Success - OK - Returns the requested user</response>
        /// <response code="204">Success - No Content - No user matched the given identifier</response>
        /// <response code="400">Error - Bad Request - It was not possible to bind the request JSON</response>
        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Get([FromRoute] Guid id)
        {
            if (id == default(Guid)) return BadRequest();

            var user = GetUser(id);

            if (user == null) return NoContent();

            return Ok(user);
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
        public IActionResult Add([FromBody] API.Models.CreateUserRequest request)
        {
            if (request == null) return BadRequest();

            var user = AddUser(request);

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
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
        public IActionResult Update(
            [FromRoute] Guid id,
            [FromBody] API.Models.UpdateUserRequest request)
        {
            if (request == null) return BadRequest();

            // ToDo --> Update the user on the system
            var user = UpdateUser(id, request);

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
        public IActionResult Update(
            [FromRoute] Guid id,
            [FromBody] JsonPatchDocument<UpdateUserRequest> patchDocument)
        {
            if (patchDocument == null) return BadRequest();

            // ToDo --> Update the user on the system
            var user = UpdateUser(id, patchDocument);

            return Ok(user);
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
        public IActionResult Delete([FromRoute] Guid id)
        {
            if (id == default(Guid)) return BadRequest();

            DeleteUser(id);

            return NoContent();
        }

        #endregion DELETE

        #region Methods

        private UserResponse AddUser(CreateUserRequest request)
        {
            // ToDo --> Try and add user to system

            // For now, fake successful response...
            return new UserResponse { Id = Guid.NewGuid() };
        }

        private void DeleteUser(Guid id)
        {
            if (id == default(Guid)) throw new ArgumentNullException(nameof(id));

            // ToDo --> Try and delete the user from system

            return;
        }

        private UserResponse GetUser(Guid id)
        {
            if (id == default(Guid)) throw new ArgumentNullException(nameof(id));

            // ToDo --> Try and get user from system

            return null;
        }

        private IEnumerable<UserResponse> GetUsers(API.Models.GetUsersRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            // ToDo --> Try and get users from system

            return null;
        }

        private UserResponse UpdateUser(Guid id, UpdateUserRequest request)
        {
            if (id == default(Guid)) throw new ArgumentNullException(nameof(id));
            if (request == null) throw new ArgumentNullException(nameof(request));

            // ToDo --> Try and update user in system

            return null;
        }

        private UserResponse UpdateUser(Guid id, JsonPatchDocument<UpdateUserRequest> patchDocument)
        {
            if (id == default(Guid)) throw new ArgumentNullException(nameof(id));
            if (patchDocument == null) throw new ArgumentNullException(nameof(patchDocument));

            // ToDo --> Try and update user in system

            return null;
        }

        #endregion Methods
    }
}