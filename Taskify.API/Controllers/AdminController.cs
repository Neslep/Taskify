﻿using Microsoft.AspNetCore.Mvc;
using System.Net;
using Taskify.API.DTOs.Requests;
using Taskify.API.Services.Repositories.IRepositories;

namespace Taskify.API.Controllers
{
    public class AdminController : BaseController<AdminController>
    {
        private readonly IAdminRepository _adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository ?? throw new ArgumentNullException(nameof(adminRepository));
        }

        [HttpPost]
        [Route("/api/admins/login")]
        public async Task<IActionResult> AdminLogin(AdminLoginRequest request)
        {
            var adminLogin = await _adminRepository.AdminLogin(request.username, request.password);
            var tokenString = GenerateJwtToken(adminLogin.Id.ToString(), "Admin");
            return CreateResponse<string>(true, "Request processed successfully.", HttpStatusCode.OK, tokenString);
        }
    }
}
