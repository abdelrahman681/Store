using DashBoard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DashBoard.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var role =await _roleManager.Roles.ToListAsync();
            return View(role);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var roleExsist = await _roleManager.RoleExistsAsync(model.Name);
                if (!roleExsist)
                {
                    var role = await _roleManager.CreateAsync(new IdentityRole(model.Name));
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "This role already exsist");
            }
            return View(nameof(Index));
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var role=await _roleManager.FindByIdAsync(Id);
            var model = new RoleViewModel()
            {
                Id=role.Id,
                Name=role.Name
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(RoleViewModel model,[FromRoute]string Id)
        {
            if(Id!=model.Id)
                return BadRequest();
            if (ModelState.IsValid)
            {
                var roleExsist = await _roleManager.RoleExistsAsync(model.Name);
                var role = await _roleManager.FindByIdAsync(Id);
                if (!roleExsist&&role is not null)
                {
                    role.Name = model.Name;
                    role.Id = model.Id;
                    var result=await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                        return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "This role already exsist");
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            var model = new RoleViewModel()
            {
                Id = role.Id,
                Name = role.Name
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(RoleViewModel model, [FromRoute] string Id)
        {
            if (Id != model.Id)
                return BadRequest();

            try
            {
                var role = await _roleManager.FindByIdAsync(Id);
                if (role is not null)
                {
                    await _roleManager.DeleteAsync(role);
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View(model);
        }
    }
}
