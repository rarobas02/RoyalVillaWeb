using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RoyalVilla.DTO;
using RoyalVillaWeb.Models;
using RoyalVillaWeb.Services.IServices;
using System.Diagnostics;

namespace RoyalVillaWeb.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _villaService = villaService;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<VillaDTO> villaList = new();
            try
            {
                var response = await _villaService.GetAllAsync<ApiResponse<List<VillaDTO>>>(""); //the blank string in the parameter is for the token,
                if(response is not null && response.Data is not null)
                {
                    villaList = response.Data;
                }
            }
             catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }
            return View(villaList);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VillaCreateDTO createDTO)
        {
            if(!ModelState.IsValid)
            {
                return View(createDTO);
            }
            try
            {
                var response = await _villaService.CreateAsync<ApiResponse<VillaDTO>>(createDTO, ""); //the blank string in the parameter is for the token,
                if (response is not null && response.Data is not null)
                {
                    TempData["success"] = "Villa created successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }
            return View(createDTO);
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 0)
            {
                TempData["Error"] = "Invalid villa ID";
                return RedirectToAction(nameof(Index));
            }
            try
            {
                var response = await _villaService.GetAsync<ApiResponse<VillaDTO>>(id, ""); //the blank string in the parameter is for the token,
                if (response is not null && response.Data is not null)
                {
                    return View(response.Data);
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, VillaDTO createDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(createDTO);
            }
            try
            {
                var response = await _villaService.DeleteAsync<ApiResponse<VillaDTO>>(id, ""); //the blank string in the parameter is for the token,
                if (response is not null && response.Data is not null)
                {
                    TempData["success"] = "Villa deleted successfully";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                TempData["error"] = $"An error occured: {ex.Message}";
            }
            return View(createDTO);
        }
    }
}
