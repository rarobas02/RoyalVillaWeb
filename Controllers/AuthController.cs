using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using RoyalVillaWeb.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Principal;

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
                if (response is not null && response.Success && response.Data is not null)
                {
                    LoginResponseDTO model = response.Data;
                    var handler = new JwtSecurityTokenHandler();
                    var jwt = handler.ReadJwtToken(model.Token);

                    var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    //data from the token extract
                    identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "email").Value));
                    identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));
                    var principal = new ClaimsPrincipal(identity);

                    //handles authentication cookie creation and management
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
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
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
