using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
namespace RVPark.Pages.Admin.Roles
{
    public class UpsertModel : PageModel
    {
        private readonly RoleManager<Role> _roleManager;

        public UpsertModel(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public Role CurrentRole { get; set; }
        public bool IsUpdate { get; set; }

        public async Task OnGetAsync(string? id)
        {
            if (string.IsNullOrWhiteSpace(id)) // Create mode
            {
                CurrentRole = new Role(); // Initialize empty role
                IsUpdate = false;         // Indicate "create" mode
            }
            else // Update mode
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role == null)
                {
                    RedirectToPage("./Index", new { success = true, message = "Role created successfully" });
                    return;
                }

                CurrentRole = role; // Load existing role
                IsUpdate = true;    // Indicate "update" mode
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrEmpty(CurrentRole.Id)) // Create
            {
                CurrentRole.NormalizedName = CurrentRole.Name.ToUpper();
                var result = await _roleManager.CreateAsync(CurrentRole);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }

                return RedirectToPage("./Index", new { success = true, message = "Role created successfully" });
            }
            else // Update
            {
                var roleFromDb = await _roleManager.FindByIdAsync(CurrentRole.Id);
                if (roleFromDb == null)
                    return NotFound();

                roleFromDb.Name = CurrentRole.Name;
                roleFromDb.NormalizedName = CurrentRole.Name.ToUpper();
                roleFromDb.Description = CurrentRole.Description;

                var result = await _roleManager.UpdateAsync(roleFromDb);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);
                    return Page();
                }

                return RedirectToPage("./Index", new { success = true, message = "Role updated successfully" });
            }
        }
    }
}