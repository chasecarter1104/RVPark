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
        [BindProperty]
        public bool IsUpdate { get; set; }
        [BindProperty]
        public string Description { get; set; }

        public async Task OnGetAsync(string? id)
        {
            if (id != null)
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role != null)
                {
                    CurrentRole = new Role
                    {
                        Id = role.Id,
                        Name = role.Name,
                        NormalizedName = role.NormalizedName,
                        Description = role.Description // Use custom 'Description' property
                    };
                    Description = CurrentRole.Description;
                    IsUpdate = true;
                }
            }
            else
            {
                CurrentRole = new Role();
                IsUpdate = false;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            if (string.IsNullOrEmpty(CurrentRole.Id))
            {
                // Creating a new role
                CurrentRole.NormalizedName = CurrentRole.Name.ToUpper();
                CurrentRole.Description = Description;

                var result = await _roleManager.CreateAsync(CurrentRole);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                    return Page();
                }

                return RedirectToPage("./Index", new { success = true, message = "Successfully Added" });
            }
            else
            {
                // Updating an existing role
                var roleFromDb = await _roleManager.FindByIdAsync(CurrentRole.Id);

                if (roleFromDb == null)
                    return NotFound();

                roleFromDb.Name = CurrentRole.Name;
                roleFromDb.NormalizedName = CurrentRole.Name.ToUpper();
                roleFromDb.Description = Description;

                var result = await _roleManager.UpdateAsync(roleFromDb);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                        ModelState.AddModelError(string.Empty, error.Description);

                    return Page();
                }

                return RedirectToPage("./Index", new { success = true, message = "Update Successful" });
            }
        }
    }
}