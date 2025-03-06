using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using Taskify.API.DTOs.Requests;
using Taskify.API.DTOs.Responses.UserDTOs;
using Taskify.API.Exceptions;
using Taskify.API.Mapper;
using Taskify.API.Models;
using Taskify.API.Services.Repositories.IRepositories;

namespace Taskify.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : BaseController<UserController>
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> UserLogin(UserLoginRequest request)
        {
            var userLogin = await _userRepository.UserLogin(request.email, request.password);
            var tokenString = GenerateJwtToken(userLogin.Id.ToString(), "User");
            return CreateResponse<string>(true, "Request processed successfully.", HttpStatusCode.OK, tokenString);
        }

        [HttpGet]
        [Authorize]
        [Route("validateToken")]
        public IActionResult ValidateToken()
        {
            return CreateResponse<string>(true, "Request processed successfully.", HttpStatusCode.OK);
        }

        [HttpGet]
        [Authorize]
        [Route("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userIdClaim = GetUserIdFromToken();

            int userId = userIdClaim ?? throw new AuthenticationException("Unauthorized: User ID not found or invalid.");

            var user = await _userRepository.GetByIdAsync(userId);

            var response = LazyMapper.Mapper.Map<UserResponse>(user);
            return CreateResponse<UserResponse>(true, "Request processed successfully.", HttpStatusCode.OK, response);
        }
    }
}
