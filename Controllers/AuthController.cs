using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using RoyalVillaWeb.Services.IServices;

namespace RoyalVillaWeb.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;
        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO loginRequestDTO)
        {
            try
            {
                var response = await _authService.LoginAsync<ApiResponse<LoginResponseDTO>>(loginRequestDTO); //the blank string in the parameter is for the token,
                if (response is not null && response.Data is not null)
                {
                    LoginResponseDTO loginResponse = response.Data;
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }
            return View();
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegistrationRequestDTO { 
                Email = string.Empty,
                Name = string.Empty,
                Password = string.Empty,
                Role = "Customer"
            });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            try
            {
                ApiResponse<UserDTO> response = await _authService.RegisterAsync<ApiResponse<UserDTO>>(registrationRequestDTO); //the blank string in the parameter is for the token,
                if (response is not null && response.Success && response.Data is not null)
                {
                    TempData["success"] = "Registration successful! Please login with your credentials.";
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["error"] = response?.Message?? "Registration failed. Please, try again.";
                    return View(registrationRequestDTO);

                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }
            return View(registrationRequestDTO);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            return View();
        }
    }
}
