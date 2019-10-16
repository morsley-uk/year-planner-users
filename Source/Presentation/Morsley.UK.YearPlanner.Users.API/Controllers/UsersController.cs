using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Morsley.UK.YearPlanner.Users.API.Models;

namespace Morsley.UK.YearPlanner.Users.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        #region GET

        [HttpGet]
        // Returns:
        // BadRequest
        // NoContent
        // OK
        public IActionResult Get(API.Models.GetUsersRequest request)
        {
            if (request == null) return BadRequest();

            var users = GetUsers(request);

            if (users == null || !users.Any()) return NoContent();

            return Ok(users);
        }

        [HttpGet("{id}")]
        // Returns:
        // BadRequest
        // NoContent
        // OK
        public IActionResult Get([FromRoute] Guid id)
        {
            if (id == default(Guid)) return BadRequest();

            var user = GetUser(id);

            if (user == null) return NoContent();

            return Ok(user);
        }

        #endregion GET

        #region POST

        [HttpPost]
        // Returns:
        // BadRequest
        // Created
        public IActionResult Add([FromBody] API.Models.CreateUserRequest request)
        {
            if (request == null) return BadRequest();

            var user = AddUser(request);

            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        #endregion POST

        #region PUT

        [HttpPut("{id}")]
        // Fully updates entity.
        // Returns:
        // BadRequest
        // NotFound
        // Ok
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

        [HttpPatch("{id}")]
        // Partially updates entity.
        // Returns:
        // BadRequest
        // NotFound
        // UnprocessableEntity
        // Ok
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

        [HttpDelete("{id}")]
        // Returns:
        // BadRequest
        // NoContent
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