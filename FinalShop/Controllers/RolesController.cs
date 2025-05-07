using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FinalShop.Models;

namespace FinalShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleMgr;
        private readonly UserManager<ApplicationUser> _userMgr;

        public RolesController(
            RoleManager<IdentityRole> roleMgr,
            UserManager<ApplicationUser> userMgr)
        {
            _roleMgr = roleMgr;
            _userMgr = userMgr;
        }

        // GET: /Roles
        public IActionResult Index()
        {
            // List all roles
            var roles = _roleMgr.Roles;
            return View(roles);
        }

        // GET: /Roles/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoleName")] CreateRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var exists = await _roleMgr.RoleExistsAsync(model.RoleName);
            if (exists)
            {
                ModelState.AddModelError("", "That role already exists.");
                return View(model);
            }

            var result = await _roleMgr.CreateAsync(new IdentityRole(model.RoleName));
            if (result.Succeeded)
                return RedirectToAction(nameof(Index));

            foreach (var err in result.Errors)
                ModelState.AddModelError("", err.Description);
            return View(model);
        }

        // GET: /Roles/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var role = await _roleMgr.FindByIdAsync(id);
            if (role == null) return NotFound();

            var model = new EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name!
            };
            // build user-assignment list...
            foreach (var user in _userMgr.Users)
            {
                var isInRole = await _userMgr.IsInRoleAsync(user, role.Name!);
                model.Users.Add(new UserRoleViewModel
                {
                    userId = user.Id,
                    UserName = user.UserName!,
                    IsSelected = isInRole
                });
            }
            return View(model);
        }

        // POST: /Roles/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var role = await _roleMgr.FindByIdAsync(model.Id);
            if (role == null) return NotFound();

            // update role name
            if (role.Name != model.RoleName)
            {
                role.Name = model.RoleName;
                var nameResult = await _roleMgr.UpdateAsync(role);
                if (!nameResult.Succeeded)
                {
                    foreach (var err in nameResult.Errors)
                        ModelState.AddModelError("", err.Description);
                    return View(model);
                }
            }

            // sync users ↔ role
            foreach (var ur in model.Users)
            {
                var user = await _userMgr.FindByIdAsync(ur.userId);
                if (user == null) continue;

                var inRole = await _userMgr.IsInRoleAsync(user, role.Name!);
                if (ur.IsSelected && !inRole)
                    await _userMgr.AddToRoleAsync(user, role.Name!);
                else if (!ur.IsSelected && inRole)
                    await _userMgr.RemoveFromRoleAsync(user, role.Name!);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: /Roles/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var role = await _roleMgr.FindByIdAsync(id);
            if (role == null) return NotFound();
            return View(role);
        }

        // POST: /Roles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var role = await _roleMgr.FindByIdAsync(id);
            if (role != null)
                await _roleMgr.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }
    }
}
